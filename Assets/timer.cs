using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour {
    public Text timerText;
    private float startTime;


	// Use this for initialization
	void Start () {
        startTime = Time.deltaTime;
	}
	
	// Update is called once per frame
	void Update () {
        float t = Time.deltaTime - startTime;

        string minutes = ((int)t / 60).ToString();
        string sec = (t % 60).ToString("f0");

        timerText.text = minutes + ":" + sec;
    }
}
