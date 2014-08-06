using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Star
{
	public static List<ISymbol> alphabet;
	public static ISymbol       axiom;
	public static Rules         rules;

	static Star()
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
		binG.init("g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0), Vector3.zero, false);
		alphabet.Add (binG);

		BindingSymbol binA = ScriptableObject.CreateInstance<BindingSymbol> ();
		binA.init("a", new Vector3(0.67309f, 0.52365f, 0.83809f), new Vector3(-45.8616f, -50.1757f, -86.2943f), Vector3.zero, true);
		alphabet.Add (binA);

		BindingSymbol binB = ScriptableObject.CreateInstance<BindingSymbol> ();
		binB.init("b", new Vector3(-0.94228f, 0.3053f, 0.9473f), new Vector3(48.033f, -112.99f, -89.888f), Vector3.zero, true);
		alphabet.Add (binB);

		BindingSymbol binC = ScriptableObject.CreateInstance<BindingSymbol> ();
		binC.init("c", new Vector3(-0.2270f, 0.6361f, -0.9190f), new Vector3(-72.7191f, 34.9005f, -37.05637f), Vector3.zero, true);
		alphabet.Add (binC);

		CommunicationSymbol mainGrow = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		mainGrow.init("C", "F", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "adpRibose", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		alphabet.Add (mainGrow);

		CommunicationSymbol branchA = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		branchA.init("C", "A", new Vector3(0.67309f, 0.52365f, 0.83809f), Quaternion.Euler(new Vector3(-45.8616f, -50.1757f, -86.2943f)), 0.0f, "adpRibose", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		alphabet.Add (branchA);

		CommunicationSymbol branchB = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		branchB.init("C", "B", new Vector3(-0.94228f, 0.3053f, 0.9473f), Quaternion.Euler(new Vector3(48.033f, -112.99f, -89.888f)), 0.0f, "adpRibose", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		alphabet.Add (branchB);

		CommunicationSymbol branchC = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		branchC.init("C", "C", new Vector3(-0.2270f, 0.6361f, -0.9190f), Quaternion.Euler(new Vector3(-72.7191f, 34.9005f, -37.05637f)), 0.0f, "adpRibose", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		alphabet.Add (branchC);

		CommunicationSymbol branchGrow = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		branchGrow.init("C", "G", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "adpRibose", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		alphabet.Add (branchGrow);

		// Rules
		CommunicationSymbol    RP  = null;
		ISymbol                RSS = null;
		List<ISymbol>          RS  = new List<ISymbol> ();
		CommunicationCondition RC  = null;
		Rule                   R   = null;

		// C(F, r) : r != 0 -> gmC(F) : 90%
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
		RC.init (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		R = ScriptableObject.CreateInstance<Rule> ();
		R.init (RP, RS, RC, 0.95f);
		rules.Add (R);
		
		// C(F, r) : r != 0 -> gAC(A, 0)C(B, 0)C(C, 0) : 10%
		
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
		((CommunicationSymbol)RSS).init (branchA);
		RS.Add (RSS);

		RSS = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		((CommunicationSymbol)RSS).init (branchB);
		RS.Add (RSS);

		RSS = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		((CommunicationSymbol)RSS).init (branchC);
		RS.Add (RSS);
		
		RC = ScriptableObject.CreateInstance<CommunicationCondition> ();
		RC.init (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		R = ScriptableObject.CreateInstance<Rule> ();
		R.init (RP, RS, RC, 0.1f);
		rules.Add (R);
		
		// C(A, r) : r != 0 -> aAC(G, 0) : 100%
		RP = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		RP.init (branchA);

		RS = new List<ISymbol> ();
		
		RSS = ScriptableObject.CreateInstance<BindingSymbol> ();
		((BindingSymbol)RSS).init (binA);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<StructureSymbol> ();
		((StructureSymbol)RSS).init (adpRibose);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		((CommunicationSymbol)RSS).init (branchGrow);
		RS.Add (RSS);
		
		RC = ScriptableObject.CreateInstance<CommunicationCondition> ();
		RC.init (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		R = ScriptableObject.CreateInstance<Rule> ();
		R.init (RP, RS, RC, 1.0f);
		rules.Add (R);
		
		// C(B, r) : r != 0 -> bAC(G, 0) : 100%
		RP = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		RP.init (branchB);
		
		RS = new List<ISymbol> ();
		
		RSS = ScriptableObject.CreateInstance<BindingSymbol> ();
		((BindingSymbol)RSS).init (binB);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<StructureSymbol> ();
		((StructureSymbol)RSS).init (adpRibose);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		((CommunicationSymbol)RSS).init (branchGrow);
		RS.Add (RSS);
		
		RC = ScriptableObject.CreateInstance<CommunicationCondition> ();
		RC.init (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		R = ScriptableObject.CreateInstance<Rule> ();
		R.init (RP, RS, RC, 1.0f);
		rules.Add (R);
		
		// C(C, r) : r != 0 -> cAC(G, 0) : 100%
		RP = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		RP.init (branchC);
		
		RS = new List<ISymbol> ();
		
		RSS = ScriptableObject.CreateInstance<BindingSymbol> ();
		((BindingSymbol)RSS).init (binC);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<StructureSymbol> ();
		((StructureSymbol)RSS).init (adpRibose);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		((CommunicationSymbol)RSS).init (branchGrow);
		RS.Add (RSS);
		
		RC = ScriptableObject.CreateInstance<CommunicationCondition> ();
		RC.init (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		R = ScriptableObject.CreateInstance<Rule> ();
		R.init (RP, RS, RC, 1.0f);
		rules.Add (R);
		
		// C(G, r) : r != 0 -> gAC(G, 0) : 100%
		RP = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		RP.init (branchGrow);
		
		RS = new List<ISymbol> ();
		
		RSS = ScriptableObject.CreateInstance<BindingSymbol> ();
		((BindingSymbol)RSS).init (binG);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<StructureSymbol> ();
		((StructureSymbol)RSS).init (adpRibose);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		((CommunicationSymbol)RSS).init (branchGrow);
		RS.Add (RSS);
		
		RC = ScriptableObject.CreateInstance<CommunicationCondition> ();
		RC.init (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		R = ScriptableObject.CreateInstance<Rule> ();
		R.init (RP, RS, RC, 1.0f);
		rules.Add (R);

		// axiom
		axiom = mainGrow;
	}
}
