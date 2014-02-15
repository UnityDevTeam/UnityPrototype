using UnityEngine;
using System.Collections;

[AddComponentMenu("Agent/Behaviour/RandomRotate")]
public class RandomRotate : BehaviourUnary
{	
	private Quaternion randomOrientation = Quaternion.identity;
	private float randomTime = 0.0f;
	private float timer = 0.0f;
	
	
	void Start ()
	{
		randomOrientation.eulerAngles = randomEulerAngles ();
	}
	
	void Update ()
	{
		timer += Time.deltaTime;
		
		if (timer > randomTime)
		{
			randomOrientation.eulerAngles = randomEulerAngles ();
			
			randomTime = Random.Range (0.5f, 2.0f);
			timer = 0.0f;
		}

		transform.rotation = Quaternion.Slerp (transform.rotation, randomOrientation, Time.deltaTime);
	}
	
	private Vector3 randomEulerAngles()
	{
		return new Vector3 (Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
	}
}