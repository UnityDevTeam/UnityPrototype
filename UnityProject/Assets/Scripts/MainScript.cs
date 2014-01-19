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
	public string[] pdbPrefabs = {"Assets/Prefabs/testMol.prefab","Assets/Prefabs/adp.prefab"};

	private GameObject[] molecules;
	private bool initialized = false;

	// Use this for initialization
	void Start ()
	{
		createBoundaryObjects ();
		if (!initialized) SpawnObjects ();
	}

	public void SpawnObjects()
	{
		molecules = new GameObject[molSize];
		for (var i=0; i<molSize; i++)
		{
			Vector3 position = new Vector3 ( Random.value * extentBox.x, Random.value * extentBox.y, Random.value * extentBox.z ) + minBox;
			int pdbid = Random.Range (0,2);
			GameObject prefab = AssetDatabase.LoadAssetAtPath(pdbPrefabs[1], typeof(GameObject)) as GameObject;
			GameObject mol = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
			mol.transform.parent = transform;
			mol.transform.localPosition = position;
			mol.GetComponent<MolScript>().minVelocity = minVelocity;
			mol.GetComponent<MolScript>().maxVelocity = maxVelocity;
			molecules[i] = mol;
		}

		initialized = true;
	}

	void createBoundaryObjects ()
	{

		GameObject zP = new GameObject ("Z+");
		zP.transform.position = transform.position + new Vector3 (0, 0, minBox.z + extentBox.z);
		zP.transform.parent = transform;
		zP.AddComponent ("BoxCollider");
		zP.GetComponent<BoxCollider> ().size = new Vector3 (extentBox.x, extentBox.y, 1);

		GameObject zM = new GameObject ("Z-");
		zM.transform.position = transform.position + new Vector3 (0, 0, minBox.z);
		zM.transform.parent = transform;
		zM.AddComponent ("BoxCollider");
		zM.GetComponent<BoxCollider> ().size = new Vector3 (extentBox.x, extentBox.y, 1);

		GameObject xP = new GameObject ("X+");
		xP.transform.position = transform.position + new Vector3 (minBox.x + extentBox.x, 0, 0);
		xP.transform.parent = transform;
		xP.AddComponent ("BoxCollider");
		xP.GetComponent<BoxCollider> ().size = new Vector3 (1, extentBox.y, extentBox.z);

		GameObject xM = new GameObject ("X-");
		xM.transform.position = transform.position + new Vector3 (minBox.x, 0, 0);
		xM.transform.parent = transform;
		xM.AddComponent ("BoxCollider");
		xM.GetComponent<BoxCollider> ().size = new Vector3 (1, extentBox.y, extentBox.z);

		GameObject yP = new GameObject ("Y+");
		yP.transform.position = transform.position + new Vector3 (0, minBox.y + extentBox.y, 0);
		yP.transform.parent = transform;
		yP.AddComponent ("BoxCollider");
		yP.GetComponent<BoxCollider> ().size = new Vector3 (extentBox.x, 1, extentBox.z);
		
		
		GameObject yM = new GameObject ("Y-");
		yM.transform.position = transform.position + new Vector3 (0, minBox.y, 0);
		yM.transform.parent = transform;
		yM.AddComponent ("BoxCollider");
		yM.GetComponent<BoxCollider> ().size = new Vector3 (extentBox.x, 1, extentBox.z);

	}

	// Update is called once per frame
	void Update ()
	{
		//print ("Hello world");
	}
}
