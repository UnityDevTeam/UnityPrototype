using UnityEngine;
using System.Collections.Generic;

public class LocalAgentSystem : MonoBehaviour
{
	public float size  = 20.0f;
	public int   count = 100;

	public string prefabString = "adp";


	private float spawnTimer       = 0.0f;
	private Transform globalSystem = null;

	void Start ()
	{
	}

	public void setGlobalSystem(GameObject go)
	{
		globalSystem = go.transform;
	}

	public void AddAgent(GameObject go)
	{
		go.transform.parent = transform;
	}

	private void CheckAgents()
	{
		int childCount = transform.childCount;
		int childIndex = 0;
		for( int i = 0; i < childCount; i++)
		{
			Transform child = transform.GetChild(childIndex);
			if(child.localPosition.magnitude > size / 2.0f)
			{
				Transform freeMolecules = globalSystem.Find("freeMolecules");
				child.parent = freeMolecules;
			}
			else
			{
				float life = child.gameObject.GetComponent<MolScript>().life + Time.deltaTime;
				life = Mathf.Min(life, 2.0f);
				child.gameObject.GetComponent<MolScript>().life = life;
				childIndex++;
			}
		}

		if (transform.childCount < count) 
		{
			if (spawnTimer > 0.5f) 
			{
				AddOneMissingAgent();
				spawnTimer = 0.0f;
			}
		}
	}

	private void AddOneMissingAgent()
	{
		Vector3 randomVector = new Vector3 (Random.value, Random.value, Random.value) - new Vector3(0.5f, 0.5f, 0.5f);
		randomVector.Normalize ();
		randomVector.Scale (new Vector3(0.4f * size, 0.4f * size, 0.4f * size));
		/*
		float posX = ((Random.value) * 0.1f + 0.8f) * size;
		float posY = ((Random.value) * 0.1f + 0.8f) * size;
		float posZ = ((Random.value) * 0.1f + 0.8f) * size;
		*/
		//Vector3 position = new Vector3 ( posX, posY, Random.value * size ) - new Vector3(0.5f*size, 0.5f*size, 0.5f*size);
		Vector3 position = randomVector;

		GameObject prefab                = Resources.Load(prefabString) as GameObject;
		GameObject molecule              = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
		molecule.transform.parent        = transform;
		molecule.transform.localPosition = position;
		molecule.GetComponent<MolScript>().life = 0.0f;	
	}

	private void AddMissingAgents()
	{
		int missingCount = count - transform.childCount;
		for(int i = 0; i < missingCount; i++)
		{
			AddOneMissingAgent();
			/*
			Vector3 position = new Vector3 ( Random.value * size, Random.value * size, Random.value * size ) - new Vector3(0.5f*size, 0.5f*size, 0.5f*size);
			
			GameObject prefab                = Resources.Load(prefabString) as GameObject;
			GameObject molecule              = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
			molecule.transform.parent        = transform;
			molecule.transform.localPosition = position;
			molecule.GetComponent<MolScript>().life = 0.0f;
			*/
		}

	}

	void Update ()
	{
		spawnTimer += Time.deltaTime;

		CheckAgents ();
	}
}
