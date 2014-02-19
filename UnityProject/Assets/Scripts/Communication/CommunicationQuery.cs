using UnityEngine;
using System.Collections;

public class CommunicationQuery
{
	private int _symbolId  = 0;
	private int _stateId   = 0;

	private Vector3    _position    = Vector3.zero;
	private Quaternion _orientation = Quaternion.identity;
	private string     _type        = "";

	public float      time   = 0.0f;
	public GameObject result = null;

	public bool changed = false;

	public int symbolId {
		get {
			return this._symbolId;
		}
	}

	public int stateId {
		get {
			return this._symbolId;
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

	public CommunicationQuery( int nSymbolId, int nStateId, Vector3 nPosition, Quaternion nOrientation, string nType, float nTime)
	{
		_symbolId  = nSymbolId;
		_symbolId  = nStateId;

		_position    = nPosition;
		_orientation = nOrientation;
		_type        = nType;

		time    = nTime;
		result  = null;
		changed = false;
	}
}
