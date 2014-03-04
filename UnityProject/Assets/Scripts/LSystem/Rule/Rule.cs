using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class Rule : ScriptableObject
{
	public ISymbol                predecessor;
	public List<ISymbol>          successor;
	public float                  probability;
	public CommunicationCondition condition;

	public Rule ()
	{
		predecessor = null;
		probability = 1;
		successor   = new List<ISymbol> ();
		condition   = null;
	}

	public Rule (ISymbol nPredecessor, List<ISymbol> nSuccessor, float nProbability)
	{
		predecessor = nPredecessor;
		successor   = nSuccessor;
		probability = nProbability;
		condition   = null;
	}

	public Rule (ISymbol nPredecessor, List<ISymbol> nSucessor, CommunicationCondition nCondition, float nProbability)
	{
		predecessor = nPredecessor;
		successor   = nSucessor;
		probability = nProbability;
		condition   = nCondition;
	}

	public void init()
	{
		predecessor = null;
		probability = 1;
		successor   = new List<ISymbol> ();
		condition   = null;
	}

	public void init(ISymbol nPredecessor, List<ISymbol> nSucessor, CommunicationCondition nCondition, float nProbability)
	{
		predecessor = nPredecessor;
		successor   = nSucessor;
		condition   = nCondition;
		probability = nProbability;
	}

	public bool passCondition(ISymbol symbol)
	{
		if (condition == null)
			return true;

		if (symbol.GetType () == typeof(CommunicationSymbol))
		{
			return condition.Evaluate ((CommunicationSymbol)symbol);
		}

		return false;
	}
}