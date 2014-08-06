using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tubulin
{
	public static List<ISymbol> alphabet;
	public static ISymbol       axiom;
	public static Rules         rules;
	
	static Tubulin()
	{
		// initializing stuff
		alphabet = new List<ISymbol> ();
		rules = ScriptableObject.CreateInstance<Rules>();
		rules.init();
		
		// alphabet
		StructureSymbol alpha_tubulin = ScriptableObject.CreateInstance<StructureSymbol> ();
		alpha_tubulin.init ("a", "alpha-tubulin", null);
		alphabet.Add (alpha_tubulin);
		
		StructureSymbol beta_tubulin = ScriptableObject.CreateInstance<StructureSymbol> ();
		beta_tubulin.init ("b", "beta-tubulin", null);
		alphabet.Add (beta_tubulin);
		
		BindingSymbol abBin = ScriptableObject.CreateInstance<BindingSymbol> ();
		abBin.init("d", new Vector3(0, 1.0697f, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), false);
		alphabet.Add (abBin);
		
		BindingSymbol tubulinBin = ScriptableObject.CreateInstance<BindingSymbol> ();
		tubulinBin.init("t", new Vector3(-0.7555f, -0.8697f, -1.2740f), new Vector3(0, 27.6639f, 0), new Vector3(0, 5.0f, 0), false);
		alphabet.Add (tubulinBin);
		
		CommunicationSymbol process = ScriptableObject.CreateInstance<CommunicationSymbol>();
		process.init("C", "G", new Vector3(-0.7555f, -0.8697f, -1.2740f), Quaternion.Euler(new Vector3(0, 27.6639f, 0)), 0.0f, "tubulin", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		alphabet.Add (process);

		// Rules
		CommunicationSymbol    RP  = null;
		ISymbol                RSS = null;
		List<ISymbol>          RS  = new List<ISymbol> ();
		CommunicationCondition RC  = null;
		Rule                   R   = null;

		// C(G, r) : r != 0 -> tbdaC(G, 0) : 100%
		RP = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		RP.init(process);
		
		RSS = ScriptableObject.CreateInstance<BindingSymbol> ();
		((BindingSymbol)RSS).init (tubulinBin);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<StructureSymbol> ();
		((StructureSymbol)RSS).init (beta_tubulin);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<BindingSymbol> ();
		((BindingSymbol)RSS).init (abBin);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<StructureSymbol> ();
		((StructureSymbol)RSS).init (alpha_tubulin);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		((CommunicationSymbol)RSS).init (process);
		RS.Add (RSS);
		
		RC = ScriptableObject.CreateInstance<CommunicationCondition> ();
		RC.init(CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		R = ScriptableObject.CreateInstance<Rule> ();
		R.init(RP, RS, RC, 1.0f);
		
		rules.Add (R);

		// axiom
		axiom = process;
	}
}
