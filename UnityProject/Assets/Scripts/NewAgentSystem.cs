using UnityEngine;
using System.Collections;

public class NewAgentSystem : MonoBehaviour
{
	public int agentsCount = 1000;
	public Vector3 extentBox = new Vector3(100, 100, 100);
	public Vector3 minBox = new Vector3(-50, -50, -50);
	public string pdbPrefabs = "testAgent2";
	
	void Start ()
	{
		for (int i = 0; i < agentsCount; i++)
		{
			Vector3 position = new Vector3 ( Random.value * extentBox.x, Random.value * extentBox.y, Random.value * extentBox.z ) + minBox;

			GameObject prefab = Resources.Load(pdbPrefabs) as GameObject;
			GameObject mol = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
			mol.transform.parent = transform;
			mol.transform.localPosition = position;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
