﻿using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class BindingSymbol : ISymbol
{
	public Vector3 bindingPosition    = Vector3.zero;
	public Vector3 bindingOrientation = Vector3.zero;
	public Vector3 orientAlteration   = Vector3.zero;
	public bool    isBranching        = false;

	public void init( string nName, Vector3 nPosition, Vector3 nOrientation, Vector3 nOrientAlteration, bool nIsBranching )
	{
		id                 = idCounter;
		name               = nName;
		bindingPosition    = nPosition;
		bindingOrientation = nOrientation;
		orientAlteration   = nOrientAlteration;
		isBranching        = nIsBranching;
		
		idCounter++;
	}

	public void init( BindingSymbol nSymbol )
	{
		init (nSymbol.name, nSymbol.bindingPosition, nSymbol.bindingOrientation, nSymbol.orientAlteration, nSymbol.isBranching);
	}

	public void alterOrientation()
	{
		float randomX = UnityEngine.Random.Range (-orientAlteration.x, orientAlteration.x);
		float randomY = UnityEngine.Random.Range (-orientAlteration.y, orientAlteration.y);
		float randomZ = UnityEngine.Random.Range (-orientAlteration.z, orientAlteration.z);

		bindingOrientation += new Vector3(randomX, randomY, randomZ);
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

	public override string toString()
	{
		return name  + "(" + bindingPosition.ToString() + ", " + bindingOrientation.ToString() + ")";
	}

	public override string toShortString()
	{
		return name;
	}
}