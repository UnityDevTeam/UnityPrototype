using UnityEngine;
using System.Collections;

public class MolScript : MonoBehaviour
{
	public float  minVelocity;
	public float  maxVelocity;
	private float randomness;

	public Vector3[] bindingPositions;
	public Vector3[] bindingOrientations;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (rigidbody.isKinematic)
		{
			rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, RandomForce(), 25 * Time.deltaTime);
			Debug.Log (rigidbody.velocity);
			
			// enforce minimum and maximum speeds
			float speed = rigidbody.velocity.magnitude;
			if (speed > maxVelocity)
			{
				rigidbody.velocity = rigidbody.velocity.normalized * maxVelocity;
			}
			else if (speed < minVelocity)
			{
				rigidbody.velocity = rigidbody.velocity.normalized * minVelocity;
			}
		}
	}

	private Vector3 RandomForce()
	{
		Vector3 randomize = new Vector3 ((Random.value *2) -1, (Random.value * 2) -1, (Random.value * 2) -1);
		randomize.Normalize();
		return randomize;
	}
}
