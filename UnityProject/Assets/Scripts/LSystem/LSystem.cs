using UnityEngine;
using System;
using System.Collections.Generic;

public class LSystem : MonoBehaviour
{
	[HideInInspector] public static float timeDelta = 0.1f;

	public List<ISymbol> alphabet = new List<ISymbol>();
	public ISymbol       axiom;
	public List<ISymbol> state = new List<ISymbol>();

	private Rules rules = new Rules ();
	
	private GameObject communicationQueryObject = null;

	private SortedDictionary<int, CommunicationSymbol> activeSymbols = new SortedDictionary<int, CommunicationSymbol> ();
	
	Material diffWhite;
	Material diffTransBlue;

	GameObject communications;

	int monomerCounting = 0;
	public int monomerCountingStop = 100;

	public int polymerExample = 1;

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

		RenderSettings.haloStrength = 1.0f;

		communications = new GameObject("Communications");

		diffWhite     = (Material)Resources.Load("Materials/diffWhite", typeof(Material));
		diffTransBlue = (Material)Resources.Load("Materials/diffTransBlue", typeof(Material));

		if (polymerExample == 0)
		{
			CommunicationSymbol ax = new CommunicationSymbol ("C", "G", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "adpRibose", null);
			state.Add (ax);
			activeSymbols.Add (0, ax);

			setTestRules ();
		}
		else if (polymerExample == 1)
		{
			CommunicationSymbol ax = new CommunicationSymbol ("C", "F", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "adpRibose", null);
			state.Add (ax);
			activeSymbols.Add (0, ax);

			setTestRules2();
		}
		else if (polymerExample == 2)
		{
			CommunicationSymbol ax = new CommunicationSymbol ("C", "M", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "adpRibose", null);
			state.Add (ax);
			activeSymbols.Add (0, ax);
			
			setTestRules3();
		}
	}
	
	void setTestRules()
	{
		///////////////////////////////////////////////////////////////////////
		
		CommunicationSymbol R2P = new CommunicationSymbol ("C");
		R2P.operationIdentifier = "G";

		BindingSymbol       R2S1 = new BindingSymbol  ("g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0));
		StructureSymbol     R2S2 = new StructureSymbol("m", "adpRibose");
		CommunicationSymbol R2S3 = new CommunicationSymbol ("C", "G", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "adpRibose", null);

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
		StructureSymbol     R3S2 = new StructureSymbol("m", "adpRibose");
		CommunicationSymbol R3S3 = new CommunicationSymbol ("C", "B", new Vector3(1.3871f, 0.7653f, 0.0029f), Quaternion.Euler(new Vector3(0, 0, 298.3136f)), 0.0f, "adpRibose", null);
		CommunicationSymbol R3S4 = new CommunicationSymbol ("C", "G", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "adpRibose", null);
		
		List<ISymbol> R3S = new List<ISymbol> ();
		R3S.Add (R3S1);
		R3S.Add (R3S2);
		R3S.Add (R3S3);
		R3S.Add (R3S4);
		
		CommunicationCondition R3C = new CommunicationCondition (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		Rule R3 = new Rule (R3P, R3S, R3C, 0.03f);
		
		///////////////////////////////////////////////////////////////////////
		
		CommunicationSymbol R4P = new CommunicationSymbol ("C");
		R4P.operationIdentifier = "B";

		BindingSymbol       R4S1 = new BindingSymbol  ("b", new Vector3(1.3871f, 0.7653f, 0.0029f), new Vector3(0, 0, 298.3136f), true);
		StructureSymbol     R4S2 = new StructureSymbol("m", "adpRibose");
		CommunicationSymbol R4S3 = new CommunicationSymbol ("C", "G", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "adpRibose", null);
		
		List<ISymbol> R4S = new List<ISymbol> ();
		R4S.Add (R4S1);
		R4S.Add (R4S2);
		R4S.Add (R4S3);
		
		CommunicationCondition R4C = new CommunicationCondition (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		Rule R4 = new Rule (R4P, R4S, R4C, 1.0f);
		
		/////////////////////////////////////////////////////////////

		CommunicationSymbol R5P = new CommunicationSymbol ("C");
		R5P.operationIdentifier = "G";
		
		EndSymbol R5S1 = new EndSymbol("e");
		
		List<ISymbol> R5S = new List<ISymbol> ();
		R5S.Add (R5S1);
		
		CommunicationCondition R5C = new CommunicationCondition (CommunicationCondition.CommParameters.time, CommunicationCondition.CommOperation.more, 5.0f);
		
		Rule R5 = new Rule (R5P, R5S, R5C, 1.0f);

		/////////////////////////////////////////////////////////////

		CommunicationSymbol R6P = new CommunicationSymbol ("C");
		R6P.operationIdentifier = "B";
		
		List<ISymbol> R6S = new List<ISymbol> ();
		
		CommunicationCondition R6C = new CommunicationCondition (CommunicationCondition.CommParameters.time, CommunicationCondition.CommOperation.more, 5.0f);
		
		Rule R6 = new Rule (R6P, R6S, R6C, 1.0f);
		
		/////////////////////////////////////////////////////////////

		rules.Add (R2);
		rules.Add (R3);
		rules.Add (R4);
		rules.Add (R5);
		rules.Add (R6);
	}

	void setTestRules2()
	{
		///////////////////////////////////////////////////////////////////////
		
		CommunicationSymbol R2P = new CommunicationSymbol ("C");
		R2P.operationIdentifier = "F";
		
		BindingSymbol       R2S1 = new BindingSymbol  ("g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0));
		StructureSymbol     R2S2 = new StructureSymbol("m", "adpRibose");
		CommunicationSymbol R2S3 = new CommunicationSymbol ("C", "F", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "adpRibose", null);
		
		List<ISymbol> R2S = new List<ISymbol> ();
		R2S.Add (R2S1);
		R2S.Add (R2S2);
		R2S.Add (R2S3);
		
		CommunicationCondition R2C = new CommunicationCondition (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		Rule R2 = new Rule (R2P, R2S, R2C, 0.9f);
		
		///////////////////////////////////////////////////////////////////////
		
		CommunicationSymbol R3P = new CommunicationSymbol ("C");
		R3P.operationIdentifier = "F";
		
		BindingSymbol       R3S1 = new BindingSymbol  ("g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0));
		StructureSymbol     R3S2 = new StructureSymbol("m", "adpRibose");
		CommunicationSymbol R3S3 = new CommunicationSymbol ("C", "A", new Vector3(0.67309f, 0.52365f, 0.83809f), Quaternion.Euler(new Vector3(-45.8616f, -50.1757f, -86.2943f)), 0.0f, "adpRibose", null);
		CommunicationSymbol R3S4 = new CommunicationSymbol ("C", "B", new Vector3(-0.94228f, 0.3053f, 0.9473f), Quaternion.Euler(new Vector3(48.033f, -112.99f, -89.888f)), 0.0f, "adpRibose", null);
		CommunicationSymbol R3S5 = new CommunicationSymbol ("C", "C", new Vector3(-0.2270f, 0.6361f, -0.9190f), Quaternion.Euler(new Vector3(-72.7191f, 34.9005f, -37.05637f)), 0.0f, "adpRibose", null);
		
		List<ISymbol> R3S = new List<ISymbol> ();
		R3S.Add (R3S1);
		R3S.Add (R3S2);
		R3S.Add (R3S3);
		R3S.Add (R3S4);
		R3S.Add (R3S5);
		
		CommunicationCondition R3C = new CommunicationCondition (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		Rule R3 = new Rule (R3P, R3S, R3C, 0.1f);
		
		///////////////////////////////////////////////////////////////////////
		
		CommunicationSymbol R4P = new CommunicationSymbol ("C");
		R4P.operationIdentifier = "A";
		
		BindingSymbol       R4S1 = new BindingSymbol  ("a", new Vector3(0.67309f, 0.52365f, 0.83809f), new Vector3(-45.8616f, -50.1757f, -86.2943f), true);
		StructureSymbol     R4S2 = new StructureSymbol("m", "adpRibose");
		CommunicationSymbol R4S3 = new CommunicationSymbol ("C", "G", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "adpRibose", null);
		
		List<ISymbol> R4S = new List<ISymbol> ();
		R4S.Add (R4S1);
		R4S.Add (R4S2);
		R4S.Add (R4S3);
		
		CommunicationCondition R4C = new CommunicationCondition (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		Rule R4 = new Rule (R4P, R4S, R4C, 1.0f);
		
       ///////////////////////////////////////////////////////////////////////
       
       CommunicationSymbol R5P = new CommunicationSymbol ("C");
       R5P.operationIdentifier = "B";
       
		BindingSymbol      R5S1 = new BindingSymbol  ("b", new Vector3(-0.94228f, 0.3053f, 0.9473f), new Vector3(48.033f, -112.99f, -89.888f), true);
		StructureSymbol     R5S2 = new StructureSymbol("m", "adpRibose");
		CommunicationSymbol R5S3 = new CommunicationSymbol ("C", "G", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "adpRibose", null);
       
       List<ISymbol> R5S = new List<ISymbol> ();
       R5S.Add (R5S1);
       R5S.Add (R5S2);
       R5S.Add (R5S3);
       
       CommunicationCondition R5C = new CommunicationCondition (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
       
		Rule R5 = new Rule (R5P, R5S, R5C, 1.0f);

		///////////////////////////////////////////////////////////////////////
		
		CommunicationSymbol R6P = new CommunicationSymbol ("C");
		R6P.operationIdentifier = "C";
		
		BindingSymbol      R6S1 = new BindingSymbol  ("c", new Vector3(-0.2270f, 0.6361f, -0.9190f), new Vector3(-72.7191f, 34.9005f, -37.05637f), true);
		StructureSymbol     R6S2 = new StructureSymbol("m", "adpRibose");
		CommunicationSymbol R6S3 = new CommunicationSymbol ("C", "G", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "adpRibose", null);
		
		List<ISymbol> R6S = new List<ISymbol> ();
		R6S.Add (R6S1);
		R6S.Add (R6S2);
		R6S.Add (R6S3);
		
		CommunicationCondition R6C = new CommunicationCondition (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		Rule R6 = new Rule (R6P, R6S, R6C, 1.0f);

		///////////////////////////////////////////////////////////////////////

		CommunicationSymbol R7P = new CommunicationSymbol ("C");
		R7P.operationIdentifier = "G";
		
		BindingSymbol       R7S1 = new BindingSymbol  ("g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0));
		StructureSymbol     R7S2 = new StructureSymbol("m", "adpRibose");
		CommunicationSymbol R7S3 = new CommunicationSymbol ("C", "G", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "adpRibose", null);
		
		List<ISymbol> R7S = new List<ISymbol> ();
		R7S.Add (R7S1);
		R7S.Add (R7S2);
		R7S.Add (R7S3);
		
		CommunicationCondition R7C = new CommunicationCondition (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		Rule R7 = new Rule (R7P, R7S, R7C, 1.0f);

		rules.Add (R2);
		rules.Add (R3);
		rules.Add (R4);
		rules.Add (R5);
		rules.Add (R6);
		rules.Add (R7);
	}

	void setTestRules3()
	{
		///////////////////////////////////////////////////////////////////////
		
		CommunicationSymbol R2P = new CommunicationSymbol ("C");
		R2P.operationIdentifier = "M";
		
		BindingSymbol       R2S1 = new BindingSymbol  ("g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0));
		StructureSymbol     R2S2 = new StructureSymbol("m", "adpRibose");
		CommunicationSymbol R2S3 = new CommunicationSymbol ("C", "M", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "adpRibose", null);
		
		List<ISymbol> R2S = new List<ISymbol> ();
		R2S.Add (R2S1);
		R2S.Add (R2S2);
		R2S.Add (R2S3);
		
		CommunicationCondition R2C = new CommunicationCondition (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		Rule R2 = new Rule (R2P, R2S, R2C, 0.9f);
		
		///////////////////////////////////////////////////////////////////////
		
		CommunicationSymbol R3P = new CommunicationSymbol ("C");
		R3P.operationIdentifier = "M";
		
		BindingSymbol       R3S1 = new BindingSymbol  ("g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0));
		StructureSymbol     R3S2 = new StructureSymbol("m", "adpRibose");
		CommunicationSymbol R3S3 = new CommunicationSymbol ("C", "N", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "molecule", null);
		
		List<ISymbol> R3S = new List<ISymbol> ();
		R3S.Add (R3S1);
		R3S.Add (R3S2);
		R3S.Add (R3S3);
		
		CommunicationCondition R3C = new CommunicationCondition (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		Rule R3 = new Rule (R3P, R3S, R3C, 0.1f);
		
		///////////////////////////////////////////////////////////////////////
		
		CommunicationSymbol R4P = new CommunicationSymbol ("C");
		R4P.operationIdentifier = "N";
		
		BindingSymbol       R4S1 = new BindingSymbol  ("a", new Vector3(0.67309f, 0.52365f, 0.83809f), new Vector3(-45.8616f, -50.1757f, -86.2943f), true);
		StructureSymbol     R4S2 = new StructureSymbol("n", "molecule");
		CommunicationSymbol R4S3 = new CommunicationSymbol ("C", "M", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "adpRibose", null);
		
		List<ISymbol> R4S = new List<ISymbol> ();
		R4S.Add (R4S1);
		R4S.Add (R4S2);
		R4S.Add (R4S3);
		
		CommunicationCondition R4C = new CommunicationCondition (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		Rule R4 = new Rule (R4P, R4S, R4C, 1.0f);

		rules.Add (R2);
		rules.Add (R3);
		rules.Add (R4);
	}

	GameObject addObject(ref Turtle turtle, string prefabName)
	{
		GameObject prefab = Resources.Load(prefabName) as GameObject;
		GameObject mol = Instantiate(prefab, turtle.position, turtle.direction) as GameObject;

		if (polymerExample == 2 && prefabName == "molecule")
		{
			mol.renderer.material = diffTransBlue;
		}
		else
		{
			mol.renderer.material = diffWhite;
		}

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

		if (mol.GetComponent<TimeScaleTransparency> ())
			Destroy (mol.GetComponent<TimeScaleTransparency> ());

		// will be removed somehow
		mol.rigidbody.isKinematic = true;

		mol.transform.parent = transform;
		mol.transform.localScale = NewAgentSystem.agentScale * mol.transform.localScale;

		monomerCounting++;

		return mol;
	}

	void Derive()
	{
		CommunicationManager cql = communicationQueryObject.GetComponent<CommunicationManager> ();

		SortedDictionary<int, CommunicationSymbol> newActiveSymbols = new SortedDictionary<int, CommunicationSymbol> ();

		int indexOffset = 0;
		foreach (KeyValuePair<int, CommunicationSymbol> activeSymbol in activeSymbols)
		{
			CommunicationSymbol symbol = (CommunicationSymbol)state[activeSymbol.Key + indexOffset];

			cql.Remove(activeSymbol.Key);

			Rule rule = rules.Get (symbol);
			if(rule != null)
			{
				DestroyImmediate(symbol.operationResult);
				state.RemoveAt(activeSymbol.Key + indexOffset);

				List<ISymbol> newSymbols = rule.successor;

				for(int i = 0; i < newSymbols.Count; i++)
				{
					int newIndex = activeSymbol.Key + indexOffset + i;

					ISymbol newSymbol = null;

					if(newSymbols[i].GetType() == typeof(CommunicationSymbol))
					{
						newSymbol = new CommunicationSymbol((CommunicationSymbol)newSymbols[i]);

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

		activeSymbols = newActiveSymbols;
	}

	private void Interpret ()
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
				
				Vector3 bindingOrientation = ((BindingSymbol)symbol).bindingOrientation;

				current.position  = current.position + current.direction * (NewAgentSystem.agentScale * ((BindingSymbol)symbol).bindingPosition);
				current.direction = current.direction * Quaternion.Euler (bindingOrientation.x, bindingOrientation.y, bindingOrientation.z);
			}
			else if(symbol.GetType() == typeof(EndSymbol))
			{
				current = stack.Pop ();
			}
			else if(symbol.GetType() == typeof(CommunicationSymbol))
			{
				((CommunicationSymbol)symbol).fillTurtleValues(current);

				if(((CommunicationSymbol)symbol).operationIdentifier == "G")
				{
					current = stack.Pop();
				}
			}
		}
	}

	private void preEnviromentStep()
	{
		List<CommunicationQuery> queries = communicationQueryObject.GetComponent<CommunicationManager> ().getQueries ();
		
		for(int i = 0; i < queries.Count; i++)
		{
			if(activeSymbols.ContainsKey(queries[i].stateId))
			{
				activeSymbols[queries[i].stateId].operationTimer  = queries[i].time;

				if(monomerCounting < monomerCountingStop)
					activeSymbols[queries[i].stateId].operationResult = queries[i].result;
			}
		}
	}

	void postEnviromentStep()
	{
		int childCount = communications.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			DestroyImmediate(communications.transform.GetChild(0).gameObject);
		}

		CommunicationManager cql = communicationQueryObject.GetComponent<CommunicationManager> ();

		foreach(KeyValuePair<int, CommunicationSymbol> symbol in activeSymbols)
		{
			cql.Add(symbol.Key, symbol.Value);

			GameObject haloEffect = new GameObject();
			haloEffect.transform.position = symbol.Value.globalPosition;
			haloEffect.AddComponent("Halo");
			haloEffect.transform.parent = communications.transform;
		}
	}

	private void TimeStep()
	{
		preEnviromentStep ();

		Derive ();
		Interpret ();

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