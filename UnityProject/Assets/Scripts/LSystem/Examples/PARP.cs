using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PARP
{
	public static List<ISymbol> alphabet;
	public static ISymbol       axiom;
	public static Rules         rules;
	
	static PARP()
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
		
		BindingSymbol binB = ScriptableObject.CreateInstance<BindingSymbol> ();
		binB.init("b", new Vector3(1.3871f, 0.7653f, 0.0029f), new Vector3(0, 0, 298.3136f), Vector3.zero, true);
		alphabet.Add (binB);
		
		CommunicationSymbol mainGrow = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		mainGrow.init("C", "G", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "adpRibose", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		alphabet.Add (mainGrow);
		
		CommunicationSymbol mainBranch = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		mainBranch.init("C", "B", new Vector3(1.3871f, 0.7653f, 0.0029f), Quaternion.Euler(new Vector3(0, 0, 298.3136f)), 0.0f, "adpRibose", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		alphabet.Add (mainBranch);
		
		CommunicationSymbol branchGrow = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		branchGrow.init("C", "BG", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "adpRibose", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		alphabet.Add (mainGrow);

		// rules
		CommunicationSymbol    RP  = null;
		ISymbol                RSS = null;
		List<ISymbol>          RS  = new List<ISymbol> ();
		CommunicationCondition RC  = null;
		Rule                   R   = null;
		
		// C(G,r) : r != 0 -> gAC(G,0) : 95% 
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
		
		// C(G,r) : r != 0 -> gAC(B,0)C(G,0) : 5% 
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
		((CommunicationSymbol)RSS).init (mainBranch);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		((CommunicationSymbol)RSS).init (mainGrow);
		RS.Add (RSS);
		
		RC = ScriptableObject.CreateInstance<CommunicationCondition> ();
		RC.init (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		R = ScriptableObject.CreateInstance<Rule> ();
		R.init (RP, RS, RC, 0.05f);
		rules.Add (R);
		
		// C(B,r) : r != 0 -> bAC(BG, 0) : 100%
		RP = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		RP.init (mainBranch);
		
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
		
		// C(BG, r) : r != 0 -> gAC(BG, 0) : 100%
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
