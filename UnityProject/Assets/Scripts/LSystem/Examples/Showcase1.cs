using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Showcase1
{
	public static List<ISymbol> alphabet;
	public static ISymbol       axiom;
	public static Rules         rules;
	
	static Showcase1()
	{
		// initializing stuff
		alphabet = new List<ISymbol> ();
		rules = ScriptableObject.CreateInstance<Rules>();
		rules.init();
	
		// alphabet
		StructureSymbol struc = ScriptableObject.CreateInstance<StructureSymbol> ();
		struc.init ("s", "Sphere", null);
		alphabet.Add (struc);
		
		StructureSymbol cube = ScriptableObject.CreateInstance<StructureSymbol> ();
		cube.init ("c", "Cube", null);
		alphabet.Add (cube);
		
		StructureSymbol cylinder = ScriptableObject.CreateInstance<StructureSymbol> ();
		cylinder.init ("d", "Cylinder", null);
		alphabet.Add (cylinder);

		BindingSymbol binG = ScriptableObject.CreateInstance<BindingSymbol> ();
		binG.init("g", new Vector3(-0.4008502f, 0.4984f, 0.8279926f), new Vector3(0, 298.2402f, 0), new Vector3(0, 0.0f, 0), false);
		alphabet.Add (binG);
		
		BindingSymbol binB = ScriptableObject.CreateInstance<BindingSymbol> ();
		binB.init("b", new Vector3(0.7569166f, 0.4194877f, 0.3090816f), new Vector3(0, 0, 288.3136f), Vector3.zero, true);
		alphabet.Add (binB);
		
		BindingSymbol star1 = ScriptableObject.CreateInstance<BindingSymbol> ();
		star1.init("s1", new Vector3(0.8802532f, 0.373954f, 0.469326f), new Vector3(0, -24.67035f, 15.86258f), new Vector3(0, 0.0f, 0), true);
		alphabet.Add (star1);
		
		BindingSymbol star2 = ScriptableObject.CreateInstance<BindingSymbol> ();
		star2.init("s2", new Vector3(-0.8616089f, 0.5282927f, 0.5845833f), new Vector3(-14.20465f, -62.14069f, 5.536514f), Vector3.zero, true);
		alphabet.Add (star2);
		
		BindingSymbol star3 = ScriptableObject.CreateInstance<BindingSymbol> ();
		star3.init("s3", new Vector3(-0.03932f, 0.51478f, -0.8856f), new Vector3(-30.1433f, -177.4573f, 0), Vector3.zero, true);
		alphabet.Add (star3);
		
		BindingSymbol cuB = ScriptableObject.CreateInstance<BindingSymbol> ();
		cuB.init("sb", new Vector3(0.99f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0f), Vector3.zero, false);
		alphabet.Add (cuB);
		
		EndSymbol end = ScriptableObject.CreateInstance<EndSymbol> ();
		end.init("e");
		alphabet.Add (end);
		
		CommunicationSymbol mainGrow = ScriptableObject.CreateInstance<CommunicationSymbol>();
		mainGrow.init("C", "G", new Vector3(-0.4008502f, 0.4984f, 0.8279926f), Quaternion.Euler(new Vector3(0, 298.2402f, 0)), 0.0f, "Sphere", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		alphabet.Add (mainGrow);
		
		CommunicationSymbol mainBranch = ScriptableObject.CreateInstance<CommunicationSymbol>();
		mainBranch.init("C", "B", new Vector3(0.7569166f, 0.4194877f, 0.3090816f), Quaternion.Euler(new Vector3(0, 0, 288.3136f)), 0.0f, "Sphere", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		alphabet.Add (mainBranch);
		
		CommunicationSymbol branchGrow = ScriptableObject.CreateInstance<CommunicationSymbol>();
		branchGrow.init("C", "BG", new Vector3(-0.4008502f, 0.4984f, 0.8279926f), Quaternion.Euler(new Vector3(0, 298.2402f, 0)), 0.0f, "Sphere", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		alphabet.Add (branchGrow);
		
		CommunicationSymbol star1Branch = ScriptableObject.CreateInstance<CommunicationSymbol>();
		star1Branch.init("C", "SB1", new Vector3(0.8802532f, 0.373954f, 0.469326f), Quaternion.Euler(new Vector3(0, -24.67035f, 15.86258f)), 0.0f, "Cube", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		alphabet.Add (star1Branch);
		
		CommunicationSymbol star2Branch = ScriptableObject.CreateInstance<CommunicationSymbol>();
		star2Branch.init("C", "SB2", new Vector3(-0.8616089f, 0.5282927f, 0.5845833f), Quaternion.Euler(new Vector3(-14.20465f, -62.14069f, 5.536514f)), 0.0f, "Cube", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		alphabet.Add (star2Branch);
		
		CommunicationSymbol star3Branch = ScriptableObject.CreateInstance<CommunicationSymbol>();
		star3Branch.init("C", "SB3", new Vector3(-0.03932f, 0.51478f, -0.8856f), Quaternion.Euler(new Vector3(-30.1433f, -177.4573f, 0)), 0.0f, "Cube", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		alphabet.Add (star3Branch);
		
		CommunicationSymbol star1Grow = ScriptableObject.CreateInstance<CommunicationSymbol>();
		star1Grow.init("C", "SG1", new Vector3(0.99f, 0.0f, 0.0f), Quaternion.Euler(new Vector3(0.0f, 0.0f, 0f)), 0.0f, "Cube", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		alphabet.Add (star1Grow);
		
		CommunicationSymbol star2Grow = ScriptableObject.CreateInstance<CommunicationSymbol>();
		star2Grow.init("C", "SG2", new Vector3(0.99f, 0.0f, 0.0f), Quaternion.Euler(new Vector3(0.0f, 0.0f, 0f)), 0.0f, "Cube", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		alphabet.Add (star2Grow);
		
		CommunicationSymbol star3Grow = ScriptableObject.CreateInstance<CommunicationSymbol>();
		star3Grow.init("C", "SG3", new Vector3(0.99f, 0.0f, 0.0f), Quaternion.Euler(new Vector3(0.0f, 0.0f, 0f)), 0.0f, "Cube", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		alphabet.Add (star3Grow);
		
		CommunicationSymbol growB = ScriptableObject.CreateInstance<CommunicationSymbol>();
		growB.init("C", "grB", new Vector3(0.99f, 0.0f, 0.0f), Quaternion.Euler(new Vector3(0.0f, 0.0f, 0f)), 0.0f, "Cylinder", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		alphabet.Add (growB);
		
		CommunicationSymbol growC = ScriptableObject.CreateInstance<CommunicationSymbol>();
		growC.init("C", "grC", new Vector3(0.99f, 0.0f, 0.0f), Quaternion.Euler(new Vector3(0.0f, 0.0f, 0f)), 0.0f, "Cube", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		alphabet.Add (growC);

		// rules
		CommunicationSymbol    RP  = null;
		ISymbol                RSS = null;
		List<ISymbol>          RS  = new List<ISymbol> ();
		CommunicationCondition RC  = null;
		Rule                   R   = null;

		// C(G, r) : r != 0 -> gsC(G, 0) : 80%
		RP = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		RP.init(mainGrow);
		
		RS = new List<ISymbol> ();
		
		RSS = ScriptableObject.CreateInstance<BindingSymbol> ();
		((BindingSymbol)RSS).init (binG);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<StructureSymbol> ();
		((StructureSymbol)RSS).init (struc);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		((CommunicationSymbol)RSS).init (mainGrow);
		RS.Add (RSS);
		
		RC = ScriptableObject.CreateInstance<CommunicationCondition> ();
		RC.init(CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		R = ScriptableObject.CreateInstance<Rule> ();
		R.init(RP, RS, RC, 0.8f);
		rules.Add (R);
		
		// C(G, r) : r != 0 -> gsC(B, 0)C(G, 0) 15%
		RP = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		RP.init (mainGrow);
		
		RS = new List<ISymbol> ();
		
		RSS = ScriptableObject.CreateInstance<BindingSymbol> ();
		((BindingSymbol)RSS).init (binG);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<StructureSymbol> ();
		((StructureSymbol)RSS).init (struc);
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
		R.init (RP, RS, RC, 0.15f);
		rules.Add (R);
		
		// C(B, r) : r != 0 -> bsC(BG, 0)e : 100%
		RP = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		RP.init (mainBranch);
		
		RS = new List<ISymbol> ();
		
		RSS = ScriptableObject.CreateInstance<BindingSymbol> ();
		((BindingSymbol)RSS).init (binB);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<StructureSymbol> ();
		((StructureSymbol)RSS).init (struc);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		((CommunicationSymbol)RSS).init (branchGrow);
		RS.Add (RSS);

		RSS = ScriptableObject.CreateInstance<EndSymbol> ();
		((EndSymbol)RSS).init (end);
		RS.Add (RSS);
		
		RC = ScriptableObject.CreateInstance<CommunicationCondition> ();
		RC.init (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		R = ScriptableObject.CreateInstance<Rule> ();
		R.init (RP, RS, RC, 1.0f);
		rules.Add (R);
		
		// C(BG, r) : r != 0 -> gsC(BG, 0) : 100%
		RP = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		RP.init (branchGrow);
		
		RS = new List<ISymbol> ();
		
		RSS = ScriptableObject.CreateInstance<BindingSymbol> ();
		((BindingSymbol)RSS).init (binG);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<StructureSymbol> ();
		((StructureSymbol)RSS).init (struc);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		((CommunicationSymbol)RSS).init (branchGrow);
		RS.Add (RSS);
		
		RC = ScriptableObject.CreateInstance<CommunicationCondition> ();
		RC.init (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		R = ScriptableObject.CreateInstance<Rule> ();
		R.init (RP, RS, RC, 1.0f);
		rules.Add (R);
		
		// C(G, r) : r != 0 -> gsC(SB1, 0)C(SB2, 0)C(SB3, 0) : 5 %
		RP = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		RP.init (mainGrow);
		
		RS = new List<ISymbol> ();
		
		RSS = ScriptableObject.CreateInstance<BindingSymbol> ();
		((BindingSymbol)RSS).init (binG);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<StructureSymbol> ();
		((StructureSymbol)RSS).init (struc);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		((CommunicationSymbol)RSS).init (star1Branch);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		((CommunicationSymbol)RSS).init (star2Branch);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		((CommunicationSymbol)RSS).init (star3Branch);
		RS.Add (RSS);
		
		RC = ScriptableObject.CreateInstance<CommunicationCondition> ();
		RC.init (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		R = ScriptableObject.CreateInstance<Rule> ();
		R.init (RP, RS, RC, 0.05f);
		rules.Add (R);
		
		// C(SB1, r) : r != 0 -> s1cC(SG1, 0)e : 100%
		RP = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		RP.init (star1Branch);
		
		RS = new List<ISymbol> ();
		
		RSS = ScriptableObject.CreateInstance<BindingSymbol> ();
		((BindingSymbol)RSS).init (star1);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<StructureSymbol> ();
		((StructureSymbol)RSS).init (cube);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		((CommunicationSymbol)RSS).init (star1Grow);
		RS.Add (RSS);

		RSS = ScriptableObject.CreateInstance<EndSymbol> ();
		((EndSymbol)RSS).init (end);
		RS.Add (RSS);
		
		RC = ScriptableObject.CreateInstance<CommunicationCondition> ();
		RC.init (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		R = ScriptableObject.CreateInstance<Rule> ();
		R.init (RP, RS, RC, 1.0f);
		rules.Add (R);
		
		// C(SG1, r) : r != 0 -> sbdC(grB, 0) : 100%
		RP = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		RP.init (star1Grow);
		
		RS = new List<ISymbol> ();
		
		RSS = ScriptableObject.CreateInstance<BindingSymbol> ();
		((BindingSymbol)RSS).init (cuB);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<StructureSymbol> ();
		((StructureSymbol)RSS).init (cylinder);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		((CommunicationSymbol)RSS).init (growB);
		RS.Add (RSS);
		
		RC = ScriptableObject.CreateInstance<CommunicationCondition> ();
		RC.init (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		R = ScriptableObject.CreateInstance<Rule> ();
		R.init (RP, RS, RC, 1.0f);
		rules.Add (R);
		
		// C(SB2, r) : r != 0 -> s2cC(SG2, 0)e : 100%
		RP = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		RP.init (star2Branch);
		
		RS = new List<ISymbol> ();
		
		RSS = ScriptableObject.CreateInstance<BindingSymbol> ();
		((BindingSymbol)RSS).init (star2);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<StructureSymbol> ();
		((StructureSymbol)RSS).init (cube);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		((CommunicationSymbol)RSS).init (star2Grow);
		RS.Add (RSS);

		RSS = ScriptableObject.CreateInstance<EndSymbol> ();
		((EndSymbol)RSS).init (end);
		RS.Add (RSS);
		
		RC = ScriptableObject.CreateInstance<CommunicationCondition> ();
		RC.init (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		R = ScriptableObject.CreateInstance<Rule> ();
		R.init (RP, RS, RC, 1.0f);
		rules.Add (R);
		
		// C(SG2, r) : r != 0 -> sbdC(grB, 0) : 100%
		RP = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		RP.init (star2Grow);
		
		RS = new List<ISymbol> ();
		
		RSS = ScriptableObject.CreateInstance<BindingSymbol> ();
		((BindingSymbol)RSS).init (cuB);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<StructureSymbol> ();
		((StructureSymbol)RSS).init (cylinder);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		((CommunicationSymbol)RSS).init (growB);
		RS.Add (RSS);
		
		RC = ScriptableObject.CreateInstance<CommunicationCondition> ();
		RC.init (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		R = ScriptableObject.CreateInstance<Rule> ();
		R.init (RP, RS, RC, 1.0f);
		rules.Add (R);
		
		// C(SB3, r) : r != 0 -> s3cC(SG3, 0)e : 100%
		
		RP = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		RP.init (star3Branch);
		
		RS = new List<ISymbol> ();
		
		RSS = ScriptableObject.CreateInstance<BindingSymbol> ();
		((BindingSymbol)RSS).init (star3);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<StructureSymbol> ();
		((StructureSymbol)RSS).init (cube);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		((CommunicationSymbol)RSS).init (star3Grow);
		RS.Add (RSS);

		RSS = ScriptableObject.CreateInstance<EndSymbol> ();
		((EndSymbol)RSS).init (end);
		RS.Add (RSS);
		
		RC = ScriptableObject.CreateInstance<CommunicationCondition> ();
		RC.init (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		R = ScriptableObject.CreateInstance<Rule> ();
		R.init (RP, RS, RC, 1.0f);
		rules.Add (R);
		
		// c(SG3, r) : r != 0 -> sbdC(grB, 0) : 100%
		
		RP = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		RP.init (star3Grow);
		
		RS = new List<ISymbol> ();
		
		RSS = ScriptableObject.CreateInstance<BindingSymbol> ();
		((BindingSymbol)RSS).init (cuB);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<StructureSymbol> ();
		((StructureSymbol)RSS).init (cylinder);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		((CommunicationSymbol)RSS).init (growB);
		RS.Add (RSS);
		
		RC = ScriptableObject.CreateInstance<CommunicationCondition> ();
		RC.init (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		R = ScriptableObject.CreateInstance<Rule> ();
		R.init (RP, RS, RC, 1.0f);
		rules.Add (R);
		
		// C(grB, r) : r != 0 -> sbdC(grC, 0) : 100%
		RP = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		RP.init (growB);
		
		RS = new List<ISymbol> ();
		
		RSS = ScriptableObject.CreateInstance<BindingSymbol> ();
		((BindingSymbol)RSS).init (cuB);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<StructureSymbol> ();
		((StructureSymbol)RSS).init (cylinder);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		((CommunicationSymbol)RSS).init (growC);
		RS.Add (RSS);
		
		RC = ScriptableObject.CreateInstance<CommunicationCondition> ();
		RC.init (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		R = ScriptableObject.CreateInstance<Rule> ();
		R.init (RP, RS, RC, 1.0f);
		rules.Add (R);
		
		// C(grC, r) : r != 0 -> sbcC(grB, 0) : 100%
		
		RP = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		RP.init (growC);
		
		RS = new List<ISymbol> ();
		
		RSS = ScriptableObject.CreateInstance<BindingSymbol> ();
		((BindingSymbol)RSS).init (cuB);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<StructureSymbol> ();
		((StructureSymbol)RSS).init (cube);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		((CommunicationSymbol)RSS).init (growB);
		RS.Add (RSS);
		
		RC = ScriptableObject.CreateInstance<CommunicationCondition> ();
		RC.init (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		R = ScriptableObject.CreateInstance<Rule> ();
		R.init (RP, RS, RC, 1.0f);
		rules.Add (R);

		//axiom
		axiom = mainGrow;
	}
}
