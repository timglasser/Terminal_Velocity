using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMan : MonoBehaviour {

    public static GameMan GameInstance;
    // no magic numbers
    private Vector3 [] startPositions = new Vector3[7];
    private Quaternion[] startRotations = new Quaternion[7];

    // Use this for initialization
    void Awake ()
    {
        GameInstance = this;
    }
    
    void Start()
    {

        //  Initialize Game;
        RaceInfo.Instance.onRaceStart();
        for (int i = 0; i < RaceInfo.RaceOrder.Length; i++)

        {
            startPositions[i] = new Vector3(RaceInfo.RaceOrder[i].gameObject.transform.position.x,
                RaceInfo.RaceOrder[i].gameObject.transform.position.y,
                RaceInfo.RaceOrder[i].gameObject.transform.position.z);

            startRotations[i] = RaceInfo.RaceOrder[i].gameObject.transform.rotation;
            // might have to init the rigid body here
        }
    }

    public static void Pause()
    {

    }

    public static void Enable()
    {
        // enable the race cars
        for (int i = 0; i < RaceInfo.RaceOrder.Length; i++)
        {
            Rigidbody com = RaceInfo.RaceOrder[i].gameObject.GetComponent<Rigidbody>();
            com.isKinematic = false;
            // might have to init the rigid body here
        }
    }

    // before the start of the game during start sequence
    public static void Disable()
    {
        // Disable the race cars
        for (int i = 0; i < RaceInfo.RaceOrder.Length; i++)
        {
            Rigidbody com = RaceInfo.RaceOrder[i].gameObject.GetComponent<Rigidbody>();
            com.isKinematic = true;
            // might have to init the rigid body here
        }
    }

    // at the beginning of the start sequence
    public static void Reset()
    {
        //  Initialize race;
   //     RaceInfo.Instance.onRaceStart();

        for (int i =0; i < RaceInfo.RaceOrder.Length; i++)
        {
            // reset the waypoint progress tracker
            RaceInfo.RaceOrder[i].gameObject.GetComponent<WaypointProgressTracker>().Reset();
            //start position

            // might have to reset the rigid body here

            // start rotation and position.
            RaceInfo.RaceOrder[i].gameObject.GetComponent<Rigidbody>().isKinematic = true;
            RaceInfo.RaceOrder[i].gameObject.transform.SetPositionAndRotation(GameInstance.startPositions[i], GameInstance.startRotations[i]);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Output event to fsm when finish line is crossed
        Debug.Log(other.transform.name + " has won");
        // set the start trigger in the FSM
        Animator anim = GetComponentInChildren<Animator>(); // the game state machine is in the camera child
        anim.SetTrigger("GameOver");
    }
}
