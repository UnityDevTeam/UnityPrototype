using UnityEngine;
using System.Collections;

[AddComponentMenu("Agent/Behaviour/RandomDistribution")]
public class RandomDistribution : BehaviourUnary
{
	void Start ()
	{
		if (!gameObject.rigidbody)
		{
			gameObject.AddComponent<Rigidbody>();
			gameObject.rigidbody.isKinematic = false;
		}
	}
	
	void Update ()
	{
		transform.position = randomPosition ();
		transform.rotation = Quaternion.Euler (randomEulerAngles());
	}

	private Vector3 randomPosition()
	{
		Vector3 dim = NewAgentSystem.systemSize;
		Vector3 min = NewAgentSystem.minBox;
		return new Vector3 ( Random.value * dim.x, Random.value * dim.y, Random.value * dim.z ) + min;
	}

	private Vector3 randomEulerAngles()
	{
		return new Vector3 (Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
	}
}
