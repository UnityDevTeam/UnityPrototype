using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(LSystem))]
public class LSystemEditor : Editor
{
	enum tabType
	{
		AlphabetTab,
		AxiomTab,
		RulesTab,
		SimulationTab
	};

	private tabType lastFocusedTab = tabType.SimulationTab;
	private LSystem lSystem;

	void OnEnable ()
	{
		lSystem = (LSystem)target;
	}

	public override void OnInspectorGUI()
	{
		GUILayout.BeginHorizontal ();

		GUI.enabled = lastFocusedTab != tabType.AlphabetTab;
		if(GUILayout.Button("Alphabet"))
		{
			lastFocusedTab = tabType.AlphabetTab;
		}

		GUI.enabled = lastFocusedTab != tabType.AxiomTab;
		if(GUILayout.Button("Axiom"))
		{
			lastFocusedTab = tabType.AxiomTab;
		}

		GUI.enabled = lastFocusedTab != tabType.RulesTab;
		if(GUILayout.Button("Rules"))
		{
			lastFocusedTab = tabType.RulesTab;
		}

		GUI.enabled = lastFocusedTab != tabType.SimulationTab;
		if(GUILayout.Button("Simulation"))
		{
			lastFocusedTab = tabType.SimulationTab;
		}

		GUILayout.EndHorizontal ();
		GUI.enabled = true;

		switch (lastFocusedTab)
		{
		case tabType.AlphabetTab:
			showAlphabetMenu();
			break;
		case tabType.AxiomTab:
			showAxiomMenu();
			break;
		case tabType.RulesTab:
			showRulesMenu();
			break;
		case tabType.SimulationTab:
			showSimulationMenu();
			break;
		default:
			break;
		}
	}

	private void showAlphabetMenu()
	{
		EditorGUILayout.LabelField("LSystem alphabet", EditorStyles.boldLabel);
	}
	
	private void showAxiomMenu()
	{
		EditorGUILayout.LabelField("LSystem axiom", EditorStyles.boldLabel);
	}

	private void showRulesMenu()
	{
		EditorGUILayout.LabelField("LSystem rules", EditorStyles.boldLabel);
	}

	private void showSimulationMenu()
	{
		EditorGUILayout.LabelField("Simulation properties", EditorStyles.boldLabel);

		lSystem.monomerCountingStop = EditorGUILayout.IntSlider ("Monomer count : ",   lSystem.monomerCountingStop, 0, 1000);
		lSystem.polymerExample      = EditorGUILayout.IntSlider ("Polymer Example : ", lSystem.polymerExample,      0, 2);

		LSystem.timeDelta = EditorGUILayout.Slider ("Time delta: ", LSystem.timeDelta, 0.02f, 3.0f);
		
		RandomMove.speed = LSystem.timeDelta * 50.0f;
		if (RandomMove.speed > 30.0f) RandomMove.speed = 30.0f;
		
		EditorGUILayout.LabelField("Speed: " + RandomMove.speed);
	}
}
