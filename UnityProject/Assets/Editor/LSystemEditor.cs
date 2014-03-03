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


	private tabType lastFocusedTab = tabType.AlphabetTab;
	private LSystem lSystem;
	
	private int    symbolTypeindex = 0;
	private string symbolName = "";

	private int structureIndex = 0;

	private Vector3 bindingPosition = Vector3.zero;
	private Vector3 bindingRotation = Vector3.zero;

	private string communicationIdentifier = "";

	private bool addNewLetter = false;

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

		EditorGUILayout.Separator ();
		EditorGUILayout.LabelField("Existing letters");

		//foreach () ...

		GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(1)});


		addNewLetter = EditorGUILayout.Toggle("Add new letter ", addNewLetter);

		if (addNewLetter)
		{
			symbolTypeindex = EditorGUILayout.Popup (symbolTypeindex, GlobalLists.symbolTypes);

			switch(symbolTypeindex)
			{
			case 0:
				EditorGUILayout.LabelField("Chcem pridat ISymbol");

				symbolName = EditorGUILayout.TextField("Symbol name : ", symbolName);

				break;
			case 1:
				EditorGUILayout.LabelField("Chcem pridat StructureSymbol");

				symbolName    = EditorGUILayout.TextField("Symbol name : ", symbolName);
				structureIndex = EditorGUILayout.Popup ("Structure type : ", structureIndex, GlobalLists.monomerTypes);

				break;
			case 2:
				EditorGUILayout.LabelField("Chcem pridat BindingSymbol");

				symbolName = EditorGUILayout.TextField("Symbol name : ", symbolName);
				bindingPosition = EditorGUILayout.Vector3Field("Binding position : ", bindingPosition);
				bindingRotation = EditorGUILayout.Vector3Field("Binding orientation : ", bindingRotation);

				break;
			case 3:
				EditorGUILayout.LabelField("Chcem pridat CommunicationSymbol");

				communicationIdentifier = EditorGUILayout.TextField("Process identifier : ", communicationIdentifier);

				break;
			}
			
			if(GUILayout.Button("Add letter"))
			{
				ISymbol newSymbol = ScriptableObject.CreateInstance<ISymbol>();
				switch(symbolTypeindex)
				{
				case 0:
					newSymbol = ScriptableObject.CreateInstance<ISymbol>();
					newSymbol.name = symbolName;
					break;
				case 1:
					newSymbol = ScriptableObject.CreateInstance<StructureSymbol>();new StructureSymbol(symbolName, GlobalLists.monomerTypes[structureIndex]);
					break;
				case 2:
					newSymbol = new BindingSymbol(symbolName, bindingPosition, bindingRotation);
					break;
				case 3:
					newSymbol = new CommunicationSymbol(symbolName, communicationIdentifier);
					break;
				}

				lSystem.alphabet.Add(newSymbol);
			}
		}
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
