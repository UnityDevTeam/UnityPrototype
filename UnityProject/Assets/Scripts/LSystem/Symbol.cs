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

	public ISymbol(int nId, string nName)
	{
		id   = nId;
		name = nName;
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
			return  x.name.GetHashCode ();
		}
	}
}