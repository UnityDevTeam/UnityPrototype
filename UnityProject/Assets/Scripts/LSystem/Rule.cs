using UnityEngine;
using System.Collections.Generic;

public class Rule
{
	private string _predecessor;
	private string _successor;
	private float _probability;
	
	public string predecessor {
		get {
			return this._predecessor;
		}
	}
	
	public float probability {
		get {
			return this._probability;
		}
	}
	
	public string successor {
		get {
			return this._successor;
		}
	}
	
	Rule (string predecessor, string sucessor, float probability)
	{
		_predecessor = predecessor;
		_successor = sucessor;
		_probability = probability;
	}
	
	public static Rule Build (string line)
	{
		string[] tokens = line.Split ('=');
		
		if (tokens.Length != 2) {
			return null;
		}
		
		string predecessor = tokens [0].Trim ();
		
		tokens = tokens [1].Trim ().Split (')');
		
		string probabilityString = tokens [0].Substring (1);
		string successor = tokens [1];
		
		float probability = float.Parse (probabilityString);
		
		return new Rule (predecessor, successor, probability);
	}
	
}