using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMan : MonoBehaviour
{
    public static CameraMan CamInstance;
    public GameObject [] Cameras = new GameObject[7];
    public int tempInt;
    public int CurrentCamerasPar;
    public int PlayerCams = 0;
    public int RearTemp;


    // Use this for initialization
    void Awake()
    {
        CamInstance = this;
    }

    public static void PlayerCamChange()
    {
        switch (CamInstance.PlayerCams)
        {
            case 0:
                CamInstance.PlayerCams++;
                break;
            case 1:
                CamInstance.PlayerCams++;
                break;
            case 2:
                CamInstance.PlayerCams = 0;
                break;
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
}
