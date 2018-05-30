using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIListener : MonoBehaviour
{
    

    //Make sure to attach these Buttons in the Inspector
    public Button m_YourButton, m_YourSecondButton;
    // Use this for initialization
    void Start()
    {
        Button btn = m_YourButton.GetComponent<Button>();
        Button btn2 = m_YourSecondButton.GetComponent<Button>();
        //Calls the TaskOnClick method when you click the Button
        btn.onClick.AddListener(TaskOnClick);

        m_YourSecondButton.onClick.AddListener(delegate { TaskWithParameters("Start Terminal Velocity"); });
    }

    void TaskOnClick()
    {


        //Output this to console when the Button is clicked
        Debug.Log("You have clicked the button!");
        // set the start trigger in the FSM

        Animator anim = GetComponentInChildren<Animator>(); // the game state machine is in the camera child
        anim.SetTrigger("MouseDown");
        

    }

    void TaskWithParameters(string message)
    {
        //Output this to console when the Button is clicked
        Debug.Log(message);
    }



    // Update is called once per frame
    void Update()
    {

    }

}