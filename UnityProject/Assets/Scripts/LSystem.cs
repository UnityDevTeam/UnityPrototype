using UnityEngine;
using System;
using System.Collections.Generic;

public class LSystem : MonoBehaviour
{	
	public string   axiom                = "A";
	public string[] lSystemRulesStr      = {"A=(1.0)G", "F=(0.97)mG", "F=(0.03)mBG", "T=(1)[G]"};
	public string[] communicatorRulesStr = {"G=F", "B=T"};
	public string   lSystemState         = "";

	[HideInInspector] public List<String>     molecule_names;
	[HideInInspector] public List<GameObject> molecule_objects;

	private GameObject agentsSystem = null;

	private Rules                  lSystemRules      = new Rules ();
	private Dictionary<char, char> communicatorRules = new Dictionary<char, char> ();

	private List<Vector3> structureIdentification;
	private bool changed = true;

	private struct Turtle
	{
		public Quaternion direction;
		public Vector3    position;
		
		public Turtle (Turtle other)
		{
			this.direction = other.direction;
			this.position  = other.position;
		}
		
		public Turtle (Quaternion direction, Vector3 position)
		{
			this.direction = direction;
			this.position  = position;
		}
	}
	
	void Start ()
	{
		lSystemState = axiom;
		updateRules ();

		structureIdentification = new List<Vector3> ();

		agentsSystem = new GameObject("agentsSystem");
		agentsSystem.AddComponent("AgentsSystem");
	}

	public void updateRules()
	{
		lSystemRules.Clear ();
		communicatorRules.Clear ();

		for (int i = 0; i < lSystemRulesStr.Length; i++)
		{
			Rule rule = Rule.Build (lSystemRulesStr[i]);
			lSystemRules.Add (rule);
		}

		for (int i = 0; i < communicatorRulesStr.Length; i++)
		{
			string[] ruleSides = communicatorRulesStr[i].Split('=');

			if (ruleSides.Length != 2)
			{
				// TODO: need some kind of warning
				return;
			}

			communicatorRules.Add(ruleSides[0][0], ruleSides[1][0]);
		}
	}
		
	void Derive ()
	{
		string newState = "";
		for (int j = 0; j < lSystemState.Length; j++)
		{
			string module = lSystemState [j] + "";
			
			if (!lSystemRules.Contains (module))
			{
				newState += module;
				continue;
			}
			
			Rule rule = lSystemRules.Get (module);
			newState += rule.successor;
		}
		lSystemState = newState;
	}

	void updateTurtle(ref Turtle turtle, GameObject gameObject, int bindingIndex)
	{
		Vector3 rotate = gameObject.GetComponent<MolScript>().bindingOrientations[bindingIndex];
		Vector3 move   = gameObject.GetComponent<MolScript>().bindingPositions[bindingIndex]; 
		
		turtle.position  = turtle.position + turtle.direction * move;
		turtle.direction = turtle.direction * Quaternion.Euler (rotate.x, rotate.y, rotate.z);
	}

	void addObject(ref Turtle turtle, GameObject gameObject)
	{
		GameObject mol = Instantiate(gameObject, turtle.position, turtle.direction) as GameObject;
		mol.GetComponent<Rigidbody> ().isKinematic = true;
		mol.transform.parent = transform;
	}

	void DestroyOld()
	{
		structureIdentification.Clear ();

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

		for (int i = 0; i < lSystemState.Length; i++)
		{
			string module = lSystemState [i] + "";

			if (module == "m")
			{
				if (i > 0)
					updateTurtle(ref current, molecule_objects[molecule_names.IndexOf(module)], 0);
				addObject(ref current, molecule_objects[molecule_names.IndexOf(module)]);
				structureIdentification.Add(current.position);
			}
			else if (module == "[")
			{
				stack.Push(current);
				current = new Turtle (current);

				updateTurtle(ref current, molecule_objects[molecule_names.IndexOf("m")], 1);
				addObject(ref current, molecule_objects[molecule_names.IndexOf("m")]);
				structureIdentification.Add(current.position);
			}
			else if (module == "]")
			{
				current = stack.Pop ();
				structureIdentification.Add(Vector3.zero);
			}
			else if (module == "G")
			{
				updateTurtle(ref current, molecule_objects[molecule_names.IndexOf("m")], 0);

				agentsSystem.GetComponent<AgentsSystem>().createLocalSystem(i.ToString(), current.position, current.direction);
				structureIdentification.Add(current.position);
			}
			else if (module == "B")
			{
				Turtle tempTurtle = new Turtle(current);
				updateTurtle(ref tempTurtle, molecule_objects[molecule_names.IndexOf("m")], 1);

				agentsSystem.GetComponent<AgentsSystem>().createLocalSystem(i.ToString(), tempTurtle.position, tempTurtle.direction);
				structureIdentification.Add(tempTurtle.position);
			}
		}
	}
	
	void Update ()
	{
		List<Vector3> finishedBindings = agentsSystem.GetComponent<AgentsSystem> ().CheckLocalAgentsSystems ();
		if (finishedBindings.Count > 0)
		{
			for (int i = 0; i < finishedBindings.Count; i++)
			{
				for(int j = 0; j < structureIdentification.Count; j++)
				{
					if(structureIdentification[j] == finishedBindings[i])
					{
						char symbol = lSystemState[j];
						if (communicatorRules.ContainsKey(symbol))
						{
							lSystemState = ReplaceAtIndex(j, communicatorRules[symbol], lSystemState);
						}
					}
				}
			}
			changed = true;
			agentsSystem.GetComponent<AgentsSystem> ().RemoveFinishedLocalAgentsSystems (finishedBindings);
		}

		if (changed)
		{
			Derive();
			Interpret ();
			changed = false;
		}
	}

	static string ReplaceAtIndex(int i, char value, string word)
	{
		char[] letters = word.ToCharArray();
		letters[i] = value;
		return new string (letters);
	}
}