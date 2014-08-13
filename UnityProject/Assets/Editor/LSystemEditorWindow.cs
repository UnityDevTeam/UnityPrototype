using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class LSystemEditorWindow : EditorWindow
{
	enum tabType
	{
		AlphabetTab,
		AxiomTab,
		RulesTab,
		SimulationTab
	};
	
	tabType lastFocusedTab = tabType.AlphabetTab;

	LSystem lsystem;

	bool addNewLetter = false;

	private int    symbolTypeindex = 0;
	private string symbolName = "";
	
	private int structureIndex = 0;
	
	private Vector3 bindingPosition   = Vector3.zero;
	private Vector3 bindingRotation   = Vector3.zero;
	private Vector3 bindingAlteration = Vector3.zero;
	private bool isBranching = false;
	
	private string communicationIdentifier = "";
	private Vector3 communicationPosition    = Vector3.zero;
	private Vector3 communicationOrientation = Vector3.zero;
	private int communicationResultTypeIndex = 0;
	private AnimationCurve communicationProbability = AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f);

	private bool changeAxiom = false;
	private int axiomTypeIndex = 0;

	private Rule newRule = null;
	private int changeRuleIndex = -1;
	private int newRuleSuccesorIndex = -1;
	private int updateRuleSuccessorIndex = -1;
	
	public void Init()
	{
		lsystem = (LSystem)FindObjectOfType (typeof(LSystem));
	}

	void OnGUI()
	{
		EditorGUILayout.Separator ();

		if (lsystem == null)
						return;

		GUILayout.BeginHorizontal ();
		lsystem.exampleIndex = EditorGUILayout.Popup ("Preload example : ", lsystem.exampleIndex, GlobalVariables.examples, GUILayout.MaxWidth(250));

		if(GUILayout.Button("Load", GUILayout.MaxWidth(100)))
		{
			lsystem.loadExample();
		}
		GUILayout.EndHorizontal ();

		EditorGUILayout.Separator ();

		GUILayout.BeginHorizontal ();
		
		GUI.enabled = lastFocusedTab != tabType.AlphabetTab;
		if(GUILayout.Button("Alphabet", GUILayout.MaxWidth(100)))
		{
			lastFocusedTab = tabType.AlphabetTab;
		}
		
		GUI.enabled = lastFocusedTab != tabType.AxiomTab;
		if(GUILayout.Button("Axiom", GUILayout.MaxWidth(100)))
		{
			lastFocusedTab = tabType.AxiomTab;
		}
		
		GUI.enabled = lastFocusedTab != tabType.RulesTab;
		if(GUILayout.Button("Rules", GUILayout.MaxWidth(100)))
		{
			lastFocusedTab = tabType.RulesTab;
		}
		
		GUILayout.EndHorizontal ();
		GUI.enabled = true;

		GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(1)});

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
		default:
			break;
		}
	}

	private void showAlphabetMenu()
	{
		EditorGUILayout.LabelField("LSystem alphabet", EditorStyles.boldLabel);
		
		EditorGUILayout.Separator ();
		EditorGUILayout.LabelField("Existing symbols : ");
		
		int toDelete = -1;

		string typeTt    = "Type of symbol.";
		string nameTt    = "Symbol character.";
		string prefabTt  = "Name of the prefab defining the structure.";
		string posTt     = "Binding site position relative to the structure.";
		string oriTt     = "Binding site orientation relative to the structure.";
		string oriAltTt  = "Orientation alterations, ranging from +oriAlt to -oriAlt.";
		string processTt = "Process identification";
		
		for (int i = 0; i < lsystem.alphabet.Count; i++)
		{
			ISymbol symbol = lsystem.alphabet[i];
			
			EditorGUILayout.BeginHorizontal ();
			
			if(symbol.GetType() == typeof(ISymbol))
			{
				ISymbol tmp = (ISymbol)lsystem.alphabet[i];

				EditorGUILayout.LabelField(new GUIContent(tmp.GetType().ToString(), typeTt), GUILayout.MaxWidth(140));
				EditorGUILayout.LabelField(new GUIContent("[" + tmp.name + "]",     nameTt), GUILayout.MaxWidth(30));

				EditorGUILayout.LabelField("", GUILayout.MaxWidth(110));
				EditorGUILayout.LabelField("", GUILayout.MaxWidth(110));
				EditorGUILayout.LabelField("", GUILayout.MaxWidth(110));
			}
			else if(symbol.GetType() == typeof(EndSymbol))
			{
				EndSymbol tmp = (EndSymbol)lsystem.alphabet[i];

				EditorGUILayout.LabelField(new GUIContent(tmp.GetType().ToString(), typeTt), GUILayout.MaxWidth(140));
				EditorGUILayout.LabelField(new GUIContent("[" + tmp.name + "]",     nameTt), GUILayout.MaxWidth(30));

				EditorGUILayout.LabelField("", GUILayout.MaxWidth(110));
				EditorGUILayout.LabelField("", GUILayout.MaxWidth(110));
				EditorGUILayout.LabelField("", GUILayout.MaxWidth(110));
			}
			else if(symbol.GetType() == typeof(StructureSymbol))
			{
				StructureSymbol tmp = (StructureSymbol)lsystem.alphabet[i];

				EditorGUILayout.LabelField(new GUIContent(tmp.GetType().ToString(),             typeTt),   GUILayout.MaxWidth(140));
				EditorGUILayout.LabelField(new GUIContent("[" + tmp.name + "]",                 nameTt),   GUILayout.MaxWidth(30));
				EditorGUILayout.LabelField(new GUIContent("[" + tmp.structurePrefabName + "]",  prefabTt), GUILayout.MaxWidth(110));

				EditorGUILayout.LabelField("", GUILayout.MaxWidth(110));
				EditorGUILayout.LabelField("", GUILayout.MaxWidth(110));
			}
			else if(symbol.GetType() == typeof(BindingSymbol))
			{
				BindingSymbol tmp = (BindingSymbol)lsystem.alphabet[i];

				EditorGUILayout.LabelField(new GUIContent(tmp.GetType().ToString(),           typeTt),   GUILayout.MaxWidth(140));
				EditorGUILayout.LabelField(new GUIContent("[" + tmp.name + "]",               nameTt),   GUILayout.MaxWidth(30));
				EditorGUILayout.LabelField(new GUIContent("[" + tmp.bindingPosition + "]",    posTt),    GUILayout.MaxWidth(110));
				EditorGUILayout.LabelField(new GUIContent("[" + tmp.bindingOrientation + "]", oriTt),    GUILayout.MaxWidth(110));
				EditorGUILayout.LabelField(new GUIContent("[" + tmp.orientAlteration + "]",   oriAltTt), GUILayout.MaxWidth(110));
			}
			else if(symbol.GetType() == typeof(CommunicationSymbol))
			{
				CommunicationSymbol tmp = (CommunicationSymbol)lsystem.alphabet[i];

				EditorGUILayout.LabelField(new GUIContent(tmp.GetType().ToString(), typeTt),    GUILayout.MaxWidth(140));
				EditorGUILayout.LabelField(new GUIContent("[" + tmp.name + "]",     nameTt),    GUILayout.MaxWidth(30));
				EditorGUILayout.LabelField(new GUIContent("[" + tmp.process + "]",  processTt), GUILayout.MaxWidth(110));

				EditorGUILayout.LabelField("", GUILayout.MaxWidth(110));
				EditorGUILayout.LabelField("", GUILayout.MaxWidth(110));
			}
			
			if(GUILayout.Button("Delete symbol", GUILayout.MaxWidth(100)))
			{
				toDelete = i;
			}
			
			EditorGUILayout.EndHorizontal ();
		}
		
		if (toDelete > -1)
		{
			lsystem.alphabet.RemoveAt(toDelete);
		}
		
		GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(1)});
		
		if (!addNewLetter)
		{
			if (GUILayout.Button ("Add letter", GUILayout.MaxWidth(150)))
			{
				addNewLetter = true;
			}
		}
		
		if (addNewLetter)
		{
			symbolTypeindex = EditorGUILayout.Popup ("Type of the new symbol ", symbolTypeindex, GlobalVariables.symbolTypes, GUILayout.MaxWidth(300));
			
			switch(symbolTypeindex)
			{
			case 0:
				symbolName = EditorGUILayout.TextField("Symbol name : ", symbolName, GUILayout.MaxWidth(300));
				
				break;
			case 1:
				symbolName     = EditorGUILayout.TextField("Symbol name : ", symbolName,                               GUILayout.MaxWidth(300));
				structureIndex = EditorGUILayout.Popup ("Structure type : ", structureIndex, GlobalVariables.monomerTypes, GUILayout.MaxWidth(300));
				
				break;
			case 2:
				symbolName        = EditorGUILayout.TextField("Symbol name : ",               symbolName,        GUILayout.MaxWidth(300));
				bindingPosition   = EditorGUILayout.Vector3Field("Binding position : ",       bindingPosition,   GUILayout.MaxWidth(300));
				bindingRotation   = EditorGUILayout.Vector3Field("Binding orientation : ",    bindingRotation,   GUILayout.MaxWidth(300));
				bindingAlteration = EditorGUILayout.Vector3Field("Binding orientationAlt : ", bindingAlteration, GUILayout.MaxWidth(300));
				isBranching       = EditorGUILayout.Toggle("Is branching : ",                 isBranching,       GUILayout.MaxWidth(300));
				break;
			case 3:
				symbolName = EditorGUILayout.TextField("Symbol name : ", symbolName,                                                            GUILayout.MaxWidth(300));
				communicationIdentifier       = EditorGUILayout.TextField("Process identifier : ", communicationIdentifier,                     GUILayout.MaxWidth(300));
				communicationPosition         = EditorGUILayout.Vector3Field("Position : ", communicationPosition,                              GUILayout.MaxWidth(300));
				communicationOrientation      = EditorGUILayout.Vector3Field("Orientation : ", communicationOrientation,                        GUILayout.MaxWidth(300));
				communicationResultTypeIndex  = EditorGUILayout.Popup("Result type : ", communicationResultTypeIndex, GlobalVariables.monomerTypes, GUILayout.MaxWidth(300));
				communicationProbability      = EditorGUILayout.CurveField("Probability function : ", communicationProbability,                 GUILayout.MaxWidth(300));
				break;
			case 4:
				symbolName = EditorGUILayout.TextField("Symbol name : ", symbolName, GUILayout.MaxWidth(300));
				break;
			}

			EditorGUILayout.BeginHorizontal ();

			if(GUILayout.Button("Add letter", GUILayout.MaxWidth(150)))
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
					((StructureSymbol)newSymbol).init(symbolName, GlobalVariables.monomerTypes[structureIndex], null);
					break;
				case 2:
					newSymbol = ScriptableObject.CreateInstance<BindingSymbol> ();
					((BindingSymbol)newSymbol).init(symbolName, bindingPosition, bindingRotation, bindingAlteration, isBranching);
					break;
				case 3:
					newSymbol = ScriptableObject.CreateInstance<CommunicationSymbol> ();
					((CommunicationSymbol)newSymbol).init(symbolName, communicationIdentifier, communicationPosition, Quaternion.Euler(communicationOrientation), 0, GlobalVariables.monomerTypes[communicationResultTypeIndex], null, communicationProbability);
					break;
				case 4:
					newSymbol = ScriptableObject.CreateInstance<EndSymbol> ();
					((EndSymbol)newSymbol).init(symbolName);
					break;
				}
				
				lsystem.alphabet.Add(newSymbol);
				addNewLetter = false;
			}

			if(GUILayout.Button("Cancel", GUILayout.MaxWidth(145)))
			{
				addNewLetter = false;
			}

			EditorGUILayout.EndHorizontal ();
		}
	}

	private void showAxiomMenu()
	{
		EditorGUILayout.LabelField("Current axiom :", EditorStyles.boldLabel);
		EditorGUILayout.Separator ();
		
		ISymbol axiom = lsystem.axiom;
		
		if (axiom != null)
		{
			if(axiom.GetType() == typeof(ISymbol))
			{
				EditorGUILayout.LabelField("ISymbol");
				
				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Name : ", GUILayout.MaxWidth(120)); EditorGUILayout.LabelField(lsystem.axiom.name);
				GUILayout.EndHorizontal();
			}
			else if(axiom.GetType() == typeof(EndSymbol))
			{
				EditorGUILayout.LabelField("EndSymbol");
				
				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Name : ", GUILayout.MaxWidth(120)); EditorGUILayout.LabelField(lsystem.axiom.name);
				GUILayout.EndHorizontal();
			}
			else if(axiom.GetType() == typeof(StructureSymbol))
			{
				EditorGUILayout.LabelField("StructureSymbol");
				
				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Name : ", GUILayout.MaxWidth(120)); EditorGUILayout.LabelField(lsystem.axiom.name);
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Prefab : ", GUILayout.MaxWidth(120)); EditorGUILayout.LabelField(((StructureSymbol)lsystem.axiom).structurePrefabName);
				GUILayout.EndHorizontal();
			}
			else if(axiom.GetType() == typeof(BindingSymbol))
			{
				EditorGUILayout.LabelField("BindingSymbol");
				
				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Name : ", GUILayout.MaxWidth(120)); EditorGUILayout.LabelField(lsystem.axiom.name);
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Binding position : ", GUILayout.MaxWidth(120)); EditorGUILayout.LabelField(((BindingSymbol)lsystem.axiom).bindingPosition.ToString());
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Binding orientation : ", GUILayout.MaxWidth(120)); EditorGUILayout.LabelField(((BindingSymbol)lsystem.axiom).bindingOrientation.ToString());
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Binding alteration : ", GUILayout.MaxWidth(120)); EditorGUILayout.LabelField(((BindingSymbol)lsystem.axiom).orientAlteration.ToString());
				GUILayout.EndHorizontal();
			}
			else if(axiom.GetType() == typeof(CommunicationSymbol))
			{
				EditorGUILayout.LabelField("CommunicationSymbol");
				
				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Name : ", GUILayout.MaxWidth(120)); EditorGUILayout.LabelField(lsystem.axiom.name);
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Process : ", GUILayout.MaxWidth(120)); EditorGUILayout.LabelField(((CommunicationSymbol)lsystem.axiom).process);
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Position : ", GUILayout.MaxWidth(120)); EditorGUILayout.LabelField(((CommunicationSymbol)lsystem.axiom).position.ToString());
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Orientation : ", GUILayout.MaxWidth(120)); EditorGUILayout.LabelField(((CommunicationSymbol)lsystem.axiom).orientation.eulerAngles.ToString());
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Result type : ", GUILayout.MaxWidth(120)); EditorGUILayout.LabelField(((CommunicationSymbol)lsystem.axiom).resultType);
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Process probability : ", GUILayout.MaxWidth(120));EditorGUILayout.CurveField("", ((CommunicationSymbol)lsystem.axiom).probability ,GUILayout.MaxWidth(180));
				GUILayout.EndHorizontal();
			}
		}
		
		GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(1)});
		
		if (!changeAxiom)
		{
			if (GUILayout.Button ("Change axiom", GUILayout.MaxWidth(150)))
			{
				changeAxiom = true;
			}
		}
		
		if(changeAxiom)
		{
			List<string> lSystemAlphabet = new List<string>();
			
			foreach(ISymbol symbol in lsystem.alphabet)
			{
				string symbolStr = "";
				if(symbol.GetType() == typeof(ISymbol))
				{
					symbolStr += "ISymbol(" + symbol.name + ")";
				}
				else if(symbol.GetType() == typeof(EndSymbol))
				{
					symbolStr += "EndSymbol(" + symbol.name + ")";
				}
				else if(symbol.GetType() == typeof(StructureSymbol))
				{
					symbolStr += "StructureSymbol(" + symbol.name + ")";
				}
				else if(symbol.GetType() == typeof(BindingSymbol))
				{
					symbolStr += "BindingSymbol(" + symbol.name + ")";
				}
				else if(symbol.GetType() == typeof(CommunicationSymbol))
				{
					symbolStr += "CommunicationSymbol(" + symbol.name + ", " + ((CommunicationSymbol)symbol).process + ")";
				}
				lSystemAlphabet.Add(symbolStr);
			}

			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Axiom : ", GUILayout.MaxWidth(120));
			axiomTypeIndex = EditorGUILayout.Popup ("", axiomTypeIndex, lSystemAlphabet.ToArray(), GUILayout.MaxWidth(180));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			if (GUILayout.Button ("Change", GUILayout.MaxWidth(180)))
			{
				ISymbol symbol = null;
				
				if(lsystem.alphabet[axiomTypeIndex].GetType() == typeof(ISymbol))
				{
					symbol = ScriptableObject.CreateInstance<ISymbol> ();
					symbol.init(lsystem.alphabet[axiomTypeIndex].name);
				}
				else if(lsystem.alphabet[axiomTypeIndex].GetType() == typeof(EndSymbol))
				{
					symbol = ScriptableObject.CreateInstance<EndSymbol> ();
					((EndSymbol)symbol).init((EndSymbol)lsystem.alphabet[axiomTypeIndex]);
				}
				else if(lsystem.alphabet[axiomTypeIndex].GetType() == typeof(StructureSymbol))
				{
					symbol = ScriptableObject.CreateInstance<StructureSymbol> ();
					((StructureSymbol)symbol).init((StructureSymbol)lsystem.alphabet[axiomTypeIndex]);
				}
				else if(lsystem.alphabet[axiomTypeIndex].GetType() == typeof(BindingSymbol))
				{
					symbol = ScriptableObject.CreateInstance<BindingSymbol> ();
					((BindingSymbol)symbol).init((BindingSymbol)lsystem.alphabet[axiomTypeIndex]);
				}
				else if(lsystem.alphabet[axiomTypeIndex].GetType() == typeof(CommunicationSymbol))
				{
					symbol = ScriptableObject.CreateInstance<CommunicationSymbol> ();
					((CommunicationSymbol)symbol).init((CommunicationSymbol)lsystem.alphabet[axiomTypeIndex]);
				}
				
				lsystem.axiom = symbol;
				changeAxiom = false;
			}

			if (GUILayout.Button ("Cancel", GUILayout.MaxWidth(120)))
			{
				changeAxiom = false;
			}
			GUILayout.EndHorizontal();
		}
	}

	private void showRulesMenu()
	{
		EditorGUILayout.LabelField("LSystem rules", EditorStyles.boldLabel);
		
		int toDeleteRule = -1;
		if(lsystem.rules != null)
		{
			for (int i = 0; i < lsystem.rules.getRulesCount(); i++)
			{
				Rule rule = lsystem.rules.getRule(i);
				
				GUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField(rule.predecessor.toShortString(), GUILayout.MaxWidth(40));
				
				string conditionString = "";
				
				conditionString += rule.condition.param.ToString();
				
				switch(rule.condition.operation)
				{
				case CommunicationCondition.CommOperation.equal:
					conditionString += "==";
					break;
				case CommunicationCondition.CommOperation.notEqual:
					conditionString += "!=";
					break;
				case CommunicationCondition.CommOperation.less:
					conditionString += "<";
					break;
				case CommunicationCondition.CommOperation.more:
					conditionString += ">";
					break;
				}

				if(rule.condition.param == CommunicationCondition.CommParameters.time)
				{
					conditionString += rule.condition.floatValue.ToString();
				}
				else
				{
					if(rule.condition.resultValue == null)
					{
						conditionString += "null";
					}
					else
					{
						conditionString += rule.condition.resultValue.name;
					}
				}
				
				EditorGUILayout.LabelField(": [" + conditionString + "]", GUILayout.MaxWidth(90));

				EditorGUILayout.LabelField("->", GUILayout.MaxWidth(20));
				
				string succesor = "";
				for(int j = 0; j < rule.successor.Count; j++)
				{
					succesor += rule.successor[j].toShortString();
				}
				EditorGUILayout.LabelField(succesor, GUILayout.MaxWidth(100));
				EditorGUILayout.LabelField("[ " + rule.probability * 100 + "% ]", GUILayout.MaxWidth(60));
				
				if(changeRuleIndex != -1)
					GUI.enabled = false;
				
				if(GUILayout.Button ("Change Rule", GUILayout.MaxWidth(100)))
				{
					changeRuleIndex = i;
				}
				
				if(GUILayout.Button ("Remove Rule", GUILayout.MaxWidth(100)))
				{
					toDeleteRule = i;
				}
				
				GUI.enabled = true;
				
				GUILayout.EndHorizontal ();
				
				if(changeRuleIndex == i)
				{
					GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(1)});
					string[] alphabetStr = lsystem.alphabetArray();
					
					GUILayout.BeginHorizontal();
					GUILayout.BeginVertical();

					int predecessorIndex = lsystem.alphabet.IndexOf(rule.predecessor);
					GUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Predecessor :", EditorStyles.boldLabel, GUILayout.MaxWidth(100));
					predecessorIndex = EditorGUILayout.Popup ("", predecessorIndex, alphabetStr, GUILayout.MaxWidth(220));

					if (GUILayout.Button ("Cancel", GUILayout.MaxWidth(100)))
					{
						if(predecessorIndex != -1)
						{
							lsystem.rules.setRule(i,rule);
							changeRuleIndex = -1;
						}
					}

					GUILayout.EndHorizontal();
					
					if(predecessorIndex != -1)
					{
						rule.predecessor = lsystem.alphabet[predecessorIndex];
						
						if(rule.predecessor.GetType() == typeof(CommunicationSymbol) && rule.condition == null)
						{
							rule.condition = ScriptableObject.CreateInstance<CommunicationCondition> ();
							rule.condition.init();
						}
					}
					
					if(rule.predecessor != null && rule.predecessor.GetType() == typeof(CommunicationSymbol))
					{
						GUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Condition :", EditorStyles.boldLabel, GUILayout.MaxWidth(100));
						
						int paramIndex     = (int)(rule.condition).param;
						int operationIndex = (int)(rule.condition).operation;
						
						paramIndex     = EditorGUILayout.Popup(paramIndex,     CommunicationCondition.CommParametersStr, GUILayout.MaxWidth(75));
						operationIndex = EditorGUILayout.Popup(operationIndex, CommunicationCondition.CommOperationStr,  GUILayout.MaxWidth(75));
						
						rule.condition.param     = (CommunicationCondition.CommParameters)paramIndex;
						rule.condition.operation = (CommunicationCondition.CommOperation)operationIndex;
						
						if(rule.condition.param == CommunicationCondition.CommParameters.time)
						{
							rule.condition.floatValue = EditorGUILayout.FloatField(rule.condition.floatValue, GUILayout.MaxWidth(60));
						}
						else
						{
							EditorGUILayout.LabelField("null");
							rule.condition.resultValue = null;
						}
						GUILayout.EndHorizontal();
					}
					
					EditorGUILayout.LabelField("Successors :", EditorStyles.boldLabel, GUILayout.MaxWidth(100));
					
					int toDeleteSuccessor = -1;
					for(int j = 0; j < rule.successor.Count; j++)
					{
						GUILayout.BeginHorizontal ();
						EditorGUILayout.LabelField(rule.successor[j].GetType().ToString() + "(" + rule.successor[j].name + ")", GUILayout.MaxWidth(180));
						if (GUILayout.Button ("Remove symbol", GUILayout.MaxWidth(140)))
						{
							toDeleteSuccessor = j;
						}
						GUILayout.EndHorizontal ();
					}
					if(toDeleteSuccessor != -1)
						rule.successor.RemoveAt(toDeleteSuccessor);
					
					GUILayout.BeginHorizontal ();
					updateRuleSuccessorIndex = EditorGUILayout.Popup (updateRuleSuccessorIndex, alphabetStr, GUILayout.MaxWidth(180));
					if (GUILayout.Button ("Add succesor", GUILayout.MaxWidth(140)))
					{
						if(updateRuleSuccessorIndex != -1)
						{
							rule.successor.Add(lsystem.alphabet[updateRuleSuccessorIndex]);
							updateRuleSuccessorIndex = -1;
						}
					}
					GUILayout.EndHorizontal ();


					GUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Probability :", EditorStyles.boldLabel, GUILayout.MaxWidth(100));
					rule.probability = EditorGUILayout.Slider("", rule.probability, 0.0f, 1.0f, GUILayout.MaxWidth(220));
					GUILayout.EndHorizontal();

					GUILayout.EndVertical();
					
					GUILayout.EndHorizontal();

					GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(1)});
				}
			}
		}
		if (toDeleteRule != -1)
		{
			lsystem.rules.Remove(toDeleteRule);
		}
		
		GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(1)});
		
		if (newRule == null)
		{
			if (GUILayout.Button ("Add new Rule", GUILayout.MaxWidth(150)))
			{
				newRule = ScriptableObject.CreateInstance<Rule>();
				newRule.init();
			}
		}
		
		if (newRule != null)
		{
			string[] alphabetStr = lsystem.alphabetArray();

			int predecessorIndex = lsystem.alphabet.IndexOf(newRule.predecessor);
			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Predecessor :", EditorStyles.boldLabel, GUILayout.MaxWidth(100));
			predecessorIndex = EditorGUILayout.Popup ("", predecessorIndex, alphabetStr, GUILayout.MaxWidth(220));

			// o tom potom

			GUILayout.EndHorizontal();

			if(predecessorIndex != -1)
			{
				newRule.predecessor = lsystem.alphabet[predecessorIndex];
				
				if(newRule.predecessor.GetType() == typeof(CommunicationSymbol) && newRule.condition == null)
				{
					newRule.condition = ScriptableObject.CreateInstance<CommunicationCondition> ();
					newRule.condition.init();
				}
			}
			
			if(newRule.predecessor != null && newRule.predecessor.GetType() == typeof(CommunicationSymbol))
			{
				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Condition :", EditorStyles.boldLabel, GUILayout.MaxWidth(100));
				
				int paramIndex     = (int)(newRule.condition).param;
				int operationIndex = (int)(newRule.condition).operation;
				
				paramIndex     = EditorGUILayout.Popup(paramIndex,     CommunicationCondition.CommParametersStr, GUILayout.MaxWidth(75));
				operationIndex = EditorGUILayout.Popup(operationIndex, CommunicationCondition.CommOperationStr,  GUILayout.MaxWidth(75));
				
				newRule.condition.param     = (CommunicationCondition.CommParameters)paramIndex;
				newRule.condition.operation = (CommunicationCondition.CommOperation)operationIndex;
				
				if(newRule.condition.param == CommunicationCondition.CommParameters.time)
				{
					newRule.condition.floatValue = EditorGUILayout.FloatField(newRule.condition.floatValue, GUILayout.MaxWidth(60));
				}
				else
				{
					EditorGUILayout.LabelField("null");
					newRule.condition.resultValue = null;
				}

				GUILayout.EndHorizontal();
			}
			
			EditorGUILayout.LabelField("Successors :", EditorStyles.boldLabel, GUILayout.MaxWidth(100));
			
			int toDeleteSuccessor = -1;
			for(int j = 0; j < newRule.successor.Count; j++)
			{
				GUILayout.BeginHorizontal ();

				EditorGUILayout.LabelField(newRule.successor[j].GetType().ToString() + "(" + newRule.successor[j].name + ")", GUILayout.MaxWidth(180));
				if (GUILayout.Button ("Remove symbol", GUILayout.MaxWidth(140)))
				{
					toDeleteSuccessor = j;
				}
				GUILayout.EndHorizontal ();
			}
			if(toDeleteSuccessor != -1)
				newRule.successor.RemoveAt(toDeleteSuccessor);
			
			GUILayout.BeginHorizontal ();

			newRuleSuccesorIndex = EditorGUILayout.Popup (newRuleSuccesorIndex, alphabetStr, GUILayout.MaxWidth(180));
			if (GUILayout.Button ("Add succesor", GUILayout.MaxWidth(140)))
			{
				if(newRuleSuccesorIndex != -1)
				{
					newRule.successor.Add(lsystem.alphabet[newRuleSuccesorIndex]);
					newRuleSuccesorIndex = -1;
				}
			}
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Probability :", EditorStyles.boldLabel, GUILayout.MaxWidth(100));
			newRule.probability = EditorGUILayout.Slider("", newRule.probability, 0.0f, 1.0f, GUILayout.MaxWidth(220));
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Add", GUILayout.MaxWidth(160)))
			{
				if(predecessorIndex != -1)
				{
					lsystem.rules.Add(newRule);
					newRule = null;
				}
			}
			if (GUILayout.Button ("Cancel", GUILayout.MaxWidth(160)))
			{
				newRule = null;
			}
			GUILayout.EndHorizontal ();
		}
	}
}
