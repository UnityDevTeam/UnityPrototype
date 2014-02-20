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

	void Start ()
	{
	
	}

	void Update ()
	{
		Dictionary<int, CommunicationQuery> queries = agentSystemScr.agentQueries;

		if (queries != null && RandomMove.speed < 30)
		{
			foreach (KeyValuePair<int, CommunicationQuery> query in queries)
			{
				Vector3 pos = transform.position - query.Value.position;
				
				if(pos.magnitude < attractionRadius)
				{	
					rigidbody.velocity = -pos.normalized * RandomMove.speed;
					rigidbody.velocity = rigidbody.velocity.normalized * RandomMove.speed;
					
					rigidbody.rotation = Quaternion.Slerp(rigidbody.rotation, query.Value.orientation, (attractionRadius - pos.magnitude) / attractionRadius);

					return;
				}
			}
		}
	}
}
