using UnityEngine;
using System.Collections.Generic;

public class Rule
{
	private Symbol       _predecessor;
	private List<Symbol> _successor;
	private float        _probability;
	
	public Symbol predecessor {
		get {
			return this._predecessor;
		}
	}
	
	public float probability {
		get {
			return this._probability;
		}
	}
	
	public List<Symbol> successor {
		get {
			return this._successor;
		}
	}

	Rule (Symbol predecessor, List<Symbol> sucessor, float probability)
	{
		_predecessor = predecessor;
		_successor   = sucessor;
		_probability = probability;
	}
}