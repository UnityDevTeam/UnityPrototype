using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallisticMovement : MonoBehaviour
{
	//public static float speed = 5.0f;
	
	public static int   bindingMonomerID = 0;
	public static float bindingTimer     = 0.0f;
	
	public AgentSystem agentSystemScr;
	
	public float attractionRadius = 2.0f;
	public float attractionPower  = 0.15f;
	public float bindingRadius = 0.2f;

	public static float bindingTimerSaved = 0.0f;
	
	public static int queryId = -1;
	
	bool alreadyRun = false;
	
	void Start ()
	{
		if (!gameObject.rigidbody)
		{
			gameObject.AddComponent<Rigidbody>();
			gameObject.rigidbody.isKinematic = false;
		}
	}
	
	void Update ()
	{
		if (alreadyRun) return;
		
		if (!rigidbody.isKinematic)
		{
			Dictionary<int, CommunicationQuery> queries = agentSystemScr.agentQueries;
			
			if (queries != null && GlobalVariables.monomerSpeed < 30)
			{
				foreach (KeyValuePair<int, CommunicationQuery> query in queries)
				{
					if(query.Value.type == transform.parent.gameObject.name)
					{
						Vector3 pos = transform.position - query.Value.position;
						
						if((pos.magnitude < bindingRadius) && ((bindingMonomerID == 0) || (bindingMonomerID == gameObject.GetInstanceID())) && ((queryId == -1) || (queryId == query.Key)))
						{
							bindingMonomerID = 0;
							queryId = -1;
							queries[query.Key].result  = transform.gameObject;

							return;
						}
						else if((pos.magnitude < attractionRadius) && ((bindingMonomerID == 0) || (bindingMonomerID == gameObject.GetInstanceID())) && ((queryId == -1) || (queryId == query.Key)))
						{
							bindingMonomerID = gameObject.GetInstanceID();
							queryId = query.Key;

							rigidbody.velocity = -pos.normalized * GlobalVariables.monomerSpeed * 0.25f;
							transform.rotation = Quaternion.Slerp(transform.rotation, query.Value.orientation, (attractionRadius - pos.magnitude) / attractionRadius);

							alreadyRun = true;
							return;
						}
					}
				}
			}
		}

		rigidbody.velocity = Vector3.Lerp (rigidbody.velocity, GlobalVariables.monomerSpeed * randomNormalVector (), 0.25f);
		rigidbody.velocity = rigidbody.velocity.normalized * GlobalVariables.monomerSpeed;
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (randomEulerAngles ()), 0.05f);

		alreadyRun = true;
	}
	
	void LateUpdate()
	{
		alreadyRun = false;

		Vector3 dim = AgentSystem.systemSize;
		Vector3 min = AgentSystem.minBox;

		Vector3 pos = transform.position + rigidbody.velocity;

		if ((pos.x > (min.x + dim.x) || pos.x < min.x) || (pos.y > (min.y + dim.y) || pos.y < min.y) || (pos.z > (min.z + dim.z) || pos.z < min.z) )
		{
			transform.position = randomPosition();
			rigidbody.velocity = -rigidbody.velocity;
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
		Vector3 dim = AgentSystem.systemSize;
		Vector3 min = AgentSystem.minBox;

		Vector3 direction = new Vector3 (Random.value, Random.value, Random.value);
		direction.Normalize ();

		return dim.x * direction + min;
	}
	
	private Vector3 randomEulerAngles()
	{
		return new Vector3 (Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
	}
}
