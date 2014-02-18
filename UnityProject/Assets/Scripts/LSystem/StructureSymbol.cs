using UnityEngine;
using System.Collections;

public class StructureSymbol : ISymbol
{
	public string structurePrefabName = "";
	public GameObject structureObject = null;

	public StructureSymbol()
	{
		id                  = idCounter;
		name                = "";
		structurePrefabName = "";
		structureObject     = null;

		idCounter++;
	}
	
	public StructureSymbol( string nName )
	{
		id                  = idCounter;
		name                = nName;
		structurePrefabName = "";
		structureObject     = null;

		idCounter++;
	}

	public StructureSymbol( string nName, string nStructurePrefabName )
	{
		id                  = idCounter;
		name                = nName;
		structurePrefabName = nStructurePrefabName;
		structureObject     = null;

		idCounter++;
	}
	
	public StructureSymbol( int nId, string nName, string nStructurePrefabName )
	{
		id                  = nId;
		name                = nName;
		structurePrefabName = nStructurePrefabName;
		structureObject     = null;
	}
	
	public StructureSymbol( StructureSymbol nSymbol )
	{
		id                  = idCounter;
		name                = nSymbol.name;
		structurePrefabName = nSymbol.structurePrefabName;
		structureObject     = nSymbol.structureObject;

		idCounter++;
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