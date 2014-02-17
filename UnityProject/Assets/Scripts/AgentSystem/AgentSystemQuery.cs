using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct CommunicationQueryPair
{
	public CommunicationQuery query;
	public bool changed;
	
	public CommunicationQueryPair(CommunicationQuery nQuery)
	{
		query   = nQuery;
		changed = false;
	}

	public CommunicationQueryPair(CommunicationQuery nQuery, bool nChanged)
	{
		query   = nQuery;
		changed = nChanged;
	}
}

public class AgentSystemQuery
{
	public List<CommunicationQueryPair> queries;

	public AgentSystemQuery()
	{
		queries = new List<CommunicationQueryPair> ();
	}
}
