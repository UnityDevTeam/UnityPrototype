using UnityEngine;
using System.Collections.Generic;

public class Rule
{
	private ISymbol                _predecessor;
	private List<ISymbol>          _successor;
	private float                  _probability;
	private CommunicationCondition _condition;
	
	public ISymbol predecessor {
		get {
			return this._predecessor;
		}
	}
	
	public float probability {
		get {
			return this._probability;
		}
	}
	
	public List<ISymbol> successor {
		get {
			return this._successor;
		}
	}

	public CommunicationCondition condition {
		get {
			return this._condition;
		}
		set {
			this._condition = value;
		}
	}

	public Rule (ISymbol predecessor, List<ISymbol> sucessor, float probability)
	{
		_predecessor = predecessor;
		_successor   = sucessor;
		_probability = probability;
		_condition   = null;
	}

	public Rule (ISymbol predecessor, List<ISymbol> sucessor, CommunicationCondition condition, float probability)
	{
		_predecessor = predecessor;
		_successor   = sucessor;
		_probability = probability;
		_condition   = condition;
	}

	public bool passCondition(ISymbol symbol)
	{
		if (_condition == null)
			return true;

		if (symbol.GetType () == typeof(CommunicationSymbol))
		{
			return _condition.Evaluate ((CommunicationSymbol)symbol);
		}

		return false;
	}
}