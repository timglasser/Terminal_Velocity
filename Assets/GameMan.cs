using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMan : MonoBehaviour {

    public static GameMan GameInstance;
    public GameObject[] Cars = new GameObject[7];
    public GameObject[] CarTransSpawn = new GameObject[7];
    Rigidbody[] m_RigidBody = new Rigidbody[7];
    // Use this for initialization
	void Awake ()
    {
        GameInstance = this;
    }
    private void Start()
    {
        InitializeAttractScreen();
        //InitializeGame();
    }

    public static void Pause()
    {

    }

    public void PlayGame()
    {
        for (int i =0; i < 7; i++)
        {

            Cars[i].transform.position = CarTransSpawn[i].transform.position;
            Cars[i].transform.rotation = CarTransSpawn[i].transform.rotation;
            m_RigidBody[i] = Cars[i].GetComponent<Rigidbody>();
            m_RigidBody[i].constraints = RigidbodyConstraints.FreezePositionX| RigidbodyConstraints.FreezePositionZ| RigidbodyConstraints.FreezeRotationX| RigidbodyConstraints.FreezeRotationY|RigidbodyConstraints.FreezeRotationZ;
            Cars[i].GetComponent<Rigidbody>().constraints = m_RigidBody[i].constraints;
        }
        InitializeGame();
        Cars[0].SetActive(true);
        m_RigidBody[0].constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
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
