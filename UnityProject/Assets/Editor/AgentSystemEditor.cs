using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(AgentSystem))]
public class AgentSystemEditor : Editor
{
	private int index = 0;

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		AgentSystem agentSystem = (AgentSystem)target;
		
		List<string> options = new List<string> (GlobalVariables.monomerTypes);
		
		EditorGUILayout.Separator ();
		EditorGUILayout.LabelField("System of densities", EditorStyles.boldLabel, GUILayout.MaxWidth(150));
		
		for (int i = 0; i < agentSystem.transform.childCount; i++)
		{
			GameObject agentType = agentSystem.transform.GetChild(i).gameObject;
			options.Remove(agentType.name);
			
			EditorGUILayout.LabelField(agentType.name, GUILayout.Width(100));
			
			EditorGUILayout.BeginHorizontal ();
			
			GameObject previewObj = Resources.Load(agentType.name) as GameObject;
			GUILayout.Label(AssetPreview.GetAssetPreview(previewObj), GUILayout.MaxWidth(120));
			
			EditorGUILayout.BeginVertical();
			
			AgentType agentTypeScript = agentType.GetComponent<AgentType>();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(new GUIContent("Constant :", "Wether the density of the agents are constant during the simulation or not."), GUILayout.Width(65));
			agentTypeScript.isConstant = EditorGUILayout.Toggle("", agentTypeScript.isConstant, GUILayout.MaxWidth(20));
			
			EditorGUILayout.LabelField( "", GUILayout.Width(55));
			
			if (GUILayout.Button ("Remove", GUILayout.MaxWidth(100)))
			{
				DestroyImmediate(agentType);
			}
			
			EditorGUILayout.EndHorizontal();
			
			if(agentTypeScript.isConstant)
			{
				agentTypeScript.densityConstant = EditorGUILayout.Slider( agentTypeScript.densityConstant, AgentType.minDensity, AgentType.maxDensity, GUILayout.MaxWidth(250) );
			}
			else
			{
				agentTypeScript.densityFunction = EditorGUILayout.CurveField(agentTypeScript.densityFunction, Color.green, new Rect(0.0f, 0.0f, 10.0f, 0.1f), new GUILayoutOption[]{GUILayout.Height(100), GUILayout.MaxWidth(250)});
			}
			
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal ();
		}
		
		GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(1)});
		
		if (options.Count != 0)
		{
			EditorGUILayout.Separator ();
			
			EditorGUILayout.BeginHorizontal ();
			
			index = EditorGUILayout.Popup (index, options.ToArray (), GUILayout.MaxWidth(270));
			if (GUILayout.Button (new GUIContent("Add", "Add new type of molecule to simulation."), GUILayout.MaxWidth(100)))
			{
				agentSystem.addAgentType (options [index]);
				
			}
			
			EditorGUILayout.EndHorizontal ();
		}

		GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(1)});

		if (GUILayout.Button("Open Agent system Window", GUILayout.Width(255)))
		{   
			LSystemEditorWindow lsWindow = (LSystemEditorWindow) EditorWindow.GetWindow(typeof(LSystemEditorWindow), false, "Lsystem");
			lsWindow.Init();
			
			AgentSystemEditorWindow asWindow = (AgentSystemEditorWindow) EditorWindow.GetWindow<AgentSystemEditorWindow>("Agent system", false, new Type[] {typeof(LSystemEditorWindow)} );
			asWindow.Init();
			
			SimulationEditorWindow simWindow = (SimulationEditorWindow) EditorWindow.GetWindow<SimulationEditorWindow>("Simulation", false, new Type[] {typeof(LSystemEditorWindow), typeof(AgentSystemEditorWindow)} );
			simWindow.Init();
			
			asWindow.Focus();
		}
	}
}
