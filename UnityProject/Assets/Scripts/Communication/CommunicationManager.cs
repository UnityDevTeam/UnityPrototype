using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommunicationManager : MonoBehaviour
{
	public Dictionary<int, CommunicationQuery> queries = new Dictionary<int, CommunicationQuery>();

	private float timer = 0.0f;

	public List<CommunicationQuery> getQueries()
	{
		return new List<CommunicationQuery> (queries.Values);
	}

	public void Add(int stateId, CommunicationSymbol symbol)
	{
		CommunicationQuery query = new CommunicationQuery(symbol.id, stateId, symbol.globalPosition, symbol.globalOrientation, symbol.operationResultType, symbol.operationTimer);

		if (queries.ContainsKey (query.stateId))
		{
			queries [query.symbolId] = query;
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
}
