using UnityEngine;
using System.Collections;

[AddComponentMenu("Agent/Trigger/GlobalBindingQuery")]
public class GlobalBindingQuery : AgentTriggerNary
{
	public AgentSystemQuery queries;

	public float bindingRadius = 0.4f;

	void Start ()
	{
	
	}

	void Update ()
	{
		for (int i = 0; i < queries.queries.Count; i++)
		{
			CommunicationQuery query = queries.queries[i].query;
			Vector3 pos = transform.position - query.position;
			
			if(pos.magnitude < bindingRadius)
			{
				bool tempBool = queries.queries[i].changed;
				tempBool = true;
				query.result = transform.gameObject;
			}
		}
	}
}
