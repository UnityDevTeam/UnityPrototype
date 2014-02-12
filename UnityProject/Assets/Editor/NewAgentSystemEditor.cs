using UnityEngine;
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
			
			agentTypeScript.densityConstant = EditorGUILayout.Slider( agentTypeScript.densityConstant, AgentType.minDensity, AgentType.maxDensity );

			GUILayoutOption[] curveOptions = { GUILayout.Width(100), GUILayout.Height(100) };
			agentTypeScript.densityFunction = EditorGUILayout.CurveField(agentTypeScript.densityFunction, Color.green, new Rect(0.0f, 0.0f, 10.0f, 0.1f), curveOptions);
			
			if (GUILayout.Button ("Remove agent types"))
			{
				//
			}
			
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal ();
		}
		
		if (options.Count != 0)
		{
			EditorGUILayout.BeginHorizontal ();
			
			index = EditorGUILayout.Popup (index, options.ToArray ());
			if (GUILayout.Button ("Add agent type"))
			{
				myAgentSystem.addAgentType (options [index]);

			}
			
			EditorGUILayout.EndHorizontal ();
		}
	}

	/*
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		NewAgentSystem myAgentSystem = (NewAgentSystem)target;

		List<string> selAgentsNames = myAgentSystem.agentTypesName;

		List<string> options = new List<string> (allOptions);

		EditorGUILayout.LabelField("Defined agent types : ");
		for (int i = 0; i < selAgentsNames.Count; i++)
		{
			options.Remove(selAgentsNames[i]);

			EditorGUILayout.LabelField(selAgentsNames[i], GUILayout.Width(100));

			EditorGUILayout.BeginHorizontal ();

			GameObject previewObj = Resources.Load(selAgentsNames[i]) as GameObject;
			GUILayout.Label(AssetPreview.GetAssetPreview(previewObj));

			EditorGUILayout.BeginVertical();

			myAgentSystem.agentTypesDensityConstant[i] = EditorGUILayout.Slider( myAgentSystem.agentTypesDensityConstant[i], NewAgentSystem.minDensity, NewAgentSystem.maxDensity );
			GUILayoutOption[] curveOptions = { GUILayout.Width(100), GUILayout.Height(100) };

			myAgentSystem.ac = EditorGUILayout.CurveField(myAgentSystem.ac, Color.green, new Rect(0.0f, 0.0f, 10.0f, 0.1f), curveOptions);
			//myAgentSystem.agentTypesDensityCurve[i] = EditorGUILayout.CurveField(myAgentSystem.agentTypesDensityCurve[i], Color.green, new Rect(0.0f, 0.0f, 10.0f, 0.1f), curveOptions);

			if (GUILayout.Button ("Remove agent types"))
			{
				myAgentSystem.agentTypesName.RemoveAt(i);
				myAgentSystem.agentTypesDensityConstant.RemoveAt(i);
				myAgentSystem.agentTypesDensityCurve.RemoveAt(i);
			}

			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal ();
		}

		if (options.Count != 0)
		{
			EditorGUILayout.BeginHorizontal ();

			index = EditorGUILayout.Popup (index, options.ToArray ());
			if (GUILayout.Button ("Add agent type"))
			{
				myAgentSystem.agentTypesName.Add (options [index]);
				myAgentSystem.agentTypesDensityConstant.Add(0.00001f);
				//myAgentSystem.agentTypesDensityCurve.Add(AnimationCurve.Linear(0.0f, NewAgentSystem.minDensity, 10.0f, NewAgentSystem.minDensity));
				myAgentSystem.ac = AnimationCurve.Linear(0.0f, NewAgentSystem.minDensity, 10.0f, NewAgentSystem.minDensity);
			}

			EditorGUILayout.EndHorizontal ();
		}
	}
	*/
}
