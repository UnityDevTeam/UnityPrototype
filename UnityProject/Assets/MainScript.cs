using UnityEngine;
using UnityEditor;
using System.Collections;

public class MainScript : MonoBehaviour {
	public int molSize = 20;
	public float minVelocity = 5;
	public float maxVelocity = 20;
	public Vector3 extentBox = new Vector3(10,10,10);
	public Vector3 minBox = new Vector3(-5,-5,-5);
	private GameObject[] molecules;
	public string[] pdbPrefabs = {"Assets/testMol.prefab"};
	//public GameObject prefab;

	// Use this for initialization
	void Start () {
		molecules = new GameObject[molSize];
		for (var i=0; i<molSize; i++)
		{
			Vector3 position = new Vector3 (
				Random.value * extentBox.x,
				Random.value * extentBox.y,
				Random.value * extentBox.z
				) + minBox;
			int pdbid = (int)(Random.value * 2);
			GameObject prefab = AssetDatabase.LoadAssetAtPath(pdbPrefabs[0], typeof(GameObject)) as GameObject;
			GameObject mol = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
			mol.transform.parent = transform;
			mol.transform.localPosition = position;
			//mol.GetComponent<BoidFlocking>().SetController (gameObject);
			mol.GetComponent<MolScript>().minVelocity = minVelocity;
			mol.GetComponent<MolScript>().maxVelocity = maxVelocity;
			molecules[i] = mol;
		}

	}
	
	// Update is called once per frame
	void Update () {
		print ("Hello world");
	}
}
