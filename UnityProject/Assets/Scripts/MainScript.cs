using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class MainScript : MonoBehaviour
{
	public int molCount = 1000;

	public Vector3 extentBox = new Vector3(100, 100, 100);
	public float damping = 0.999f;

	private GameObject[] molecules;
	private bool initialized = false;

	private Mesh mesh;
	private Material material;

	//compute shaders
	public ComputeShader computeShader;
	private ComputeBuffer oldPosVelo;
	private ComputeBuffer newPosVelo;
	
	private int blockSize = 128;   
	private int posVeloSizeOf;
	struct PosVelo
	{
		public Vector3 position;
		public Vector3 velocity;
	};

	// Use this for initialization
	void Start ()
	{
		PosVelo pv = new PosVelo();
		posVeloSizeOf = Marshal.SizeOf(pv);
		oldPosVelo = new ComputeBuffer(molCount, posVeloSizeOf);
		newPosVelo = new ComputeBuffer(molCount, posVeloSizeOf);
		PosVelo[] pvInitBuffer = new PosVelo[molCount];
		
		for (var i=0; i < molCount; i++) 
		{
				Vector3 position = new Vector3 ((Random.value - 0.5f) * extentBox.x, (Random.value - 0.5f) * extentBox.y, (Random.value - 0.5f) * extentBox.z);
				pv = new PosVelo();
				pv.position = position;
				pv.velocity = Random.insideUnitSphere;
				pvInitBuffer[i] = pv;
		}
		oldPosVelo.SetData(pvInitBuffer);
		newPosVelo.SetData(pvInitBuffer);
		material = new Material(Shader.Find("Custom/MolShader"));
		material.SetColor("_Color", Color.red);
		material.SetFloat("_SpriteSize", 20);
		material.SetBuffer("particles", newPosVelo);
		/*
		if (!initialized)
		{
				SpawnObjects ();
		}
		*/
	}
	public void SpawnObjects()
	{
	}
	/*
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
		material.SetFloat("_SpriteSize", 20);

		initialized = true;
	}
	*/
	// Update is called once per frame
	void Update ()
	{
		//print ("Update");
		/*
		int dimx = (molCount + (blockSize - 1)) / blockSize;
		
		if (computeShader) {
			// run compute shader
			computeShader.SetBuffer(0, "oldPosVelo", oldPosVelo);
			computeShader.SetBuffer(0, "newPosVelo", newPosVelo);
			
			computeShader.SetFloat("dt", Time.deltaTime);
			computeShader.SetFloat("damping", damping);
			computeShader.SetInts("numParticles", molCount);
			computeShader.SetInts("dimx", dimx);
			
			computeShader.Dispatch(0, dimx, 1, 1);
		}
		*/
		//Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, material, 0);
		if (material) {
			print ("Post render Update");
			//material.SetBuffer("oldPosVelo", oldPosVelo);
			//material.SetBuffer("newPosVelo", newPosVelo);
			material.SetPass (0);
			material.SetColor("_Color", Color.red);
			material.SetFloat("_SpriteSize", 20);
			material.SetBuffer("particles", newPosVelo);
			Graphics.DrawProcedural(MeshTopology.Points, molCount, 0);
		}
	}
	/*
	void OnPostRender () {
		if (material) {
			print ("Post render Update");
			//material.SetBuffer("oldPosVelo", oldPosVelo);
			//material.SetBuffer("newPosVelo", newPosVelo);
			material.SetPass (0);
			material.SetColor("_Color", Color.red);
			material.SetFloat("_SpriteSize", 20);
			material.SetBuffer("particles", newPosVelo);
			Graphics.DrawProcedural(MeshTopology.Points, molCount, 0);
		}
	}
	*/

	void OnDisable()
	{
		if(oldPosVelo != null)
			oldPosVelo.Dispose();
		if(newPosVelo != null)
			newPosVelo.Dispose();
	}
}
