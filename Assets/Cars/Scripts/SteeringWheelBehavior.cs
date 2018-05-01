using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{
    //  [RequireComponent(typeof(CarController))]
   

    public class SteeringWheelBehavior : MonoBehaviour
    {
        public float turnSpeed;
        float maxAngle = (100);
        public CarController mycar; 
   
        public GameObject SteeringWheel;


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // transform.localRotation = Quaternion.identity;

            float angle = mycar.CurrentSteerAngle;
             
          // // if (Input.GetKey(KeyCode.LeftArrow))
            {

                SteeringWheel.transform.Rotate(-Vector3.forward * Time.deltaTime * angle * turnSpeed);

            }

       //     if (Input.GetKey(KeyCode.RightArrow))
       //     {

        //        SteeringWheel.transform.Rotate(-Vector3.forward * Time.deltaTime * turnSpeed);

         //   }

        }
        public void Move(float steering, float accel, float footbrake, float handbrake)
        {


        }
        
    }
}
