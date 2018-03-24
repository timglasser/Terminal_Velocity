// C# example.
using UnityEditor;
using UnityEngine;

public class NewLevel : EditorWindow
{
	string myString = "New Level";
	bool groupEnabled;
	bool myBool = true;
	float myFloat = 1.23f;
	
	// Add menu item named "My Window" to the Window menu
	[MenuItem("Window/New Level")]
	public static void ShowWindow()
	{
		//Show existing window instance. If one doesn't exist, make one.
		EditorWindow.GetWindow(typeof(NewLevel));
	}
	
	void OnGUI()
	{
		GUILayout.Label ("Base Settings", EditorStyles.boldLabel);
		myString = EditorGUILayout.TextField ("Text Field", myString);
		
		groupEnabled = EditorGUILayout.BeginToggleGroup ("Optional Settings", groupEnabled);
		myBool = EditorGUILayout.Toggle ("Toggle", myBool);
		myFloat = EditorGUILayout.Slider ("Slider", myFloat, -3, 3);
		EditorGUILayout.EndToggleGroup ();
	}
}
