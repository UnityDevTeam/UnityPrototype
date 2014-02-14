using UnityEngine;
using System.Collections.Generic;
using System;

public class StrRules
{
	private Dictionary<string, List<StrRule>> _lookupTable = new Dictionary<string, List<StrRule>> ();
	
	public void Add (StrRule rule)
	{
		List<StrRule> list;
		if (!_lookupTable.ContainsKey (rule.predecessor))
		{
			list = new List<StrRule> ();
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
	
	public bool Contains (string module)
	{
		return _lookupTable.ContainsKey (module);
	}
	
	public StrRule Get (string module)
	{
		if (!_lookupTable.ContainsKey (module))
		{
			return null;
		}
		
		List<StrRule> list = _lookupTable [module];
		
		if (list.Count == 1)
		{
			return list [0];
		}
		
		float chance      = UnityEngine.Random.value;
		float probability = 0.0f;
		
		foreach (StrRule rule in list)
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
		foreach (List<StrRule> list in _lookupTable.Values)
		{
			// Shortcut for modules with only 1 rule
			if (list.Count == 1 && list [0].probability != 1)
			{
				return false;
			}
			
			float acc = 0;
			foreach (StrRule rule in list)
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