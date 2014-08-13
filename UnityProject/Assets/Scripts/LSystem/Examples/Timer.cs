using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Timer
{
	public static List<ISymbol> alphabet;
	public static ISymbol       axiom;
	public static Rules         rules;
	
	static Timer()
	{
		// initializing stuff
		alphabet = new List<ISymbol> ();
		rules = ScriptableObject.CreateInstance<Rules>();
		rules.init();
		
		// alphabet
		StructureSymbol adpRibose = ScriptableObject.CreateInstance<StructureSymbol> ();
		adpRibose.init("m", "adpRibose", null);
		alphabet.Add (adpRibose);
		
		BindingSymbol binG = ScriptableObject.CreateInstance<BindingSymbol> ();
		binG.init("g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0), new Vector3(0, 20.0f, 0), false);
		alphabet.Add (binG);

		CommunicationSymbol mainGrow = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		mainGrow.init("C", "G", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "NAD", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		alphabet.Add (mainGrow);

		// rules
		CommunicationSymbol    RP  = null;
		ISymbol                RSS = null;
		List<ISymbol>          RS  = new List<ISymbol> ();
		CommunicationCondition RC  = null;
		Rule                   R   = null;

		// C(G, t) : t > 3 -> gmC(G, 0) : 100%
		RP = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		RP.init (mainGrow);
		
		RS = new List<ISymbol> ();
		
		RSS = ScriptableObject.CreateInstance<BindingSymbol> ();
		((BindingSymbol)RSS).init (binG);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<StructureSymbol> ();
		((StructureSymbol)RSS).init (adpRibose);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		((CommunicationSymbol)RSS).init (mainGrow);
		RS.Add (RSS);
		
		RC = ScriptableObject.CreateInstance<CommunicationCondition> ();
		RC.init (CommunicationCondition.CommParameters.time, CommunicationCondition.CommOperation.more, 3.0f);
		
		R = ScriptableObject.CreateInstance<Rule> ();
		R.init (RP, RS, RC, 1.0f);
		rules.Add (R);

		// axiom
		axiom = mainGrow;
	}
}
