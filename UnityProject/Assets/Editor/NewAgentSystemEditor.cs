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

			EditorGUILayout.BeginHorizontal ();

			EditorGUILayout.LabelField(selAgentsNames[i], GUILayout.Width(100));
			myAgentSystem.agentTypesDensity[i] = EditorGUILayout.Slider( myAgentSystem.agentTypesDensity[i], 0.00001f, 0.0001f );

			if (GUILayout.Button ("Remove agent types"))
			{
				myAgentSystem.agentTypesName.RemoveAt(i);
				myAgentSystem.agentTypesDensity.RemoveAt(i);
			}

			EditorGUILayout.EndHorizontal ();
		}

		if (options.Count != 0)
		{
			EditorGUILayout.BeginHorizontal ();

			index = EditorGUILayout.Popup (index, options.ToArray ());
			if (GUILayout.Button ("Add agent type"))
			{
				myAgentSystem.agentTypesName.Add (options [index]);
				myAgentSystem.agentTypesDensity.Add(0.00001f);
			}

			EditorGUILayout.EndHorizontal ();
		}
	}
}
