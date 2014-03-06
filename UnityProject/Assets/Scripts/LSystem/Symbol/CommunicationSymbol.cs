using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class CommunicationSymbol : ISymbol
{
	// this need more inteligent solution
	public string         process;
	public bool           processVar;
	public Vector3        position;
	public bool           positionVar;
	public Quaternion     orientation;
	public bool           orientationVar;
	public string         resultType;
	public bool           resultTypeVar;
	[SerializeField] public AnimationCurve probability;

	// filled by enviroment
	public float      timer;
	public bool       timerVar;
	public GameObject result;
	public bool       resultVar;

	// filled by interpret
	private Vector3    _turtlePosition    = Vector3.zero;
	private Quaternion _turtleOrientation = Quaternion.identity;

	// operationPosition and operationOrientation transformed to global
	private Vector3    _globalPosition    = Vector3.zero;
	private Quaternion _globalOrientation = Quaternion.identity;

	public Vector3 globalPosition
	{
		get { return this._globalPosition; }
	}
	
	public Quaternion globalOrientation
	{
		get { return this._globalOrientation; }
	}

	public void init( string nName, string nProcess)
	{
		id         = idCounter;
		name       = nName;
		process    = nProcess;
		processVar = true;
		
		idCounter++;
	}

	public void init( string nName, string nProcess, Vector3 nPosition, Quaternion nOrientation, float nTimer, string nResultType, GameObject nResult, AnimationCurve nProbability )
	{
		id             = idCounter;
		name           = nName;
		process        = nProcess;
		processVar     = true;
		position       = nPosition;
		positionVar    = true;
		orientation    = nOrientation;
		orientationVar = true;
		timer          = nTimer;
		timerVar       = true;
		resultType     = nResultType;
		resultTypeVar  = true;
		result         = nResult;
		resultVar      = false;
		probability    = nProbability;
		
		idCounter++;
	}

	public void init( CommunicationSymbol nSymbol )
	{
		id   = idCounter;
		name = nSymbol.name;

		process        = nSymbol.process;
		processVar     = nSymbol.processVar;
		position       = nSymbol.position;
		positionVar    = nSymbol.positionVar;
		orientation    = nSymbol.orientation;
		orientationVar = nSymbol.orientationVar;
		timer          = nSymbol.timer;
		timerVar       = nSymbol.timerVar;
		resultType     = nSymbol.resultType;
		result         = nSymbol.result;
		resultVar      = nSymbol.resultVar;
		probability    = nSymbol.probability;
		
		idCounter++;
	}

	public static bool operator ==(CommunicationSymbol x, CommunicationSymbol y) 
	{
		if (x.name == y.name)
		{
			if(x.processVar && y.processVar)
				if(x.process != y.process)
					return false;

			if(x.positionVar && y.positionVar)
				if(x.position != y.position)
					return false;

			if(x.orientationVar && y.orientationVar)
				if(x.orientation != y.orientation)
					return false;

			if(x.resultTypeVar && y.resultTypeVar)
				if(x.resultType != y.resultType)
					return false;

			return true;
		}
		
		return false;
	}
	
	public static bool operator !=(CommunicationSymbol x, CommunicationSymbol y) 
	{
		if (x.name == y.name)
		{
			if(x.processVar && y.processVar)
				if(x.process != y.process)
					return true;
			
			if(x.positionVar && y.positionVar)
				if(x.position != y.position)
					return true;
			
			if(x.orientationVar && y.orientationVar)
				if(x.orientation != y.orientation)
					return true;

			if(x.resultTypeVar && y.resultTypeVar)
				if(x.resultType != y.resultType)
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
			if(processVar && p.processVar)
				if(process != p.process)
					return false;
			
			if(positionVar && p.positionVar)
				if(position != p.position)
					return false;
			
			if(orientationVar && p.orientationVar)
				if(orientation != p.orientation)
					return false;

			if(resultTypeVar && p.resultTypeVar)
				if(resultType != p.resultType)
					return false;
			
			return true;
		}
		
		return false;
	}
	
	public override int GetHashCode()
	{
		return name.GetHashCode ();
	}

	public void fillTurtleValues(Turtle turtle)
	{
		_turtlePosition    = turtle.position;
		_turtleOrientation = turtle.direction;

		_globalPosition    = _turtlePosition + _turtleOrientation * position;
		_globalOrientation = _turtleOrientation * orientation;
	}

	public override string toString()
	{
		return name + "(" + process + ", " + position.ToString() + ", " + orientation.eulerAngles.ToString() + ", " + resultType + " )";
	}

	public override string toShortString()
	{
		return name + "(" + process + ")";
	}
}

