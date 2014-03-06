using System;
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

	private Vector3 bindingPosition   = Vector3.zero;
	private Vector3 bindingRotation   = Vector3.zero;
	private Vector3 bindingAlteration = Vector3.zero;
	private bool isBranching = false;

	private string communicationIdentifier = "";
	private Vector3 communicationPosition    = Vector3.zero;
	private Vector3 communicationOrientation = Vector3.zero;
	private int communicationResultTypeIndex = 0;
	private AnimationCurve communicationProbability = AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f);

	private bool addNewLetter = false;

	private bool changeAxiom = false;
	private int axiomTypeIndex = 0;
	
	private Rule newRule = null;
	private int changeRuleIndex = -1;
	private int newRuleSuccesorIndex = -1;
	private int updateRuleSuccessorIndex = -1;

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
				EditorGUILayout.LabelField(symbol.GetType().ToString()         , GUILayout.MaxWidth(140));
				EditorGUILayout.LabelField("[" + lSystem.alphabet[i].name + "]", GUILayout.MaxWidth(30));
				EditorGUILayout.LabelField(""                                  , GUILayout.MaxWidth(180));
				EditorGUILayout.LabelField(""                                  , GUILayout.MaxWidth(180));
			}
			else if(symbol.GetType() == typeof(EndSymbol))
			{
				EditorGUILayout.LabelField(symbol.GetType().ToString()         , GUILayout.MaxWidth(140));
				EditorGUILayout.LabelField("[" + lSystem.alphabet[i].name + "]", GUILayout.MaxWidth(30));
				EditorGUILayout.LabelField(""                                  , GUILayout.MaxWidth(180));
				EditorGUILayout.LabelField(""                                  , GUILayout.MaxWidth(180));
			}
			else if(symbol.GetType() == typeof(StructureSymbol))
			{
				EditorGUILayout.LabelField(symbol.GetType().ToString()                               , GUILayout.MaxWidth(140));
				EditorGUILayout.LabelField("[" + lSystem.alphabet[i].name + "]"                      , GUILayout.MaxWidth(30));
				EditorGUILayout.LabelField(((StructureSymbol)lSystem.alphabet[i]).structurePrefabName, GUILayout.MaxWidth(180));
				EditorGUILayout.LabelField(""                                                        , GUILayout.MaxWidth(180));
			}
			else if(symbol.GetType() == typeof(BindingSymbol))
			{
				EditorGUILayout.LabelField(symbol.GetType().ToString()                                                       , GUILayout.MaxWidth(140));
				EditorGUILayout.LabelField("[" + lSystem.alphabet[i].name + "]"                                              , GUILayout.MaxWidth(30));
				EditorGUILayout.LabelField("[position - " + ((BindingSymbol)lSystem.alphabet[i]).bindingPosition + "]"       , GUILayout.MaxWidth(180));
				EditorGUILayout.LabelField("[orientation - " + ((BindingSymbol)lSystem.alphabet[i]).bindingOrientation + "]" , GUILayout.MaxWidth(180));
				EditorGUILayout.LabelField("[orientationAlt - " + ((BindingSymbol)lSystem.alphabet[i]).orientAlteration + "]", GUILayout.MaxWidth(180));
			}
			else if(symbol.GetType() == typeof(CommunicationSymbol))
			{
				EditorGUILayout.LabelField(symbol.GetType().ToString()                                             , GUILayout.MaxWidth(140));
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
				symbolName = EditorGUILayout.TextField("Symbol name : ", symbolName);

				break;
			case 1:
				symbolName     = EditorGUILayout.TextField("Symbol name : ", symbolName);
				structureIndex = EditorGUILayout.Popup ("Structure type : ", structureIndex, GlobalLists.monomerTypes);

				break;
			case 2:
				symbolName        = EditorGUILayout.TextField("Symbol name : ",               symbolName);
				bindingPosition   = EditorGUILayout.Vector3Field("Binding position : ",       bindingPosition);
				bindingRotation   = EditorGUILayout.Vector3Field("Binding orientation : ",    bindingRotation);
				bindingAlteration = EditorGUILayout.Vector3Field("Binding orientationAlt : ", bindingAlteration);
				isBranching       = EditorGUILayout.Toggle("Is branching : ",                 isBranching);
				break;
			case 3:
				symbolName = EditorGUILayout.TextField("Symbol name : ", symbolName);
				communicationIdentifier       = EditorGUILayout.TextField("Process identifier : ", communicationIdentifier);
				communicationPosition         = EditorGUILayout.Vector3Field("Process position : ", communicationPosition);
				communicationOrientation      = EditorGUILayout.Vector3Field("Process orientation : ", communicationOrientation);
				communicationResultTypeIndex  = EditorGUILayout.Popup("Process result type : ", communicationResultTypeIndex, GlobalLists.monomerTypes);
				communicationProbability      = EditorGUILayout.CurveField("Process probability function : ", communicationProbability);

				AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f);
				break;
			case 4:
				symbolName = EditorGUILayout.TextField("Symbol name : ", symbolName);
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
					newSymbol = ScriptableObject.CreateInstance<BindingSymbol> ();
					((BindingSymbol)newSymbol).init(symbolName, bindingPosition, bindingRotation, bindingAlteration, isBranching);
					break;
				case 3:
					newSymbol = ScriptableObject.CreateInstance<CommunicationSymbol> ();
					((CommunicationSymbol)newSymbol).init(symbolName, communicationIdentifier, communicationPosition, Quaternion.Euler(communicationOrientation), 0, GlobalLists.monomerTypes[communicationResultTypeIndex], null, communicationProbability);
					break;
				case 4:
					newSymbol = ScriptableObject.CreateInstance<EndSymbol> ();
					((EndSymbol)newSymbol).init(symbolName);
					break;
				}

				lSystem.alphabet.Add(newSymbol);
				addNewLetter = false;
			}
		}
	}
	
	private void showAxiomMenu()
	{
		EditorGUILayout.LabelField("Current axiom :", EditorStyles.boldLabel);
		EditorGUILayout.Separator ();

		ISymbol axiom = lSystem.axiom;

		if (axiom != null)
		{
			if(axiom.GetType() == typeof(ISymbol))
			{
				EditorGUILayout.LabelField("ISymbol");

				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Name : ", GUILayout.MaxWidth(120)); EditorGUILayout.LabelField(lSystem.axiom.name);
				GUILayout.EndHorizontal();
			}
			else if(axiom.GetType() == typeof(EndSymbol))
			{
				EditorGUILayout.LabelField("EndSymbol");

				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Name : ", GUILayout.MaxWidth(120)); EditorGUILayout.LabelField(lSystem.axiom.name);
				GUILayout.EndHorizontal();
			}
			else if(axiom.GetType() == typeof(StructureSymbol))
			{
				EditorGUILayout.LabelField("StructureSymbol");

				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Name : ", GUILayout.MaxWidth(120)); EditorGUILayout.LabelField(lSystem.axiom.name);
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Prefab : ", GUILayout.MaxWidth(120)); EditorGUILayout.LabelField(((StructureSymbol)lSystem.axiom).structurePrefabName);
				GUILayout.EndHorizontal();
			}
			else if(axiom.GetType() == typeof(BindingSymbol))
			{
				EditorGUILayout.LabelField("BindingSymbol");

				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Name : ", GUILayout.MaxWidth(120)); EditorGUILayout.LabelField(lSystem.axiom.name);
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Binding position : ", GUILayout.MaxWidth(120)); EditorGUILayout.LabelField(((BindingSymbol)lSystem.axiom).bindingPosition.ToString());
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Binding orientation : ", GUILayout.MaxWidth(120)); EditorGUILayout.LabelField(((BindingSymbol)lSystem.axiom).bindingOrientation.ToString());
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Binding alteration : ", GUILayout.MaxWidth(120)); EditorGUILayout.LabelField(((BindingSymbol)lSystem.axiom).orientAlteration.ToString());
				GUILayout.EndHorizontal();
			}
			else if(axiom.GetType() == typeof(CommunicationSymbol))
			{
				EditorGUILayout.LabelField("CommunicationSymbol");

				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Name : ", GUILayout.MaxWidth(120)); EditorGUILayout.LabelField(lSystem.axiom.name);
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Process : ", GUILayout.MaxWidth(120)); EditorGUILayout.LabelField(((CommunicationSymbol)lSystem.axiom).process);
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Position : ", GUILayout.MaxWidth(120)); EditorGUILayout.LabelField(((CommunicationSymbol)lSystem.axiom).position.ToString());
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Orientation : ", GUILayout.MaxWidth(120)); EditorGUILayout.LabelField(((CommunicationSymbol)lSystem.axiom).orientation.eulerAngles.ToString());
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Result type : ", GUILayout.MaxWidth(120)); EditorGUILayout.LabelField(((CommunicationSymbol)lSystem.axiom).resultType);
				GUILayout.EndHorizontal();

				EditorGUILayout.CurveField("Process probability : ", ((CommunicationSymbol)lSystem.axiom).probability);
			}
		}

		GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(1)});
		
		if (!changeAxiom)
		{
			if (GUILayout.Button ("Change axiom"))
			{
				changeAxiom = true;
			}
		}

		if(changeAxiom)
		{
			List<string> lSystemAlphabet = new List<string>();

			foreach(ISymbol symbol in lSystem.alphabet)
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

			axiomTypeIndex = EditorGUILayout.Popup ("Axiom : ", axiomTypeIndex, lSystemAlphabet.ToArray());

			if (GUILayout.Button ("Add / Change"))
			{
				ISymbol symbol = null;

				if(lSystem.alphabet[axiomTypeIndex].GetType() == typeof(ISymbol))
				{
					symbol = ScriptableObject.CreateInstance<ISymbol> ();
					symbol.init(lSystem.alphabet[axiomTypeIndex].name);
				}
				else if(lSystem.alphabet[axiomTypeIndex].GetType() == typeof(EndSymbol))
				{
					symbol = ScriptableObject.CreateInstance<EndSymbol> ();
					((EndSymbol)symbol).init((EndSymbol)lSystem.alphabet[axiomTypeIndex]);
				}
				else if(lSystem.alphabet[axiomTypeIndex].GetType() == typeof(StructureSymbol))
				{
					symbol = ScriptableObject.CreateInstance<StructureSymbol> ();
					((StructureSymbol)symbol).init((StructureSymbol)lSystem.alphabet[axiomTypeIndex]);
				}
				else if(lSystem.alphabet[axiomTypeIndex].GetType() == typeof(BindingSymbol))
				{
					symbol = ScriptableObject.CreateInstance<BindingSymbol> ();
					((BindingSymbol)symbol).init((BindingSymbol)lSystem.alphabet[axiomTypeIndex]);
				}
				else if(lSystem.alphabet[axiomTypeIndex].GetType() == typeof(CommunicationSymbol))
				{
					symbol = ScriptableObject.CreateInstance<CommunicationSymbol> ();
					((CommunicationSymbol)symbol).init((CommunicationSymbol)lSystem.alphabet[axiomTypeIndex]);
				}

				lSystem.axiom = symbol;
				changeAxiom = false;
			}
		}
	}

	private void showRulesMenu()
	{
		EditorGUILayout.LabelField("LSystem rules", EditorStyles.boldLabel);

		int toDeleteRule = -1;
		if(lSystem.rules != null)
		{
			for (int i = 0; i < lSystem.rules.getRulesCount(); i++)
			{
				Rule rule = lSystem.rules.getRule(i);

				GUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField(rule.predecessor.toShortString(), GUILayout.MaxWidth(35));

				EditorGUILayout.LabelField("->", GUILayout.MaxWidth(20));

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


				// fuj
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

				EditorGUILayout.LabelField("[" + conditionString + "]", GUILayout.MaxWidth(100));

				string succesor = "";
				for(int j = 0; j < rule.successor.Count; j++)
				{
					succesor += rule.successor[j].toShortString();
				}
				EditorGUILayout.LabelField(succesor, GUILayout.MaxWidth(150));
				EditorGUILayout.LabelField("[ " + rule.probability + " ]");

				if(changeRuleIndex != -1)
					GUI.enabled = false;

				if(GUILayout.Button ("Change Rule"))
				{
					changeRuleIndex = i;
				}

				if(GUILayout.Button ("Remove Rule"))
				{
					toDeleteRule = i;
				}

				GUI.enabled = true;

				GUILayout.EndHorizontal ();

				if(changeRuleIndex == i)
				{
					string[] alphabetStr = lSystem.alphabetArray();

					GUILayout.BeginHorizontal();
					GUILayout.BeginVertical();

					EditorGUILayout.LabelField("Predecessor ", EditorStyles.boldLabel);
					int predecessorIndex = lSystem.alphabet.IndexOf(rule.predecessor);
					predecessorIndex = EditorGUILayout.Popup ("Predecessor : ", predecessorIndex, alphabetStr);
					
					if(predecessorIndex != -1)
					{
						rule.predecessor = lSystem.alphabet[predecessorIndex];
						
						if(rule.predecessor.GetType() == typeof(CommunicationSymbol) && rule.condition == null)
						{
							rule.condition = ScriptableObject.CreateInstance<CommunicationCondition> ();
							rule.condition.init();
						}
					}
					
					if(rule.predecessor != null && rule.predecessor.GetType() == typeof(CommunicationSymbol))
					{
						EditorGUILayout.LabelField("Condition ", EditorStyles.boldLabel);
						
						int paramIndex     = (int)(rule.condition).param;
						int operationIndex = (int)(rule.condition).operation;
						
						paramIndex     = EditorGUILayout.Popup(paramIndex,     CommunicationCondition.CommParametersStr);
						operationIndex = EditorGUILayout.Popup(operationIndex, CommunicationCondition.CommOperationStr);
						
						rule.condition.param     = (CommunicationCondition.CommParameters)paramIndex;
						rule.condition.operation = (CommunicationCondition.CommOperation)operationIndex;
						
						if(rule.condition.param == CommunicationCondition.CommParameters.time)
						{
							rule.condition.floatValue = EditorGUILayout.FloatField(rule.condition.floatValue);
						}
						else
						{
							EditorGUILayout.LabelField("null");
							rule.condition.resultValue = null;
						}
					}
					
					EditorGUILayout.LabelField("Successors ", EditorStyles.boldLabel);
					
					int toDeleteSuccessor = -1;
					for(int j = 0; j < rule.successor.Count; j++)
					{
						GUILayout.BeginHorizontal ();
						EditorGUILayout.LabelField(rule.successor[j].GetType().ToString() + "(" + rule.successor[j].name + ")");
						if (GUILayout.Button ("Del"))
						{
							toDeleteSuccessor = j;
						}
						GUILayout.EndHorizontal ();
					}
					if(toDeleteSuccessor != -1)
						rule.successor.RemoveAt(toDeleteSuccessor);
					
					GUILayout.BeginHorizontal ();
					updateRuleSuccessorIndex = EditorGUILayout.Popup (updateRuleSuccessorIndex, alphabetStr);
					if (GUILayout.Button ("Add succesor"))
					{
						if(updateRuleSuccessorIndex != -1)
						{
							rule.successor.Add(lSystem.alphabet[updateRuleSuccessorIndex]);
							updateRuleSuccessorIndex = -1;
						}
					}
					GUILayout.EndHorizontal ();
					
					rule.probability = EditorGUILayout.Slider("Probability ", rule.probability, 0.0f, 1.0f);

					GUILayout.EndVertical();

					if (GUILayout.Button ("Cancel", new GUILayoutOption[] {GUILayout.ExpandHeight(true), GUILayout.MaxHeight(140)}))
					{
						if(predecessorIndex != -1)
						{
							lSystem.rules.setRule(i,rule);
							changeRuleIndex = -1;
						}
					}

					GUILayout.EndHorizontal();
				}
			}
		}
		if (toDeleteRule != -1)
		{
			lSystem.rules.Remove(toDeleteRule);
		}

		GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(1)});
		
		if (newRule == null)
		{
			if (GUILayout.Button ("Add new Rule"))
			{
				newRule = ScriptableObject.CreateInstance<Rule>();
				newRule.init();
			}
		}

		if (newRule != null)
		{
			string[] alphabetStr = lSystem.alphabetArray();

			EditorGUILayout.LabelField("Predecessor ", EditorStyles.boldLabel);
			int predecessorIndex = lSystem.alphabet.IndexOf(newRule.predecessor);
			predecessorIndex = EditorGUILayout.Popup ("Predecessor : ", predecessorIndex, alphabetStr);

			if(predecessorIndex != -1)
			{
				newRule.predecessor = lSystem.alphabet[predecessorIndex];

				if(newRule.predecessor.GetType() == typeof(CommunicationSymbol) && newRule.condition == null)
				{
					newRule.condition = ScriptableObject.CreateInstance<CommunicationCondition> ();
					newRule.condition.init();
				}
			}

			if(newRule.predecessor != null && newRule.predecessor.GetType() == typeof(CommunicationSymbol))
			{
				EditorGUILayout.LabelField("Condition ", EditorStyles.boldLabel);

				int paramIndex     = (int)(newRule.condition).param;
				int operationIndex = (int)(newRule.condition).operation;

				paramIndex     = EditorGUILayout.Popup(paramIndex,     CommunicationCondition.CommParametersStr);
				operationIndex = EditorGUILayout.Popup(operationIndex, CommunicationCondition.CommOperationStr);

				newRule.condition.param     = (CommunicationCondition.CommParameters)paramIndex;
				newRule.condition.operation = (CommunicationCondition.CommOperation)operationIndex;

				if(newRule.condition.param == CommunicationCondition.CommParameters.time)
				{
					newRule.condition.floatValue = EditorGUILayout.FloatField(newRule.condition.floatValue);
				}
				else
				{
					EditorGUILayout.LabelField("null");
					newRule.condition.resultValue = null;
				}
			}

			EditorGUILayout.LabelField("Successors ", EditorStyles.boldLabel);

			int toDeleteSuccessor = -1;
			for(int j = 0; j < newRule.successor.Count; j++)
			{
				GUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField(newRule.successor[j].GetType().ToString() + "(" + newRule.successor[j].name + ")");
				if (GUILayout.Button ("Del"))
				{
					toDeleteSuccessor = j;
				}
				GUILayout.EndHorizontal ();
			}
			if(toDeleteSuccessor != -1)
				newRule.successor.RemoveAt(toDeleteSuccessor);

			GUILayout.BeginHorizontal ();
			newRuleSuccesorIndex = EditorGUILayout.Popup (newRuleSuccesorIndex, alphabetStr);
			if (GUILayout.Button ("Add succesor"))
			{
				if(newRuleSuccesorIndex != -1)
				{
					newRule.successor.Add(lSystem.alphabet[newRuleSuccesorIndex]);
					newRuleSuccesorIndex = -1;
				}
			}
			GUILayout.EndHorizontal ();

			newRule.probability = EditorGUILayout.Slider("Probability ", newRule.probability, 0.0f, 1.0f);

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Add"))
			{
				if(predecessorIndex != -1)
				{
					lSystem.rules.Add(newRule);
					newRule = null;
				}
			}
			if (GUILayout.Button ("Cancel"))
			{
				newRule = null;
			}
			GUILayout.EndHorizontal ();
		}
	}

	private void showSimulationMenu()
	{
		EditorGUILayout.LabelField("Simulation properties", EditorStyles.boldLabel);

		lSystem.monomerCountingStop = EditorGUILayout.IntSlider ("Monomer count : ",   lSystem.monomerCountingStop, 0, 1000);
		lSystem.exampleIndex        = EditorGUILayout.Popup ("Polymer Example : ", lSystem.exampleIndex, lSystem.examples);

		LSystem.timeDelta = EditorGUILayout.Slider ("Time delta: ", LSystem.timeDelta, 0.02f, 3.0f);
		
		RandomMove.speed = LSystem.timeDelta * 50.0f;
		if (RandomMove.speed > 30.0f) RandomMove.speed = 30.0f;
		
		EditorGUILayout.LabelField("Speed: " + RandomMove.speed);
	}
}