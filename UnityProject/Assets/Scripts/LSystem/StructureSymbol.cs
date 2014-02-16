﻿using UnityEngine;
using System.Collections;

public class StructureSymbol : ISymbol
{
	public string structurePrefabName = "";

	public StructureSymbol()
	{
		id                  = 0;
		name                = "";
		structurePrefabName = "";
	}
	
	public StructureSymbol( string nName )
	{
		id                  = 0;
		name                = nName;
		structurePrefabName = "";
	}
	
	public StructureSymbol( int nId, string nName, string nStructurePrefabName )
	{
		id                  = nId;
		name                = nName;
		structurePrefabName = nStructurePrefabName;
	}
	
	public StructureSymbol( StructureSymbol nSymbol )
	{
		id                  = nSymbol.id;
		name                = nSymbol.name;
		structurePrefabName = nSymbol.structurePrefabName;
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
}