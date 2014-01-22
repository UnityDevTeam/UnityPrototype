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

	[HideInInspector] public Quaternion bindingOrientation;
	public float life    = 2.0f;
	
	void Start ()
	{
		Transform tr  = transform.GetChild (0);
		Color col = tr.renderer.material.color;
		col.a = life / 2.0f;
		tr.renderer.material.color = col;
	}

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

			if(transform.parent.GetComponent<LocalAgentSystem>())
			{
				Vector3 pos = transform.position - transform.parent.position;
				Vector3 nPos = pos + rigidbody.velocity;

				if(pos.magnitude < bindingRadius)
				{
					rigidbody.AddForce(-pos * bindingAttraction);
				}

				if(pos.magnitude < 1.25f)
				{
					rigidbody.rotation = Quaternion.Slerp(rigidbody.rotation, Quaternion.identity, pos.magnitude - 0.25f);
				}

				if(pos.magnitude < 0.4f)
				{
					rigidbody.isKinematic = true;
					transform.parent.GetComponent<LocalAgentSystem>().finished = true;
					Destroy(GetComponent<MeshRenderer>());
				}
			}
		}

		Transform tr  = transform.GetChild (0);
		Color col = tr.renderer.material.color;
		col.a = life / 2.0f;
		tr.renderer.material.color = col;
	}

	private Vector3 RandomForce()
	{
		Vector3 randomize = new Vector3 ((Random.value *2) -1, (Random.value * 2) -1, (Random.value * 2) -1);
		randomize.Normalize();
		return randomize;
	}
}
