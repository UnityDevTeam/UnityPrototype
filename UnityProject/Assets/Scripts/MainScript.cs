using UnityEngine;
using System.Collections;

public class MainScript : MonoBehaviour
{
	public int molCount = 1000;

	// Use this for initialization
	void Start ()
	{

	}

	public void CreateMolContainer()
	{
		Vector3[] vertices = new Vector3[molCount];
		int[] indices = new int[molCount];

		for (var i=0; i < molCount; i++)
		{
			indices[i] = i;
			vertices[i] = new Vector3 ( (Random.value - 0.5f), (Random.value - 0.5f), (Random.value - 0.5f));
		}

		GameObject molContainer = new GameObject("Mol Container");	

		MeshRenderer meshRenderer = molContainer.AddComponent<MeshRenderer>();
		meshRenderer.material = Resources.Load("MolMaterial") as Material;

		MeshFilter meshFilter = molContainer.AddComponent<MeshFilter>();	
		meshFilter.sharedMesh = new Mesh();
		meshFilter.sharedMesh.vertices = vertices;
		meshFilter.sharedMesh.SetIndices(indices, MeshTopology.Points, 0);

//		material = Resources.Load("MolMaterial") as Material;
//
//		mesh = new Mesh();
//		mesh.vertices = vertices;
//		mesh.SetIndices(indices, MeshTopology.Points, 0);
	}
	
	// Update is called once per frame
	void Update ()
	{
		//Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, material, 0);
	}
}
