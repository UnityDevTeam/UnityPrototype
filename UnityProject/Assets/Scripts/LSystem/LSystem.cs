using UnityEngine;
using System;
using System.Collections.Generic;

public class LSystem : MonoBehaviour
{	
	public Symbol       axiom = new Symbol ("A");
	public List<Symbol> state = new List<Symbol>();

	[HideInInspector] public List<String>     molecule_names;
	[HideInInspector] public List<GameObject> molecule_objects;
	
	private GameObject agentsSystem = null;
	private Rules rules = new Rules ();

	public string strState;
	
	Dictionary<ISymbol, string> testing;
	
	void Awake()
	{
		ISymbol.EqualityComparer comparer = new ISymbol.EqualityComparer ();
		testing = new Dictionary<ISymbol, string> (comparer);

		Symbol aaa = new Symbol("aaa");
		Symbol bbb = new Symbol("bbb");
		Symbol ccc = new Symbol("ccc");

		Symbol a = new Symbol ("aaa");

		CommunicationSymbol cs = new CommunicationSymbol ("aaa", "G", new Vector3 (1.0f, 1.0f, 1.0f), new Quaternion (1.0f, 1.0f, 1.0f, 1.0f), 10.0f, null);
		CommunicationSymbol cs1 = new CommunicationSymbol ("aaa", "A", new Vector3 (1.0f, 1.0f, 1.0f), new Quaternion (1.0f, 1.0f, 1.0f, 1.0f), 10.0f, null);

		cs1.operationIdentifierVar = false;

		testing.Add(aaa, "a");
		testing.Add(bbb, "b");
		testing.Add(ccc, "c");
		testing.Add(cs, "d");

		if (aaa == a) {
			print ("1 testing");
		}
		
		
		if (testing.ContainsKey (cs1))
		{
			print ("2 testing");
		}
		/*
		state.Add(axiom);
				
		agentsSystem = new GameObject("agentsSystem");
		agentsSystem.AddComponent("AgentsSystem");
		*/
	}
	
	void Derive ()
	{
		List<Symbol> newState = new List<Symbol> ();
		for (int j = 0; j < state.Count; j++)
		{
			Symbol module = state[j];
			
			if (!rules.Contains (module))
			{
				newState.Add(module);
				continue;
			}
			
			Rule rule = rules.Get (module);
			newState.AddRange(rule.successor);
		}
		state = newState;
	}
	
	void updateTurtle(ref Turtle turtle, GameObject gameObject, int bindingIndex)
	{
		Vector3 rotate = gameObject.GetComponent<MolScript>().bindingOrientations[bindingIndex];
		Vector3 move   = gameObject.GetComponent<MolScript>().bindingPositions[bindingIndex]; 
		
		turtle.position  = turtle.position + turtle.direction * move;
		turtle.direction = turtle.direction * Quaternion.Euler (rotate.x, rotate.y, rotate.z);
	}
	
	void addObject(ref Turtle turtle, GameObject go)
	{
		go.GetComponent<MolScript> ().life = 2.0f;
		GameObject mol = Instantiate(go, turtle.position, turtle.direction) as GameObject;
		mol.GetComponent<Rigidbody> ().isKinematic = true;
		mol.transform.parent = transform;
	}
	
	void DestroyOld()
	{
		int childs = transform.childCount;
		
		for (int i = childs - 1; i >= 0; i--)
		{
			GameObject.Destroy(transform.GetChild(i).gameObject);	
		}
	}
	
	void Interpret ()
	{
		DestroyOld();
		
		Turtle current = new Turtle (Quaternion.identity, Vector3.zero);
		Stack<Turtle> stack = new Stack<Turtle> ();
		/*
		for (int i = 0; i < state.Count; i++)
		{
			Symbol module = state[i];
			
			if (module.name == "m")
			{
				if (i > 0)
					updateTurtle(ref current, molecule_objects[molecule_names.IndexOf(module.name)], 0);
				addObject(ref current, molecule_objects[molecule_names.IndexOf(module.name)]);
			}
			else if (module == "[")
			{
				stack.Push(current);
				current = new Turtle (current);
				
				updateTurtle(ref current, molecule_objects[molecule_names.IndexOf("m")], 1);
				addObject(ref current, molecule_objects[molecule_names.IndexOf("m")]);
			}
			else if (module == "]")
			{
				current = stack.Pop ();
			}
		}
		*/
	}

	void Query()
	{
		strState = "";
		for (int i = 0; i < state.Count; i++)
		{
			strState += state[i].name;
		}

	}
	
	private void TimeStep()
	{
		Derive();
		Query ();
		Interpret ();
	}
	
	void FixedUpdate()
	{
		//TimeStep ();
	}
}