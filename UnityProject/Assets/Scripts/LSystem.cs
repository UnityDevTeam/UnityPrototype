using UnityEngine;
using System;
using System.Collections.Generic;

public class LSystem : MonoBehaviour
{	
	public string   _axiom               = "F";
	public string[] _stringRules         = {"F=(0.97)fF", "F=(0.03)f[F]F"};
	public int      _numberOfDerivations = 3;

	public Rules    _rules = new Rules ();
	public string   _moduleString        = "fffffffffff[ffffffffffffF]ffffffffffffffF";

	[HideInInspector] public List<String>     molecule_names;
	[HideInInspector] public List<GameObject> molecule_objects;
	
	private struct Turtle
	{
		public Quaternion direction;
		public Quaternion idefault;
		public Vector3    position;
		
		public Turtle (Turtle other)
		{
			this.direction = other.direction;
			this.position  = other.position;
			this.idefault  = other.idefault;
		}
		
		public Turtle (Quaternion direction, Vector3 position, Vector3 step)
		{
			this.direction = direction;
			this.position  = position;
			this.idefault  = Quaternion.Inverse(this.direction);
		}
	}
	
	void Start ()
	{
		for (int i = 0; i < _stringRules.Length; i++)
		{
			Rule rule = Rule.Build (_stringRules[i]);
			_rules.Add (rule);
		}

		//Derive ();
		Interpret ();
	}
		
	void Derive ()
	{
		_moduleString = _axiom;
		for (int i = 0; i < Math.Max(1, _numberOfDerivations); i++)
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
		
	}

	void addObject(ref Turtle turtle, GameObject gameObject, int bindingIndex)
	{
		GameObject mol = Instantiate(gameObject, turtle.position, turtle.direction) as GameObject;
		mol.transform.parent = transform;

		Vector3 rotate = gameObject.GetComponent<MolScript>().bindingOrientations[bindingIndex];
		Vector3 move   = gameObject.GetComponent<MolScript>().bindingPositions[bindingIndex]; 

		turtle.position  = turtle.position + turtle.direction * move;
		turtle.direction = Quaternion.Euler (rotate.x, rotate.y, rotate.z) * (turtle.idefault * turtle.direction);
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
		
		Turtle current = new Turtle (Quaternion.identity, Vector3.zero, new Vector3 (0, 0, 0));
		Stack<Turtle> stack = new Stack<Turtle> ();

		for (int i = 0; i < _moduleString.Length; i++)
		{
			string module = _moduleString [i] + "";
			
			if (module == "F")
			{
				addObject(ref current, molecule_objects[molecule_names.IndexOf(module)], 0);
			}
			if (module == "f")
			{
				addObject(ref current, molecule_objects[molecule_names.IndexOf(module)], 0);
			}
			else if (module == "[")
			{
				stack.Push (current);
				current = new Turtle (current);
				addObject(ref current, molecule_objects[molecule_names.IndexOf("F")], 1);
			}
			else if (module == "]")
			{
				current = stack.Pop ();
			}
		}
	}
	
	void Update ()
	{

	}
}