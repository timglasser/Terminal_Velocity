using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public static CameraManager camMan;
    private int cameraAssign = 0;
    public Camera[] cameras = new Camera[9];



    /*

       In-Game Cameras

       0.Third-Person (Near)
       1.First-Person (Cockpit)
       2.First-Person (Hood)
       3.Rear View

       Attraction Cameras
       
       4.User Defined
       5.Following Player (Front)
       6.Following Player (Profile)
       7.Following AI (Third-Person)
       8.Following AI (Front)

    */
    // Use this for initialization
    public void AlternateInGameCam()
    {
        switch (cameraAssign)
        {
            case 0:
                {
                    break;
                }

        }
    }

    void Awake () {
        camMan = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
