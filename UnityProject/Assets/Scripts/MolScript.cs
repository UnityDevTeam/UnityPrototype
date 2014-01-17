using UnityEngine;
using System.Collections;

public class MolScript : MonoBehaviour
{
	private bool inited = false;
	public float minVelocity;
	public float maxVelocity;
	private float randomness;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
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

	private Vector3 RandomForce()
	{
		Vector3 randomize = new Vector3 ((Random.value *2) -1, (Random.value * 2) -1, (Random.value * 2) -1);
		randomize.Normalize();
		return randomize;
	}
}
