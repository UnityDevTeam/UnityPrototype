using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommunicationManager : MonoBehaviour
{
	public Dictionary<int, CommunicationQuery> queries = new Dictionary<int, CommunicationQuery>();

	public List<CommunicationQuery> getQueries()
	{
		return new List<CommunicationQuery> (queries.Values);
	}

	public void Add(int stateId, CommunicationSymbol symbol)
	{
		CommunicationQuery query = new CommunicationQuery(stateId, symbol.globalPosition, symbol.globalOrientation, symbol.resultType, symbol.timer, symbol.probability);

		if (queries.ContainsKey (query.stateId))
		{
			queries [query.stateId] = query;
		}
		else
		{
			queries.Add (query.stateId, query);
		}
	}

	public void Remove(int stateId)
	{
		queries.Remove (stateId);
	}

	void Update()
	{
		int childs = transform.childCount;
		for (int i = 0; i < childs; i++)
		{
			DestroyImmediate(transform.GetChild(0).gameObject);
		}

		if (LSystem.timeDelta > 0.6f)
		{
			Movement.bindingMonomerID = 0;
			Movement.queryId = -1;
			Movement.bindingTimerSaved = 0.0f;
			Movement.bindingTimer = 0.0f;
			NewAgentSystem.motionTimer = 0.0f;
			NewAgentSystem.motionTime = 0.0f;

			foreach (KeyValuePair<int, CommunicationQuery> query in queries)
			{
				float prob = query.Value.probability.Evaluate(LSystem.timeDelta);

				bool populate = prob == 1.0f;

				if(!populate)
				{
					float chance = UnityEngine.Random.value;

					if(chance < prob)
						populate = true;
				}

				if(populate && LSystem.canAddItem)
				{
					GameObject go = Resources.Load(queries[query.Key].type) as GameObject;
					go = Instantiate(go, Vector3.zero, Quaternion.identity) as GameObject;
					go.transform.parent = transform;
					go.SetActive(false);
					queries[query.Key].result = go;
				}
			}
		}
	}
}
