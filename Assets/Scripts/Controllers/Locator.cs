using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//To the extent possible under law, 
//Tim Glasser 
//tim_glasser@hotmail.com
//has waived all copyright and related or neighboring rights to
//Unity Locator C# Class (modified from an old JS Unity Demo).
//This work is published from:
//California.

// As indicated by the Creative Commons, the text on this page may be copied, 
// modified and adapted for your use, without any other permission from the author.

// Please do not remove this notice



[ExecuteInEditMode]
public class Locator : MonoBehaviour {

    TextMesh txtMesh;
    public Color gizmoColor;
	// Use this for initialization

	void Start () {
        txtMesh = GetComponent<TextMesh>();
        txtMesh.text = gameObject.name;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(transform.position, new Vector3(1.0f, 1.0f, 1.0f));
        //  Gizmos.DrawWireCube(transform.position, new Vector3(1.0f, 1.0f, 1.0f));
    }
}
