using UnityEngine;
using System.Collections.Generic;

public class LocalAgentSystem : MonoBehaviour
{
	public float size  = 20.0f;
	public int   count = 100;

	public string prefabString = "adp";
	
	private float spawnTimer       = 0.0f;
	private Transform globalSystem = null;

	public Quaternion bindingOrientation = Quaternion.identity;
	public bool finished = false;

	public int   stepCounter = 0;
	public float simTime     = 0.0f;

	void Awake ()
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

		Vector3 position = randomVector;

		GameObject prefab                = Resources.Load(prefabString) as GameObject;
		prefab.GetComponent<MolScript> ().life = 0.0f;

		GameObject molecule              = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
		molecule.transform.parent        = transform;
		molecule.transform.localPosition = position;
		molecule.rigidbody.velocity      = -position;
		molecule.GetComponent<MolScript>().life = 0.0f;	
		molecule.GetComponent<MolScript>().bindingOrientation = bindingOrientation;	
	}

	private void AddMissingAgents()
	{
		int missingCount = count - transform.childCount;
		for(int i = 0; i < missingCount; i++)
		{
			AddOneMissingAgent();
		}

	}

	private void TimeStep()
	{
		spawnTimer += Time.fixedDeltaTime;
		CheckAgents ();
	}

	void FixedUpdate()
	{
		TimeStep ();
	}

	void Update ()
	{
		/*
		spawnTimer += Time.deltaTime;

		CheckAgents ();

		if (!finished)
		{
			simTime += Time.deltaTime;
			stepCounter++;
		}
		*/
	}
}
