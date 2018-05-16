using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMan : MonoBehaviour {

    public static GameMan GameInstance;
    public Transform[] CarTransSpawn = new Transform[7];
    // Use this for initialization
	void Awake ()
    {
        GameInstance = this;
    }
    private void Start()
    {
        InitializeGame();
    }

    public static void Pause()
    {

    }
    public static void InitializeAttractScreen()
    {
        CameraMan.CamInstance.CurrentCamerasPar = 0;
    }
    public static void InitializeGame()
    {
        CameraMan.CamInstance.CurrentCamerasPar = 1;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
