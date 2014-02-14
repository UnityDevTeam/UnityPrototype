using UnityEngine;
using System.Collections;

public class Turtle
{
	public Quaternion direction;
	public Vector3    position;
	
	public Turtle (Turtle other)
	{
		this.direction = other.direction;
		this.position  = other.position;
	}
	
	public Turtle (Quaternion direction, Vector3 position)
	{
		this.direction = direction;
		this.position  = position;
	}
}
