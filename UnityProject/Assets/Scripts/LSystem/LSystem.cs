using UnityEngine;
using System;
using System.Collections.Generic;

public class LSystem : MonoBehaviour
{	
	private int symbolIdCounter = 1;
	public ISymbol       axiom = new ISymbol ( 0, "A" );
	public List<ISymbol> state = new List<ISymbol>();

	[HideInInspector] public List<String>     molecule_names;
	[HideInInspector] public List<GameObject> molecule_objects;

	private Rules rules = new Rules ();

	public string strState;

	int counter = 0;

	float timer = 0.0f;
	
	private GameObject communicationQueryObject = null;
	
	void Awake()
	{
		if (!communicationQueryObject)
		{
			communicationQueryObject = GameObject.Find("Communication Query");
			if(!communicationQueryObject)
			{
				communicationQueryObject = new GameObject("Communication Query");
				communicationQueryObject.AddComponent<CommunicationQueryList>();
			}
		}

		state.Add(axiom);


		setTestState ();
		interpret ();
		//setTestRules ();
	}

	void setTestState ()
	{
		BindingSymbol   g0  = new BindingSymbol  (0,  "g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0));
		StructureSymbol m1  = new StructureSymbol(1,  "m", "testAgent2");
		BindingSymbol   g2  = new BindingSymbol  (2,  "g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0));
		StructureSymbol m3  = new StructureSymbol(3,  "m", "testAgent2");
		BindingSymbol   g4  = new BindingSymbol  (4,  "g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0));
		StructureSymbol m5  = new StructureSymbol(5,  "m", "testAgent2");
		BindingSymbol   g6  = new BindingSymbol  (6,  "g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0));
		StructureSymbol m7  = new StructureSymbol(7,  "m", "testAgent2");
		BindingSymbol   g8  = new BindingSymbol  (8,  "g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0));
		StructureSymbol m9  = new StructureSymbol(9,  "m", "testAgent2");
		BindingSymbol   g10 = new BindingSymbol  (10, "g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0));
		StructureSymbol m11 = new StructureSymbol(11, "m", "testAgent2");
		BindingSymbol   g12 = new BindingSymbol  (12, "g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0));
		StructureSymbol m13 = new StructureSymbol(13, "m", "testAgent2");
		BindingSymbol   g14 = new BindingSymbol  (14, "g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0));
		StructureSymbol m15 = new StructureSymbol(15, "m", "testAgent2");
		BindingSymbol   g16 = new BindingSymbol  (16, "g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0));
		StructureSymbol m17 = new StructureSymbol(17, "m", "testAgent2");
		BindingSymbol   g18 = new BindingSymbol  (18, "g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0));
		StructureSymbol m19 = new StructureSymbol(19, "m", "testAgent2");
		BindingSymbol   g20 = new BindingSymbol  (20, "g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0));
		StructureSymbol m21 = new StructureSymbol(21, "m", "testAgent2");
		BindingSymbol   g22 = new BindingSymbol  (22, "g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0));
		StructureSymbol m23 = new StructureSymbol(23, "m", "testAgent2");
		BindingSymbol   g24 = new BindingSymbol  (24, "g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0));

		state.Add (g0);
		state.Add (m1);
		state.Add (g2);
		state.Add (m3);
		state.Add (g4);
		state.Add (m5);
		state.Add (g6);
		state.Add (m7);
		state.Add (g8);
		state.Add (m9);
		state.Add (g10);
		state.Add (m11);
		state.Add (g12);
		state.Add (m13);
		state.Add (g14);
		state.Add (m15);
		state.Add (g16);
		state.Add (m17);
		state.Add (g18);
		state.Add (m19);
		state.Add (g20);
		state.Add (m21);
		state.Add (g22);
		state.Add (m23);
		state.Add (g24);

	}
	/*
	void setTestRules()
	{
		Symbol A = new Symbol("A");
		A.id = symbolIdCounter;
		symbolIdCounter++;

		CommunicationSymbol ACG = new CommunicationSymbol ("C", "G", Vector3.zero, Quaternion.identity, 0.0f, "m", null);
		List<ISymbol> ARS = new List<ISymbol> ();
		ARS.Add (ACG);

		Rule P1 = new Rule (A, ARS, 1.0f);

		///////////////////////////////////////////////////////////////////////

		CommunicationSymbol C1 = new CommunicationSymbol ("C");
		C1.operationIdentifier = "G";

		Symbol m = new Symbol("m");

		CommunicationSymbol C1CG = new CommunicationSymbol ("C", "G", Vector3.zero, Quaternion.identity, 0.0f, "m", null);
		List<ISymbol> C1RS = new List<ISymbol> ();
		C1RS.Add (m);
		C1RS.Add (C1CG);

		CommunicationCondition C1C = new CommunicationCondition (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);

		Rule P2 = new Rule (C1, C1RS, C1C, 0.5f);

		////////////////////////////////////////////////////////////////////////
		
		CommunicationSymbol C2CG = new CommunicationSymbol ("C", "G", Vector3.zero, Quaternion.identity, 0.0f, "m", null);
		CommunicationSymbol C2CB = new CommunicationSymbol ("C", "B", Vector3.zero, Quaternion.identity, 0.0f, "m", null);

		List<ISymbol> C2RS = new List<ISymbol> ();
		C2RS.Add (m);
		C2RS.Add (C2CG);
		C2RS.Add (C2CB);
		
		Rule P3 = new Rule (C1, C2RS, C1C, 0.5f);

		/////////////////////////////////////////////////////////////////////////

		CommunicationSymbol C2 = new CommunicationSymbol ("C");
		C2.operationIdentifier = "B";
		
		Symbol b = new Symbol("b");
		
		CommunicationSymbol C3CG = new CommunicationSymbol ("C", "G", Vector3.zero, Quaternion.identity, 0.0f, "m", null);
		List<ISymbol> C3RS = new List<ISymbol> ();
		C3RS.Add (b);
		C3RS.Add (C3CG);
		
		Rule P4 = new Rule (C2, C3RS, C1C, 1.0f);

		/////////////////////////////////////////////////////////////////////////

		rules.Add (P1);
		rules.Add (P2);
		rules.Add (P3);
		rules.Add (P4);
	}
	*/
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
					}
					else
					{
						newSymbol = new StructureSymbol((StructureSymbol)newSymbols[i]);
					}

					newSymbol.id = symbolIdCounter;
					symbolIdCounter++;

					newState.Add(newSymbol);

					if(newSymbol.GetType() == typeof(CommunicationSymbol))
					{
						communicationQueryObject.GetComponent<CommunicationQueryList> ().Add(createQuery((CommunicationSymbol)newSymbol));
					}
				}

				communicationQueryObject.GetComponent<CommunicationQueryList> ().Remove(state[j].id);
			}
			else
			{
				newState.Add(module);
			}
		}
		state = newState;
	}

	void printState()
	{
		strState = "";
		for (int i = 0; i < state.Count; i++)
		{
			if(state[i].GetType() == typeof(CommunicationSymbol))
			{
				CommunicationSymbol cs = state[i] as CommunicationSymbol;
				strState += cs.name + "{" + cs.id + "}" + "(" + cs.operationIdentifier + ")";
			}
			else
			{
				strState += state[i].name + "{" + state[i].id + "}";
			}
		}
		print (strState);
	}

	public List<CommunicationQuery> sendCommunicationQuerries()
	{
		List<CommunicationQuery> queries = new List<CommunicationQuery> ();
		
		for(int i = 0; i < state.Count; i++)
		{
			if(state[i].GetType() == typeof(CommunicationSymbol))
			{
				CommunicationSymbol cs = state[i] as CommunicationSymbol;
				CommunicationQuery newQuery = new CommunicationQuery(cs.id, 0, cs.operationPosition, cs.operationOrientation, cs.operationResultType, cs.operationTimer);
				queries.Add(newQuery);
			}
		}
		
		return queries;
	}

	private CommunicationQuery createQuery(CommunicationSymbol symbol)
	{
		return new CommunicationQuery(symbol.id, 0, symbol.operationPosition, symbol.operationOrientation, symbol.operationResultType, symbol.operationTimer);
	}
	
	private void preEnviromentStep()
	{
		List<CommunicationQuery> queries = communicationQueryObject.GetComponent<CommunicationQueryList> ().getQueries ();

		for(int i = 0; i < state.Count; i++)
		{
			if(state[i].GetType() == typeof(CommunicationSymbol))
			{
				CommunicationSymbol cs = state[i] as CommunicationSymbol;

				for(int j = 0; j < queries.Count; j++)
				{
					if(queries[j].symbolId == cs.id)
					{
						cs.operationTimer  = queries[j].time;
						cs.operationResult = queries[j].result;
						break;
					}
				}
			}
		}
	}
	
	void DestroyOld()
	{
		int childs = transform.childCount;
		
		for (int i = childs - 1; i >= 0; i--)
		{
			GameObject.Destroy(transform.GetChild(i).gameObject);	
		}
	}

	void addObject(ref Turtle turtle, string prefabName)
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
	}

	void updateTurtle(ref Turtle turtle, Vector3 positionDelta, Vector3 orientationDelta)
	{
		turtle.position  = turtle.position + turtle.direction * positionDelta;
		turtle.direction = turtle.direction * Quaternion.Euler (orientationDelta.x, orientationDelta.y, orientationDelta.z);
	}

	private void interpret ()
	{
		DestroyOld();
		
		Turtle current = new Turtle (Quaternion.identity, Vector3.zero);
		Stack<Turtle> stack = new Stack<Turtle> ();
		
		for (int i = 0; i < state.Count; i++)
		{
			ISymbol symbol = state[i];

			if(symbol.GetType() == typeof(StructureSymbol))
			{
				addObject(ref current, ((StructureSymbol)symbol).structurePrefabName);
			}
			else if(symbol.GetType() == typeof(BindingSymbol))
			{
				updateTurtle(ref current, ((BindingSymbol)symbol).bindingPosition, ((BindingSymbol)symbol).bindingOrientation);
			}
		}
	}

	private void TimeStep()
	{
		preEnviromentStep ();
		derive();
		interpret ();

		// debug
		printState ();
	}
	
	void Update()
	{
		/*
		timer += Time.deltaTime;

		if (timer > 3.0f)
		{
			TimeStep ();
			timer = 0.0f;
			counter++;
		}
		*/
	}
}