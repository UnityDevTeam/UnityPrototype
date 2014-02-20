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

		EditorGUILayout.LabelField("Time delta: ");
		LSystem.timeDelta = EditorGUILayout.Slider (LSystem.timeDelta, 0.02f, 3.0f);

		RandomMove.speed = LSystem.timeDelta * 50.0f;

		if (RandomMove.speed > 30.0f)
			RandomMove.speed = 30.0f;

		EditorGUILayout.LabelField("Speed: " + RandomMove.speed);
	}
}
