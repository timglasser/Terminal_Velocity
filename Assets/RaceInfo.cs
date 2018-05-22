using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

//To the extent possible under law, 
//Tim Glasser 
//tim_glasser@hotmail.com
//has waived all copyright and related or neighboring rights to
//Unity RaceInfo C# Class 
//This work is published from:
//California.

// As indicated by the Creative Commons, the text on this page may be copied, 
// modified and adapted for your use, without any other permission from the author.

// Please do not remove this notice


public class RaceInfo : MonoBehaviour {
    // demonstrate a singleton pattern for the raceinfo
    // never make public just to show in the inspector
    [SerializeField]
    private static RaceInfo Instance;
  
    public GameObject[] participants ;

    // we need a list data structure. we are going to order the racers based on their progress along the track
    private static List<WaypointProgressTracker> humanparticipants = new List<WaypointProgressTracker>();
    private static List<WaypointProgressTracker> raceOrder = new List<WaypointProgressTracker>();
    private float[] prevProgress = new float [7];
    private static float currRaceTime;
    [SerializeField]
    private Listener wrongwayEvent;  // event of a controlled car going the wrong way
    private bool isRacing , isWrongway = false;

    // example of c# built accessor methods
    public static WaypointProgressTracker [] RaceOrder
    {
        get { return raceOrder.ToArray();}
        //   private set { currRaceTime = value; }
    }
    // example of c# built accessor methods
    public static float CurrRaceTime
    {
        get { return currRaceTime; }
     //   private set { currRaceTime = value; }
    }

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {

        onRaceStart();
    }

    // reset event at the strt of a race
    public void onRaceEnd()
    {
        // get winner

        // is it you

        // if so get another game

    }

    public void onRaceStart()
    {
        currRaceTime = 0;
        int i = 0;

        // populate the lists
        foreach (GameObject participant in participants)
        {
            if (participant.gameObject.GetComponent<CarUserControl>())
            {
                humanparticipants.Add(participant.GetComponent<WaypointProgressTracker>());
               // prevProgress[i] = 0.0f;
            }
            prevProgress[i] = 0.0f;
            raceOrder.Add(participant.GetComponent<WaypointProgressTracker>());
            i++;
        }
    }
  

    // Update is called from a variable time interval
    void FixedUpdate()
    {
        // update the race order based on progress
        // If you need to sort the list in-place then you can use the Sort method, 
        // passing a Comparison < T > delegate:
        raceOrder.Sort((x, y) => y.ProgressDistance.CompareTo(x.ProgressDistance));
    }

    // If the participant has not made progress it must be going the wrong way
    void Update () {
        currRaceTime += Time.deltaTime;// Time is a singleton
        int i = 0;
        foreach (WaypointProgressTracker participant in humanparticipants)
        {
            if ( participant.ProgressDistance < prevProgress[i])
            {
                // wrong way on 
                isWrongway = true;
            }
            else if (isWrongway)
            {
                // wrong way off
                isWrongway = false;
            }
            prevProgress[i] = participant.ProgressDistance;
            i++;
        }
        Debug.Log("car " + raceOrder[0].gameObject.name + " progress is " + raceOrder[0].ProgressDistance);
    }
}
