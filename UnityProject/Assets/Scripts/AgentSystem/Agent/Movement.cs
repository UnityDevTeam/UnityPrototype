using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Agent/Behaviour/Movement")]
public class Movement : MonoBehaviour
{	
	//public static float speed = 5.0f;

	public static int   bindingMonomerID = 0;
	public static float bindingTimer     = 0.0f;

	public AgentSystem agentSystemScr;

	public float attractionRadius = 2.0f;
	public float attractionPower  = 0.15f;
	public float bindingRadius = 0.2f;

	private Vector3    velocity = Vector3.zero;
	private Quaternion rotation = Quaternion.identity;

	public Color colFrom = new Color(0.8470588f, 0.4784314f, 0.4784314f);
	public Color colTo   = new Color(1.0f, 1.0f, 1.0f);
	public static float bindingTimerSaved = 0.0f;

	public Vector3 orientAlteration = new Vector3(0, 10.0f, 10.0f);
	private Quaternion orientation = Quaternion.identity;

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

	void FixedUpdate()
	{
		AgentSystem.bindingMotion = false;
	}

	void Update ()
	{
		if (alreadyRun) return;

		if (!rigidbody.isKinematic && AgentSystem.motionTimer < 0 && LSystem.canAddItem)
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
							if(bindingTimerSaved == 0.0f)
							{
								bindingTimerSaved = bindingTimer;
								orientation = query.Value.orientation;
							}

							bindingTimer -= Time.deltaTime;
							AgentSystem.bindingMotion = true;
							transform.position = query.Value.position;

							velocity = Vector3.zero;

							if(bindingTimer < 0.0f)
							{
								bindingMonomerID = 0;
								queries[query.Key].result  = transform.gameObject;
								bindingTimer = 0.0f;
								bindingTimerSaved = 0.0f;
								queryId = -1;
							}

							alreadyRun = true;
							return;
						}
						else if((pos.magnitude < attractionRadius) && ((bindingMonomerID == 0) || (bindingMonomerID == gameObject.GetInstanceID())) && ((queryId == -1) || (queryId == query.Key)))
						{
							bindingMonomerID = gameObject.GetInstanceID();
							bindingTimer += Time.deltaTime;

							velocity = -pos.normalized * GlobalVariables.monomerSpeed * 0.25f;
							
							rotation = Quaternion.Slerp(rotation, query.Value.orientation, (attractionRadius - pos.magnitude) / attractionRadius);
							AgentSystem.bindingMotion = true;
							queryId = query.Key;

							alreadyRun = true;
							return;
						}
						else
						{
							velocity = Vector3.Lerp (velocity, GlobalVariables.monomerSpeed * randomNormalVector (), 0.25f);
							velocity = velocity.normalized * GlobalVariables.monomerSpeed;
							rotation = Quaternion.Slerp (rotation, Quaternion.Euler (randomEulerAngles ()), 0.05f);
						}
					}
					else
					{
						velocity = Vector3.Lerp (velocity, GlobalVariables.monomerSpeed * randomNormalVector (), 0.25f);
						velocity = velocity.normalized * GlobalVariables.monomerSpeed;
						rotation = Quaternion.Slerp (rotation, Quaternion.Euler (randomEulerAngles ()), 0.05f);
					}
				}
			}
		}


		velocity = Vector3.Lerp (velocity, GlobalVariables.monomerSpeed * randomNormalVector (), 0.25f);
		velocity = velocity.normalized * GlobalVariables.monomerSpeed;
		rotation = Quaternion.Slerp (rotation, Quaternion.Euler (randomEulerAngles ()), 0.05f);

		alreadyRun = true;
	}

	void LateUpdate()
	{
		alreadyRun = false;

		if (!rigidbody.isKinematic)
		{
			if (AgentSystem.bindingMotion)
			{
				if(((bindingMonomerID != gameObject.GetInstanceID())) && (bindingTimerSaved != 0.0f))
				{
					transform.position = randomPosition ();
					transform.rotation = Quaternion.Euler (randomEulerAngles ());
				}
				else
				{
					rigidbody.velocity = velocity;
					transform.rotation = rotation;
				}
			}
			else
			{
				if(((bindingMonomerID != gameObject.GetInstanceID())))
				{
					transform.position = randomPosition ();
					transform.rotation = Quaternion.Euler (randomEulerAngles ());
				}
				else
				{
					bindingMonomerID = 0;
					bindingTimer = 0.0f;
					bindingTimerSaved = 0.0f;
					queryId = -1;
				}
			}
		}

		if (((bindingMonomerID == gameObject.GetInstanceID ())))
		{
			if(bindingTimerSaved != 0.0f)
			{
				if(transform.parent.gameObject.name != "tubulin")
				{
					Color color = Color.Lerp( colFrom, colTo, 1 - (bindingTimer / bindingTimerSaved));
					gameObject.renderer.material.color = color;
				}

				float randomX = UnityEngine.Random.Range (-orientAlteration.x, orientAlteration.x);
				float randomY = UnityEngine.Random.Range (-orientAlteration.y, orientAlteration.y);
				float randomZ = UnityEngine.Random.Range (-orientAlteration.z, orientAlteration.z);

				transform.rotation = orientation * Quaternion.Euler (new Vector3(randomX, randomY, randomZ));
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
		Vector3 dim = AgentSystem.systemSize;
		Vector3 min = AgentSystem.minBox;
		return new Vector3 ( Random.value * dim.x, Random.value * dim.y, Random.value * dim.z ) + min;
	}
	
	private Vector3 randomEulerAngles()
	{
		return new Vector3 (Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
	}
}
