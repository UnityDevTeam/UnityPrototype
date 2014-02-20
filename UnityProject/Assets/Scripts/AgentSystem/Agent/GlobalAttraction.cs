using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Agent/Behaviour/GlobalAttraction")]
public class GlobalAttraction : AgentBehaviourNary
{
	//public AgentSystemQuery queries;
	//public Dictionary<int, CommunicationQuery> queries;
	public NewAgentSystem agentSystemScr;

	public float attractionRadius = 5.0f;
	public float attractionPower  = 10.0f;

	public int queryId = -1;

	void Start ()
	{
	
	}

	void Update ()
	{
		Dictionary<int, CommunicationQuery> queries = agentSystemScr.agentQueries;

		if (queries != null && RandomMove.speed < 30)
		{
			if(queryId > -1 && queries.ContainsKey(queryId))
			{
				Vector3 pos = transform.position - queries[queryId].position;
				
				if(pos.magnitude < attractionRadius)
				{
					int objIndex = transform.gameObject.GetInstanceID();
					int index = GetInstanceID();
					
					rigidbody.velocity = -pos.normalized * RandomMove.speed;
					rigidbody.velocity = rigidbody.velocity.normalized * RandomMove.speed;
					
					rigidbody.rotation = Quaternion.Slerp(rigidbody.rotation, queries[queryId].orientation, (attractionRadius - pos.magnitude) / attractionRadius);
				}
			}
			else
			{
				foreach (KeyValuePair<int, CommunicationQuery> query in queries)
				{
					Vector3 pos = transform.position - query.Value.position;
					
					if(pos.magnitude < attractionRadius)
					{
						int objIndex = transform.gameObject.GetInstanceID();
						int index = GetInstanceID();
						
						rigidbody.velocity = -pos.normalized * RandomMove.speed;
						rigidbody.velocity = rigidbody.velocity.normalized * RandomMove.speed;
						
						rigidbody.rotation = Quaternion.Slerp(rigidbody.rotation, query.Value.orientation, (attractionRadius - pos.magnitude) / attractionRadius);

						queryId = query.Key;

						return;
					}
				}
			}
		}
	}
}
