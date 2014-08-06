using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cellulose
{
	public static List<ISymbol> alphabet;
	public static ISymbol       axiom;
	public static Rules         rules;
	
	static Cellulose()
	{
		// initializing stuff
		alphabet = new List<ISymbol> ();
		rules = ScriptableObject.CreateInstance<Rules>();
		rules.init();
	
		// alphabet
		StructureSymbol bDGlucose = ScriptableObject.CreateInstance<StructureSymbol> ();
		bDGlucose.init ("g", "b-D-glucose", null);
		alphabet.Add (bDGlucose);
		
		BindingSymbol bDGlucoseBin = ScriptableObject.CreateInstance<BindingSymbol> ();
		bDGlucoseBin.init("b", new Vector3(0.0f, 0.8550f, 0), new Vector3(0, 180f, 0), new Vector3(0, 10.0f, 10.0f), false);
		alphabet.Add (bDGlucoseBin);

		CommunicationSymbol CG = ScriptableObject.CreateInstance<CommunicationSymbol>();
		CG.init("C", "G", new Vector3(0, 0.8550f, 0), Quaternion.Euler(new Vector3(0, 180.0f, 0)), 0.0f, "glucose", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		alphabet.Add (CG);

		// Rules
		CommunicationSymbol    RP  = null;
		ISymbol                RSS = null;
		List<ISymbol>          RS  = new List<ISymbol> ();
		CommunicationCondition RC  = null;
		Rule                   R   = null;

		// C(G, r) : r != 0 -> bgC(G, 0) : 100%
		RP = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		RP.init(CG);
		
		RSS = ScriptableObject.CreateInstance<BindingSymbol> ();
		((BindingSymbol)RSS).init (bDGlucoseBin);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<StructureSymbol> ();
		((StructureSymbol)RSS).init (bDGlucose);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		((CommunicationSymbol)RSS).init (CG);
		RS.Add (RSS);
		
		RC = ScriptableObject.CreateInstance<CommunicationCondition> ();
		RC.init(CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		R = ScriptableObject.CreateInstance<Rule> ();
		R.init(RP, RS, RC, 1.0f);
		
		rules.Add (R);

		// Axiom
		axiom = CG;
	}
}
