using UnityEngine;
using System.Collections;

[AddComponentMenu("Agent/Behaviour/RandomMove")]
public class RandomMove : BehaviourUnary
{
	public static float speed = 5.0f;
	
	private Vector3 randomDirection = Vector3.zero;
	private float randomTime = 0.0f;
	private float timer = 0.0f;


	void Start ()
	{
		if (!gameObject.rigidbody)
		{
			gameObject.AddComponent<Rigidbody>();
			gameObject.rigidbody.isKinematic = false;
		}
	
		if (!gameObject.collider)
		{
			gameObject.AddComponent<SphereCollider>();
			gameObject.GetComponent<SphereCollider>().radius = gameObject.GetComponent<MeshFilter>().mesh.bounds.extents.x;
		}

		randomDirection = speed * randomNormalVector ();
	}

	void Update ()
	{
		if (!rigidbody.isKinematic)
		{
			timer += Time.deltaTime;
			
			if (timer > randomTime)
			{
				randomDirection = speed * randomNormalVector ();
				
				randomTime = Random.Range (0.5f, 2.0f);
				timer = 0.0f;
			}
			
			rigidbody.velocity = Vector3.Lerp (rigidbody.velocity, randomDirection, Time.deltaTime );
			rigidbody.velocity = rigidbody.velocity.normalized * speed;
		}
	}

	private Vector3 randomNormalVector()
	{
		Vector3 randomize = new Vector3 ((Random.value *2) -1, (Random.value * 2) -1, (Random.value * 2) -1);
		randomize.Normalize();
		return randomize;
	}
}
