using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class Rules : ScriptableObject
{
	[SerializeField] public List<Rule> _lookupTable;

	public void init()
	{
		_lookupTable = new List<Rule> ();
	}

	public int getRulesCount()
	{
		return _lookupTable.Count;
	}

	public Rule getRule(int index)
	{
		return _lookupTable [index];
	}

	public void setRule(int index, Rule rule)
	{
		_lookupTable [index] = rule;
	}

	public void Add (Rule rule)
	{
		_lookupTable.Add (rule);
	}

	public void Remove (int index)
	{
		_lookupTable.RemoveAt(index);
	}
	
	public void Clear()
	{
		_lookupTable.Clear ();
	}

	private List<Rule> conditioning(ISymbol module, List<Rule> rules)
	{
		List<Rule> result = new List<Rule> ();

		for (int i = 0; i < rules.Count; i++)
		{
			if(rules[i].passCondition(module))
				result.Add(rules[i]);
		}

		return result;
	}
	
	public Rule Get (ISymbol module, out float chance)
	{
		chance = 1.0f;
		List<Rule> list = new List<Rule> ();
		foreach (Rule row in _lookupTable)
		{
			if((CommunicationSymbol)module == (CommunicationSymbol)row.predecessor)
				list.Add(row);
		}

		list = conditioning(module, list);

		
		if (list.Count == 1)
		{
			chance = 2.0f;
			return list [0];
		}
		
		chance      = UnityEngine.Random.value;
		float probability = 0.0f;
		
		foreach (Rule rule in list)
		{
			probability += rule.probability;
			if (probability >= chance)
			{
				return rule;
			}
		}
		
		return null;
	}

	/*
	public bool CheckProbabilities ()
	{
		foreach (List<Rule> list in _lookupTable.Values)
		{
			// Shortcut for modules with only 1 rule
			if (list.Count == 1 && list [0].probability != 1)
			{
				return false;
			}
			
			float acc = 0;
			foreach (Rule rule in list)
			{
				acc += rule.probability;
			}
			
			if (acc != 1)
			{
				return false;
			}
		}
		
		return true;
	}
	*/
	
}
