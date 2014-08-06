using UnityEngine;
using System;
using System.Collections.Generic;

public class LSystem : MonoBehaviour
{
	[SerializeField] public List<ISymbol> alphabet;
	[SerializeField] public ISymbol       axiom;
	[SerializeField] public Rules         rules;
	                 public List<ISymbol> state = new List<ISymbol>();

	public SortedDictionary<int, CommunicationSymbol> activeSymbols = new SortedDictionary<int, CommunicationSymbol> ();

	[HideInInspector] public static float timeDelta = 0.1f;
		
	private GameObject communicationQueryObject = null;

	int monomerCounting = 0;
	public int monomerCountingStop = 100;
	
	public int exampleIndex = 1;

	public static bool canAddItem;

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

		if (!rules)
		{
			rules = ScriptableObject.CreateInstance<Rules>();
			rules.init();
		}

		switch (exampleIndex)
		{
		case 1:
			axiom    = PARP.axiom;
			alphabet = PARP.alphabet;
			rules    = PARP.rules;
			break;
		case 2:
			axiom    = Star.axiom;
			alphabet = Star.alphabet;
			rules    = Star.rules;
			break;
		case 3:
			axiom    = Copolymer.axiom;
			alphabet = Copolymer.alphabet;
			rules    = Copolymer.rules;
			break;
		case 4:
			axiom    = Cellulose.axiom;
			alphabet = Cellulose.alphabet;
			rules    = Cellulose.rules;
			break;
		case 5:
			axiom    = Tubulin.axiom;
			alphabet = Tubulin.alphabet;
			rules    = Tubulin.rules;
			break;
		case 6:
			axiom    = Showcase1.axiom;
			alphabet = Showcase1.alphabet;
			rules    = Showcase1.rules;
			break;
		default:
			// if necessary
			break;
		}

		state.Add(axiom);
		activeSymbols.Add (0, (CommunicationSymbol)axiom);
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

		if (mol.GetComponent<TimeScaleTransparency> ())
			Destroy (mol.GetComponent<TimeScaleTransparency> ());

		if (mol.GetComponent<Movement> ())
			Destroy (mol.GetComponent<Movement> ());

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

			float chance = 0.0f;

			Rule rule = rules.Get (symbol, out chance);
			if(rule != null)
			{
				DestroyImmediate(symbol.result);
				state.RemoveAt(activeSymbol.Key + indexOffset);

				List<ISymbol> newSymbols = rule.successor;

				for(int i = 0; i < newSymbols.Count; i++)
				{
					int newIndex = activeSymbol.Key + indexOffset + i;

					ISymbol newSymbol = null;

					if(newSymbols[i].GetType() == typeof(CommunicationSymbol))
					{
						newSymbol = ScriptableObject.CreateInstance<CommunicationSymbol>();
						((CommunicationSymbol)newSymbol).init((CommunicationSymbol)newSymbols[i]);

						newActiveSymbols.Add(newIndex, symbol);
					}
					else if(newSymbols[i].GetType() == typeof(BindingSymbol))
					{
						newSymbol = ScriptableObject.CreateInstance<BindingSymbol> ();
						((BindingSymbol)newSymbol).init((BindingSymbol)newSymbols[i]);
						((BindingSymbol)newSymbol).alterOrientation();
					}
					else if(newSymbols[i].GetType() == typeof(StructureSymbol))
					{
						newSymbol = ScriptableObject.CreateInstance<StructureSymbol> ();
						((StructureSymbol)newSymbol).init((StructureSymbol)newSymbols[i]);
					}
					else if(newSymbols[i].GetType() == typeof(EndSymbol))
					{
						newSymbol = ScriptableObject.CreateInstance<EndSymbol> ();
						((EndSymbol)newSymbol).init((EndSymbol)newSymbols[i]);
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
				activeSymbols[queries[i].stateId].timer  = queries[i].time;

				if(monomerCounting < monomerCountingStop)
					activeSymbols[queries[i].stateId].result = queries[i].result;
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
	
	void Update()
	{
		preEnviromentStep ();
		
		Derive ();
		
		Interpret ();
		
		postEnviromentStep ();

		canAddItem = (monomerCountingStop - monomerCounting) > 0;
	}

	public string[] alphabetArray()
	{
		List<string> lSystemAlphabet = new List<string> ();

		foreach(ISymbol symbol in alphabet)
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
				symbolStr += "StructureSymbol(" + ((StructureSymbol)symbol).structurePrefabName + ")";
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

		return lSystemAlphabet.ToArray ();
	}
}