using UnityEngine;
using System;
using System.Collections.Generic;

public class LSystem : MonoBehaviour
{	
	public string _axiom               = "F";
	public float  _angle               = 22.5f;
	public int    _numberOfDerivations = 3;
	public float _segmentWidth         = 0.1f;
	public float _segmentHeight        = 1f;

	public Material _trunkMaterial;

	public Rules _rules = new Rules ();
	public string _moduleString;
	
	private Transform _trunk;
	private Dictionary<int, Mesh> _segmentsCache = new Dictionary<int, Mesh> ();
	
	private struct Turtle
	{
		public Quaternion direction;
		public Vector3 position;
		public Vector3 step;
		
		public Turtle (Turtle other)
		{
			this.direction = other.direction;
			this.position = other.position;
			this.step = other.step;
		}
		
		public Turtle (Quaternion direction, Vector3 position, Vector3 step)
		{
			this.direction = direction;
			this.position = position;
			this.step = step;
		}
		
		public void Forward ()
		{
			position += direction * step;
		}
		
		public void RotateX (float angle)
		{
			direction *= Quaternion.Euler (angle, 0, 0);
		}
		
		public void RotateY (float angle)
		{
			direction *= Quaternion.Euler (0, angle, 0);
		}
		
		public void RotateZ (float angle)
		{
			direction *= Quaternion.Euler (0, 0, angle);
		}
	}
	
	void Start ()
	{
		Rule rule = Rule.Build ("F=(1)F[-&^F][^++&F]||F[--&^F][+&F]");
		_rules.Add (rule);
		
		gameObject.AddComponent<BoxCollider> ();
		
		GameObject child = new GameObject ("Trunk");
		child.transform.parent = transform;
		_trunk = child.transform;

		Derive ();
		Interpret ();
	}
		
	void Derive ()
	{
		_moduleString = _axiom;
		for (int i = 0; i < Math.Max(1, _numberOfDerivations); i++)
		{
			string newModuleString = "";
			for (int j = 0; j < _moduleString.Length; j++)
			{
				string module = _moduleString [j] + "";
				
				if (!_rules.Contains (module))
				{
					newModuleString += module;
					continue;
				}
				
				Rule rule = _rules.Get (module);
				newModuleString += rule.successor;
			}
			_moduleString = newModuleString;
		}
		
	}
	
	void CreateNewChunk (Mesh mesh, ref int count)
	{
		GameObject chunk = new GameObject ("Chunk " + (++count));
		chunk.transform.parent = _trunk;
		chunk.transform.localPosition = Vector3.zero;
		chunk.AddComponent<MeshRenderer> ().material = _trunkMaterial;
		chunk.AddComponent<MeshFilter> ().mesh = mesh;
	}
	
	void CreateSegment (Turtle turtle, int nestingLevel, ref Mesh currentMesh, ref int chunkCount)
	{
		Vector3[] newVertices;
		Vector3[] newNormals;
		Vector2[] newUVs;
		int[] newIndices;
		
		Mesh segment;
		if (_segmentsCache.ContainsKey (nestingLevel)) {
			segment = _segmentsCache [nestingLevel];
		} else {
			float thickness = _segmentWidth * 0.5f;
			segment = ProceduralMeshes.CreateCylinder (3, 3, thickness, _segmentHeight);
			_segmentsCache [nestingLevel] = segment;
		}
		
		newVertices = segment.vertices;
		newNormals = segment.normals;
		newUVs = segment.uv;
		newIndices = segment.triangles;
		
		if (currentMesh.vertices.Length + newVertices.Length > 65000) {
			CreateNewChunk (currentMesh, ref chunkCount);
			currentMesh = new Mesh ();
		}
		
		int numVertices = currentMesh.vertices.Length + newVertices.Length;
		int numTriangles = currentMesh.triangles.Length + newIndices.Length;
		
		Vector3[] vertices = new Vector3[numVertices];
		Vector3[] normals = new Vector3[numVertices];
		int[] indices = new int[numTriangles];
		Vector2[] uvs = new Vector2[numVertices];
		
		Array.Copy (currentMesh.vertices, 0, vertices, 0, currentMesh.vertices.Length);
		Array.Copy (currentMesh.normals, 0, normals, 0, currentMesh.normals.Length);
		Array.Copy (currentMesh.triangles, 0, indices, 0, currentMesh.triangles.Length);
		Array.Copy (currentMesh.uv, 0, uvs, 0, currentMesh.uv.Length);
		
		Vector3 vertexOffset = turtle.position - (turtle.direction * (new Vector3 (_segmentWidth, _segmentHeight, 0) * 0.5f));
		
		int offset = currentMesh.vertices.Length;
		for (int i = 0; i < newVertices.Length; i++) {
			Vector3 vertex = newVertices [i];
			vertices [offset + i] = vertexOffset + (turtle.direction * vertex);
		}
		
		int trianglesOffset = currentMesh.vertices.Length;
		offset = currentMesh.triangles.Length;
		for (int i = 0; i < newIndices.Length; i++) {
			int index = newIndices [i];
			indices [offset + i] = (trianglesOffset + index);
		}
		
		Array.Copy (newNormals, 0, normals, currentMesh.normals.Length, newNormals.Length);
		Array.Copy (newUVs, 0, uvs, currentMesh.uv.Length, newUVs.Length);
		
		currentMesh.vertices = vertices;
		currentMesh.normals = normals;
		currentMesh.triangles = indices;
		currentMesh.uv = uvs;
		
		currentMesh.Optimize ();
	}
	
