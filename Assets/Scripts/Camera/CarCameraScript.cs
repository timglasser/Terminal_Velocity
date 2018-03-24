using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCameraScript : MonoBehaviour
{
    public Transform car;
    public float distance = 8.0f;
    public float height = 2.5f;
    public float rotationDampening = 3.0f;
    public float heightDampening = 2.0f;
    public float zoomRatio = 20.0f;
    public float defaultFOV = 60.0f;

    private Vector3 rotationVector;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        float wantedAngle = rotationVector.y;
        float wantedHeight = car.position.y + height;
        float myAngle = transform.eulerAngles.y;
        float myHeight = transform.position.y;
        Quaternion currentRotation = Quaternion.Euler(0, myAngle, 0);

        myAngle = Mathf.LerpAngle(myAngle, wantedAngle, rotationDampening * Time.deltaTime);
        myHeight = Mathf.Lerp(myHeight, wantedHeight, heightDampening * Time.deltaTime);

        transform.position = car.position;
        transform.position -= currentRotation * Vector3.forward * distance;
        transform.position = new Vector3(transform.position.x, myHeight, transform.position.z);
        transform.LookAt(car);

    }

    void FixedUpdate()
    {
        Vector3 localVelocity = car.InverseTransformDirection(car.GetComponent<Rigidbody>().velocity);


        if (localVelocity.z < -0.5f)
        {
            rotationVector.y = car.eulerAngles.y + 180;
        }
        else
        {
            rotationVector.y = car.eulerAngles.y;
        }

        float acceleration = car.GetComponent<Rigidbody>().velocity.magnitude;
        GetComponent<Camera>().fieldOfView = defaultFOV + acceleration * zoomRatio * Time.deltaTime;
    }
}


