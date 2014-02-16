using UnityEngine;
using System.Collections;

[AddComponentMenu("Agent/Behaviour/GlobalAttraction")]
public class GlobalAttraction : AgentBehaviourNary
{
	public AgentSystemQuery queries;
	public float attractionRadius = 5.0f;
	public float attractionPower  = 10.0f;

	void Start ()
	{
	
	}

	void Update ()
	{
		for (int i = 0; i < queries.queries.Count; i++)
		{
			CommunicationQuery query = queries.queries[i].query;
			Vector3 pos = transform.position - query.position;

			if(pos.magnitude < attractionRadius)
			{
				rigidbody.AddForce(-pos * attractionPower);
				rigidbody.rotation = Quaternion.Slerp(rigidbody.rotation, query.orientation, (attractionRadius - pos.magnitude) / attractionRadius);
			}
		}
	}
}
