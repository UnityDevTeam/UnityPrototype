using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewAgentSystem : MonoBehaviour
{
	public Vector3 systemSize = new Vector3(100, 100, 100);	

	[HideInInspector] public int agentsCount = 0;
	[HideInInspector] public float time      = 0.0f;
	[HideInInspector] public float agentScale = 1.0f;


	[HideInInspector] public AgentSystemQuery queries;

	private float oldAgentScale    = 1.0f;
	private Vector3 minBox;
	private float volume;

	private GameObject communicationQueryObject = null;

	public void addAgentType(string prefabName)
	{
		GameObject go = new GameObject (prefabName);
		go.AddComponent<AgentType> ();
		go.transform.parent = transform;
	}

	public void checkAgentsCounts (float time)
	{
		agentsCount = 0;

		for (int i = 0; i < transform.childCount; i++)
		{
			GameObject agentTypeObject = transform.GetChild (i).gameObject;

			int newAgentTypeCount = (int)(agentTypeObject.GetComponent<AgentType>().evaluate(time) * volume);
			agentsCount += newAgentTypeCount;

			if(newAgentTypeCount < agentTypeObject.transform.childCount)
			{
				int countDelta = agentTypeObject.transform.childCount - newAgentTypeCount;
				for(int j = 0; j < countDelta; j++)
				{
					DestroyImmediate(agentTypeObject.transform.GetChild(0).gameObject);
				}
			}
			else if(newAgentTypeCount > agentTypeObject.transform.childCount)
			{
				int countDelta =  newAgentTypeCount - agentTypeObject.transform.childCount;
				for(int j = 0; j < countDelta; j++)
				{
					spawnAgent(agentTypeObject);
					/*
					Vector3 position = new Vector3 ( Random.value * systemSize.x, Random.value * systemSize.y, Random.value * systemSize.z ) + minBox;
					
					GameObject prefab = Resources.Load(agentTypeObject.name) as GameObject;
					GameObject mol = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
					mol.transform.parent = agentTypeObject.transform;
					mol.transform.localPosition = position;
					*/
				}
			}
		}
	}

	public void scaleAgents ()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			GameObject agentTypeObject = transform.GetChild (i).gameObject;
			

			for(int j = 0; j < agentTypeObject.transform.childCount; j++)
			{
				Transform agentTransform = agentTypeObject.transform.GetChild(j);

				agentTransform.transform.localScale = new Vector3(agentScale, agentScale, agentScale);

			}
		}

		oldAgentScale = agentScale;
	}

	void Awake()
	{
		queries = new AgentSystemQuery();

		if (!communicationQueryObject)
		{
			communicationQueryObject = GameObject.Find("Communication Query");
			if(!communicationQueryObject)
			{
				communicationQueryObject = new GameObject("Communication Query");
				communicationQueryObject.AddComponent<CommunicationQueryList>();
			}
		}
	}

	private void spawnAgent(GameObject agentTypeObject)
	{
		Vector3 position = new Vector3 ( Random.value * systemSize.x, Random.value * systemSize.y, Random.value * systemSize.z ) + minBox;
		
		GameObject prefab = Resources.Load(agentTypeObject.name) as GameObject;
		GameObject mol = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
		mol.transform.parent = agentTypeObject.transform;
		mol.transform.localPosition = position;

		if (mol.GetComponent<GlobalAttraction> ())
			mol.GetComponent<GlobalAttraction> ().queries = queries;

		if (mol.GetComponent<GlobalQuery> ())
			mol.GetComponent<GlobalQuery> ().queries = queries;
	}

	void Start ()
	{
		agentsCount = 0;
		minBox = new Vector3 (transform.position.x - systemSize.x / 2.0f, transform.position.y - systemSize.y / 2.0f, transform.position.z - systemSize.z / 2.0f);
		volume = systemSize.x * systemSize.y * systemSize.z;
		
		for (int i = 0; i < transform.childCount; i++)
		{
			GameObject agentTypeObject = transform.GetChild(i).gameObject;
			
			int agentTypeCount = (int)(agentTypeObject.GetComponent<AgentType>().densityConstant * volume);
			
			for (int j = 0; j < agentTypeCount; j++)
			{
				spawnAgent(agentTypeObject);
				/*
				Vector3 position = new Vector3 ( Random.value * systemSize.x, Random.value * systemSize.y, Random.value * systemSize.z ) + minBox;
				
				GameObject prefab = Resources.Load(agentTypeObject.name) as GameObject;
				GameObject mol = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
				mol.transform.parent = agentTypeObject.transform;
				mol.transform.localPosition = position;
				*/
			}
			
			agentsCount += agentTypeCount;
		}
	}

	void Update ()
	{
		time += Time.deltaTime;
		checkAgentsCounts (time);

		if (agentScale != oldAgentScale)
		{
			scaleAgents ();
		}
	}
}
