using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(LSystem))]
public class LSystemEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		LSystem myLSystemScript = (LSystem)target;

		if (GUILayout.Button ("Add"))
		{
			GameObject gameObject = new GameObject();
			gameObject.transform.parent = myLSystemScript.transform;
			
			myLSystemScript.molecule_names.Add("");
			myLSystemScript.molecule_objects.Add(null);
		}

		for (int i = 0; i < myLSystemScript.molecule_names.Count; i++)
		{
			EditorGUILayout.BeginHorizontal();
			myLSystemScript.molecule_names[i]   = EditorGUILayout.TextField(myLSystemScript.molecule_names[i]);
			myLSystemScript.molecule_objects[i] = EditorGUILayout.ObjectField(myLSystemScript.molecule_objects[i], typeof(GameObject), false) as GameObject;
			EditorGUILayout.EndHorizontal();
		}
	}
}
