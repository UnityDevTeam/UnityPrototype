using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Agent/Behaviour/Movement")]
public class Movement : BehaviourUnary
{	
	public static float speed = 5.0f;

	public NewAgentSystem agentSystemScr;

	public float attractionRadius = 2.0f;
	public float attractionPower  = 2.0f;
	public float bindingRadius = 0.4f;

	private Vector3    velocity = Vector3.zero;
	private Quaternion rotation = Quaternion.identity;

	void Start ()
	{
		if (!gameObject.rigidbody)
		{
			gameObject.AddComponent<Rigidbody>();
			gameObject.rigidbody.isKinematic = false;
		}
	}

	void FixedUpdate()
	{
		NewAgentSystem.bindingMotion = false;
	}

	void Update ()
	{
		if (!rigidbody.isKinematic && NewAgentSystem.motionTimer < 0)
		{
			Dictionary<int, CommunicationQuery> queries = agentSystemScr.agentQueries;
			
			if (queries != null && speed < 30)
			{
				foreach (KeyValuePair<int, CommunicationQuery> query in queries)
				{
					if(query.Value.type == transform.parent.gameObject.name)
					{
						Vector3 pos = transform.position - query.Value.position;

						if(pos.magnitude < bindingRadius)
						{
							queries[query.Key].result  = transform.gameObject;
							NewAgentSystem.motionTimer = NewAgentSystem.motionTime;
						}
						else if(pos.magnitude < attractionRadius)
						{
							velocity = -pos.normalized * speed * 0.25f;
							
							rotation = Quaternion.Slerp(rotation, query.Value.orientation, (attractionRadius - pos.magnitude) / attractionRadius);
							NewAgentSystem.bindingMotion = true;
						}
						else
						{
							velocity = Vector3.Lerp (velocity, speed * randomNormalVector (), 0.25f);
							velocity = velocity.normalized * speed;
							rotation = Quaternion.Slerp (rotation, Quaternion.Euler (randomEulerAngles ()), 0.15f);
						}
					}
				}
			}
		}
	}

	void LateUpdate()
	{
		if (!rigidbody.isKinematic)
		{
			if (NewAgentSystem.bindingMotion)
			{
				rigidbody.velocity = velocity;
				transform.rotation = rotation;
			}
			else
			{
				transform.position = randomPosition ();
				transform.rotation = Quaternion.Euler (randomEulerAngles ());
			}
		}
	}
	
	private Vector3 randomNormalVector()
	{
		Vector3 randomize = new Vector3 ((Random.value *2) -1, (Random.value * 2) -1, (Random.value * 2) -1);
		randomize.Normalize();
		return randomize;
	}
	
	private Vector3 randomPosition()
	{
		Vector3 dim = NewAgentSystem.systemSize;
		Vector3 min = NewAgentSystem.minBox;
		return new Vector3 ( Random.value * dim.x, Random.value * dim.y, Random.value * dim.z ) + min;
	}
	
	private Vector3 randomEulerAngles()
	{
		return new Vector3 (Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
	}
}
