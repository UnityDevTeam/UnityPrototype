using UnityEngine;
using System.Collections.Generic;
using System;

public class Rules
{
	
	private Dictionary<string, List<Rule>> _lookupTable = new Dictionary<string, List<Rule>> ();
	
	public void Add (Rule rule)
	{
		List<Rule> list;
		if (!_lookupTable.ContainsKey (rule.predecessor))
		{
			list = new List<Rule> ();
			_lookupTable [rule.predecessor] = list;
		} else {
			list = _lookupTable [rule.predecessor];
		}
		list.Add(rule);
	}
	
	public bool Contains (string module)
	{
		return _lookupTable.ContainsKey (module);
	}
	
	public Rule Get (string module)
	{
		if (!_lookupTable.ContainsKey (module))
		{
			return null;
		}
		
		List<Rule> list = _lookupTable [module];
		
		if (list.Count == 1)
		{
			return list [0];
		}
		
		float chance = UnityEngine.Random.value;
		
		foreach (Rule rule in list)
		{
			if (rule.probability <= chance)
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