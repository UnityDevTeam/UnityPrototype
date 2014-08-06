using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Copolymer
{
	public static List<ISymbol> alphabet;
	public static ISymbol       axiom;
	public static Rules         rules;
	
	static Copolymer()
	{
		// initializing stuff
		alphabet = new List<ISymbol> ();
		rules = ScriptableObject.CreateInstance<Rules>();
		rules.init();

		//alphabet
		StructureSymbol adpRibose = ScriptableObject.CreateInstance<StructureSymbol> ();
		adpRibose.init("m", "adpRibose", null);
		alphabet.Add (adpRibose);

		StructureSymbol molecule = ScriptableObject.CreateInstance<StructureSymbol> ();
		molecule.init("n", "molecule", null);
		alphabet.Add (molecule);

		BindingSymbol binG = ScriptableObject.CreateInstance<BindingSymbol> ();
		binG.init("g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0), Vector3.zero, false);
		alphabet.Add (binG);

		BindingSymbol binA = ScriptableObject.CreateInstance<BindingSymbol> ();
		binA.init("a", new Vector3(0.67309f, 0.52365f, 0.83809f), new Vector3(-45.8616f, -50.1757f, -86.2943f), Vector3.zero, true);
		alphabet.Add (binA);

		CommunicationSymbol growM = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		growM.init("C", "M", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "adpRibose", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		alphabet.Add (growM);

		CommunicationSymbol growN = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		growN.init("C", "N", new Vector3(0.67309f, 0.52365f, 0.83809f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "molecule", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		alphabet.Add (growN);

		// Rules
		CommunicationSymbol    RP  = null;
		ISymbol                RSS = null;
		List<ISymbol>          RS  = new List<ISymbol> ();
		CommunicationCondition RC  = null;
		Rule                   R   = null;

		// C(M, r) : r !=0 -> gmC(M, 0) : 90%
		RP = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		RP.init (growM);
		
		RS = new List<ISymbol> ();
		
		RSS = ScriptableObject.CreateInstance<BindingSymbol> ();
		((BindingSymbol)RSS).init (binG);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<StructureSymbol> ();
		((StructureSymbol)RSS).init (adpRibose);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		((CommunicationSymbol)RSS).init (growM);
		RS.Add (RSS);
		
		RC = ScriptableObject.CreateInstance<CommunicationCondition> ();
		RC.init (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		R = ScriptableObject.CreateInstance<Rule> ();
		R.init (RP, RS, RC, 0.9f);
		rules.Add (R);
		
		// C(M, r) : r != 0 -> gmC(N, 0) : 10%
		RP = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		RP.init (growM);
		
		RS = new List<ISymbol> ();
		
		RSS = ScriptableObject.CreateInstance<BindingSymbol> ();
		((BindingSymbol)RSS).init (binG);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<StructureSymbol> ();
		((StructureSymbol)RSS).init (adpRibose);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		((CommunicationSymbol)RSS).init (growN);
		RS.Add (RSS);
		
		RC = ScriptableObject.CreateInstance<CommunicationCondition> ();
		RC.init (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		R = ScriptableObject.CreateInstance<Rule> ();
		R.init (RP, RS, RC, 0.1f);
		rules.Add (R);
		
		// C(N, r) : r != 0 -> anC(M, 0) : 100%
		RP = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		RP.init (growN);
		
		RS = new List<ISymbol> ();
		
		RSS = ScriptableObject.CreateInstance<BindingSymbol> ();
		((BindingSymbol)RSS).init (binA);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<StructureSymbol> ();
		((StructureSymbol)RSS).init (molecule);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		((CommunicationSymbol)RSS).init (growM);
		RS.Add (RSS);
		
		RC = ScriptableObject.CreateInstance<CommunicationCondition> ();
		RC.init (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		R = ScriptableObject.CreateInstance<Rule> ();
		R.init (RP, RS, RC, 1.0f);
		rules.Add (R);

		// axiom
		axiom = growM;
	}
}
