using UnityEngine;
using UnityEditor;
using System.Collections;

public class MainScript : MonoBehaviour
{
	public int molCount = 20;

	public float minVelocity = 5;
	public float maxVelocity = 20;

	public Vector3 extentBox = new Vector3(100, 100, 100);
	public string[] pdbPrefabs = {"Assets/Prefabs/MolObject.prefab"};

	private GameObject[] molecules;
	private bool initialized = false;

	// Use this for initialization
	void Start ()
	{
		if (!initialized)
		{
				SpawnObjects ();
		}
	}

	public void SpawnObjects()
	{
		if (initialized) 
		{
			foreach(GameObject obj in molecules)
			{
				DestroyImmediate(obj);
			}				
		}

		molecules = new GameObject[molCount];

		for (var i=0; i < molCount; i++)
		{
			Vector3 position = new Vector3 ( (Random.value - 0.5f) * extentBox.x, (Random.value - 0.5f) * extentBox.y, (Random.value - 0.5f) * extentBox.z );

			GameObject prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/MolObject.prefab", typeof(GameObject)) as GameObject;
			GameObject mol = Instantiate(prefab, transform.position, transform.rotation) as GameObject;

			mol.transform.parent = transform;
			mol.transform.localPosition = position;
//			mol.GetComponent<MolScript>().minVelocity = minVelocity;
//			mol.GetComponent<MolScript>().maxVelocity = maxVelocity;
			molecules[i] = mol;
		}

		initialized = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		//print ("Hello world");
	}
}
