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
			int childs = myMainScript.transform.childCount;
			
			for (int i = childs - 1; i >= 0; i--)
			{
				GameObject.DestroyImmediate(myMainScript.transform.GetChild(i).gameObject);	
			}


			myMainScript.SpawnObjects();
		}
	}
}
