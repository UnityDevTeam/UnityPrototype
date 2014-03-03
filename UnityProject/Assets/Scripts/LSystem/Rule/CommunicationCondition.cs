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

	public CommunicationCondition(CommParameters nParam, CommOperation nOperation, System.Object nValue)
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
					result = symbol.result != go;
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
					return symbol.timer != valueTime;
				}
				case CommOperation.equal:
				{
					return symbol.timer == valueTime;
				}
				case CommOperation.less:
				{
					return symbol.timer < valueTime;
				}
				case CommOperation.more:
				{
					return symbol.timer > valueTime;
				}
				default:
					return false;
			}

		}
		return false;
	}
}
