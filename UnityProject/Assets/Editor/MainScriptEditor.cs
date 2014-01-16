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
	
	private void CallbackFunction()
    {
		MainScript myMainScript = (MainScript)target;
        //myMainScript.Update();
    }
 
    void OnEnable()
    {
        EditorApplication.update += CallbackFunction;
    }
 
    void OnDisable()
    {
        EditorApplication.update -= CallbackFunction;
    }
}
