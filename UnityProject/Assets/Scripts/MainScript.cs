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
	private bool evenTurn = true;

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
		//material.SetColor("_Color", Color.red);
		//material.SetFloat("_SpriteSize", 20);
		//material.SetBuffer("particles", newPosVelo);
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

	// Update is called once per frame
	void Update ()
	{
		//print ("Update");

		int dimx = (molCount + (blockSize - 1)) / blockSize;
		
		if (computeShader) {
			// run compute shader
			if (!evenTurn)
			{
				computeShader.SetBuffer(0, "oldPosVelo", newPosVelo);
				computeShader.SetBuffer(0, "newPosVelo", oldPosVelo);
			} else
			{
				computeShader.SetBuffer(0, "oldPosVelo", oldPosVelo);
				computeShader.SetBuffer(0, "newPosVelo", newPosVelo);
			}
			evenTurn=!evenTurn;

			
			computeShader.SetFloat("dt", Time.deltaTime);
			computeShader.SetFloat("damping", damping);
			computeShader.SetInts("numParticles", molCount);
			computeShader.SetInts("dimx", dimx);
			
			computeShader.Dispatch(0, dimx, 1, 1);
		}
	}

	void OnRenderObject () {
		if (material) {
			print ("Post render Update");
			//material.SetBuffer("oldPosVelo", oldPosVelo);
			//material.SetBuffer("newPosVelo", newPosVelo);
			material.SetPass (0);
			material.SetColor("_Color", Color.red);
			material.SetFloat("_SpriteSize", 1.0f);
			if (!evenTurn)
				material.SetBuffer("particles", newPosVelo);
			else
				material.SetBuffer("particles", oldPosVelo);
			Graphics.DrawProcedural(MeshTopology.Points, molCount);
		}
	}

	void OnDisable()
	{
		if(oldPosVelo != null)
			oldPosVelo.Dispose();
		if(newPosVelo != null)
			newPosVelo.Dispose();
	}
}
