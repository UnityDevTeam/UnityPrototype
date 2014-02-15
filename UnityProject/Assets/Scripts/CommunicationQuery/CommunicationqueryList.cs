using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommunicationQueryList : MonoBehaviour
{
	public Dictionary<int, CommunicationQuery> queries = new Dictionary<int, CommunicationQuery>();

	private float timer = 0.0f;

	public List<CommunicationQuery> getQueries()
	{
		return new List<CommunicationQuery> (queries.Values);
	}

	public void Add(CommunicationQuery query)
	{
		queries.Add (query.symbolId, query);
	}

	public void Remove(int symbolId)
	{
		queries.Remove (symbolId);
	}

	void Start ()
	{
	
	}

	void Update ()
	{
		timer += Time.deltaTime;

		if (timer > 4.0f)
		{
			if(queries.Count > 0)
			{
				foreach(KeyValuePair<int, CommunicationQuery> query in queries)
				{
					query.Value.result = new GameObject("test");
					break;
				}
			}

			timer = 0.0f;
		}
	}
}
