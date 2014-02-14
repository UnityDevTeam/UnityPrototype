using UnityEngine;
using System.Collections.Generic;

public class CommunicationSymbol : ISymbol
{
	private KeyValuePair<string,     bool> _operationIdentifier;
	private KeyValuePair<Vector3,    bool> _operationPosition;
	private KeyValuePair<Quaternion, bool> _operationOrientation;
	private KeyValuePair<float,      bool> _operationTimer;
	private KeyValuePair<GameObject, bool> _operationResult;

	public string operationIdentifier
	{
		get { return this._operationIdentifier.Key; }
		set { this._operationIdentifier = new KeyValuePair<string, bool>(value, true); }
	}

	public bool operationIdentifierVar
	{
		get { return this._operationIdentifier.Value; }
		set { this._operationIdentifier = new KeyValuePair<string, bool>(this._operationIdentifier.Key, value ); }
	}

	public Vector3 operationPosition
	{
		get { return this._operationPosition.Key; }
		set { this._operationPosition = new KeyValuePair<Vector3, bool>(value, true); }
	}
	
	public bool operationPositionVar
	{
		get { return this._operationPosition.Value; }
		set { this._operationPosition = new KeyValuePair<Vector3, bool>(this._operationPosition.Key, value ); }
	}

	public Quaternion operationOrientation
	{
		get { return this._operationOrientation.Key; }
		set { this._operationOrientation = new KeyValuePair<Quaternion, bool>(value, true); }
	}
	
	public bool operationOrientationVar
	{
		get { return this._operationOrientation.Value; }
		set { this._operationOrientation = new KeyValuePair<Quaternion, bool>(this._operationOrientation.Key, value ); }
	}

	public float operationTimer
	{
		get { return this._operationTimer.Key; }
		set { this._operationTimer = new KeyValuePair<float, bool>(value, true); }
	}
	
	public bool operationTimerVar
	{
		get { return this._operationTimer.Value; }
		set { this._operationTimer = new KeyValuePair<float, bool>(this._operationTimer.Key, value ); }
	}

	public GameObject operationResult
	{
		get { return this._operationResult.Key; }
		set { this._operationResult = new KeyValuePair<GameObject, bool>(value, true); }
	}
	
	public bool operationResultVar
	{
		get { return this._operationResult.Value; }
		set { this._operationResult = new KeyValuePair<GameObject, bool>(this._operationResult.Key, value ); }
	}



	public CommunicationSymbol()
	{
		name = "";
	}
	
	public CommunicationSymbol( string nName, string nOperationIdentifier, Vector3 nOperationPosition, Quaternion nOperationOrientation, float nOperationTimer, GameObject nOperationResult )
	{
		name                 = nName;
		operationIdentifier  = nOperationIdentifier;
		operationPosition    = nOperationPosition;
		operationOrientation = nOperationOrientation;
		operationTimer       = nOperationTimer;
		operationResult      = nOperationResult;
	}
	
	public static bool operator ==(CommunicationSymbol x, CommunicationSymbol y) 
	{
		if (x.name == y.name)
		{
			if(x.operationIdentifierVar && y.operationIdentifierVar)
				if(x.operationIdentifier != y.operationIdentifier)
					return false;

			if(x.operationPositionVar && y.operationPositionVar)
				if(x.operationPosition != y.operationPosition)
					return false;

			if(x.operationOrientationVar && y.operationOrientationVar)
				if(x.operationOrientation != y.operationOrientation)
					return false;

			if(x.operationTimerVar && y.operationTimerVar)
				if(x.operationTimer != y.operationTimer)
					return false;

			return true;
		}
		
		return false;
	}
	
	public static bool operator !=(CommunicationSymbol x, CommunicationSymbol y) 
	{
		if (x.name == y.name)
		{
			if(x.operationIdentifierVar && y.operationIdentifierVar)
				if(x.operationIdentifier != y.operationIdentifier)
					return true;
			
			if(x.operationPositionVar && y.operationPositionVar)
				if(x.operationPosition != y.operationPosition)
					return true;
			
			if(x.operationOrientationVar && y.operationOrientationVar)
				if(x.operationOrientation != y.operationOrientation)
					return true;
			
			if(x.operationTimerVar && y.operationTimerVar)
				if(x.operationTimer != y.operationTimer)
					return true;
			
			return false;
		}
		
		return true;
	}

	public override bool Equals(System.Object obj)
	{
		if (obj == null)
		{
			return false;
		}
		
		CommunicationSymbol p = obj as CommunicationSymbol;
		if ((System.Object)p == null)
		{
			return false;
		}
		
		if (name == p.name)
		{
			if(operationIdentifierVar && p.operationIdentifierVar)
				if(operationIdentifier != p.operationIdentifier)
					return false;
			
			if(operationPositionVar && p.operationPositionVar)
				if(operationPosition != p.operationPosition)
					return false;
			
			if(operationOrientationVar && p.operationOrientationVar)
				if(operationOrientation != p.operationOrientation)
					return false;
			
			if(operationTimerVar && p.operationTimerVar)
				if(operationTimer != p.operationTimer)
					return false;
			
			return true;
		}
		
		return false;
	}
	
	public override int GetHashCode()
	{
		return name.GetHashCode() ^
			_operationIdentifier.GetHashCode() ^
			_operationPosition.GetHashCode() ^
			_operationOrientation.GetHashCode() ^
			_operationTimer.GetHashCode() ^
			_operationResult.GetHashCode()
				;

	}
}

