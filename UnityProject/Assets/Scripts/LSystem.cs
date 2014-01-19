using UnityEngine;
using System;
using System.Collections.Generic;

public class LSystem : MonoBehaviour
{	
	public string   _axiom        = "F";
	public string[] _stringRules  = {"F=(0.97)fF", "F=(0.03)f[F]F"};
	public string   _moduleString = "";

	[HideInInspector] public List<String>     molecule_names;
	[HideInInspector] public List<GameObject> molecule_objects;

	private float mTime = 0.0f;
	private Rules _rules = new Rules ();
	
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
			
			if (module == "F")
			{
				addObject(ref current, molecule_objects[molecule_names.IndexOf(module)]);
			}
			if (module == "f")
			{
				if (i > 0)
					updateTurtle(ref current, molecule_objects[molecule_names.IndexOf(module)], 0);
				addObject(ref current, molecule_objects[molecule_names.IndexOf(module)]);
			}
			else if (module == "[")
			{
				stack.Push(current);
				current = new Turtle (current);

				updateTurtle(ref current, molecule_objects[molecule_names.IndexOf("F")], 1);
				addObject(ref current, molecule_objects[molecule_names.IndexOf("F")]);
			}
			else if (module == "]")
			{
				current = stack.Pop ();
			}
		}
	}
	
	void Update ()
	{
		mTime += Time.deltaTime;
		if (mTime > 1.0f)
		{
			Derive();
			Interpret();
			mTime = 0.0f;
		}

	}
}