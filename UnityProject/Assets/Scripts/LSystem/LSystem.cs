using UnityEngine;
using System;
using System.Collections.Generic;

public class LSystem : MonoBehaviour
{	
	public Symbol        axiom = new Symbol ("A");
	public List<ISymbol> state = new List<ISymbol>();

	[HideInInspector] public List<String>     molecule_names;
	[HideInInspector] public List<GameObject> molecule_objects;

	private Rules rules = new Rules ();
	private int symbolIdCounter = 0;
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

		setTestRules ();
	}

	void setTestRules()
	{
		Symbol A = new Symbol("A");
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
						newSymbol = new Symbol((Symbol)newSymbols[i]);
					}

					newSymbol.id = symbolIdCounter;
					symbolIdCounter++;

					newState.Add(newSymbol);
				}
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

	private void postEnviromentStep ()
	{
		communicationQueryObject.GetComponent<CommunicationQueryList> ().addQueries (sendCommunicationQuerries());
	}
	
	private void interpret ()
	{
		// move turtle move
	}

	private void TimeStep()
	{
		preEnviromentStep ();
		derive();
		interpret ();
		postEnviromentStep ();

		// debug
		printState ();
	}
	
	void Update()
	{
		timer += Time.deltaTime;

		if (timer > 3.0f)
		{
			TimeStep ();
			timer = 0.0f;
			counter++;
		}
	}
}