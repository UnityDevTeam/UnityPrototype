using UnityEngine;
using System.Collections;

public class EndSymbol : ISymbol
{
	public EndSymbol()
	{
		id   = 0;
		name = "";
	}

	public EndSymbol(string nName)
	{
		id   = 0;
		name = nName;
	}

	public EndSymbol(int nId, string nName)
	{
		id   = nId;
		name = nName;
	}

	public EndSymbol(EndSymbol nSymbol)
	{
		id   = nSymbol.id;
		name = nSymbol.name;
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
