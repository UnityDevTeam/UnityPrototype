using UnityEngine;
using System.Collections;

public class CommunicationQuery
{
	private int _stateId   = 0;

	private Vector3    _position    = Vector3.zero;
	private Quaternion _orientation = Quaternion.identity;
	private string     _type        = "";

	public float      time   = 0.0f;
	public GameObject result = null;

	public AnimationCurve probability;

	public int stateId {
		get {
			return this._stateId;
		}
	}

	public Vector3 position {
		get {
			return this._position;
		}
	}

	public Quaternion orientation {
		get {
			return this._orientation;
		}
	}

	public string type {
		get {
			return this._type;
		}
	}

	public CommunicationQuery( int nStateId, Vector3 nPosition, Quaternion nOrientation, string nType, float nTime, AnimationCurve nProbability)
	{
		_stateId  = nStateId;

		_position    = nPosition;
		_orientation = nOrientation;
		_type        = nType;

		probability = nProbability;
		time        = nTime;
		result      = null;
	}
}
