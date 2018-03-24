
//To the extent possible under law, 
//Tim Glasser 
//tim_glasser@hotmail.com
//has waived all copyright and related or neighboring rights and responsibilties to
//Unity ListenerPattern C# Classes.
//This work is published from:
//California.

// As indicated by the Creative Commons, the text on this page may be copied, 
// modified and adapted for your use, without any other permission from the author.

// Please do not remove this notice
using UnityEngine;
using System.Collections;

[System.Serializable]
public class Sender{
		public void Send(MonoBehaviour sender) {
			if (hasFired == false || onlyOnce == false) {
				foreach (Listener listen in listeners) {
					listen.Send(sender);
				}
				hasFired = true;
			}
		}
		public Listener[] listeners;
		public bool onlyOnce;
		private bool hasFired =false;
}
	

	
[System.Serializable]
public  class Listener {
	
		public GameObject listener ;
		public string action = "OnEvent";
	
		public  void Send (MonoBehaviour sender) {
			if (listener)
				listener.SendMessage (action);
			else
				Debug.LogWarning ("No receiver of event" + "\""+action+"\" on object "+sender.name+" ("+sender.GetType().Name+")", sender);
		}
		// need send with delay as a co-routine
}

