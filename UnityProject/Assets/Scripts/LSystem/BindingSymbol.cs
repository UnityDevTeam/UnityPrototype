using UnityEngine;
using System.Collections;

public class BindingSymbol : ISymbol
{
	public Vector3 bindingPosition    = Vector3.zero;
	public Vector3 bindingOrientation = Vector3.zero;
	public bool    isBranching        = false;
	
	public BindingSymbol()
	{
		id                 = idCounter;
		name               = "";
		bindingPosition    = Vector3.zero;
		bindingOrientation = Vector3.zero;
		isBranching        = false;

		idCounter++;
	}
	
	public BindingSymbol( string nName )
	{
		id                 = idCounter;
		name               = nName;
		bindingPosition    = Vector3.zero;
		bindingOrientation = Vector3.zero;
		isBranching        = false;

		idCounter++;
	}

	public BindingSymbol( string nName, Vector3 nPosition, Vector3 nOrientation )
	{
		id                 = idCounter;
		name               = nName;
		bindingPosition    = nPosition;
		bindingOrientation = nOrientation;
		isBranching        = false;

		idCounter++;
	}

	public BindingSymbol( int nId, string nName, Vector3 nPosition, Vector3 nOrientation )
	{
		id                 = nId;
		name               = nName;
		bindingPosition    = nPosition;
		bindingOrientation = nOrientation;
		isBranching        = false;
	}

	public BindingSymbol( string nName, Vector3 nPosition, Vector3 nOrientation, bool nIsBranching )
	{
		id                 = idCounter;
		name               = nName;
		bindingPosition    = nPosition;
		bindingOrientation = nOrientation;
		isBranching        = nIsBranching;

		idCounter++;
	}

	public BindingSymbol( int nId, string nName, Vector3 nPosition, Vector3 nOrientation, bool nIsBranching )
	{
		id                 = nId;
		name               = nName;
		bindingPosition    = nPosition;
		bindingOrientation = nOrientation;
		isBranching        = nIsBranching;
	}
	
	public BindingSymbol( BindingSymbol nSymbol )
	{
		id                 = idCounter;
		name               = nSymbol.name;
		bindingPosition    = nSymbol.bindingPosition;
		bindingOrientation = nSymbol.bindingOrientation;
		isBranching        = nSymbol.isBranching;

		idCounter++;
	}
	
	public static bool operator ==(BindingSymbol x, BindingSymbol y) 
	{
		return (x.name == y.name) && (x.bindingPosition == y.bindingPosition) && (x.bindingOrientation == y.bindingOrientation) && (x.isBranching == y.isBranching);
	}
	
	public static bool operator !=(BindingSymbol x, BindingSymbol y) 
	{
		return (x.name != y.name) && (x.bindingPosition != y.bindingPosition) && (x.bindingOrientation != y.bindingOrientation) || (x.isBranching != y.isBranching);
	}
	
	public override bool Equals(System.Object obj)
	{
		if (obj == null)
		{
			return false;
		}
		
		BindingSymbol p = obj as BindingSymbol;
		if ((System.Object)p == null)
		{
			return false;
		}
		
		return (name == p.name) && (bindingPosition == p.bindingPosition) && (bindingOrientation == p.bindingOrientation) && (isBranching == p.isBranching);
	}
	
	public override int GetHashCode()
	{
		return name.GetHashCode() ^ bindingPosition.GetHashCode() ^ bindingOrientation.GetHashCode() ^ isBranching.GetHashCode();
	}
}