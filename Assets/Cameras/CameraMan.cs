using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMan : MonoBehaviour
{
    public static CameraMan Instance;
    public GameObject [] CameraLocators = new GameObject[7];
    public GameObject[] CameraLookAt = new GameObject[1];

    /*  public enum CameraEnum =
      // need to enum the camera locators
      public Transform GetLocator(CameraEnum type)
      {

      }
  */

    public Transform GetLookAt(int index)
    {
        return CameraLookAt[0].transform;
    }
    public Transform GetLocator(int index)
    {
        return CameraLocators[index].transform;
    }

    // Use this for initialization
    void Awake()
    {
        Instance = this;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
