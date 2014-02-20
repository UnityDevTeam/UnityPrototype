using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Agent/Trigger/GlobalBindingQuery")]
public class GlobalBindingQuery : AgentTriggerNary
{
	//public AgentSystemQuery queries;
	//public Dictionary<int, CommunicationQuery> queries;
	public NewAgentSystem agentSystemScr;

	public float bindingRadius = 0.4f;

	void Start ()
	{
	
	}

	void Update ()
	{
		Dictionary<int, CommunicationQuery> queries = agentSystemScr.agentQueries;

		if (queries != null)
		{
			foreach(KeyValuePair<int, CommunicationQuery> query in queries)
			{
				Vector3 pos = transform.position - query.Value.position;
				
				if(pos.magnitude < NewAgentSystem.agentScale * bindingRadius)
				{	
					queries[query.Key].result  = transform.gameObject;
					queries[query.Key].changed = true;
					return;
				}
			}
		}
		/*
		for (int i = 0; i < queries.queries.Count; i++)
		{
			CommunicationQuery query = queries.queries[i].query;
			Vector3 pos = transform.position - query.position;
			
			if(pos.magnitude < NewAgentSystem.agentScale * bindingRadius)
			{

				query.result = transform.gameObject;

				queries.queries[i] = new CommunicationQueryPair(query, true);

			}
		}
		*/
	}
}
