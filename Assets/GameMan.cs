using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMan : MonoBehaviour {

    public static GameMan GameInstance;
    public GameObject[] CarTransSpawn = new GameObject[7];
    public GameObject[] tempSpawn = new GameObject[7];
    // Use this for initialization
	void Awake ()
    {
        GameInstance = this;
    }
    private void Start()
    {
        //InitializeAttractScreen();
        InitializeGame();
        for (int i = 0; i < 7; i++)
        {
            tempSpawn[i] = CarTransSpawn[i];
        }
    }

    public static void Pause()
    {

    }

    public void Respawn()
    {
        for (int i =0; i < 7; i++)
        {
            CarTransSpawn[i] = tempSpawn[i];
        }
        Debug.Log("Button Pressed");
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
