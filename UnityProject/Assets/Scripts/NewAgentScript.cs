using UnityEngine;
using System.Collections;

public class NewAgentScript : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{/*
		if (rigidbody && !rigidbody.isKinematic)
		{
			rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, RandomForce(), Time.deltaTime);
			
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

			// Bouncing - will be removed or specified otherwise
			Vector3 pos = transform.position + rigidbody.velocity;
			if (Mathf.Abs(pos.x) > 50)
			{
				rigidbody.velocity = new Vector3 ( -rigidbody.velocity.x,  rigidbody.velocity.y,  rigidbody.velocity.z);
			}
				
			if (Mathf.Abs(pos.y) > 50)
			{
				rigidbody.velocity = new Vector3 (  rigidbody.velocity.x, -rigidbody.velocity.y,  rigidbody.velocity.z);
			}

			if (Mathf.Abs(pos.z) > 50)
			{
				rigidbody.velocity = new Vector3 (  rigidbody.velocity.x,  rigidbody.velocity.y, -rigidbody.velocity.z);
			}
		}*/
	}
	/*
	private Vector3 RandomForce()
	{
		Vector3 randomize = new Vector3 ((Random.value *2) -1, (Random.value * 2) -1, (Random.value * 2) -1);
		randomize.Normalize();
		return randomize;
	}
	*/
}
