using UnityEngine;
using System.Collections;

public class MainScript : MonoBehaviour
{
	public int molCount = 1000;

	public Vector3 extentBox = new Vector3(100, 100, 100);

	private GameObject[] molecules;
	private bool initialized = false;

	private Mesh mesh;
	private Material material;

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
//		if (initialized) 
//		{
//			foreach(GameObject obj in molecules)
//			{
//				DestroyImmediate(obj);
//			}				
//		}

		molecules = new GameObject[molCount];
		Vector3[] vertices = new Vector3[molCount];
		int[] indices = new int[molCount];

		for (var i=0; i < molCount; i++)
		{
			Vector3 position = new Vector3 ( (Random.value - 0.5f) * extentBox.x, (Random.value - 0.5f) * extentBox.y, (Random.value - 0.5f) * extentBox.z );

			indices[i] = i;
			vertices[i] = position;

//			GameObject mol = Instantiate(Resources.Load<GameObject>("MolObject"), transform.position, transform.rotation) as GameObject;
//			mol.transform.parent = transform;
//			mol.transform.localPosition = position;
//			molecules[i] = mol;
		}

		mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.SetIndices(indices, MeshTopology.Points, 0);

		material = new Material(Shader.Find("Custom/MolShader"));
		material.SetColor("_Color", Color.red);
		material.SetFloat("_SpriteSize", 10);

		initialized = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		//print ("Update");

		Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, material, 0);
	}
}
