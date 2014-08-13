using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class SimulationEditorWindow : EditorWindow
{
	LSystem lsystem;

	public void Init()
	{
		lsystem = (LSystem)FindObjectOfType (typeof(LSystem));
	}

	void OnGUI()
	{
		EditorGUILayout.Separator ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField(new GUIContent("Polymer size : ", "Setting the monomers count of the resulting polymer.(0-1000)"), GUILayout.MaxWidth(100));
		lsystem.monomerCountingStop = EditorGUILayout.IntSlider ("",   lsystem.monomerCountingStop, 0, 1000, GUILayout.MaxWidth(300));
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField(new GUIContent("Time delta : ", "Setting the time delta of the simulation."), GUILayout.MaxWidth(100));
		GlobalVariables.timeDelta = EditorGUILayout.Slider ("", GlobalVariables.timeDelta, 0.02f, 3.0f, GUILayout.MaxWidth(300));
		EditorGUILayout.EndHorizontal ();

		GlobalVariables.monomerSpeed = GlobalVariables.timeDelta * 50.0f;
		if (GlobalVariables.monomerSpeed > 30.0f) GlobalVariables.monomerSpeed = 30.0f;
	}
}
