using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentSystem : MonoBehaviour
{
	public static Vector3 systemSize = new Vector3(100, 100, 100);	
	public static Vector3 minBox;

	[HideInInspector] public int agentsCount = 0;
	[HideInInspector] public float time      = 0.0f;

	[HideInInspector] public Dictionary<int, CommunicationQuery> agentQueries;

	private GameObject communicationQueryObject = null;

	private float volume;
	public static bool  bindingMotion = false;
	public static float motionTime    = 0.5f;
	public static float motionTimer   = motionTime;


	/*
	* AgentSystem.addAgentType(string prefabName)
	* 
	* Creates new GameObject in AgentSystem Unity object hierarchy, which will encapsulate all the agents from the specified type. The type is called by the prefabName string
	* and agents are defined by the prefab with prefabName identification.
	*/
	public void addAgentType(string prefabName)
	{
		GameObject go = new GameObject (prefabName);
		go.AddComponent<AgentType> ();
		go.transform.parent = transform;
	}

	/*
	* AgentSystem.checkAgentsCounts (float time)
	* 
	* Checks wether if the count of agents agrees with the number of agents defined by density in time. If the count of the agents are lower then expected, new agents are spawned.
	* On the other hand if the number of agents are higher, appropriate number of agents are removed from the system.
	*/
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
				}
			}
		}
	}

	/*
	* AgentSystem.spawnAgent(GameObject agentTypeObject)
	* 
	* Adding the new agent of the agentTypeObject to the AgentSystem object hierarchy.
	*/
	private void spawnAgent(GameObject agentTypeObject)
	{
		Vector3 position = new Vector3 ( Random.value * systemSize.x, Random.value * systemSize.y, Random.value * systemSize.z ) + minBox;
		
		GameObject prefab = Resources.Load(agentTypeObject.name) as GameObject;
		GameObject mol = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
		mol.transform.parent = agentTypeObject.transform;
		mol.transform.localPosition = position;

		if (mol.GetComponent<Movement> ())
		{
			mol.GetComponent<Movement> ().agentSystemScr = this;
		}

		if (mol.GetComponent<BallisticMovement> ())
		{
			mol.GetComponent<BallisticMovement> ().agentSystemScr = this;
		}

	}

	/*
	* AgentSystem.getCommunicationQuerries()
	* 
	* Gets the queries from the communication system and updates the time of all queries
	*/
	public void getCommunicationQuerries()
	{
		agentQueries = communicationQueryObject.GetComponent<CommunicationManager>().queries;

		if (agentQueries != null)
		{
			foreach(KeyValuePair<int, CommunicationQuery> query in agentQueries)
			{
				agentQueries[query.Key].time += Time.deltaTime;
			}
		}
	}

	/*
	* AgentSystem.Awake()
	* 
	* Makes sure, that Communication system is present in the scene.
	*/
	void Awake()
	{
		agentQueries = new Dictionary<int, CommunicationQuery>();

		if (!communicationQueryObject)
		{
			communicationQueryObject = GameObject.Find("Communication Manager");
			if(!communicationQueryObject)
			{
				communicationQueryObject = new GameObject("Communication Manager");
				communicationQueryObject.AddComponent<CommunicationManager>();
			}
		}
	}

	/*
	* AgentSystem.Awake()
	* 
	* Initialize the agent system. The number of agents are calculated and spawned in the system.
	*/
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
			}
			
			agentsCount += agentTypeCount;
		}
	}

	/*
	* AgentSystem.Update()
	* 
	* Time step of agent system. Time scale is checked and based on that, the agents are hidden or shown. Also the number of agents are checked.
	*/
	void Update ()
	{
		time += Time.deltaTime;
		motionTimer -= Time.deltaTime;

		if (GlobalVariables.monomerSpeed == 30)
		{
			for(int i = 0; i < transform.childCount; i++)
				transform.GetChild (i).gameObject.SetActive (false);
		}
		else
		{
			for(int i = 0; i < transform.childCount; i++)
				transform.GetChild (i).gameObject.SetActive (true);
		}

		checkAgentsCounts (time);
		getCommunicationQuerries ();
	}
}
