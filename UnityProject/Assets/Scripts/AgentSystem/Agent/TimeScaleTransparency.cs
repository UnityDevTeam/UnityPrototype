using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Agent/Condition/TimeScaleTransparency")]
public class TimeScaleTransparency : AgentConditionUnary
{
	private float speed = Movement.speed;
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
		if (speed != Movement.speed)
		{
			for(int i = 0; i < materials.Count; i++)
			{
				Color col = materials[i].color;
				
				float transparency = 1.0f;
				if (Movement.speed > 25.0f)
				{
					transparency = (30.0f - Movement.speed) / 5.0f; 
				}
				col.a = transparency;
				
				materials[i].color = col;
				
				speed = Movement.speed;
			}
		}
	}

	void LateUpdate ()
	{
		if (!NewAgentSystem.bindingMotion || ((Movement.bindingMonomerID != gameObject.GetInstanceID()) && (Movement.bindingTimerSaved != 0.0f)))
		{
			for (int i = 0; i < materials.Count; i++)
			{
				Color col = materials [i].color;
				col.a = 0.5f;
				
				materials [i].color = col;
			}
		}
		else
		{
			for (int i = 0; i < materials.Count; i++)
			{
				Color col = materials [i].color;
				col.a = 1.0f;
				
				materials [i].color = col;
			}
		}
	}
}
