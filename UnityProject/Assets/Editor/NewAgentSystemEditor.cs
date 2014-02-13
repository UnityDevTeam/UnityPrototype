﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(NewAgentSystem))]
public class NewAgentSystemEditor : Editor
{
	private string[] allOptions = {"testAgent", "testAgent2"};
	private int index = 0;

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		NewAgentSystem myAgentSystem = (NewAgentSystem)target;
		
		List<string> options = new List<string> (allOptions);

		EditorGUILayout.Separator ();
		EditorGUILayout.LabelField("Defined agent types : ");
		for (int i = 0; i < myAgentSystem.transform.childCount; i++)
		{
			GameObject agentType = myAgentSystem.transform.GetChild(i).gameObject;
			options.Remove(agentType.name);
			
			EditorGUILayout.LabelField(agentType.name, GUILayout.Width(100));
			
			EditorGUILayout.BeginHorizontal ();
			
			GameObject previewObj = Resources.Load(agentType.name) as GameObject;
			GUILayout.Label(AssetPreview.GetAssetPreview(previewObj));
			
			EditorGUILayout.BeginVertical();

			AgentType agentTypeScript = agentType.GetComponent<AgentType>();

			agentTypeScript.isConstant = EditorGUILayout.Toggle("Is density constant ", agentTypeScript.isConstant);

			if(agentTypeScript.isConstant)
			{
				agentTypeScript.densityConstant = EditorGUILayout.Slider( agentTypeScript.densityConstant, AgentType.minDensity, AgentType.maxDensity );
			}
			else
			{
				agentTypeScript.densityFunction = EditorGUILayout.CurveField(agentTypeScript.densityFunction, Color.green, new Rect(0.0f, 0.0f, 10.0f, 0.1f), GUILayout.Height(80));
			}

			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal ();

			if (GUILayout.Button ("Remove agent types"))
			{
				DestroyImmediate(agentType);
			}
		}

		if (options.Count != 0)
		{
			EditorGUILayout.Separator ();
			EditorGUILayout.LabelField("Add new agent types : ");

			EditorGUILayout.BeginHorizontal ();
			
			index = EditorGUILayout.Popup (index, options.ToArray ());
			if (GUILayout.Button ("Add agent type"))
			{
				myAgentSystem.addAgentType (options [index]);

			}
			
			EditorGUILayout.EndHorizontal ();
		}

		EditorGUILayout.Separator ();
		float agentSpeed = RandomMove.minVelocity;
		agentSpeed = EditorGUILayout.Slider ("Agents speed : ", agentSpeed, 5.0f, 40.0f);
		RandomMove.minVelocity = agentSpeed;
		RandomMove.maxVelocity = agentSpeed + 10.0f;
	}
}
