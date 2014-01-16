using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MainScript))]
public class MainScriptEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		MainScript myMainScript = (MainScript)target;

		if (GUILayout.Button ("Spawn objects"))
		{
			myMainScript.SpawnObjects();
		}
	}
}
