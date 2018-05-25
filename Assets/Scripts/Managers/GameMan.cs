using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMan : MonoBehaviour {

    public static GameMan GameInstance;
    //  public static int NumContestants;
    // public static GameObject[] CarTransSpawn = new GameObject[NumContestants];

    // this should be private
    // no magic numbers
    private Vector3 [] startPositions = new Vector3[7];
    private Quaternion[] startRotations = new Quaternion[7];
    //  private static Transform[] tempSpawn;
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
            //       tempSpawn[i] = com.gameObject.transform;
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
            //       tempSpawn[i] = com.gameObject.transform;

            // might have to init the rigid body here
        }
    }

    // at the beginning of the start sequence
    public static void Reset()
    {
        // reset the waypoint manager

        for (int i =0; i < RaceInfo.RaceOrder.Length; i++)
        {
            RaceInfo.RaceOrder[i].gameObject.GetComponent<WaypointProgressTracker>().Reset();
            //start position

            // might have to reset the rigid body here

            // freeze position rotation and velocities.
            RaceInfo.RaceOrder[i].gameObject.GetComponent<Rigidbody>().isKinematic = true;
            //start position
            RaceInfo.RaceOrder[i].gameObject.transform.SetPositionAndRotation(GameInstance.startPositions[i], GameInstance.startRotations[i]);

        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Output this to console when the Button is clicked
        Debug.Log(other.transform.name + " has won");
        // set the start trigger in the FSM
        Animator anim = GetComponentInChildren<Animator>(); // the game state machine is in the camera child
        anim.SetTrigger("GameOver");
    }


}
