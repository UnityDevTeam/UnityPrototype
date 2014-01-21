using UnityEngine;
using System.Collections.Generic;

public class AgentsSystem : MonoBehaviour
{
	/*
	public int     molCount   = 50;
	public Vector3 extentBox  = new Vector3( 20, 20, 20);
	public Vector3 minBox     = new Vector3(-10,-10,-10);
	public string  pdbPrefab  = "adp";
	*/
	
	private List<GameObject> free_molecules;
	private List<GameObject> locals;

	void Start ()
	{
		locals         = new List<GameObject> ();
		free_molecules = new List<GameObject> ();

		createLocalSystem (Vector3.zero);
	}
	
	public void SpawnObjects()
	{
		/*
		molecules = new GameObject[molCount];

		for (var i=0; i<molCount; i++)
		{
			Vector3 position = new Vector3 ( Random.value * extentBox.x, Random.value * extentBox.y, Random.value * extentBox.z ) + minBox;

			GameObject prefab                = Resources.Load(pdbPrefab) as GameObject;
			GameObject molecule              = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
			molecule.transform.parent        = transform;
			molecule.transform.localPosition = position;

			molecules[i] = molecule;
		}
		*/

		createLocalSystem (Vector3.zero);
	}

	public void createLocalSystem(Vector3 position)
	{
		GameObject go = new GameObject ("local");
		go.transform.parent   = transform;
		go.transform.position = position;
		go.AddComponent("LocalAgentSystem");
		locals.Add (go);
	}

	void Update ()
	{
		if (free_molecules.Count > 0)
		{
			for (int i = 0; i < free_molecules.Count; i++)
			{
				for (int j = 0; j < locals.Count; j++)
				{
					Vector3 distance = free_molecules [i].transform.position - locals [j].transform.position;

					if (distance.magnitude < locals [j].GetComponent<LocalAgentSystem>().size)
					{
						locals [j].GetComponent<LocalAgentSystem>().AddAgent(free_molecules [i]);
						break;
					}
				}
			}

			free_molecules = new List<GameObject> ();
		}

	}
}
