using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Agent/Condition/TimeScaleTransparency")]
public class TimeScaleTransparency : AgentConditionUnary
{
	private float speed = RandomMove.speed;
	private List<Material> materials = new List<Material> ();

	void Start ()
	{
		if (renderer == null)
		{
			for(int i = 0; i < transform.childCount; i++)
			{
				GameObject go = transform.GetChild(i).gameObject;
				materials.Add(go.renderer.material);
			}
		}
		else
		{
			materials.Add(renderer.material);
		}
	}

	void Update ()
	{
		if (speed != RandomMove.speed)
		{
			for(int i = 0; i < materials.Count; i++)
			{
				Color col = materials[i].color;
				
				float transparency = 1.0f;
				if (RandomMove.speed > 25.0f)
				{
					transparency = (30.0f - RandomMove.speed) / 5.0f; 
				}
				col.a = transparency;
				
				materials[i].color = col;
				
				speed = RandomMove.speed;
			}
		}
	}
}
