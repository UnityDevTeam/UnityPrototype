using UnityEngine;
using System.Collections;

public class AgentType : MonoBehaviour
{
	static public float minDensity = 0.00001f;
	static public float maxDensity = 0.001f;

	public AnimationCurve densityFunction = AnimationCurve.Linear(0.0f, minDensity * 100.0f, 10.0f, minDensity * 100.0f);
	public float          densityConstant = minDensity;
	public bool           isConstant      = true;

	public float evaluate(float time)
	{
		float ret = densityConstant;

		if (!isConstant)
		{
			if(densityFunction != null)
			{
				ret = densityFunction.Evaluate (time) / 100.0f;
			}
		}

		return ret;
	}
}
