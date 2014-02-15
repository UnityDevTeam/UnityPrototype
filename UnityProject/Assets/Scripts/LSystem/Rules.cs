using UnityEngine;
using System.Collections.Generic;
using System;

public class Rules
{
	private Dictionary<ISymbol, List<Rule>> _lookupTable = new Dictionary<ISymbol, List<Rule>> (new ISymbol.EqualityComparer ());
	
	public void Add (Rule rule)
	{
		List<Rule> list;
		if (!_lookupTable.ContainsKey (rule.predecessor))
		{
			list = new List<Rule> ();
			_lookupTable [rule.predecessor] = list;
		}
		else
		{
			list = _lookupTable [rule.predecessor];
		}
		
		list.Add(rule);
	}
	
	public void Clear()
	{
		_lookupTable.Clear ();
	}
	
	public bool Contains (ISymbol module)
	{
		return _lookupTable.ContainsKey(module);
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
	
	public Rule Get (ISymbol module)
	{
		if (!_lookupTable.ContainsKey (module))
		{
			return null;
		}
		
		List<Rule> list = conditioning(module, _lookupTable [module]);

		
		if (list.Count == 1)
		{
			return list [0];
		}
		
		float chance      = UnityEngine.Random.value;
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
	
}
