using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Agent/Trigger/GlobalBindingQuery")]
public class GlobalBindingQuery : AgentTriggerNary
{
	public NewAgentSystem agentSystemScr;

	public float bindingRadius = 0.4f;

	void Start ()
	{
	
	}

	void Update ()
	{
		Dictionary<int, CommunicationQuery> queries = agentSystemScr.agentQueries;

		if (queries != null && RandomMove.speed < 30)
		{
			foreach(KeyValuePair<int, CommunicationQuery> query in queries)
			{
				if(query.Value.type == transform.parent.gameObject.name)
				{
					Vector3 pos = transform.position - query.Value.position;
					
					if(pos.magnitude < NewAgentSystem.agentScale * bindingRadius)
					{	
						queries[query.Key].result  = transform.gameObject;
						return;
					}
				}
			}
		}
	}
}
