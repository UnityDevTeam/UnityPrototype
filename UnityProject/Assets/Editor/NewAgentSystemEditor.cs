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
				DestroyImmediate(agentType);
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
}
