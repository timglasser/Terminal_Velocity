using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour
{
    public float speed = 10.0f;


    void Update()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
    }
}
