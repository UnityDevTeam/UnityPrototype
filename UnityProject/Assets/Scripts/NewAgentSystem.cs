using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewAgentSystem : MonoBehaviour
{
	public Vector3 extentBox = new Vector3(100, 100, 100);
	public Vector3 minBox = new Vector3(-50, -50, -50);
	
	public int agentsCount = 0;
	private float volume = 0.0f;
	public float time = 0.0f;

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
					Vector3 position = new Vector3 ( Random.value * extentBox.x, Random.value * extentBox.y, Random.value * extentBox.z ) + minBox;
					
					GameObject prefab = Resources.Load(agentTypeObject.name) as GameObject;
					GameObject mol = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
					mol.transform.parent = agentTypeObject.transform;
					mol.transform.localPosition = position;
				}
			}
		}
	}

	void Start ()
	{
		agentsCount = 0;
		volume = extentBox.x * extentBox.y * extentBox.z;
		
		for (int i = 0; i < transform.childCount; i++)
		{
			GameObject agentTypeObject = transform.GetChild(i).gameObject;
			
			int agentTypeCount = (int)(agentTypeObject.GetComponent<AgentType>().densityConstant * volume);
			
			for (int j = 0; j < agentTypeCount; j++)
			{
				Vector3 position = new Vector3 ( Random.value * extentBox.x, Random.value * extentBox.y, Random.value * extentBox.z ) + minBox;
				
				GameObject prefab = Resources.Load(agentTypeObject.name) as GameObject;
				GameObject mol = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
				mol.transform.parent = agentTypeObject.transform;
				mol.transform.localPosition = position;
			}
			
			agentsCount += agentTypeCount;
		}
	}

	void Update ()
	{
		time += Time.deltaTime;
		checkAgentsCounts (time);
	}
}
