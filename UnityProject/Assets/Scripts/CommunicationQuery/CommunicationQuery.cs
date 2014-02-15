using UnityEngine;
using System.Collections;

public class CommunicationQuery
{
	private int _symbolId  = 0;
	private int _processId = 0;

	private Vector3    _position    = Vector3.zero;
	private Quaternion _orientation = Quaternion.identity;
	private string     _type        = "";

	public float      time   = 0.0f;
	public GameObject result = null;

	public int symbolId {
		get {
			return this._symbolId;
		}
	}

	public int processId {
		get {
			return this._processId;
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

	public CommunicationQuery( int nSymbolId, int nProcessId, Vector3 nPosition, Quaternion nOrientation, string nType, float nTime)
	{
		_symbolId  = nSymbolId;
		_processId = nProcessId;

		_position    = nPosition;
		_orientation = nOrientation;
		_type        = nType;

		time   = nTime;
		result = null;
	}
}
