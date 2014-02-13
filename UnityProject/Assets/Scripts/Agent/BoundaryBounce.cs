using UnityEngine;
using System.Collections;

[AddComponentMenu("Agent/Condition/BoundaryBounce")]
public class BoundaryBounce : AgentConditionUnary
{
	private Vector3 worldPosition;
	private Vector3 worldSize;

	void Start ()
	{
		Transform currentTransform = transform;

		while(currentTransform && !currentTransform.gameObject.GetComponent<NewAgentSystem>())
		{
			currentTransform = currentTransform.parent;
		}

		worldPosition = currentTransform.position;
		worldSize = currentTransform.GetComponent<NewAgentSystem> ().systemSize;

	}

	void Update ()
	{
		Vector3 pos = transform.position + rigidbody.velocity;
		if (pos.x > (worldPosition.x + worldSize.x / 2.0f) || pos.x < (worldPosition.x - worldSize.x / 2.0f))
		{
			rigidbody.velocity = new Vector3 ( -rigidbody.velocity.x,  rigidbody.velocity.y,  rigidbody.velocity.z);
		}
		
		if (pos.y > (worldPosition.y + worldSize.y / 2.0f) || pos.y < (worldPosition.y - worldSize.y / 2.0f))
		{
			rigidbody.velocity = new Vector3 (  rigidbody.velocity.x, -rigidbody.velocity.y,  rigidbody.velocity.z);
		}
		
		if (pos.z > (worldPosition.z + worldSize.z / 2.0f) || pos.z < (worldPosition.z - worldSize.z / 2.0f))
		{
			rigidbody.velocity = new Vector3 (  rigidbody.velocity.x,  rigidbody.velocity.y, -rigidbody.velocity.z);
		}
	}
}
