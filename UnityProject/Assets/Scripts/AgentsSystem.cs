using UnityEngine;
using System.Collections;

public class AgentsSystem : MonoBehaviour
{
	public int     molCount   = 50;
	public Vector3 extentBox  = new Vector3( 20, 20, 20);
	public Vector3 minBox     = new Vector3(-10,-10,-10);
	public string  pdbPrefab  = "adp";
	
	private GameObject[] molecules;

	void Start ()
	{
		SpawnObjects ();
	}
	
	public void SpawnObjects()
	{
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
	}

	void Update ()
	{

	}
}
