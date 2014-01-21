using UnityEngine;
using System.Collections.Generic;

public class LocalAgentSystem : MonoBehaviour
{
	public float size  = 20.0f;
	public int   count = 3;

	public string prefabString = "adp";

	void Start ()
	{
	}

	public void AddAgent(GameObject go)
	{
		go.transform.parent = transform;
	}

	private void CheckAgents()
	{
		List<GameObject> children = new List<GameObject>();
		
		foreach (Transform child in transform) children.Add(child.gameObject);
		for( int i = 0; i < children.Count; i++)
		{
			if(children[i].transform.localPosition.magnitude > size / 2.0f)
				Destroy(children[i]);
		}

		if (transform.childCount < count) 
		{
			AddMissingAgents();
		}
	}

	private void AddMissingAgents()
	{
		int missingCount = count - transform.childCount;
		for(int i = 0; i < missingCount; i++)
		{
			Vector3 position = new Vector3 ( Random.value * size, Random.value * size, Random.value * size ) - new Vector3(0.5f*size, 0.5f*size, 0.5f*size);
			
			GameObject prefab                = Resources.Load(prefabString) as GameObject;
			GameObject molecule              = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
			molecule.transform.parent        = transform;
			molecule.transform.localPosition = position;
		}

	}
	
	// Update is called once per frame
	void Update ()
	{
		CheckAgents ();
	}
}
