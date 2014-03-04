﻿using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class StructureSymbol : ISymbol
{
	public string structurePrefabName = "";
	public GameObject structureObject = null;

	public void init(string nName, string nStructurePrefabName, GameObject nStructureObject)
	{
		id                  = idCounter;
		name                = nName;
		structurePrefabName = nStructurePrefabName;
		structureObject     = nStructureObject;
		
		idCounter++;
	}

	public void init(StructureSymbol nSymbol)
	{
		init (nSymbol.name, nSymbol.structurePrefabName, nSymbol.structureObject);
	}
	
	public static bool operator ==(StructureSymbol x, StructureSymbol y) 
	{
		return (x.name == y.name) && (x.structurePrefabName == y.structurePrefabName);
	}
	
	public static bool operator !=(StructureSymbol x, StructureSymbol y) 
	{
		return (x.name != y.name) || (x.structurePrefabName != y.structurePrefabName);
	}
	
	public override bool Equals(System.Object obj)
	{
		if (obj == null)
		{
			return false;
		}
		
		StructureSymbol p = obj as StructureSymbol;
		if ((System.Object)p == null)
		{
			return false;
		}
		
		return (name == p.name) && (structurePrefabName == p.structurePrefabName);
	}
	
	public override int GetHashCode()
	{
		return name.GetHashCode() ^ structurePrefabName.GetHashCode();
	}

	public override string toString()
	{
		return name + "(" + structurePrefabName + ")";
	}

	public override string toShortString()
	{
		return name;
	}

}