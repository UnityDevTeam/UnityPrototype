using UnityEngine;
using System.Collections.Generic;

public class AgentsSystem : MonoBehaviour
{
	private Transform freeMolecules = null;
	private Transform oldMolecules  = null;
	private Transform locals        = null;
	
	private float timing = 0.0f;
	private float posun  = 2.0f;
	private GameObject sphereDebug   = null;

	void Start ()
	{
		freeMolecules = transform.Find("freeMolecules");
		oldMolecules  = transform.Find("oldMolecules");
		locals        = transform.Find("locals");

		if (!freeMolecules)
		{
			GameObject objectFreeMolecules = new GameObject("freeMolecules");
			objectFreeMolecules.transform.parent = transform;
			freeMolecules = objectFreeMolecules.transform;
		}

		if (!oldMolecules)
		{
			GameObject objectOldMolecules = new GameObject("oldMolecules");
			objectOldMolecules.transform.parent = transform;
			oldMolecules = objectOldMolecules.transform;
		}

		if (!locals)
		{
			GameObject objectLocals = new GameObject("locals");
			objectLocals.transform.parent = transform;
			locals = objectLocals.transform;
		}

		sphereDebug = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		sphereDebug.transform.localScale = new Vector3(10.0f, 10.0f, 10.0f);
		sphereDebug.transform.parent = transform;
		Destroy (sphereDebug.GetComponent<MeshRenderer> ());

		createLocalSystem ("local", Vector3.zero);
	}

	public void createLocalSystem(string name, Vector3 position)
	{
		GameObject go = new GameObject (name);
		go.transform.parent   = locals.transform;
		go.transform.position = position;
		go.AddComponent("LocalAgentSystem");
		go.GetComponent<LocalAgentSystem> ().setGlobalSystem (gameObject);
	}

	public void removeLocalSystem(string name)
	{
		Transform local = locals.transform.Find (name);

		if (local)
		{
			int childCount = local.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = local.GetChild(0);
				child.parent = freeMolecules.transform;
			}

			DestroyImmediate(local.gameObject);
		}
	}

	public void distributeFreeMolecules()
	{
		int freeMoleculesCount = freeMolecules.childCount;
		int freeMoleculesIndex = 0;
		for (int i = 0; i < freeMoleculesCount; i++)
		{	
			for (int j = 0; j < locals.childCount; j++)
			{
				GameObject child = locals.transform.GetChild(j).gameObject;

				int maxLocalCount = child.GetComponent<LocalAgentSystem>().count;
				if(child.transform.childCount >= maxLocalCount)
				{
					break;
				}

				GameObject molChild = freeMolecules.GetChild(freeMoleculesIndex).gameObject;
				Vector3 distance = molChild.transform.position - child.transform.position;
				
				if (distance.magnitude < child.GetComponent<LocalAgentSystem>().size / 2.0f)
				{
					freeMolecules.GetChild(freeMoleculesIndex).parent = child.transform;
					break;
				}
				freeMoleculesIndex++;
			}
		}
	}

	public void removeFreeMolecules()
	{
		int freeMoleculesCount = freeMolecules.childCount;
		for (int i = 0; i < freeMoleculesCount; i++)
		{
			DestroyImmediate(freeMolecules.GetChild(0).gameObject);
		}
	}

	public void removeOldMolecules()
	{
		int oldMoleculesCount = oldMolecules.childCount;
		for (int i = 0; i < oldMoleculesCount; i++)
		{
			DestroyImmediate(oldMolecules.GetChild(0).gameObject);
		}
	}

	public void TimeUpdateFreeMolecules()
	{
		int freeMoleculesCount = freeMolecules.childCount;
		int freeMoleculesIndex = 0;

		for (int i = 0; i < freeMoleculesCount; i++)
		{
			Transform child = freeMolecules.GetChild(freeMoleculesIndex);
			child.GetComponent<MolScript>().life -= Time.deltaTime;

			if(child.GetComponent<MolScript>().life < 0)
			{
				child.parent = oldMolecules;
			}
			else
			{
				freeMoleculesIndex++;
			}
		}
		removeOldMolecules ();
	}

	void Update ()
	{
		/*
		if(timing >= 0.0f)
			timing += Time.deltaTime;
		
		if (timing > 1.0f)
		{
			removeLocalSystem("local");
		}
		*/

		TimeUpdateFreeMolecules ();
		distributeFreeMolecules ();
		/*
		if(timing >= 0.0f)
			timing += Time.deltaTime;

		if (timing > 1.0f)
		{
			removeLocalSystem("local");
			createLocalSystem("local", Vector3.zero + new Vector3(posun, 0.0f, 0.0f));

			sphereDebug.transform.position = Vector3.zero + new Vector3(posun, 0.0f, 0.0f);

			posun += 2.0f;

			distributeFreeMolecules();
			removeFreeMolecules();

			timing = 0.0f;
		}
		*/
	}
}
