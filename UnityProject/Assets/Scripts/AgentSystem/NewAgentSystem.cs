using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewAgentSystem : MonoBehaviour
{
	public static float agentScale = 1.0f;

	public static Vector3 systemSize = new Vector3(100, 100, 100);	
	public static Vector3 minBox;

	[HideInInspector] public int agentsCount = 0;
	[HideInInspector] public float time      = 0.0f;

	[HideInInspector] public Dictionary<int, CommunicationQuery> agentQueries;

	private GameObject communicationQueryObject = null;

	private float oldAgentScale = 1.0f;

	private float volume;
	public static bool  bindingMotion = false;
	public static float motionTime    = 0.5f;
	public static float motionTimer   = motionTime;


	////////////////////////////////////////////////////////////////////////////////////////

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

	private void spawnAgent(GameObject agentTypeObject)
	{
		Vector3 position = new Vector3 ( Random.value * systemSize.x, Random.value * systemSize.y, Random.value * systemSize.z ) + minBox;
		
		GameObject prefab = Resources.Load(agentTypeObject.name) as GameObject;
		GameObject mol = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
		mol.transform.parent = agentTypeObject.transform;
		mol.transform.localPosition = position;
		
		if (mol.GetComponent<GlobalAttraction> ())
		{
			mol.GetComponent<GlobalAttraction> ().agentSystemScr = this;
		}
		
		if (mol.GetComponent<GlobalBindingQuery> ())
		{
			mol.GetComponent<GlobalBindingQuery> ().agentSystemScr = this;
		}

		if (mol.GetComponent<Movement> ())
		{
			mol.GetComponent<Movement> ().agentSystemScr = this;
		}

	}

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

	void Update ()
	{
		time += Time.deltaTime;
		motionTimer -= Time.deltaTime;

		if (agentScale != oldAgentScale)
		{
			scaleAgents ();
		}

		if (Movement.speed == 30)
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
