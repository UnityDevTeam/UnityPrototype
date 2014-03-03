using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class BindingSymbol : ISymbol
{
	public Vector3 bindingPosition    = Vector3.zero;
	public Vector3 bindingOrientation = Vector3.zero;
	public bool    isBranching        = false;
	
	public BindingSymbol()
	{
		init ("", Vector3.zero, Vector3.zero, false);
	}
	
	public BindingSymbol( string nName )
	{
		init (nName, Vector3.zero, Vector3.zero, false);
	}

	public BindingSymbol( string nName, Vector3 nPosition, Vector3 nOrientation )
	{
		init (nName, nPosition, nOrientation, false);
	}

	public BindingSymbol( string nName, Vector3 nPosition, Vector3 nOrientation, bool nIsBranching )
	{
		init (nName, nPosition, nOrientation, nIsBranching);
	}

	public BindingSymbol( BindingSymbol nSymbol )
	{
		init (nSymbol);
	}

	public void init( string nName, Vector3 nPosition, Vector3 nOrientation, bool nIsBranching )
	{
		id                 = idCounter;
		name               = nName;
		bindingPosition    = nPosition;
		bindingOrientation = nOrientation;
		isBranching        = nIsBranching;
		
		idCounter++;
	}

	public void init( BindingSymbol nSymbol )
	{
		init (nSymbol.name, nSymbol.bindingPosition, nSymbol.bindingOrientation, nSymbol.isBranching);
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