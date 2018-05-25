using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof(CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use


        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
        }


        // added camera change input
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
               // CameraMan.PlayerCamChange();
            }
          
            if (Input.GetKeyDown(KeyCode.R))
            {
              //  CameraMan.CamInstance.PlayerCams = CameraMan.CamInstance.RearTemp;
               
            }

        }

        // add light control here

        void FixedUpdate()
        {


            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");

#if !MOBILE_INPUT
            float handbrake = CrossPlatformInputManager.GetAxis("Jump");
    //        Debug.Log("Input Car drive " + gameObject.name);
            m_Car.Move(h, v, v, handbrake);
#else
            m_Car.Move(h, v, v, 0.0f);
#endif
        }



    }

}