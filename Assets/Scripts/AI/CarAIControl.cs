using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarAIControl : MonoBehaviour
    {
        public enum BrakeCondition
        {
            NeverBrake,                 // the car simply accelerates at full throttle all the time.
            TargetDirectionDifference,  // the car will brake according to the upcoming change in direction of the target. Useful for route-based AI, slowing for corners.
            TargetDistance,             // the car will brake as it approaches its target, regardless of the target's direction. Useful if you want the car to
                                        // head for a stationary target and come to rest when it arrives there.
        }

        // This script provides input to the car controller in the same way that the user control script does.
        // As such, it is really 'driving' the car, with no special physics or animation tricks to make the car behave properly.

        // "wandering" is used to give the cars a more human, less robotic feel. They can waver slightly
        // in speed and direction while driving towards their target.

        [SerializeField] [Range(0, 1)] private float m_CautiousSpeedFactor = 0.05f;               // percentage of max speed to use when being maximally cautious
        [SerializeField] [Range(0, 180)] private float m_CautiousMaxAngle = 50f;                  // angle of approaching corner to treat as warranting maximum caution
        [SerializeField] private float m_CautiousMaxDistance = 100f;                              // distance at which distance-based cautiousness begins
        [SerializeField] private float m_CautiousAngularVelocityFactor = 30f;                     // how cautious the AI should be when considering its own current angular velocity (i.e. easing off acceleration if spinning!)
        [SerializeField] private float m_SteerSensitivity = 0.05f;                                // how sensitively the AI uses steering input to turn to the desired direction
        [SerializeField] private float m_AccelSensitivity = 0.04f;                                // How sensitively the AI uses the accelerator to reach the current desired speed
        [SerializeField] private float m_BrakeSensitivity = 1f;                                   // How sensitively the AI uses the brake to reach the current desired speed
        [SerializeField] private float m_LateralWanderDistance = 3f;                              // how far the car will wander laterally towards its target
        [SerializeField] private float m_LateralWanderSpeed = 0.1f;                               // how fast the lateral wandering will fluctuate
        [SerializeField] [Range(0, 1)] private float m_AccelWanderAmount = 0.1f;                  // how much the cars acceleration will wander
        [SerializeField] private float m_AccelWanderSpeed = 0.1f;                                 // how fast the cars acceleration wandering will fluctuate
        [SerializeField] private BrakeCondition m_BrakeCondition = BrakeCondition.TargetDistance; // what should the AI consider when accelerating/braking?
        [SerializeField] private bool m_Driving;                                                  // whether the AI is currently actively driving or stopped.
        [SerializeField] private Transform m_Target;                                              // 'target' the target object to aim for.
        [SerializeField] private bool m_StopWhenTargetReached;                                    // should we stop driving when we reach the target?
        [SerializeField] private float m_ReachTargetThreshold = 2;                                // proximity to target to consider we 'reached' it, and stop driving.

        private float m_RandomPerlin;             // A random value for the car to base its wander on (so that AI cars don't all wander in the same pattern)
        private CarController m_CarController;    // Reference to actual car controller we are controlling
        private float m_AvoidOtherCarTime;        // time until which to avoid the car we recently collided with
        private float m_AvoidOtherCarSlowdown;    // how much to slow down due to colliding with another car, whilst avoiding
        private float m_AvoidPathOffset;          // direction (-1 or 1) in which to offset path to avoid other car, whilst avoiding
        private Rigidbody m_Rigidbody;
        public bool crashed;
        public float crashedTime = 0.0f;
        Vector3 RandomStopCheckPos;
        float randomStopTime = 5.0f;

        private void Awake()
        {
            RandomStopCheckPos = transform.position;
            // get the car controller reference
            m_CarController = GetComponent<CarController>();

            // give the random perlin a random value
            m_RandomPerlin = Random.value*100;

            m_Rigidbody = GetComponent<Rigidbody>();
        }
       


        private void FixedUpdate()
        {
            bool carinFront = false;
            bool carLeft = false;
            bool carRight = false;
            Debug.DrawRay(transform.position, transform.forward*10);
            RaycastHit[] hit = Physics.RaycastAll(new Ray(transform.position + transform.up - transform.forward, transform.forward * -1), 3);
            foreach (RaycastHit h in hit)
            {
                if (h.collider.gameObject.tag == "SideWall" || h.collider.gameObject.GetComponent<CarController>())
                {
                    crashed = false;
                    //crashedTime = 5.0f;
                    //Debug.Log(h.collider.gameObject.name + " " + h.point + ", " + h.distance);
                    
                }
                
                
                Debug.DrawRay(transform.position + transform.up - transform.forward, transform.forward * -10);
                //crashed = false;
            }
            //randomStop();
            if (crashed)
            {
                hit = Physics.RaycastAll(new Ray(transform.position + transform.up - transform.forward, transform.forward), 5);
                foreach (RaycastHit h in hit)
                {
                    if (h.collider.gameObject.tag == "SideWall")
                    {
                        crashed = true;
                        //crashedTime = 5.0f;
                        //Debug.Log(h.collider.gameObject.name + " " + h.point + ", " + h.distance);
                        break;
                    }
                    else
                    {
                        crashed = false;
                    }

                    Debug.DrawRay(transform.position + transform.up - transform.forward, transform.forward * -3);
                    //crashed = false;
                }
                //crashedTime -= Time.fixedDeltaTime;
                //if (crashedTime <= 0)
                //    crashed = false;
            }
            
            if (m_Target == null || !m_Driving)
            {
                // Car should not be moving,
                // use handbrake to stop
                m_CarController.Move(0, 0, -1f, 1f);
            }
            else
            {
                Vector3 fwd = transform.forward;
                if (m_Rigidbody.velocity.magnitude > m_CarController.MaxSpeed*0.1f)
                {
                    fwd = m_Rigidbody.velocity;
                }

                float desiredSpeed = m_CarController.MaxSpeed;
                

                // now it's time to decide if we should be slowing down...
                switch (m_BrakeCondition)
                {
                    case BrakeCondition.TargetDirectionDifference:
                        {
                            // the car will brake according to the upcoming change in direction of the target. Useful for route-based AI, slowing for corners.

                            // check out the angle of our target compared to the current direction of the car
                            float approachingCornerAngle = Vector3.Angle(m_Target.forward, fwd);

                            // also consider the current amount we're turning, multiplied up and then compared in the same way as an upcoming corner angle
                            float spinningAngle = m_Rigidbody.angularVelocity.magnitude*m_CautiousAngularVelocityFactor;

                            // if it's different to our current angle, we need to be cautious (i.e. slow down) a certain amount
                            float cautiousnessRequired = Mathf.InverseLerp(0, m_CautiousMaxAngle,
                                                                           Mathf.Max(spinningAngle,
                                                                                     approachingCornerAngle));
                            desiredSpeed = Mathf.Lerp(m_CarController.MaxSpeed, m_CarController.MaxSpeed*m_CautiousSpeedFactor,
                                                      cautiousnessRequired);
                            break;
                        }

                    case BrakeCondition.TargetDistance:
                        {
                            // the car will brake as it approaches its target, regardless of the target's direction. Useful if you want the car to
                            // head for a stationary target and come to rest when it arrives there.

                            // check out the distance to target
                            Vector3 delta = m_Target.position - transform.position;
                            float distanceCautiousFactor = Mathf.InverseLerp(m_CautiousMaxDistance, 0, delta.magnitude);

                            // also consider the current amount we're turning, multiplied up and then compared in the same way as an upcoming corner angle
                            float spinningAngle = m_Rigidbody.angularVelocity.magnitude*m_CautiousAngularVelocityFactor;

                            // if it's different to our current angle, we need to be cautious (i.e. slow down) a certain amount
                            float cautiousnessRequired = Mathf.Max(
                                Mathf.InverseLerp(0, m_CautiousMaxAngle, spinningAngle), distanceCautiousFactor);
                            desiredSpeed = Mathf.Lerp(m_CarController.MaxSpeed, m_CarController.MaxSpeed*m_CautiousSpeedFactor,
                                                      cautiousnessRequired);
                            break;
                        }

                    case BrakeCondition.NeverBrake:
                        break;
                }
               CarController c = GetComponent<CarController>();
             //   m_SteerSensitivity = 0.01f * ( c.CurrentSpeed/ c.MaxSpeed);
                // Evasive action due to collision with other cars:

                // our target position starts off as the 'real' target position
                Vector3 offsetTargetPos = m_Target.position;

                // if are we currently taking evasive action to prevent being stuck against another car:
                if (Time.time < m_AvoidOtherCarTime)
                {
                    // slow down if necessary (if we were behind the other car when collision occured)

                    desiredSpeed *= m_AvoidOtherCarSlowdown;

                    // and veer towards the side of our path-to-target that is away from the other car
                    offsetTargetPos += m_Target.right*m_AvoidPathOffset;
                }
                else
                {
                    // no need for evasive action, we can just wander across the path-to-target in a random way,
                    // which can help prevent AI from seeming too uniform and robotic in their driving
                    offsetTargetPos += m_Target.right*
                                       (Mathf.PerlinNoise(Time.time*m_LateralWanderSpeed, m_RandomPerlin)*2 - 1)*
                                       m_LateralWanderDistance;
                }

                // use different sensitivity depending on whether accelerating or braking:
                float accelBrakeSensitivity = (desiredSpeed < m_CarController.CurrentSpeed)
                                                  ? m_BrakeSensitivity
                                                  : m_AccelSensitivity;

                // decide the actual amount of accel/brake input to achieve desired speed.
                //Debug.Log(crashed);
                float accel = Mathf.Clamp((desiredSpeed * (crashed ? -1:1) - m_CarController.CurrentSpeed)*accelBrakeSensitivity, -1, 1);

                // add acceleration 'wander', which also prevents AI from seeming too uniform and robotic in their driving
                // i.e. increasing the accel wander amount can introduce jostling and bumps between AI cars in a race
                accel *= (1 - m_AccelWanderAmount) +
                         (Mathf.PerlinNoise(Time.time*m_AccelWanderSpeed, m_RandomPerlin)*m_AccelWanderAmount);

                // calculate the local-relative position of the target, to steer towards
                Vector3 localTarget = transform.InverseTransformPoint(offsetTargetPos);

                // work out the local angle towards the target
                float targetAngle = Mathf.Atan2(localTarget.x * (crashed ? -1 : 1), localTarget.z * (crashed ? -1 : 1)
                    ) * Mathf.Rad2Deg;
                //float targetAngle = Mathf.Atan2(localTarget.x , localTarget.z 
                //    ) * Mathf.Rad2Deg;
                targetAngle = Mathf.Abs(targetAngle) > 0.1 ? targetAngle : 0;
                //Debug.Log(targetAngle);
                float originalAngle = targetAngle;
                RaycastHit[] hitForward = Physics.RaycastAll(new Ray(transform.position + transform.up/2, transform.forward), 10);
                RaycastHit[] hitLeft = Physics.RaycastAll(new Ray(transform.position + transform.up/2, transform.TransformDirection((2*Vector3.forward) + Vector3.left)), 10);
                RaycastHit[] hitRight = Physics.RaycastAll(new Ray(transform.position + transform.up/2, transform.TransformDirection((2*Vector3.forward) + Vector3.right)), 10);
                Debug.DrawRay(transform.position + transform.up/2, Vector3.Normalize(transform.TransformDirection((2*Vector3.forward) + Vector3.right)) * 10);
                Debug.DrawRay(transform.position + transform.up/2, Vector3.Normalize(transform.TransformDirection((2 * Vector3.forward) + Vector3.left)) * 10);
                float distancef = 1, distancel = 1, distancer = 1;
                foreach(RaycastHit h in hitForward)
                {
                    if(h.collider.gameObject.GetComponentInParent<CarController>())
                    {
                        carinFront = true;
                        distancef = h.distance;
                        targetAngle += (targetAngle >= 0 ? 100 : -100) / distancef;
                    }
                    //Debug.Log(h.collider.gameObject);
                    
                }
                foreach (RaycastHit h in hitLeft)
                {
                    if (h.collider.gameObject.GetComponentInParent<CarController>())
                    {
                        carLeft = true;
                        distancel = h.distance;
                        targetAngle += 25 / distancel;
                    }
                    if (h.collider.gameObject.tag == "SideWall")
                    {
                        carLeft = true;
                        distancel = h.distance;
                        targetAngle += 50 / distancel;
                    }
                    //Debug.Log(h.collider.gameObject);
                }
                foreach (RaycastHit h in hitRight)
                {
                    if (h.collider.gameObject.GetComponentInParent<CarController>())
                    {
                        carRight = true;
                        distancer = h.distance;
                        targetAngle += -25 / distancer;
                    }
                    if (h.collider.gameObject.tag == "SideWall")
                    {
                        carRight = true;
                        distancer = h.distance;
                        targetAngle += -50 / distancer;
                    }
                    //Debug.Log(h.collider.gameObject);
                }
                
                //Debug.Log(originalAngle + " " + targetAngle);

                // get the amount of steering needed to aim the car towards the target
                float steer = Mathf.Clamp(targetAngle*m_SteerSensitivity, -1, 1)*Mathf.Sign(m_CarController.CurrentSpeed);

                // feed input to the car controller.
                m_CarController.Move(steer, accel, accel, 0f);

                // if appropriate, stop driving when we're close enough to the target.
                if (m_StopWhenTargetReached && localTarget.magnitude < m_ReachTargetThreshold)
                {
                    m_Driving = false;
                }
            }
        }


        private void OnCollisionStay(Collision col)
        {
            RandomStopCheckPos = transform.position;
            // detect collision against other cars, so that we can take evasive action
            if (col.rigidbody != null)
            {
                var otherAI = col.rigidbody.GetComponent<CarAIControl>();
                if (otherAI != null)
                {
                    // we'll take evasive action for 1 second
                    m_AvoidOtherCarTime = Time.time + 1;

                    // but who's in front?...
                    if (Vector3.Angle(transform.forward, otherAI.transform.position - transform.position) < 90)
                    {
                        // the other ai is in front, so it is only good manners that we ought to brake...
                        m_AvoidOtherCarSlowdown = 0.5f;
                    }
                    else
                    {
                        // we're in front! ain't slowing down for anybody...
                        m_AvoidOtherCarSlowdown = 1;
                    }

                    // both cars should take evasive action by driving along an offset from the path centre,
                    // away from the other car
                    var otherCarLocalDelta = transform.InverseTransformPoint(otherAI.transform.position);
                    float otherCarAngle = Mathf.Atan2(otherCarLocalDelta.x, otherCarLocalDelta.z);
                    m_AvoidPathOffset = m_LateralWanderDistance*-Mathf.Sign(otherCarAngle);
                }
               
            }
            //RaycastHit[] hit = Physics.RaycastAll(new Ray(transform.position + transform.up, transform.forward * 10));
            //foreach (RaycastHit h in hit)
            //{
            //    if (h.collider.gameObject.tag == "SideWall" )
            //    {
            //        crashed = true;
            //        crashedTime = 5.0f;
            //        break;
            //    }
            //    //Debug.Log("hit front");
            //    //Debug.DrawRay(transform.position, Vector3.forward);
            //    crashed = false;
            //}
        }


        public void SetTarget(Transform target)
        {
            m_Target = target;
            m_Driving = true;
        }
        public void randomStop()
        {
            randomStopTime -= Time.fixedDeltaTime;
            if (randomStopTime <= 0)
            {
                if (Vector3.Distance(RandomStopCheckPos, transform.position) > 5)
                {
                    RandomStopCheckPos = transform.position;

                }
                else
                {
                    RaycastHit[] hit = Physics.RaycastAll(new Ray(transform.position + transform.up, transform.forward * 10));
                    foreach(RaycastHit h in hit)
                    { 
                        if (h.collider.gameObject.tag == "SideWall")
                        {
                            crashed = true;
                            crashedTime = 5.0f;
                            break;
                        }
                        //Debug.Log("hit front");
                        //Debug.DrawRay(transform.position, Vector3.forward);
                        crashed = false;
                    }
                }
                randomStopTime = 5.0f;
            }
        }
        public void setCrashed()
        {
            crashed = true;
            crashedTime = 5.0f;
        }
    }
}
