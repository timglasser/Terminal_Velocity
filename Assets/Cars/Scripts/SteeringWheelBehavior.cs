using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{
    //  [RequireComponent(typeof(CarController))]
   

    public class SteeringWheelBehavior : MonoBehaviour
    {

        public CarController mycar;
        public float sensitivity=1.5f;
   
     //   public GameObject SteeringWheel;


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
        //    {

                // SteeringWheel.transform.Rotate(-Vector3.forward * Time.deltaTime * angle * turnSpeed);
                Quaternion rot = Quaternion.Euler(0.0f,0.0f, -angle*sensitivity);
                transform.localRotation= rot;


         //   }

            //     if (Input.GetKey(KeyCode.RightArrow))
            //     {

            //        SteeringWheel.transform.Rotate(-Vector3.forward * Time.deltaTime * turnSpeed);

            //   }

        }
    }
}
