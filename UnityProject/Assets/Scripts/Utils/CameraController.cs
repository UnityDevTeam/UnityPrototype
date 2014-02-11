using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/CameraController")]
public class CameraController : MonoBehaviour
{
	public float distance = 5.0f;
	public float xSpeed = 200.0f;
	public float ySpeed = 200.0f;
	public int yMinLimit = -80;
	public int yMaxLimit = 80;
	public int zoomRate = 40;
	public float moveSpeed = 0.3f;
	public float rotationSpeed = 5.0f;

	private Transform target;
	private float xDeg = 0.0f;
	private float yDeg = 0.0f;
	private Quaternion currentRotation;
	private Quaternion desiredRotation;
	private Quaternion rotation;
	private Vector3 position;
	
	void Start() { Init(); }
	void OnEnable() { Init(); }
	
	public void Init()
	{
		if (!target)
		{
			GameObject go = new GameObject ("Target");
			go.transform.position = transform.position + (transform.forward * distance);
			target = go.transform;
		}
		
		distance = Vector3.Distance(transform.position, target.position);
		
		//be sure to grab the current rotations as starting points.
		position = transform.position;
		rotation = transform.rotation;
		currentRotation = transform.rotation;
		desiredRotation = transform.rotation;
		
		xDeg = Vector3.Angle(Vector3.right, transform.right );
		yDeg = Vector3.Angle(Vector3.up, transform.up );
	}

	void LateUpdate()
	{
		if (Input.GetMouseButton(0))
		{
			xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
			yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
			
			////////OrbitAngle
			yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);
			desiredRotation = Quaternion.Euler(yDeg, xDeg, 0);
			currentRotation = transform.rotation;
			
			rotation = Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime * rotationSpeed);
			transform.rotation = rotation;
		}
		else if (Input.GetMouseButton(1))
		{
			target.rotation = transform.rotation;
			target.Translate(Vector3.right * -Input.GetAxis("Mouse X") * moveSpeed);
			target.Translate(transform.up * -Input.GetAxis("Mouse Y") * moveSpeed, Space.World);
		}
		else if (Input.GetKey(KeyCode.W))
		{
			target.rotation = transform.rotation;
			target.Translate(Vector3.forward * moveSpeed);
		}
		else if (Input.GetKey(KeyCode.S))
		{
			target.rotation = transform.rotation;
			target.Translate(Vector3.forward * -moveSpeed);
		}
		else if (Input.GetKey(KeyCode.A))
		{
			target.rotation = transform.rotation;
			target.Translate(Vector3.right * -moveSpeed);
		}
		else if (Input.GetKey(KeyCode.D))
		{
			target.rotation = transform.rotation;
			target.Translate(Vector3.right * moveSpeed);
		}
				
		// calculate position based on the distance 
		position = target.position - (rotation * Vector3.forward * distance);
		transform.position = position;
	}
	
	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp(angle, min, max);
	}
}