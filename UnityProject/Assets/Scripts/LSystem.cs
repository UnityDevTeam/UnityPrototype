using UnityEngine;
using System;
using System.Collections.Generic;

public class LSystem : MonoBehaviour
{	
	public string   _axiom        = "F";
	public string[] _stringRules  = {"A=(1.0)G", "F=(0.97)mG", "F=(0.03)mBG", "T=(1)[G]"};
	public string   _moduleString = "";

	[HideInInspector] public List<String>     molecule_names;
	[HideInInspector] public List<GameObject> molecule_objects;
	
	private Rules _rules = new Rules ();
	private List<GameObject> bindings;
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
		for (int i = 0; i < _stringRules.Length; i++)
		{
			Rule rule = Rule.Build (_stringRules[i]);
			_rules.Add (rule);
		}

		_moduleString = _axiom;

		bindings = new List<GameObject> ();
	}
		
	void Derive ()
	{
		string newModuleString = "";
		for (int j = 0; j < _moduleString.Length; j++)
		{
			string module = _moduleString [j] + "";
			
			if (!_rules.Contains (module))
			{
				newModuleString += module;
				continue;
			}
			
			Rule rule = _rules.Get (module);
			newModuleString += rule.successor;
		}
		_moduleString = newModuleString;
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

		for (int i = 0; i < _moduleString.Length; i++)
		{
			string module = _moduleString [i] + "";

			if (module == "m")
			{
				if (i > 0)
					updateTurtle(ref current, molecule_objects[molecule_names.IndexOf(module)], 0);
				addObject(ref current, molecule_objects[molecule_names.IndexOf(module)]);
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
			else if (module == "G")
			{
				// create something different
				updateTurtle(ref current, molecule_objects[molecule_names.IndexOf("m")], 0);

				GameObject binding = new GameObject(i.ToString());
				binding.transform.parent = transform;
				binding.transform.position = current.position;
				binding.AddComponent("MainScript");
				binding.GetComponent<MainScript>().bindingOrientation = current.direction;

				bindings.Add(binding);
			}
			else if (module == "B")
			{
				// create something different
				updateTurtle(ref current, molecule_objects[molecule_names.IndexOf("m")], 1);
				
				GameObject binding = new GameObject(i.ToString());
				binding.transform.parent = transform;
				binding.transform.position = current.position;
				binding.AddComponent("MainScript");
				binding.GetComponent<MainScript>().bindingOrientation = current.direction;
				
				bindings.Add(binding);
			}
		}
	}
	
	void Update ()
	{
		for (int i = 0; i < bindings.Count; i++)
		{
			if(bindings[i].GetComponent<MainScript>().finished)
			{
				changed = true;
				int index = Convert.ToInt32( bindings[i].name );
				Destroy(bindings[i]);
				if (_moduleString[index] == 'G')
				{
					_moduleString = ReplaceAtIndex(index, 'F', _moduleString);
				}
				else if (_moduleString[index] == 'B')
				{
					_moduleString = ReplaceAtIndex(index, 'T', _moduleString);
				}
			}
		}

		bindings.RemoveAll (p => p.GetComponent<MainScript> ().finished);

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