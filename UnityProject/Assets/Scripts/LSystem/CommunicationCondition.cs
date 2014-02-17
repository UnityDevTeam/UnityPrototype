using UnityEngine;
using System.Collections;

public class CommunicationCondition
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

	private CommParameters param;
	private CommOperation operation;
	private System.Object value;

	public CommunicationCondition(CommParameters nParam, CommOperation nOperation, Object nValue)
	{
		param     = nParam;
		operation = nOperation;
		value     = nValue;
	}

	public bool Evaluate(CommunicationSymbol symbol)
	{
		if (param == CommParameters.result)
		{
			GameObject go = value as GameObject;
			bool result = false;

			switch(operation)
			{
				case CommOperation.notEqual:
					result = symbol.operationResult != go;
					break;
				default:
					result = false;
					break;
			}

			return result;
		}
		else if (param == CommParameters.time)
		{
			float valueTime = (float)value;
			switch(operation)
			{
				case CommOperation.notEqual:
				{
					return symbol.operationTimer != valueTime;
				}
				case CommOperation.equal:
				{
					return symbol.operationTimer == valueTime;
				}
				case CommOperation.less:
				{
					return symbol.operationTimer < valueTime;
				}
				case CommOperation.more:
				{
					return symbol.operationTimer > valueTime;
				}
				default:
					return false;
			}

		}
		return false;
	}
}
