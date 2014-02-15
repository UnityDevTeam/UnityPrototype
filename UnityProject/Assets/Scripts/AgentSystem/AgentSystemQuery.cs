using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentSystemQuery
{
	public struct CommunicationQueryPair
	{
		public CommunicationQuery query;
		public bool changed;
	}

	public List<CommunicationQueryPair> queries;

	public AgentSystemQuery()
	{
		queries = new List<CommunicationQueryPair> ();
	}
}
