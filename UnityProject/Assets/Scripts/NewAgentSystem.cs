﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewAgentSystem : MonoBehaviour
{
	static public float minDensity = 0.00001f;
	static public float maxDensity = 0.001f;

	public int agentsCount = 1000;

	public Vector3 extentBox = new Vector3(100, 100, 100);
	public Vector3 minBox = new Vector3(-50, -50, -50);

	[HideInInspector] public List<string>         agentTypesName            = new List<string> ();
	[HideInInspector] public List<float>          agentTypesDensityConstant = new List<float> ();
	[HideInInspector] public List<AnimationCurve> agentTypesDensityCurve    = new List<AnimationCurve> ();

	public float time = 0.0f;

	public void addAgentTypesIntensity()
	{
		AnimationCurve ac = new AnimationCurve ();

		agentTypesDensityCurve.Add (new AnimationCurve());
	}

	void Start ()
	{
		agentsCount = 0;
		float agentSystemVolume = extentBox.x * extentBox.y * extentBox.z;

		for (int i = 0; i < agentTypesName.Count; i++)
		{
			GameObject agentTypeObject = new GameObject(agentTypesName[i]);
			agentTypeObject.transform.parent = transform;

			int agentTypeCount = (int)(agentTypesDensityConstant[i] * agentSystemVolume);

			for (int j = 0; j < agentTypeCount; j++)
			{
				Vector3 position = new Vector3 ( Random.value * extentBox.x, Random.value * extentBox.y, Random.value * extentBox.z ) + minBox;
				
				GameObject prefab = Resources.Load(agentTypesName[i]) as GameObject;
				GameObject mol = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
				mol.transform.parent = agentTypeObject.transform;
				mol.transform.localPosition = position;
			}

			agentsCount += agentTypeCount;
		}
	}

	void Update ()
	{
		/*
		time += Time.deltaTime;
		float agentsCount = agentTypesDensityCurve [0].Evaluate (time);
		print (agentsCount);
		*/
	}
}
