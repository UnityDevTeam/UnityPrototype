using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class CommunicationCondition : ScriptableObject
{
	public enum CommParameters
	{
		time,
		result
	};

	public enum CommOperation
	{
		equal,
		notEqual,
		less,
		more
	};

	public static string[] CommParametersStr = { "time", "result" };
	public static string[] CommOperationStr  = { "equal", "notEqual", "less", "more" };

	public CommParameters param;
	public CommOperation operation;
	public GameObject resultValue;
	public float floatValue;


	public CommunicationCondition()
	{
		param       = CommParameters.time;
		operation   = CommOperation.equal;
		resultValue = null;
		floatValue  = 0.0f;
	}

	public CommunicationCondition(CommParameters nParam, CommOperation nOperation, GameObject nValue)
	{
		param       = nParam;
		operation   = nOperation;
		resultValue = nValue;
		floatValue  = 0.0f;
	}

	public CommunicationCondition(CommParameters nParam, CommOperation nOperation, float nValue)
	{
		param       = nParam;
		operation   = nOperation;
		resultValue = null;
		floatValue  = nValue;
	}

	public void init()
	{
		param       = CommParameters.time;
		operation   = CommOperation.equal;
		resultValue = null;
		floatValue  = 0.0f;
	}

	public bool Evaluate(CommunicationSymbol symbol)
	{
		if (param == CommParameters.result)
		{
			bool result = false;

			switch(operation)
			{
				case CommOperation.notEqual:
					result = symbol.result != resultValue;
					break;
				default:
					result = false;
					break;
			}

			return result;
		}
		else if (param == CommParameters.time)
		{
			switch(operation)
			{
				case CommOperation.notEqual:
				{
					return symbol.timer != floatValue;
				}
				case CommOperation.equal:
				{
					return symbol.timer == floatValue;
				}
				case CommOperation.less:
				{
					return symbol.timer < floatValue;
				}
				case CommOperation.more:
				{
					return symbol.timer > floatValue;
				}
				default:
					return false;
			}

		}
		return false;
	}
}
