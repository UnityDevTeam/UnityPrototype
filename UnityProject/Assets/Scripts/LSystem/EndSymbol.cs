using UnityEngine;
using System.Collections;

public class EndSymbol : ISymbol
{
	public EndSymbol()
	{
		id   = idCounter;
		name = "";

		idCounter++;
	}

	public EndSymbol(string nName)
	{
		id   = idCounter;
		name = nName;

		idCounter++;
	}

	public EndSymbol(int nId, string nName)
	{
		id   = nId;
		name = nName;
	}

	public EndSymbol(EndSymbol nSymbol)
	{
		id   = idCounter;
		name = nSymbol.name;

		idCounter++;
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
}
