using UnityEngine;
using System.Collections;

public class MolScript : MonoBehaviour
{
	public float     minVelocity;
	public float     maxVelocity;

	public float     bindingRadius = 5.0f;
	public float     bindingAttraction = 10.0f;

	public Vector3[] bindingPositions;
	public Vector3[] bindingOrientations;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!rigidbody.isKinematic)
		{
			rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, RandomForce(), 25 * Time.deltaTime);
			
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

			//Attract to middle
			if(transform.position.magnitude < bindingRadius)
			{
				rigidbody.AddForce(-transform.position - rigidbody.velocity * bindingAttraction);
			}

			if(transform.position.magnitude < 1.25f)
			{
				rigidbody.rotation = Quaternion.Slerp(rigidbody.rotation, Quaternion.identity, transform.position.magnitude - 0.25f);
			}

			if(transform.position.magnitude < 0.25f)
				rigidbody.isKinematic = true;
		}
	}

	private Vector3 RandomForce()
	{
		Vector3 randomize = new Vector3 ((Random.value *2) -1, (Random.value * 2) -1, (Random.value * 2) -1);
		randomize.Normalize();
		return randomize;
	}
}