	void DestroyTrunk()
	{
		for (int i = 0; i < _trunk.childCount; i++) {
			Destroy (_trunk.GetChild (i).gameObject);
		}
		
		_trunk.DetachChildren ();
	}
	
	void Interpret ()
	{
		_segmentsCache.Clear ();
		
		DestroyTrunk();
		
		Mesh currentMesh = new Mesh ();
		
		int chunkCount = 0;
		
		Turtle current = new Turtle (Quaternion.identity, Vector3.zero, new Vector3 (0, _segmentHeight, 0));
		Stack<Turtle> stack = new Stack<Turtle> ();
		for (int i = 0; i < _moduleString.Length; i++) {
			string module = _moduleString [i] + "";
			
			if (module == "F") {
				current.Forward ();
				CreateSegment (current, stack.Count, ref currentMesh, ref chunkCount);
			} else if (module == "+") {
				current.RotateZ (_angle);
			} else if (module == "-") {
				current.RotateZ (-_angle);
			} else if (module == "&") {
				current.RotateX (_angle);
			} else if (module == "^") {
				current.RotateX (-_angle);
			} else if (module == "\\") {
				current.RotateY (_angle);
			} else if (module == "/") {
				current.RotateY (-_angle);
			} else if (module == "|") {
				current.RotateZ (180);
			} else if (module == "[") {
				stack.Push (current);
				current = new Turtle (current);
			} else if (module == "]") {
				current = stack.Pop ();
			}
		}
		
		CreateNewChunk (currentMesh, ref chunkCount);
		
		UpdateColliderBounds ();
	}
	
	void UpdateColliderBounds ()
	{
		// Calculate AABB
		Vector3 min = new Vector3 (float.MaxValue, float.MaxValue, float.MaxValue);
		Vector3 max = new Vector3 (float.MinValue, float.MinValue, float.MinValue);
		for (int i = 0; i < _trunk.childCount; i++) {
			Transform chunk = _trunk.GetChild (i);
			min.x = Mathf.Min (min.x, chunk.renderer.bounds.min.x);
			min.y = Mathf.Min (min.y, chunk.renderer.bounds.min.y);
			min.z = Mathf.Min (min.z, chunk.renderer.bounds.min.z);
			max.x = Mathf.Max (max.x, chunk.renderer.bounds.max.x);
			max.y = Mathf.Max (max.y, chunk.renderer.bounds.max.y);
			max.z = Mathf.Max (max.z, chunk.renderer.bounds.max.z);
		}
		
		Bounds bounds = new Bounds ();
		bounds.SetMinMax (min, max);
		
		BoxCollider collider = gameObject.GetComponent<BoxCollider> ();
		collider.center = bounds.center - transform.position;
		collider.extents = bounds.extents;
	}
	
	void Update ()
	{

	}
}