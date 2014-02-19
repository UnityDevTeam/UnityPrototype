using UnityEngine;
using System;
using System.Collections.Generic;

public class LSystem : MonoBehaviour
{
	public bool useDerive    = true;
	public bool useInterpret = true;
	public bool useInterpretDeleteAdd = false;

	public ISymbol       axiom = new ISymbol ( 0, "A" );
	public List<ISymbol> state = new List<ISymbol>();

	private Rules rules = new Rules ();
	
	private GameObject communicationQueryObject = null;
	private Dictionary<int, CommunicationSymbol> communicationSymbols = new Dictionary<int, CommunicationSymbol> ();

	private SortedDictionary<int, CommunicationSymbol> activeSymbols = new SortedDictionary<int, CommunicationSymbol> ();
	
	void Awake()
	{
		if (!communicationQueryObject)
		{
			communicationQueryObject = GameObject.Find("Communication Manager");
			if(!communicationQueryObject)
			{
				communicationQueryObject = new GameObject("Communication Manager");
				communicationQueryObject.AddComponent<CommunicationManager>();
			}
		}

		//state.Add(axiom);
		CommunicationSymbol ax = new CommunicationSymbol ("C", "G", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "m", null);
		state.Add (ax);
		activeSymbols.Add (0, ax);

		setTestRules ();
	}

