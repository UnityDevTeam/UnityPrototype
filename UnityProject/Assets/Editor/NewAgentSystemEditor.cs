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

			myAgentSystem.agentTypesDensityCurve[i] = EditorGUILayout.CurveField(myAgentSystem.agentTypesDensityCurve[i], Color.green, new Rect(0.0f, 0.0f, 10.0f, 0.1f), curveOptions);

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
				myAgentSystem.agentTypesDensityCurve.Add(new AnimationCurve());
			}

			EditorGUILayout.EndHorizontal ();
		}
	}
}
