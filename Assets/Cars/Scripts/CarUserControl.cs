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

        void Update()
        {
            //Debug.Log("Regular: Input Car drive" + Time.deltaTime);
            // pass the input to the car!

            //m_Car.Move(h, v, v, 0f);
        }

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