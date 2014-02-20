using UnityEngine;
using System.Collections;

[AddComponentMenu("Agent/Condition/TimeScaleTransparency")]
public class TimeScaleTransparency : AgentConditionUnary
{
	private float speed = RandomMove.speed;

	void Update ()
	{
		if (speed != RandomMove.speed)
		{
			Color col = renderer.material.color;
			
			float transparency = 1.0f;
			if (RandomMove.speed > 25.0f)
			{
				transparency = (30.0f - RandomMove.speed) / 5.0f; 
			}
			col.a = transparency;
			
			renderer.material.color = col;

			speed = RandomMove.speed;
		}
	}
}
