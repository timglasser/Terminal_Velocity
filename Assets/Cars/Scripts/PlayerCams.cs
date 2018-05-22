using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCams : MonoBehaviour {

    // Use this for initialization

    bool tempChanged = false;

    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CameraMan.PlayerCamChange();
        }
        if (Input.GetKey(KeyCode.R))
        {
            switch (tempChanged)
            {
                case false:
                    CameraMan.CamInstance.RearTemp = CameraMan.CamInstance.PlayerCams;
                    CameraMan.CamInstance.PlayerCams = 3;
                    tempChanged = true;
                    break;
                case true:
                    CameraMan.CamInstance.PlayerCams = 3;
                    break;
            }

        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            CameraMan.CamInstance.PlayerCams = CameraMan.CamInstance.RearTemp;
            tempChanged = false;
        }

    }
}
