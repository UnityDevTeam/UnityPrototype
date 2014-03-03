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
		EditorGUILayout.LabelField("Existing letters : ");

		int toDelete = -1;

		for (int i = 0; i < lSystem.alphabet.Count; i++)
		{
			ISymbol symbol = lSystem.alphabet[i];

			EditorGUILayout.BeginHorizontal ();

			if(symbol.GetType() == typeof(ISymbol))
			{
				EditorGUILayout.LabelField("ISymbol"                           , GUILayout.MaxWidth(140));
				EditorGUILayout.LabelField("[" + lSystem.alphabet[i].name + "]", GUILayout.MaxWidth(30));
				EditorGUILayout.LabelField(""                                  , GUILayout.MaxWidth(180));
				EditorGUILayout.LabelField(""                                  , GUILayout.MaxWidth(180));
			}
			else if(symbol.GetType() == typeof(EndSymbol))
			{
				EditorGUILayout.LabelField("EndSymbol"                         , GUILayout.MaxWidth(140));
				EditorGUILayout.LabelField("[" + lSystem.alphabet[i].name + "]", GUILayout.MaxWidth(30));
				EditorGUILayout.LabelField(""                                  , GUILayout.MaxWidth(180));
				EditorGUILayout.LabelField(""                                  , GUILayout.MaxWidth(180));
			}
			else if(symbol.GetType() == typeof(StructureSymbol))
			{
				EditorGUILayout.LabelField("StructureSymbol"                                         , GUILayout.MaxWidth(140));
				EditorGUILayout.LabelField("[" + lSystem.alphabet[i].name + "]"                      , GUILayout.MaxWidth(30));
				EditorGUILayout.LabelField(((StructureSymbol)lSystem.alphabet[i]).structurePrefabName, GUILayout.MaxWidth(180));
				EditorGUILayout.LabelField(""                                                        , GUILayout.MaxWidth(180));
			}
			else if(symbol.GetType() == typeof(BindingSymbol))
			{
				EditorGUILayout.LabelField("BindingSymbol"                                                                  , GUILayout.MaxWidth(140));
				EditorGUILayout.LabelField("[" + lSystem.alphabet[i].name + "]"                                             , GUILayout.MaxWidth(30));
				EditorGUILayout.LabelField("[position - " + ((BindingSymbol)lSystem.alphabet[i]).bindingPosition + "]"      , GUILayout.MaxWidth(180));
				EditorGUILayout.LabelField("[orientation - " + ((BindingSymbol)lSystem.alphabet[i]).bindingOrientation + "]", GUILayout.MaxWidth(180));
			}
			else if(symbol.GetType() == typeof(CommunicationSymbol))
			{
				EditorGUILayout.LabelField("CommunicationSymbol"                                                   , GUILayout.MaxWidth(140));
				EditorGUILayout.LabelField("[" + lSystem.alphabet[i].name + "]"                                    , GUILayout.MaxWidth(30));
				EditorGUILayout.LabelField("[process - " + ((CommunicationSymbol)lSystem.alphabet[i]).process + "]", GUILayout.MaxWidth(180));
				EditorGUILayout.LabelField(""                                                                      , GUILayout.MaxWidth(180));
			}


			
			if(GUILayout.Button("D"))
			{
				toDelete = i;
			}

			EditorGUILayout.EndHorizontal ();
		}

		if (toDelete > -1)
		{
			lSystem.alphabet.RemoveAt(toDelete);
		}

		GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(1)});


		if (!addNewLetter)
		{
			if (GUILayout.Button ("Add letter"))
			{
				addNewLetter = true;
			}
		}

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

				symbolName = EditorGUILayout.TextField("Symbol name : ", symbolName);
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
					newSymbol = ScriptableObject.CreateInstance<StructureSymbol>();
					((StructureSymbol)newSymbol).init(symbolName, GlobalLists.monomerTypes[structureIndex], null);
					break;
				case 2:
					// branching ? 
					newSymbol = ScriptableObject.CreateInstance<BindingSymbol> ();
					((BindingSymbol)newSymbol).init(symbolName, bindingPosition, bindingRotation, false);
					break;
				case 3:
					newSymbol = ScriptableObject.CreateInstance<CommunicationSymbol> ();
					((CommunicationSymbol)newSymbol).init(symbolName, communicationIdentifier);
					break;
				}

				lSystem.alphabet.Add(newSymbol);
				addNewLetter = false;
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
