using UnityEngine;
using System.Collections.Generic;

public class AgentsSystem : MonoBehaviour
{
	private Transform freeMolecules = null;
	private Transform oldMolecules  = null;
	private Transform locals        = null;

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
	}

	public void createLocalSystem(string name, Vector3 position, Quaternion orientation)
	{
		// TODO: better identification of local agents system
		for (int i = 0; i < locals.childCount; i++)
		{
			if(locals.GetChild(i).position == position)
			{
				return;
			}
		}

		GameObject go = new GameObject (name);
		go.transform.parent   = locals.transform;
		go.transform.position = position;
		go.AddComponent("LocalAgentSystem");
		go.GetComponent<LocalAgentSystem> ().setGlobalSystem (gameObject);
		go.GetComponent<LocalAgentSystem> ().bindingOrientation = orientation;
	}

	public void removeLocalSystem(Transform local)
	{	
		int childCount = local.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = local.GetChild(0);
			child.parent = freeMolecules.transform;
		}

		DestroyImmediate(local.gameObject);
	}

	public void distributeFreeMolecules()
	{
		RemoveInactiveFreeMolecules ();

		int freeMoleculesCount = freeMolecules.childCount;
		int freeMoleculesIndex = 0;
		for (int i = 0; i < freeMoleculesCount; i++)
		{
			bool removed = false;
			for (int j = 0; j < locals.childCount; j++)
			{
				GameObject   child       = locals.transform.GetChild(j).gameObject;
				GameObject molChild      = freeMolecules.GetChild(freeMoleculesIndex).gameObject;
				int        maxLocalCount = child.GetComponent<LocalAgentSystem>().count;
				Vector3    distance      = molChild.transform.position - child.transform.position;
				
				if ( (child.transform.childCount >= maxLocalCount) && (distance.magnitude < child.GetComponent<LocalAgentSystem>().size / 2.0f))
				{
					freeMolecules.GetChild(freeMoleculesIndex).parent = child.transform;
					removed = true;
					break;
				}
			}

			if(!removed)
			{
				freeMoleculesIndex++;
			}
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

	public List<Vector3> CheckLocalAgentsSystems()
	{
		List<Vector3> returnValue = new List<Vector3> ();

		int localsCount = locals.childCount;
		for (int i = 0; i < localsCount; i++)
		{
			GameObject local = locals.GetChild(i).gameObject;

			if(local.GetComponent<LocalAgentSystem>().finished)
			{
				returnValue.Add(local.transform.position);
			}
		}

		return returnValue;
	}

	public void RemoveFinishedLocalAgentsSystems(List<Vector3> finishedLocalAgentsSystems)
	{
		int localAgentsCount = locals.childCount;
		int localAgentsIndex = 0;
		bool removed = false;

		for (int i = 0; i < localAgentsCount; i++)
		{
			for(int j = 0; j < finishedLocalAgentsSystems.Count; j++)
			{
				Transform localChild = locals.GetChild(localAgentsIndex);
				if(localChild.position == finishedLocalAgentsSystems[j])
				{
					removeLocalSystem(localChild);
					removed = true;
					break;
				}
			}

			// This shouldn't happen
			if(!removed)
			{
				localAgentsIndex++;
			}
		}
	}

	private void RemoveInactiveFreeMolecules()
	{	
		int freeMoleculesCount = freeMolecules.childCount;
		int freeMoleculesIndex = 0;
		for (int i = 0; i < freeMoleculesCount; i++)
		{
			Transform molecule = freeMolecules.GetChild(freeMoleculesIndex);

			if(molecule.gameObject.rigidbody.isKinematic)
			{
				Destroy (molecule.gameObject);
				break;
			}

			freeMoleculesIndex++;
		}
	}

	void Update ()
	{
		TimeUpdateFreeMolecules ();
		distributeFreeMolecules ();
	}
}
