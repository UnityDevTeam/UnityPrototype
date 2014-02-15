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

	public void addQueries (List<CommunicationQuery> newQueries)
	{
		// kind of dirty trick
		queries.Clear ();

		for (int i = 0; i < newQueries.Count; i++)
		{
			if(!queries.ContainsKey(newQueries[i].symbolId))
				queries.Add(newQueries[i].symbolId, newQueries[i]);
		}
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