	void setTestRules()
	{
		/*
		ISymbol R1P = new ISymbol("A");
		
		CommunicationSymbol R1S1 = new CommunicationSymbol ("C", "G", Vector3.zero, Quaternion.identity, 0.0f, "m", null);
		List<ISymbol> R1S = new List<ISymbol> ();
		R1S.Add (R1S1);
		
		Rule R1 = new Rule (R1P, R1S, 1.0f);
		*/

		
		///////////////////////////////////////////////////////////////////////
		
		CommunicationSymbol R2P = new CommunicationSymbol ("C");
		R2P.operationIdentifier = "G";

		BindingSymbol       R2S1 = new BindingSymbol  ("g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0));
		StructureSymbol     R2S2 = new StructureSymbol("m", "testAgent2");
		CommunicationSymbol R2S3 = new CommunicationSymbol ("C", "G", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "m", null);

		List<ISymbol> R2S = new List<ISymbol> ();
		R2S.Add (R2S1);
		R2S.Add (R2S2);
		R2S.Add (R2S3);
		
		CommunicationCondition R2C = new CommunicationCondition (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		Rule R2 = new Rule (R2P, R2S, R2C, 0.97f);

		///////////////////////////////////////////////////////////////////////
		
		CommunicationSymbol R3P = new CommunicationSymbol ("C");
		R3P.operationIdentifier = "G";
		
		BindingSymbol       R3S1 = new BindingSymbol  ("g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0));
		StructureSymbol     R3S2 = new StructureSymbol("m", "testAgent2");
		CommunicationSymbol R3S3 = new CommunicationSymbol ("C", "B", new Vector3(1.3871f, 0.7653f, 0.0029f), Quaternion.Euler(new Vector3(0, 0, 298.3136f)), 0.0f, "m", null);
		EndSymbol           R3S4 = new EndSymbol("e");
		CommunicationSymbol R3S5 = new CommunicationSymbol ("C", "G", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "m", null);
		
		List<ISymbol> R3S = new List<ISymbol> ();
		R3S.Add (R3S1);
		R3S.Add (R3S2);
		R3S.Add (R3S3);
		R3S.Add (R3S4);
		R3S.Add (R3S5);
		
		CommunicationCondition R3C = new CommunicationCondition (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		Rule R3 = new Rule (R3P, R3S, R3C, 0.03f);
		
		///////////////////////////////////////////////////////////////////////
		
		CommunicationSymbol R4P = new CommunicationSymbol ("C");
		R4P.operationIdentifier = "B";

		BindingSymbol       R4S1 = new BindingSymbol  ("b", new Vector3(1.3871f, 0.7653f, 0.0029f), new Vector3(0, 0, 298.3136f), true);
		StructureSymbol     R4S2 = new StructureSymbol("m", "testAgent2");
		CommunicationSymbol R4S3 = new CommunicationSymbol ("C", "G", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "m", null);
		
		List<ISymbol> R4S = new List<ISymbol> ();
		R4S.Add (R4S1);
		R4S.Add (R4S2);
		R4S.Add (R4S3);
		
		CommunicationCondition R4C = new CommunicationCondition (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		Rule R4 = new Rule (R4P, R4S, R4C, 1.0f);
		
		/////////////////////////////////////////////////////////////
		
		//rules.Add (R1);
		rules.Add (R2);
		rules.Add (R3);
		rules.Add (R4);
	}
	
	void derive ()
	{
		List<ISymbol> newState = new List<ISymbol> ();

		for (int j = 0; j < state.Count; j++)
		{
			ISymbol module = state[j];
			
			if (!rules.Contains (module))
			{
				newState.Add(module);
				continue;
			}
			
			Rule rule = rules.Get (module);
			if(rule != null)
			{
				List<ISymbol> newSymbols = rule.successor;
				for(int i = 0; i < newSymbols.Count; i++)
				{
					ISymbol newSymbol = null;

					if(newSymbols[i].GetType() == typeof(CommunicationSymbol))
					{
						newSymbol = new CommunicationSymbol((CommunicationSymbol)newSymbols[i]);

						communicationSymbols.Add(newSymbol.id, (CommunicationSymbol)newSymbol);
					}
					else if(newSymbols[i].GetType() == typeof(BindingSymbol))
					{
						newSymbol = new BindingSymbol((BindingSymbol)newSymbols[i]);
					}
					else if(newSymbols[i].GetType() == typeof(StructureSymbol))
					{
						newSymbol = new StructureSymbol((StructureSymbol)newSymbols[i]);
					}
					else if(newSymbols[i].GetType() == typeof(EndSymbol))
					{
						newSymbol = new EndSymbol((EndSymbol)newSymbols[i]);
					}

					newState.Add(newSymbol);
				}

				if(state[j].GetType() == typeof(CommunicationSymbol))
				{
					Destroy(((CommunicationSymbol)state[j]).operationResult);
				}

				communicationSymbols.Remove(state[j].id);
				communicationQueryObject.GetComponent<CommunicationManager> ().Remove(state[j].id);
			}
			else
			{
				newState.Add(module);
			}
		}
		state = newState;
	}
	
	void DestroyOld()
	{
		int childs = transform.childCount;
		
		for (int i = childs - 1; i >= 0; i--)
		{
			GameObject.Destroy(transform.GetChild(i).gameObject);	
		}
	}

	GameObject addObject(ref Turtle turtle, string prefabName)
	{
		GameObject prefab = Resources.Load(prefabName) as GameObject;
		GameObject mol = Instantiate(prefab, turtle.position, turtle.direction) as GameObject;

		// remove agents components
		if (mol.GetComponent<RandomMove> ())
			Destroy (mol.GetComponent<RandomMove> ());

		if (mol.GetComponent<RandomRotate> ())
			Destroy (mol.GetComponent<RandomRotate> ());

		if (mol.GetComponent<GlobalAttraction> ())
			Destroy (mol.GetComponent<GlobalAttraction> ());

		if (mol.GetComponent<GlobalBindingQuery> ())
			Destroy (mol.GetComponent<GlobalBindingQuery> ());

		if (mol.GetComponent<BoundaryBounce> ())
			Destroy (mol.GetComponent<BoundaryBounce> ());

		mol.rigidbody.isKinematic = true;

		mol.transform.parent = transform;
		mol.transform.localScale = NewAgentSystem.agentScale * mol.transform.localScale;

		return mol;
	}

	void updateTurtle(ref Turtle turtle, Vector3 positionDelta, Vector3 orientationDelta)
	{
		turtle.position  = turtle.position + turtle.direction * (NewAgentSystem.agentScale * positionDelta);
		turtle.direction = turtle.direction * Quaternion.Euler (orientationDelta.x, orientationDelta.y, orientationDelta.z);
	}

	private void interpret ()
	{
		if(useInterpretDeleteAdd)
			DestroyOld();
		
		Turtle current = new Turtle (Quaternion.identity, Vector3.zero);
		Stack<Turtle> stack = new Stack<Turtle> ();
		stack.Push(current);
		
		for (int i = 0; i < state.Count; i++)
		{
			ISymbol symbol = state[i];

			if(symbol.GetType() == typeof(StructureSymbol))
			{
				if(useInterpretDeleteAdd)
					addObject(ref current, ((StructureSymbol)symbol).structurePrefabName);
			}
			else if(symbol.GetType() == typeof(BindingSymbol))
			{
				if(((BindingSymbol)symbol).isBranching)
				{
					stack.Push(current);
					current = new Turtle (current);
				}

				updateTurtle(ref current, ((BindingSymbol)symbol).bindingPosition, ((BindingSymbol)symbol).bindingOrientation);
			}
			else if(symbol.GetType() == typeof(EndSymbol))
			{
				current = stack.Pop ();
			}
			else if(symbol.GetType() == typeof(CommunicationSymbol))
			{
				// uff :(
				if(((CommunicationSymbol)symbol).operationIdentifier == "B")
				{
					stack.Push(current);
					current = new Turtle (current);
				}

				((CommunicationSymbol)symbol).fillTurtleValues(current);
			}
		}
	}

	void newStart()
	{

	}

	void newDerive()
	{
		List<int> toRemove = new List<int>();
		Dictionary<int, CommunicationSymbol> toAdd = new Dictionary<int, CommunicationSymbol>();
		CommunicationManager cql = communicationQueryObject.GetComponent<CommunicationManager> ();

		SortedDictionary<int, CommunicationSymbol> newActiveSymbols = new SortedDictionary<int, CommunicationSymbol> ();

		int indexOffset = 0;
		foreach (KeyValuePair<int, CommunicationSymbol> activeSymbol in activeSymbols)
		{
			ISymbol deb = state[activeSymbol.Key + indexOffset];
			CommunicationSymbol symbol = (CommunicationSymbol)state[activeSymbol.Key + indexOffset];

			cql.Remove(activeSymbol.Key);

			Rule rule = rules.Get (symbol);
			if(rule != null)
			{
				Destroy(symbol.operationResult);
				state.RemoveAt(activeSymbol.Key + indexOffset);
				//toRemove.Add(activeSymbol.Key);

				List<ISymbol> newSymbols = rule.successor;

				for(int i = 0; i < newSymbols.Count; i++)
				{
					int newIndex = activeSymbol.Key + indexOffset + i;

					ISymbol newSymbol = null;

					if(newSymbols[i].GetType() == typeof(CommunicationSymbol))
					{
						newSymbol = new CommunicationSymbol((CommunicationSymbol)newSymbols[i]);
						
						//toAdd.Add(newIndex, (CommunicationSymbol)newSymbol);
						newActiveSymbols.Add(newIndex, symbol);
					}
					else if(newSymbols[i].GetType() == typeof(BindingSymbol))
					{
						newSymbol = new BindingSymbol((BindingSymbol)newSymbols[i]);
					}
					else if(newSymbols[i].GetType() == typeof(StructureSymbol))
					{
						newSymbol = new StructureSymbol((StructureSymbol)newSymbols[i]);
					}
					else if(newSymbols[i].GetType() == typeof(EndSymbol))
					{
						newSymbol = new EndSymbol((EndSymbol)newSymbols[i]);
					}
					
					state.Insert( newIndex, newSymbol );
				}

				indexOffset += newSymbols.Count - 1;
			}
			else
			{
				newActiveSymbols.Add(activeSymbol.Key + indexOffset, symbol);
			}
		}
/*
		foreach (int indexRemove in toRemove)
		{
			activeSymbols.Remove(indexRemove);
		}

		foreach (KeyValuePair<int, CommunicationSymbol> newSymbol in toAdd)
		{
			activeSymbols.Add(newSymbol.Key, newSymbol.Value);
		}
*/
		activeSymbols = newActiveSymbols;
		//debugPrint ();
	}

	private void newInterpret ()
	{
		Turtle current = new Turtle (Quaternion.identity, Vector3.zero);
		Stack<Turtle> stack = new Stack<Turtle> ();
		stack.Push(current);
		
		for (int i = 0; i < state.Count; i++)
		{
			ISymbol symbol = state[i];
			
			if(symbol.GetType() == typeof(StructureSymbol))
			{
				if(((StructureSymbol)symbol).structureObject == null)
				{
					((StructureSymbol)state[i]).structureObject = addObject(ref current, ((StructureSymbol)symbol).structurePrefabName);
				}
			}
			else if(symbol.GetType() == typeof(BindingSymbol))
			{
				if(((BindingSymbol)symbol).isBranching)
				{
					stack.Push(current);
					current = new Turtle (current);
				}
				
				updateTurtle(ref current, ((BindingSymbol)symbol).bindingPosition, ((BindingSymbol)symbol).bindingOrientation);
			}
			else if(symbol.GetType() == typeof(EndSymbol))
			{
				current = stack.Pop ();
			}
			else if(symbol.GetType() == typeof(CommunicationSymbol))
			{
				// uff :(
				if(((CommunicationSymbol)symbol).operationIdentifier == "B")
				{
					stack.Push(current);
					current = new Turtle (current);
				}
				
				((CommunicationSymbol)symbol).fillTurtleValues(current);
			}
		}
	}

	private void preEnviromentStep()
	{
		List<CommunicationQuery> queries = communicationQueryObject.GetComponent<CommunicationManager> ().getQueries ();
		
		for(int i = 0; i < queries.Count; i++)
		{
			if(queries[i].changed)
			{
				if(activeSymbols.ContainsKey(queries[i].stateId))
				{
					activeSymbols[queries[i].stateId].operationTimer  = queries[i].time;
					activeSymbols[queries[i].stateId].operationResult = queries[i].result;
				}
			}
		}
	}

	void postEnviromentStep()
	{
		CommunicationManager cql = communicationQueryObject.GetComponent<CommunicationManager> ();

		foreach(KeyValuePair<int, CommunicationSymbol> symbol in activeSymbols)
		{
			cql.Add(symbol.Key, symbol.Value);
		}
	}

	private void TimeStep()
	{
		preEnviromentStep ();

		newDerive ();
		newInterpret ();

		postEnviromentStep ();
	}
	
	void Update()
	{
		TimeStep ();
	}

	void debugPrint()
	{
		string output = "";
		for (int i = 0; i < state.Count; i++)
		{
			output += state[i].name;

			if(state[i].GetType() == typeof(CommunicationSymbol))
			{
				output += "(" + ((CommunicationSymbol)state[i]).operationIdentifier + ")";
			}
		}

		print (output);
	}
}