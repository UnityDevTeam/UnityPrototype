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

	public void updateFromAgents (List<CommunicationQuery> updatedQueries)
	{
		for (int i = 0; i < updatedQueries.Count; i++)
		{
			if(queries.ContainsKey(updatedQueries[i].symbolId))
			{
				queries[updatedQueries[i].symbolId].result = updatedQueries[i].result;
			}
		}
	}

	void Start ()
	{
	
	}

	void Update ()
	{

	}
}
