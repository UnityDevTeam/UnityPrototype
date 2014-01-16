using UnityEngine;
using UnityEditor;
using System.Collections;

public class MainScript : MonoBehaviour
{
	public int molSize = 20;
	public float minVelocity = 5;
	public float maxVelocity = 20;
	public Vector3 extentBox = new Vector3(10,10,10);
	public Vector3 minBox = new Vector3(-5,-5,-5);
	public string[] pdbPrefabs = {"Assets/testMol.prefab","Assets/adp.prefab"};

	private GameObject[] molecules;
	private bool initialized = false;

	// Use this for initialization
	void Start ()
	{
		if (!initialized) SpawnObjects ();

	}

	public void SpawnObjects()
	{
		molecules = new GameObject[molSize];
		for (var i=0; i<molSize; i++)
		{
			Vector3 position = new Vector3 (
				Random.value * extentBox.x,
				Random.value * extentBox.y,
				Random.value * extentBox.z
				) + minBox;
			int pdbid = Random.Range (0,2);
			GameObject prefab = AssetDatabase.LoadAssetAtPath(pdbPrefabs[pdbid], typeof(GameObject)) as GameObject;
			GameObject mol = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
			mol.transform.parent = transform;
			mol.transform.localPosition = position;
			mol.GetComponent<MolScript>().minVelocity = minVelocity;
			mol.GetComponent<MolScript>().maxVelocity = maxVelocity;
			molecules[i] = mol;
		}

		initialized = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		print ("Hello world");
	}
}
