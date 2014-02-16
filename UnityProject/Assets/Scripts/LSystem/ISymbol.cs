using System.Collections.Generic;

public class ISymbol
{
	public string name;
	public int id;

	public ISymbol()
	{
		id   = 0;
		name = "";
	}

	public ISymbol(string nName)
	{
		id   = 0;
		name = nName;
	}

	public ISymbol(int nId, string nName)
	{
		id   = nId;
		name = nName;
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