using System.Collections.Generic;

public class ISymbol
{
	public string name;
	public int id;

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
			return  x.name.GetHashCode ();
		}
	}
}

public class Symbol : ISymbol
{
	public Symbol()
	{
		id   = 0;
		name = "";
	}

	public Symbol( string nName )
	{
		id   = 0;
		name = nName;
	}

	public Symbol( int nId, string nName )
	{
		id   = nId;
		name = nName;
	}

	public Symbol( Symbol nSymbol )
	{
		id   = nSymbol.id;
		name = nSymbol.name;
	}

	public static bool operator ==(Symbol x, Symbol y) 
	{
		return (x.name == y.name);
	}
	
	public static bool operator !=(Symbol x, Symbol y) 
	{
		return (x.name != y.name);
	}

	public override bool Equals(System.Object obj)
	{
		if (obj == null)
		{
			return false;
		}

		Symbol p = obj as Symbol;
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
}
