using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameReset : MonoBehaviour {

    public static GameReset GameInstance;
    //  public static int NumContestants;
    // public static GameObject[] CarTransSpawn = new GameObject[NumContestants];

    // this should be private
    // no magic numbers
   // private static Transform[] tempSpawn = new Transform[7];
    private static Transform[] tempSpawn = new Transform[RaceInfo.RaceOrder.Length];
    // Use this for initialization
	void Awake ()
    {
        GameInstance = this;
    }
    private void Start()
    {
        //InitializeAttractScreen();
       // Initialize Contestants
        for (int i = 0; i < RaceInfo.RaceOrder.Length; i++)
        {
            tempSpawn[i] = RaceInfo.RaceOrder[i].gameObject.transform;

            // might have to init the rigid body here
        }
    }

    public static void Pause()
    {

    }

    public static void Reset()
    {
        for (int i =0; i < RaceInfo.RaceOrder.Length; i++)
        {
            //start position
            RaceInfo.RaceOrder[i].gameObject.transform.SetPositionAndRotation(tempSpawn[i].transform.position, tempSpawn[i].rotation) ;

            // might have to reset the rigid body here
        }
    }

    /* dont need these. The state machine will do this
    public static void InitializeAttractScreen()
    {
        CameraMan.CamInstance.CurrentCamerasPar = 0;
    }
    public static void InitializeGame()
    {
        CameraMan.CamInstance.CurrentCamerasPar = 1;
    }
	*/
	// Update is called once per frame
	void Update () {
		
	}
}
