using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class EndSymbol : ISymbol
{
	public void init(EndSymbol nSymbol)
	{
		init (nSymbol.name);
	}

	public override bool Equals(System.Object obj)
	{
		if (obj == null)
		{
			return false;
		}
		
		EndSymbol p = obj as EndSymbol;
		if ((System.Object)p == null)
		{
			return false;
		}
		
		return true;
	}

	public override int GetHashCode()
	{
		return name.GetHashCode();
	}

	public override string toString()
	{
		return name;
	}

	public override string toShortString()
	{
		return name;
	}
}
