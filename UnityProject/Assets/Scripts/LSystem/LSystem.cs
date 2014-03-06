using UnityEngine;
using System;
using System.Collections.Generic;

public class LSystem : MonoBehaviour
{
	// global variables ?
	[HideInInspector] public static float timeDelta = 0.1f;

	[SerializeField] public List<ISymbol> alphabet;
	[SerializeField] public ISymbol       axiom;
	[SerializeField] public Rules         rules;

	public List<ISymbol> state = new List<ISymbol>();
	
	private GameObject communicationQueryObject = null;

	private SortedDictionary<int, CommunicationSymbol> activeSymbols = new SortedDictionary<int, CommunicationSymbol> ();
	
	Material diffWhite;
	Material diffTransBlue;

	GameObject communications;

	int monomerCounting = 0;
	public int monomerCountingStop = 100;

	public string[] examples = {"none", "PARP", "Star", "Differ", "Cellulose", "Tubulin" };
	public int exampleIndex = 1;

	void Awake()
	{
		if (!communicationQueryObject)
		{
			communicationQueryObject = GameObject.Find("Communication Manager");
			if(!communicationQueryObject)
			{
				communicationQueryObject = new GameObject("Communication Manager");
				communicationQueryObject.AddComponent<CommunicationManager>();
			}
		}

		if (rules == null)
		{
			rules = ScriptableObject.CreateInstance<Rules>();
			rules.init();
		}

		if (exampleIndex == 0 && axiom != null && axiom.GetType () == typeof(CommunicationSymbol))
		{
			state.Add(axiom);
			activeSymbols.Add(0, (CommunicationSymbol)axiom);
		}

		// fuj
		RenderSettings.haloStrength = 1.0f;
		communications = new GameObject("Communications");

		// fuj
		diffWhite     = (Material)Resources.Load("Materials/diffWhite", typeof(Material));
		diffTransBlue = (Material)Resources.Load("Materials/diffTransBlue", typeof(Material));

		if (exampleIndex == 1)
		{
			CommunicationSymbol ax = ScriptableObject.CreateInstance<CommunicationSymbol>();
			ax.init("C", "G", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "adpRibose", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));

			axiom = ax;
			state.Add (axiom);
			activeSymbols.Add (0, (CommunicationSymbol)axiom);

			setTestRules ();
		}
		else if (exampleIndex == 2)
		{
			CommunicationSymbol ax = ScriptableObject.CreateInstance<CommunicationSymbol>();
			ax.init("C", "F", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "adpRibose", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
			axiom = ax;

			state.Add (axiom);
			activeSymbols.Add (0, (CommunicationSymbol)axiom);

			setTestRules2();
		}
		else if (exampleIndex == 3)
		{
			CommunicationSymbol ax = ScriptableObject.CreateInstance<CommunicationSymbol>();
			ax.init("C", "M", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "adpRibose", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
			axiom = ax;

			state.Add (axiom);
			activeSymbols.Add (0, (CommunicationSymbol)axiom);
			
			setTestRules3();
		}
		else if (exampleIndex == 4)
		{
			CommunicationSymbol ax = ScriptableObject.CreateInstance<CommunicationSymbol>();
			ax.init("C", "G", new Vector3(0, 0.8550f, 0), Quaternion.Euler(new Vector3(0, 180.0f, 0)), 0.0f, "b-D-glucose", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
			axiom = ax;

			state.Add (axiom);
			activeSymbols.Add (0, (CommunicationSymbol)axiom);
			
			setTestRules4();
		}
		else if (exampleIndex == 5)
		{
			CommunicationSymbol ax = ScriptableObject.CreateInstance<CommunicationSymbol>();
			ax.init("C", "G", new Vector3(-0.7555f, -0.8697f, -1.2740f), Quaternion.Euler(new Vector3(0, 27.6639f, 0)), 0.0f, "tubulin", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
			axiom = ax;
			
			state.Add (axiom);
			activeSymbols.Add (0, (CommunicationSymbol)axiom);
			
			setTestRules5();
		}
	}

	void testState()
	{
		StructureSymbol bDGlucose = ScriptableObject.CreateInstance<StructureSymbol> ();
		bDGlucose.init ("g", "b-D-glucose", null);

		BindingSymbol bDGlucoseBin = ScriptableObject.CreateInstance<BindingSymbol> ();
		bDGlucoseBin.init("b", new Vector3(0, 0.8550f, 0), new Vector3(0, 180f, 0), new Vector3(0, 10.0f, 10.0f), false);

		for (int i = 0; i < 100; i++)
		{
			StructureSymbol ss = ScriptableObject.CreateInstance<StructureSymbol> ();
			ss.init (bDGlucose);
			state.Add (ss);

			BindingSymbol bs = ScriptableObject.CreateInstance<BindingSymbol> ();
			bs.init (bDGlucoseBin);
			bs.alterOrientation ();
			state.Add (bs);
		}

		Interpret ();
	}

	void testState2()
	{
		StructureSymbol tubulin = ScriptableObject.CreateInstance<StructureSymbol> ();
		tubulin.init ("t", "tubulin", null);
		
		BindingSymbol tubulinBin = ScriptableObject.CreateInstance<BindingSymbol> ();
		tubulinBin.init("b", new Vector3(-0.7555f, 0.2f, -1.2740f), new Vector3(0, 27.6639f, 0), new Vector3(0, 0, 0), false);

		for (int i = 0; i < 100; i++)
		{
			StructureSymbol ss = ScriptableObject.CreateInstance<StructureSymbol> ();
			ss.init (tubulin);
			state.Add (ss);

			BindingSymbol bs = ScriptableObject.CreateInstance<BindingSymbol> ();
			bs.init (tubulinBin);
			bs.alterOrientation ();
			state.Add (bs);
		}
		
		Interpret ();
	}

	void testState3()
	{
		StructureSymbol alpha_tubulin = ScriptableObject.CreateInstance<StructureSymbol> ();
		alpha_tubulin.init ("a", "alpha-tubulin", null);

		StructureSymbol beta_tubulin = ScriptableObject.CreateInstance<StructureSymbol> ();
		beta_tubulin.init ("b", "beta-tubulin", null);

		BindingSymbol abBin = ScriptableObject.CreateInstance<BindingSymbol> ();
		abBin.init("d", new Vector3(0, 1.0697f, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), false);

		BindingSymbol tubulinBin = ScriptableObject.CreateInstance<BindingSymbol> ();
		tubulinBin.init("t", new Vector3(-0.7555f, -0.8697f, -1.2740f), new Vector3(0, 27.6639f, 0), new Vector3(0, 0, 0), false);
		
		for (int i = 0; i < 100; i++)
		{
			StructureSymbol ss = ScriptableObject.CreateInstance<StructureSymbol> ();
			ss.init (alpha_tubulin);
			state.Add (ss);
			
			BindingSymbol bs = ScriptableObject.CreateInstance<BindingSymbol> ();
			bs.init (abBin);
			state.Add (bs);

			ss = ScriptableObject.CreateInstance<StructureSymbol> ();
			ss.init (beta_tubulin);
			state.Add (ss);
			
			bs = ScriptableObject.CreateInstance<BindingSymbol> ();
			bs.init (tubulinBin);
			state.Add (bs);
		}
		
		Interpret ();
	}

	void setTestRules()
	{
		///////////////////////////////////////////////////////////////////////

		CommunicationSymbol R1P = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		R1P.init ("C", "G");

		BindingSymbol       R1S1 = ScriptableObject.CreateInstance<BindingSymbol> ();
		StructureSymbol     R1S2 = ScriptableObject.CreateInstance<StructureSymbol> ();
		CommunicationSymbol R1S3 = ScriptableObject.CreateInstance<CommunicationSymbol> ();

		R1S1.init("g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0), Vector3.zero, false);
		R1S2.init("m", "adpRibose", null);
		R1S3.init("C", "G", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "adpRibose", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));

		List<ISymbol> R1S = new List<ISymbol> ();
		R1S.Add (R1S1);
		R1S.Add (R1S2);
		R1S.Add (R1S3);

		CommunicationCondition R1C = ScriptableObject.CreateInstance<CommunicationCondition> ();
		R1C.init(CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		Rule R1 = ScriptableObject.CreateInstance<Rule> ();
		R1.init(R1P, R1S, R1C, 0.97f);

		///////////////////////////////////////////////////////////////////////

		CommunicationSymbol R2P = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		R2P.init("C", "G");

		BindingSymbol       R2S1 = ScriptableObject.CreateInstance<BindingSymbol> ();
		StructureSymbol     R2S2 = ScriptableObject.CreateInstance<StructureSymbol> ();
		CommunicationSymbol R2S3 = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		CommunicationSymbol R2S4 = ScriptableObject.CreateInstance<CommunicationSymbol> ();

		R2S1.init("g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0), Vector3.zero, false);
		R2S2.init("m", "adpRibose", null);
		R2S3.init("C", "B", new Vector3(1.3871f, 0.7653f, 0.0029f), Quaternion.Euler(new Vector3(0, 0, 298.3136f)), 0.0f, "adpRibose", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		R2S4.init("C", "G", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "adpRibose", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));

		List<ISymbol> R2S = new List<ISymbol> ();
		R2S.Add (R2S1);
		R2S.Add (R2S2);
		R2S.Add (R2S3);
		R2S.Add (R2S4);

		CommunicationCondition R2C = ScriptableObject.CreateInstance<CommunicationCondition> ();
		R2C.init(CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		Rule R2 = ScriptableObject.CreateInstance<Rule> ();
		R2.init(R2P, R2S, R2C, 0.03f);

		///////////////////////////////////////////////////////////////////////

		CommunicationSymbol R3P = ScriptableObject.CreateInstance<CommunicationSymbol>();
		R3P.init("C", "B");
		
		BindingSymbol       R3S1 = ScriptableObject.CreateInstance<BindingSymbol> ();
		StructureSymbol     R3S2 = ScriptableObject.CreateInstance<StructureSymbol> ();
		CommunicationSymbol R3S3 = ScriptableObject.CreateInstance<CommunicationSymbol> ();

		R3S1.init("b", new Vector3(1.3871f, 0.7653f, 0.0029f), new Vector3(0, 0, 298.3136f), Vector3.zero, true);
		R3S2.init("m", "adpRibose", null);
		R3S3.init("C", "G", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "adpRibose", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));

		List<ISymbol> R3S = new List<ISymbol> ();
		R3S.Add (R3S1);
		R3S.Add (R3S2);
		R3S.Add (R3S3);
		
		CommunicationCondition R3C = ScriptableObject.CreateInstance<CommunicationCondition> ();
		R3C.init(CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		Rule R3 = ScriptableObject.CreateInstance<Rule> ();
		R3.init(R3P, R3S, R3C, 1.0f);

		///////////////////////////////////////////////////////////////////////

		CommunicationSymbol R4P = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		R4P.init("C", "G");
		
		EndSymbol R4S1 = ScriptableObject.CreateInstance<EndSymbol> ();
		R4S1.init("e");
		
		List<ISymbol> R4S = new List<ISymbol> ();
		R4S.Add (R4S1);
		
		CommunicationCondition R4C = ScriptableObject.CreateInstance<CommunicationCondition> ();
		R4C.init(CommunicationCondition.CommParameters.time, CommunicationCondition.CommOperation.more, 5.0f);
		
		Rule R4 = ScriptableObject.CreateInstance<Rule> ();
		R4.init(R4P, R4S, R4C, 1.0f);

		///////////////////////////////////////////////////////////////////////

		CommunicationSymbol R5P = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		R5P.init("C", "B");
		
		List<ISymbol> R5S = new List<ISymbol> ();
		
		CommunicationCondition R5C = ScriptableObject.CreateInstance<CommunicationCondition> ();
		R5C.init(CommunicationCondition.CommParameters.time, CommunicationCondition.CommOperation.more, 5.0f);
		
		Rule R5 = ScriptableObject.CreateInstance<Rule> ();
		R5.init(R5P, R5S, R5C, 1.0f);

		/////////////////////////////////////////////////////////////
		
		rules.Add (R1);
		rules.Add (R2);
		rules.Add (R3);
		rules.Add (R4);
		rules.Add (R5);
	}

	void setTestRules2()
	{
		///////////////////////////////////////////////////////////////////////
		
		CommunicationSymbol R1P = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		R1P.init("C", "F");
		
		BindingSymbol       R1S1 = ScriptableObject.CreateInstance<BindingSymbol> ();
		StructureSymbol     R1S2 = ScriptableObject.CreateInstance<StructureSymbol> (); 
		CommunicationSymbol R1S3 = ScriptableObject.CreateInstance<CommunicationSymbol> ();

		R1S1.init("g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0), Vector3.zero, false);
		R1S2.init("m", "adpRibose", null);
		R1S3.init("C", "F", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "adpRibose", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));

		List<ISymbol> R1S = new List<ISymbol> ();
		R1S.Add (R1S1);
		R1S.Add (R1S2);
		R1S.Add (R1S3);
		
		CommunicationCondition R1C = new CommunicationCondition (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		Rule R1 = new Rule (R1P, R1S, R1C, 0.9f);
		
		///////////////////////////////////////////////////////////////////////

		CommunicationSymbol R2P = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		R2P.init ("C", "F");
		
		BindingSymbol       R2S1 = ScriptableObject.CreateInstance<BindingSymbol> ();
		StructureSymbol     R2S2 = ScriptableObject.CreateInstance<StructureSymbol> ();
		CommunicationSymbol R2S3 = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		CommunicationSymbol R2S4 = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		CommunicationSymbol R2S5 = ScriptableObject.CreateInstance<CommunicationSymbol> ();

		R2S1.init("g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0), Vector3.zero, false);
		R2S2.init("m", "adpRibose", null);
		R2S3.init("C", "A", new Vector3(0.67309f, 0.52365f, 0.83809f), Quaternion.Euler(new Vector3(-45.8616f, -50.1757f, -86.2943f)), 0.0f, "adpRibose", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		R2S4.init("C", "B", new Vector3(-0.94228f, 0.3053f, 0.9473f), Quaternion.Euler(new Vector3(48.033f, -112.99f, -89.888f)), 0.0f, "adpRibose", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		R2S5.init("C", "C", new Vector3(-0.2270f, 0.6361f, -0.9190f), Quaternion.Euler(new Vector3(-72.7191f, 34.9005f, -37.05637f)), 0.0f, "adpRibose", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));

		List<ISymbol> R2S = new List<ISymbol> ();
		R2S.Add (R2S1);
		R2S.Add (R2S2);
		R2S.Add (R2S3);
		R2S.Add (R2S4);
		R2S.Add (R2S5);
		
		CommunicationCondition R2C = new CommunicationCondition (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		Rule R2 = new Rule (R2P, R2S, R2C, 0.1f);
		
		///////////////////////////////////////////////////////////////////////

		CommunicationSymbol R3P = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		R3P.init ("C", "A");
		
		BindingSymbol       R3S1 = ScriptableObject.CreateInstance<BindingSymbol> ();
		StructureSymbol     R3S2 = ScriptableObject.CreateInstance<StructureSymbol> ();
		CommunicationSymbol R3S3 = ScriptableObject.CreateInstance<CommunicationSymbol> ();

		R3S1.init("a", new Vector3(0.67309f, 0.52365f, 0.83809f), new Vector3(-45.8616f, -50.1757f, -86.2943f), Vector3.zero, true);
		R3S2.init("m", "adpRibose", null);
		R3S3.init("C", "G", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "adpRibose", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));

		List<ISymbol> R3S = new List<ISymbol> ();
		R3S.Add (R3S1);
		R3S.Add (R3S2);
		R3S.Add (R3S3);
		
		CommunicationCondition R3C = new CommunicationCondition (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		Rule R3 = new Rule (R3P, R3S, R3C, 1.0f);
		
		///////////////////////////////////////////////////////////////////////
       
		CommunicationSymbol R4P = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		R4P.init("C", "B");
       
		BindingSymbol       R4S1 = ScriptableObject.CreateInstance<BindingSymbol> ();
		StructureSymbol     R4S2 = ScriptableObject.CreateInstance<StructureSymbol> ();
		CommunicationSymbol R4S3 = ScriptableObject.CreateInstance<CommunicationSymbol> ();

		R4S1.init("b", new Vector3(-0.94228f, 0.3053f, 0.9473f), new Vector3(48.033f, -112.99f, -89.888f), Vector3.zero, true);
		R4S2.init("m", "adpRibose", null);
		R4S3.init("C", "G", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "adpRibose", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));

		List<ISymbol> R4S = new List<ISymbol> ();
		R4S.Add (R4S1);
		R4S.Add (R4S2);
		R4S.Add (R4S3);
       
		CommunicationCondition R4C = new CommunicationCondition (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
       
		Rule R4 = new Rule (R4P, R4S, R4C, 1.0f);

		///////////////////////////////////////////////////////////////////////
		
		CommunicationSymbol R5P = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		R5P.init ("C", "C");
		
		BindingSymbol       R5S1 = ScriptableObject.CreateInstance<BindingSymbol> ();
		StructureSymbol     R5S2 = ScriptableObject.CreateInstance<StructureSymbol> ();
		CommunicationSymbol R5S3 = ScriptableObject.CreateInstance<CommunicationSymbol> ();

		R5S1.init("c", new Vector3(-0.2270f, 0.6361f, -0.9190f), new Vector3(-72.7191f, 34.9005f, -37.05637f), Vector3.zero, true);
		R5S2.init("m", "adpRibose", null);
		R5S3.init("C", "G", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "adpRibose", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));

		List<ISymbol> R5S = new List<ISymbol> ();
		R5S.Add (R5S1);
		R5S.Add (R5S2);
		R5S.Add (R5S3);
		
		CommunicationCondition R5C = new CommunicationCondition (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		Rule R5 = new Rule (R5P, R5S, R5C, 1.0f);

		///////////////////////////////////////////////////////////////////////

		CommunicationSymbol R6P = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		R6P.init("C", "G");
		
		BindingSymbol       R6S1 = ScriptableObject.CreateInstance<BindingSymbol> ();
		StructureSymbol     R6S2 = ScriptableObject.CreateInstance<StructureSymbol> ();
		CommunicationSymbol R6S3 = ScriptableObject.CreateInstance<CommunicationSymbol> ();

		R6S1.init("g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0), Vector3.zero, false);
		R6S2.init("m", "adpRibose", null);
		R6S3.init("C", "G", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "adpRibose", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));

		List<ISymbol> R6S = new List<ISymbol> ();
		R6S.Add (R6S1);
		R6S.Add (R6S2);
		R6S.Add (R6S3);
		
		CommunicationCondition R6C = new CommunicationCondition (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		Rule R6 = new Rule (R6P, R6S, R6C, 1.0f);

		rules.Add (R1);
		rules.Add (R2);
		rules.Add (R3);
		rules.Add (R4);
		rules.Add (R5);
		rules.Add (R6);
	}

	void setTestRules3()
	{
		///////////////////////////////////////////////////////////////////////
		
		CommunicationSymbol R1P = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		R1P.init("C", "M");
		
		BindingSymbol       R1S1 = ScriptableObject.CreateInstance<BindingSymbol> ();
		StructureSymbol     R1S2 = ScriptableObject.CreateInstance<StructureSymbol> ();
		CommunicationSymbol R1S3 = ScriptableObject.CreateInstance<CommunicationSymbol> ();

		R1S1.init("g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0), Vector3.zero, false);
		R1S2.init("m", "adpRibose", null);
		R1S3.init("C", "M", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "adpRibose", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));

		List<ISymbol> R1S = new List<ISymbol> ();
		R1S.Add (R1S1);
		R1S.Add (R1S2);
		R1S.Add (R1S3);
		
		CommunicationCondition R1C = new CommunicationCondition (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		Rule R1 = new Rule (R1P, R1S, R1C, 0.9f);
		
		///////////////////////////////////////////////////////////////////////
		
		CommunicationSymbol R2P = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		R2P.init("C", "M");
		
		BindingSymbol       R2S1 = ScriptableObject.CreateInstance<BindingSymbol> ();
		StructureSymbol     R2S2 = ScriptableObject.CreateInstance<StructureSymbol> ();
		CommunicationSymbol R2S3 = ScriptableObject.CreateInstance<CommunicationSymbol> ();

		R2S1.init("g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0), Vector3.zero, false);
		R2S2.init("m", "adpRibose", null);
		R2S3.init("C", "N", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "molecule", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		
		List<ISymbol> R2S = new List<ISymbol> ();
		R2S.Add (R2S1);
		R2S.Add (R2S2);
		R2S.Add (R2S3);
		
		CommunicationCondition R2C = new CommunicationCondition (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		Rule R2 = new Rule (R2P, R2S, R2C, 0.1f);
		
		///////////////////////////////////////////////////////////////////////
		
		CommunicationSymbol R3P = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		R3P.init("C", "N");
		
		BindingSymbol       R3S1 = ScriptableObject.CreateInstance<BindingSymbol> ();
		StructureSymbol     R3S2 = ScriptableObject.CreateInstance<StructureSymbol> ();
		CommunicationSymbol R3S3 = ScriptableObject.CreateInstance<CommunicationSymbol> ();

		R3S1.init("a", new Vector3(0.67309f, 0.52365f, 0.83809f), new Vector3(-45.8616f, -50.1757f, -86.2943f), Vector3.zero, true);
		R3S2.init("n", "molecule", null);
		R3S3.init("C", "M", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "adpRibose", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));

		List<ISymbol> R3S = new List<ISymbol> ();
		R3S.Add (R3S1);
		R3S.Add (R3S2);
		R3S.Add (R3S3);
		
		CommunicationCondition R3C = new CommunicationCondition (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		Rule R3 = new Rule (R3P, R3S, R3C, 1.0f);

		rules.Add (R1);
		rules.Add (R2);
		rules.Add (R3);
	}

	void setTestRules4()
	{
		CommunicationSymbol CG = ScriptableObject.CreateInstance<CommunicationSymbol>();
		CG.init("C", "G", new Vector3(0, 0.8550f, 0), Quaternion.Euler(new Vector3(0, 180.0f, 0)), 0.0f, "b-D-glucose", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));

		StructureSymbol bDGlucose = ScriptableObject.CreateInstance<StructureSymbol> ();
		bDGlucose.init ("g", "b-D-glucose", null);
		
		BindingSymbol bDGlucoseBin = ScriptableObject.CreateInstance<BindingSymbol> ();
		bDGlucoseBin.init("b", new Vector3(0, 0.8550f, 0), new Vector3(0, 180f, 0), new Vector3(0, 10.0f, 10.0f), false);

		CommunicationSymbol    RP  = null;
		ISymbol                RSS = null;
		List<ISymbol>          RS  = new List<ISymbol> ();
		CommunicationCondition RC  = null;
		Rule                   R   = null;
		///////////////////////////////////////////////////////////////////////
		
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
	}

	void setTestRules5()
	{
		StructureSymbol alpha_tubulin = ScriptableObject.CreateInstance<StructureSymbol> ();
		alpha_tubulin.init ("a", "alpha-tubulin", null);
		
		StructureSymbol beta_tubulin = ScriptableObject.CreateInstance<StructureSymbol> ();
		beta_tubulin.init ("b", "beta-tubulin", null);
		
		BindingSymbol abBin = ScriptableObject.CreateInstance<BindingSymbol> ();
		abBin.init("d", new Vector3(0, 1.0697f, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), false);
		
		BindingSymbol tubulinBin = ScriptableObject.CreateInstance<BindingSymbol> ();
		tubulinBin.init("t", new Vector3(-0.7555f, -0.8697f, -1.2740f), new Vector3(0, 27.6639f, 0), new Vector3(0, 0, 0), false);

		CommunicationSymbol process = ScriptableObject.CreateInstance<CommunicationSymbol>();
		process.init("C", "G", new Vector3(-0.7555f, -0.8697f, -1.2740f), Quaternion.Euler(new Vector3(0, 27.6639f, 0)), 0.0f, "tubulin", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));

		CommunicationSymbol    RP  = null;
		ISymbol                RSS = null;
		List<ISymbol>          RS  = new List<ISymbol> ();
		CommunicationCondition RC  = null;
		Rule                   R   = null;
		///////////////////////////////////////////////////////////////////////
		
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
	}

	GameObject addObject(ref Turtle turtle, string prefabName)
	{
		GameObject prefab = Resources.Load(prefabName) as GameObject;
		GameObject mol = Instantiate(prefab, turtle.position, turtle.direction) as GameObject;

		//fuj
		if (exampleIndex == 3 && prefabName == "molecule")
		{
			mol.renderer.material = diffTransBlue;
		}
		else
		{
			if(prefabName == "adpRibose")
			{
				mol.renderer.material = diffWhite;
			}
		}

		// remove agents components
		if (mol.GetComponent<RandomMove> ())
			Destroy (mol.GetComponent<RandomMove> ());

		if (mol.GetComponent<RandomRotate> ())
			Destroy (mol.GetComponent<RandomRotate> ());

		if (mol.GetComponent<GlobalAttraction> ())
			Destroy (mol.GetComponent<GlobalAttraction> ());

		if (mol.GetComponent<GlobalBindingQuery> ())
			Destroy (mol.GetComponent<GlobalBindingQuery> ());

		if (mol.GetComponent<BoundaryBounce> ())
			Destroy (mol.GetComponent<BoundaryBounce> ());

		if (mol.GetComponent<TimeScaleTransparency> ())
			Destroy (mol.GetComponent<TimeScaleTransparency> ());

		// will be removed somehow
		mol.rigidbody.isKinematic = true;

		mol.transform.parent = transform;
		mol.transform.localScale = NewAgentSystem.agentScale * mol.transform.localScale;

		monomerCounting++;

		return mol;
	}

	void Derive()
	{
		CommunicationManager cql = communicationQueryObject.GetComponent<CommunicationManager> ();

		SortedDictionary<int, CommunicationSymbol> newActiveSymbols = new SortedDictionary<int, CommunicationSymbol> ();

		int indexOffset = 0;
		foreach (KeyValuePair<int, CommunicationSymbol> activeSymbol in activeSymbols)
		{
			CommunicationSymbol symbol = (CommunicationSymbol)state[activeSymbol.Key + indexOffset];

			cql.Remove(activeSymbol.Key);

			Rule rule = rules.Get (symbol);
			if(rule != null)
			{
				DestroyImmediate(symbol.result);
				state.RemoveAt(activeSymbol.Key + indexOffset);

				List<ISymbol> newSymbols = rule.successor;

				for(int i = 0; i < newSymbols.Count; i++)
				{
					int newIndex = activeSymbol.Key + indexOffset + i;

					ISymbol newSymbol = null;

					if(newSymbols[i].GetType() == typeof(CommunicationSymbol))
					{
						newSymbol = ScriptableObject.CreateInstance<CommunicationSymbol>();
						((CommunicationSymbol)newSymbol).init((CommunicationSymbol)newSymbols[i]);

						newActiveSymbols.Add(newIndex, symbol);
					}
					else if(newSymbols[i].GetType() == typeof(BindingSymbol))
					{
						newSymbol = ScriptableObject.CreateInstance<BindingSymbol> ();
						((BindingSymbol)newSymbol).init((BindingSymbol)newSymbols[i]);
						((BindingSymbol)newSymbol).alterOrientation();
					}
					else if(newSymbols[i].GetType() == typeof(StructureSymbol))
					{
						newSymbol = ScriptableObject.CreateInstance<StructureSymbol> ();
						((StructureSymbol)newSymbol).init((StructureSymbol)newSymbols[i]);
					}
					else if(newSymbols[i].GetType() == typeof(EndSymbol))
					{
						newSymbol = ScriptableObject.CreateInstance<EndSymbol> ();
						((EndSymbol)newSymbol).init((EndSymbol)newSymbols[i]);
					}
					
					state.Insert( newIndex, newSymbol );
				}

				indexOffset += newSymbols.Count - 1;
			}
			else
			{
				newActiveSymbols.Add(activeSymbol.Key + indexOffset, symbol);
			}
		}

		activeSymbols = newActiveSymbols;
	}

	private void Interpret ()
	{
		Turtle current = new Turtle (Quaternion.identity, Vector3.zero);
		Stack<Turtle> stack = new Stack<Turtle> ();
		stack.Push(current);
		
		for (int i = 0; i < state.Count; i++)
		{
			ISymbol symbol = state[i];
			
			if(symbol.GetType() == typeof(StructureSymbol))
			{
				if(((StructureSymbol)symbol).structureObject == null)
				{
					((StructureSymbol)state[i]).structureObject = addObject(ref current, ((StructureSymbol)symbol).structurePrefabName);
				}
			}
			else if(symbol.GetType() == typeof(BindingSymbol))
			{
				if(((BindingSymbol)symbol).isBranching)
				{
					stack.Push(current);
					current = new Turtle (current);
				}
				
				Vector3 bindingOrientation = ((BindingSymbol)symbol).bindingOrientation;

				current.position  = current.position + current.direction * (NewAgentSystem.agentScale * ((BindingSymbol)symbol).bindingPosition);
				current.direction = current.direction * Quaternion.Euler (bindingOrientation.x, bindingOrientation.y, bindingOrientation.z);
			}
			else if(symbol.GetType() == typeof(EndSymbol))
			{
				current = stack.Pop ();
			}
			else if(symbol.GetType() == typeof(CommunicationSymbol))
			{
				((CommunicationSymbol)symbol).fillTurtleValues(current);

				// fuj
				if(((CommunicationSymbol)symbol).process == "G")
				{
					current = stack.Pop();
				}
			}
		}
	}

	private void preEnviromentStep()
	{
		List<CommunicationQuery> queries = communicationQueryObject.GetComponent<CommunicationManager> ().getQueries ();
		
		for(int i = 0; i < queries.Count; i++)
		{
			if(activeSymbols.ContainsKey(queries[i].stateId))
			{
				activeSymbols[queries[i].stateId].timer  = queries[i].time;

				if(monomerCounting < monomerCountingStop)
					activeSymbols[queries[i].stateId].result = queries[i].result;
			}
		}
	}

	void postEnviromentStep()
	{
		int childCount = communications.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			DestroyImmediate(communications.transform.GetChild(0).gameObject);
		}

		CommunicationManager cql = communicationQueryObject.GetComponent<CommunicationManager> ();

		foreach(KeyValuePair<int, CommunicationSymbol> symbol in activeSymbols)
		{
			cql.Add(symbol.Key, symbol.Value);

			GameObject haloEffect = new GameObject();
			haloEffect.transform.position = symbol.Value.globalPosition;
			haloEffect.AddComponent("Halo");
			haloEffect.transform.parent = communications.transform;
		}
	}

	private void TimeStep()
	{

		preEnviromentStep ();

		Derive ();
		Interpret ();

		postEnviromentStep ();

		//debugState ();
	}
	
	void Update()
	{
		TimeStep ();
	}

	public void debugAxioms()
	{
		string output = "";
		for (int i = 0; i < alphabet.Count; i++)
		{
			if(alphabet[i].GetType() == typeof(StructureSymbol))
			{
				output += "StructureSymbol(" + alphabet[i].name + ")";
			}
			else if(alphabet[i].GetType() == typeof(BindingSymbol))
			{
				output += "BindingSymbol(" + alphabet[i].name + ")";
			}
			else if(alphabet[i].GetType() == typeof(CommunicationSymbol))
			{
				output += "Communication(" + ((CommunicationSymbol)alphabet[i]).process + ")";
			}
			else if(alphabet[i].GetType() == typeof(ISymbol))
			{
				output += "ISymbol(" + alphabet[i].name + ")";
			}
		}
		
		print (output);
	}

	void debugState()
	{
		string output = "";
		for (int i = 0; i < state.Count; i++)
		{
			output += state[i].name;

			if(state[i].GetType() == typeof(CommunicationSymbol))
			{
				output += "(" + ((CommunicationSymbol)state[i]).process + ")";
			}
		}

		print (output);
	}

	public string[] alphabetArray()
	{
		List<string> lSystemAlphabet = new List<string> ();

		foreach(ISymbol symbol in alphabet)
		{
			string symbolStr = "";
			if(symbol.GetType() == typeof(ISymbol))
			{
				symbolStr += "ISymbol(" + symbol.name + ")";
			}
			else if(symbol.GetType() == typeof(EndSymbol))
			{
				symbolStr += "EndSymbol(" + symbol.name + ")";
			}
			else if(symbol.GetType() == typeof(StructureSymbol))
			{
				symbolStr += "StructureSymbol(" + symbol.name + ")";
			}
			else if(symbol.GetType() == typeof(BindingSymbol))
			{
				symbolStr += "BindingSymbol(" + symbol.name + ")";
			}
			else if(symbol.GetType() == typeof(CommunicationSymbol))
			{
				symbolStr += "CommunicationSymbol(" + symbol.name + ", " + ((CommunicationSymbol)symbol).process + ")";
			}
			lSystemAlphabet.Add(symbolStr);
		}

		return lSystemAlphabet.ToArray ();
	}
}