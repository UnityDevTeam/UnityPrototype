using UnityEngine;
using System.Collections;

public class BindingSymbol : ISymbol
{
	public Vector3 bindingPosition    = Vector3.zero;
	public Vector3 bindingOrientation = Vector3.zero;
	
	public BindingSymbol()
	{
		id                 = 0;
		name               = "";
		bindingPosition    = Vector3.zero;
		bindingOrientation = Vector3.zero;
	}
	
	public BindingSymbol( string nName )
	{
		id                 = 0;
		name               = nName;
		bindingPosition    = Vector3.zero;
		bindingOrientation = Vector3.zero;
	}
	
	public BindingSymbol( int nId, string nName, Vector3 nPosition, Vector3 nOrientation )
	{
		id                 = nId;
		name               = nName;
		bindingPosition    = nPosition;
		bindingOrientation = nOrientation;
	}
	
	public BindingSymbol( BindingSymbol nSymbol )
	{
		id                 = nSymbol.id;
		name               = nSymbol.name;
		bindingPosition    = nSymbol.bindingPosition;
		bindingOrientation = nSymbol.bindingOrientation;
	}
	
	public static bool operator ==(BindingSymbol x, BindingSymbol y) 
	{
		return (x.name == y.name) && (x.bindingPosition == y.bindingPosition) && (x.bindingOrientation == y.bindingOrientation);
	}
	
	public static bool operator !=(BindingSymbol x, BindingSymbol y) 
	{
		return (x.name != y.name) && (x.bindingPosition != y.bindingPosition) && (x.bindingOrientation != y.bindingOrientation);
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
		
		return (name == p.name) && (bindingPosition == p.bindingPosition) && (bindingOrientation == p.bindingOrientation);
	}
	
	public override int GetHashCode()
	{
		return name.GetHashCode() ^ bindingPosition.GetHashCode() ^ bindingOrientation.GetHashCode();
	}
}