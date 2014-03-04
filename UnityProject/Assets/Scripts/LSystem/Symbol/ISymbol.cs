using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[Serializable]
public class ISymbol : ScriptableObject
{
	public static int idCounter = 0;
	public string name;
	public int id;

	public void init(string nName)
	{
		id   = idCounter;
		name = nName;
		
		idCounter++;
	}

	public override bool Equals(System.Object obj)
	{
		if (obj == null)
		{
			return false;
		}
		
		ISymbol p = obj as ISymbol;
		if ((System.Object)p == null)
		{
			return false;
		}
		
		return (name == p.name);
	}

	public override int GetHashCode()
	{
		return name.GetHashCode();
	}

	public virtual string toString()
	{
		return name;
	}

	public virtual string toShortString()
	{
		return name;
	}
	
	public class EqualityComparer : IEqualityComparer<ISymbol>
	{
		public bool Equals(ISymbol x, ISymbol y)
		{
			if (x.GetType () == y.GetType ())
			{
				return x.Equals(y);
			}
			else
			{
				return false;
			}
			
		}
		
		public int GetHashCode(ISymbol x)
		{
			return  x.GetHashCode ();
		}
	}
}