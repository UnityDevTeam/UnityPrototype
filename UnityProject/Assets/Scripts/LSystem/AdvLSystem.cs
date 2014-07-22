using UnityEngine;
using System;
using System.Collections.Generic;

public class AdvLSystem : MonoBehaviour
{
	[SerializeField] public List<ISymbol> alphabet;
	[SerializeField] public ISymbol       axiom;
	[SerializeField] public Rules         rules;
	public List<ISymbol> state = new List<ISymbol>();
	
	public SortedDictionary<int, CommunicationSymbol> activeSymbols = new SortedDictionary<int, CommunicationSymbol> ();
	
	[HideInInspector] public static float timeDelta = 0.1f;
	
	private GameObject communicationQueryObject = null;
	
	int monomerCounting = 0;
	public int monomerCountingStop = 100;
	
	Material basicWhite;
	Material diffTransBlue;
	
	Material diffTransLightBlue;
	Material diffTransDarkBlue;
	
	public string[] examples = {"none", "PARP", "Star", "Differ", "Cellulose", "Tubulin", "Showcase1", "Showcase2" };
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
		basicWhite     = (Material)Resources.Load("Materials/basicWhite", typeof(Material));
		diffTransBlue = (Material)Resources.Load("Materials/diffTransBlue", typeof(Material));
		
		diffTransLightBlue = (Material)Resources.Load("Materials/basicLightBlue", typeof(Material));
		diffTransDarkBlue  = (Material)Resources.Load("Materials/basicDarkBlue", typeof(Material));;
		
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
		
		else if (exampleIndex == 6) // Showcase 1
		{
			CommunicationSymbol ax = ScriptableObject.CreateInstance<CommunicationSymbol>();
			ax.init("C", "G", new Vector3(-0.4008502f, 0.4984f, 0.8279926f), Quaternion.Euler(new Vector3(0, 298.2402f, 0)), 0.0f, "Sphere", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
			
			axiom = ax;
			
			state.Add (axiom);
			activeSymbols.Add (0, (CommunicationSymbol)axiom);
			
			setTestRules6();
		}
		
		else if (exampleIndex == 7) // Showcase 2
		{
			CommunicationSymbol ax = ScriptableObject.CreateInstance<CommunicationSymbol>();
			ax.init("C", "GC", new Vector3(0.0f, 0.8f, 0.0f), Quaternion.Euler(new Vector3(0, 0, 0)), 0.0f, "Cube", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
			axiom = ax;
			
			state.Add (axiom);
			activeSymbols.Add (0, (CommunicationSymbol)axiom);
			
			setTestRules7();
		}
		
		
		//testState1 ();
		//testState2 ();
		//testState3 ();
		//testState4 ();
		//testState5 ();
	}
	
	void testState1 ()
	{
		StructureSymbol bDGlucose = ScriptableObject.CreateInstance<StructureSymbol> ();
		bDGlucose.init ("g", "b-D-glucose", null);
		
		BindingSymbol bDGlucoseBin = ScriptableObject.CreateInstance<BindingSymbol> ();
		bDGlucoseBin.init("b", new Vector3(0.0f, 0.8550f, 0), new Vector3(0, 180f, 0), new Vector3(0, 10.0f, 10.0f), false);
		
		for (int i = 0; i < 15; i++)
		{
			StructureSymbol structure = ScriptableObject.CreateInstance<StructureSymbol> ();
			structure.init (bDGlucose);
			state.Add(structure);
			
			BindingSymbol binding = ScriptableObject.CreateInstance<BindingSymbol> ();
			binding.init(bDGlucoseBin);
			state.Add(binding);
		}
		
		Interpret ();
		debugState ();
	}
	
	void testState2 ()
	{
		StructureSymbol bDGlucose = ScriptableObject.CreateInstance<StructureSymbol> ();
		bDGlucose.init ("m", "b-D-glucose", null);
		
		BindingSymbol bDGlucoseBin = ScriptableObject.CreateInstance<BindingSymbol> ();
		bDGlucoseBin.init("g", new Vector3(0.0f, 0.8550f, 0), new Vector3(0, 180f, 0), new Vector3(0, 0.0f, 0.0f), false);
		
		BindingSymbol bDGlucoseBra = ScriptableObject.CreateInstance<BindingSymbol> ();
		bDGlucoseBra.init("b", new Vector3(0.850f, 0.0f, 0), new Vector3(0, 0, -45f), new Vector3(0, 0.0f, 0.0f), true);
		
		EndSymbol end = ScriptableObject.CreateInstance<EndSymbol> ();
		end.init("e");
		
		StructureSymbol str = null;
		BindingSymbol bin = null;
		EndSymbol ee = null;
		
		for (int i = 0; i < 2; i++)
		{
			str = ScriptableObject.CreateInstance<StructureSymbol> ();
			str.init (bDGlucose);
			state.Add(str);
			
			bin = ScriptableObject.CreateInstance<BindingSymbol> ();
			bin.init(bDGlucoseBin);
			state.Add(bin);
		}
		
		str = ScriptableObject.CreateInstance<StructureSymbol> ();
		str.init (bDGlucose);
		state.Add(str);
		
		bin = ScriptableObject.CreateInstance<BindingSymbol> ();
		bin.init(bDGlucoseBra);
		state.Add(bin);
		
		for (int i = 0; i < 7; i++)
		{
			str = ScriptableObject.CreateInstance<StructureSymbol> ();
			str.init (bDGlucose);
			state.Add(str);
			
			bin = ScriptableObject.CreateInstance<BindingSymbol> ();
			bin.init(bDGlucoseBin);
			state.Add(bin);
		}
		
		str = ScriptableObject.CreateInstance<StructureSymbol> ();
		str.init (bDGlucose);
		state.Add(str);
		
		ee = ScriptableObject.CreateInstance<EndSymbol> ();
		ee.init (end);
		state.Add (ee);
		
		bin = ScriptableObject.CreateInstance<BindingSymbol> ();
		bin.init(bDGlucoseBin);
		state.Add(bin);
		
		for (int i = 0; i < 2; i++)
		{
			str = ScriptableObject.CreateInstance<StructureSymbol> ();
			str.init (bDGlucose);
			state.Add(str);
			
			bin = ScriptableObject.CreateInstance<BindingSymbol> ();
			bin.init(bDGlucoseBin);
			state.Add(bin);
		}
		
		str = ScriptableObject.CreateInstance<StructureSymbol> ();
		str.init (bDGlucose);
		state.Add(str);
		
		bin = ScriptableObject.CreateInstance<BindingSymbol> ();
		bin.init(bDGlucoseBra);
		state.Add(bin);
		
		for (int i = 0; i < 5; i++)
		{
			str = ScriptableObject.CreateInstance<StructureSymbol> ();
			str.init (bDGlucose);
			state.Add(str);
			
			bin = ScriptableObject.CreateInstance<BindingSymbol> ();
			bin.init(bDGlucoseBin);
			state.Add(bin);
		}
		
		str = ScriptableObject.CreateInstance<StructureSymbol> ();
		str.init (bDGlucose);
		state.Add(str);
		
		ee = ScriptableObject.CreateInstance<EndSymbol> ();
		ee.init (end);
		state.Add (ee);
		
		bin = ScriptableObject.CreateInstance<BindingSymbol> ();
		bin.init(bDGlucoseBin);
		state.Add(bin);
		
		for (int i = 0; i < 2; i++)
		{
			str = ScriptableObject.CreateInstance<StructureSymbol> ();
			str.init (bDGlucose);
			state.Add(str);
			
			bin = ScriptableObject.CreateInstance<BindingSymbol> ();
			bin.init(bDGlucoseBin);
			state.Add(bin);
		}
		
		str = ScriptableObject.CreateInstance<StructureSymbol> ();
		str.init (bDGlucose);
		state.Add(str);
		
		bin = ScriptableObject.CreateInstance<BindingSymbol> ();
		bin.init(bDGlucoseBra);
		state.Add(bin);
		
		for (int i = 0; i < 3; i++)
		{
			str = ScriptableObject.CreateInstance<StructureSymbol> ();
			str.init (bDGlucose);
			state.Add(str);
			
			bin = ScriptableObject.CreateInstance<BindingSymbol> ();
			bin.init(bDGlucoseBin);
			state.Add(bin);
		}
		
		str = ScriptableObject.CreateInstance<StructureSymbol> ();
		str.init (bDGlucose);
		state.Add(str);
		
		ee = ScriptableObject.CreateInstance<EndSymbol> ();
		ee.init (end);
		state.Add (ee);
		
		bin = ScriptableObject.CreateInstance<BindingSymbol> ();
		bin.init(bDGlucoseBin);
		state.Add(bin);
		
		for (int i = 0; i < 4; i++)
		{
			str = ScriptableObject.CreateInstance<StructureSymbol> ();
			str.init (bDGlucose);
			state.Add(str);
			
			bin = ScriptableObject.CreateInstance<BindingSymbol> ();
			bin.init(bDGlucoseBin);
			state.Add(bin);
		}
		
		str = ScriptableObject.CreateInstance<StructureSymbol> ();
		str.init (bDGlucose);
		state.Add(str);
		
		Interpret ();
		debugState ();
		
	}
	
	void testState3 ()
	{
		StructureSymbol struc = ScriptableObject.CreateInstance<StructureSymbol> ();
		struc.init ("m", "adpRibose", null);
		
		BindingSymbol binG = ScriptableObject.CreateInstance<BindingSymbol> ();
		binG.init("g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0), new Vector3(0, 0.0f, 0), false);
		
		EndSymbol end = ScriptableObject.CreateInstance<EndSymbol> ();
		end.init("e");
		
		StructureSymbol str = null;
		BindingSymbol bin = null;
		//EndSymbol ee = null;
		
		for (int i = 0; i < 34; i++)
		{
			str = ScriptableObject.CreateInstance<StructureSymbol> ();
			str.init (struc);
			state.Add(str);
			
			bin = ScriptableObject.CreateInstance<BindingSymbol> ();
			bin.init(binG);
			state.Add(bin);
		}
		
		str = ScriptableObject.CreateInstance<StructureSymbol> ();
		str.init (struc);
		state.Add(str);
		
		Interpret ();
		debugState ();
	}
	
	void testState4 ()
	{
		StructureSymbol struc = ScriptableObject.CreateInstance<StructureSymbol> ();
		struc.init ("m", "adpRibose", null);
		
		BindingSymbol binG = ScriptableObject.CreateInstance<BindingSymbol> ();
		binG.init("g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0), new Vector3(0, 20.0f, 0), false);
		
		BindingSymbol binB = ScriptableObject.CreateInstance<BindingSymbol> ();
		binB.init("b", new Vector3(1.3871f, 0.7653f, 0.0029f), new Vector3(0, 0, 288.3136f), Vector3.zero, true);
		
		EndSymbol end = ScriptableObject.CreateInstance<EndSymbol> ();
		end.init("e");
		
		StructureSymbol str = null;
		BindingSymbol bin = null;
		EndSymbol ee = null;
		
		for (int i = 0; i < 6; i++)
		{
			str = ScriptableObject.CreateInstance<StructureSymbol> ();
			str.init (struc);
			state.Add(str);
			
			bin = ScriptableObject.CreateInstance<BindingSymbol> ();
			bin.init(binG);
			state.Add(bin);
		}
		
		str = ScriptableObject.CreateInstance<StructureSymbol> ();
		str.init (struc);
		state.Add(str);
		
		bin = ScriptableObject.CreateInstance<BindingSymbol> ();
		bin.init(binB);
		state.Add(bin);
		
		for (int i = 0; i < 15; i++)
		{
			str = ScriptableObject.CreateInstance<StructureSymbol> ();
			str.init (struc);
			state.Add(str);
			
			bin = ScriptableObject.CreateInstance<BindingSymbol> ();
			bin.init(binG);
			state.Add(bin);
		}
		
		str = ScriptableObject.CreateInstance<StructureSymbol> ();
		str.init (struc);
		state.Add(str);
		
		ee = ScriptableObject.CreateInstance<EndSymbol> ();
		ee.init (end);
		state.Add (ee);
		
		bin = ScriptableObject.CreateInstance<BindingSymbol> ();
		bin.init(binG);
		state.Add(bin);
		
		for (int i = 0; i < 8; i++)
		{
			str = ScriptableObject.CreateInstance<StructureSymbol> ();
			str.init (struc);
			state.Add(str);
			
			bin = ScriptableObject.CreateInstance<BindingSymbol> ();
			bin.init(binG);
			state.Add(bin);
		}
		
		str = ScriptableObject.CreateInstance<StructureSymbol> ();
		str.init (struc);
		state.Add(str);
		
		bin = ScriptableObject.CreateInstance<BindingSymbol> ();
		bin.init(binB);
		state.Add(bin);
		
		for (int i = 0; i < 10; i++)
		{
			str = ScriptableObject.CreateInstance<StructureSymbol> ();
			str.init (struc);
			state.Add(str);
			
			bin = ScriptableObject.CreateInstance<BindingSymbol> ();
			bin.init(binG);
			state.Add(bin);
		}
		
		str = ScriptableObject.CreateInstance<StructureSymbol> ();
		str.init (struc);
		state.Add(str);
		
		ee = ScriptableObject.CreateInstance<EndSymbol> ();
		ee.init (end);
		state.Add (ee);
		
		bin = ScriptableObject.CreateInstance<BindingSymbol> ();
		bin.init(binG);
		state.Add(bin);
		
		for (int i = 0; i < 18; i++)
		{
			str = ScriptableObject.CreateInstance<StructureSymbol> ();
			str.init (struc);
			state.Add(str);
			
			bin = ScriptableObject.CreateInstance<BindingSymbol> ();
			bin.init(binG);
			state.Add(bin);
		}
		
		str = ScriptableObject.CreateInstance<StructureSymbol> ();
		str.init (struc);
		state.Add(str);
		
		Interpret ();
		debugState ();
	}
	
	void testState5 ()
	{
		StructureSymbol struc = ScriptableObject.CreateInstance<StructureSymbol> ();
		struc.init ("s", "Sphere", null);
		
		StructureSymbol cube = ScriptableObject.CreateInstance<StructureSymbol> ();
		cube.init ("c", "Cube", null);
		
		StructureSymbol cylinder = ScriptableObject.CreateInstance<StructureSymbol> ();
		cylinder.init ("d", "Cylinder", null);
		
		BindingSymbol binG = ScriptableObject.CreateInstance<BindingSymbol> ();
		binG.init("g", new Vector3(-0.4008502f, 0.4984f, 0.8279926f), new Vector3(0, 298.2402f, 0), new Vector3(0, 0.0f, 0), false);
		
		BindingSymbol binB = ScriptableObject.CreateInstance<BindingSymbol> ();
		binB.init("b", new Vector3(0.7569166f, 0.4194877f, 0.3090816f), new Vector3(0, 0, 288.3136f), Vector3.zero, true);
		
		BindingSymbol star1 = ScriptableObject.CreateInstance<BindingSymbol> ();
		star1.init("s1", new Vector3(0.8802532f, 0.373954f, 0.469326f), new Vector3(0, -24.67035f, 15.86258f), new Vector3(0, 0.0f, 0), true);
		
		BindingSymbol star2 = ScriptableObject.CreateInstance<BindingSymbol> ();
		star2.init("s2", new Vector3(-0.8616089f, 0.5282927f, 0.5845833f), new Vector3(-14.20465f, -62.14069f, 5.536514f), Vector3.zero, true);
		
		BindingSymbol star3 = ScriptableObject.CreateInstance<BindingSymbol> ();
		star3.init("s3", new Vector3(-0.03932f, 0.51478f, -0.8856f), new Vector3(-30.1433f, -177.4573f, 0), Vector3.zero, true);
		
		BindingSymbol cuB = ScriptableObject.CreateInstance<BindingSymbol> ();
		cuB.init("sb", new Vector3(0.99f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0f), Vector3.zero, false);
		
		
		EndSymbol end = ScriptableObject.CreateInstance<EndSymbol> ();
		end.init("e");
		
		StructureSymbol str = null;
		BindingSymbol bin = null;
		EndSymbol ee = null;
		
		for (int i = 0; i < 6; i++)
		{
			str = ScriptableObject.CreateInstance<StructureSymbol> ();
			str.init (struc);
			state.Add(str);
			
			bin = ScriptableObject.CreateInstance<BindingSymbol> ();
			bin.init(binG);
			state.Add(bin);
		}
		
		str = ScriptableObject.CreateInstance<StructureSymbol> ();
		str.init (struc);
		state.Add(str);
		
		bin = ScriptableObject.CreateInstance<BindingSymbol> ();
		bin.init(binB);
		state.Add(bin);
		
		for (int i = 0; i < 15; i++)
		{
			str = ScriptableObject.CreateInstance<StructureSymbol> ();
			str.init (struc);
			state.Add(str);
			
			bin = ScriptableObject.CreateInstance<BindingSymbol> ();
			bin.init(binG);
			state.Add(bin);
		}
		
		str = ScriptableObject.CreateInstance<StructureSymbol> ();
		str.init (struc);
		state.Add(str);
		
		ee = ScriptableObject.CreateInstance<EndSymbol> ();
		ee.init (end);
		state.Add (ee);
		
		bin = ScriptableObject.CreateInstance<BindingSymbol> ();
		bin.init(binG);
		state.Add(bin);
		
		for (int i = 0; i < 8; i++)
		{
			str = ScriptableObject.CreateInstance<StructureSymbol> ();
			str.init (struc);
			state.Add(str);
			
			bin = ScriptableObject.CreateInstance<BindingSymbol> ();
			bin.init(binG);
			state.Add(bin);
		}
		
		str = ScriptableObject.CreateInstance<StructureSymbol> ();
		str.init (struc);
		state.Add(str);
		
		bin = ScriptableObject.CreateInstance<BindingSymbol> ();
		bin.init(binB);
		state.Add(bin);
		
		for (int i = 0; i < 10; i++)
		{
			str = ScriptableObject.CreateInstance<StructureSymbol> ();
			str.init (struc);
			state.Add(str);
			
			bin = ScriptableObject.CreateInstance<BindingSymbol> ();
			bin.init(binG);
			state.Add(bin);
		}
		
		str = ScriptableObject.CreateInstance<StructureSymbol> ();
		str.init (struc);
		state.Add(str);
		
		ee = ScriptableObject.CreateInstance<EndSymbol> ();
		ee.init (end);
		state.Add (ee);
		
		bin = ScriptableObject.CreateInstance<BindingSymbol> ();
		bin.init(binG);
		state.Add(bin);
		
		for (int i = 0; i < 18; i++)
		{
			str = ScriptableObject.CreateInstance<StructureSymbol> ();
			str.init (struc);
			state.Add(str);
			
			bin = ScriptableObject.CreateInstance<BindingSymbol> ();
			bin.init(binG);
			state.Add(bin);
		}
		
		str = ScriptableObject.CreateInstance<StructureSymbol> ();
		str.init (struc);
		state.Add(str);
		
		bin = ScriptableObject.CreateInstance<BindingSymbol> ();
		bin.init(star1);
		state.Add(bin);
		
		for (int i = 0; i < 18; i++)
		{
			str = ScriptableObject.CreateInstance<StructureSymbol> ();
			if(i%2 == 0)
				str.init (cube);
			else
				str.init(cylinder);
			state.Add(str);
			
			bin = ScriptableObject.CreateInstance<BindingSymbol> ();
			bin.init(cuB);
			state.Add(bin);
		}
		
		str = ScriptableObject.CreateInstance<StructureSymbol> ();
		str.init (cube);
		state.Add(str);
		
		ee = ScriptableObject.CreateInstance<EndSymbol> ();
		ee.init (end);
		state.Add (ee);
		
		bin = ScriptableObject.CreateInstance<BindingSymbol> ();
		bin.init(star2);
		state.Add(bin);
		
		for (int i = 0; i < 18; i++)
		{
			str = ScriptableObject.CreateInstance<StructureSymbol> ();
			if(i%2 == 0)
				str.init (cube);
			else
				str.init(cylinder);
			state.Add(str);
			
			bin = ScriptableObject.CreateInstance<BindingSymbol> ();
			bin.init(cuB);
			state.Add(bin);
		}
		
		str = ScriptableObject.CreateInstance<StructureSymbol> ();
		str.init (cube);
		state.Add(str);
		
		ee = ScriptableObject.CreateInstance<EndSymbol> ();
		ee.init (end);
		state.Add (ee);
		
		bin = ScriptableObject.CreateInstance<BindingSymbol> ();
		bin.init(star3);
		state.Add(bin);
		
		for (int i = 0; i < 18; i++)
		{
			str = ScriptableObject.CreateInstance<StructureSymbol> ();
			if(i%2 == 0)
				str.init (cube);
			else
				str.init(cylinder);
			state.Add(str);
			
			bin = ScriptableObject.CreateInstance<BindingSymbol> ();
			bin.init(cuB);
			state.Add(bin);
		}
		
		str = ScriptableObject.CreateInstance<StructureSymbol> ();
		str.init (cube);
		state.Add(str);
		
		ee = ScriptableObject.CreateInstance<EndSymbol> ();
		ee.init (end);
		state.Add (ee);
		
		Interpret ();
		debugState ();
	}
	
	void setTestRules()
	{
		rules.Clear ();
		
		BindingSymbol binG = ScriptableObject.CreateInstance<BindingSymbol> ();
		binG.init("g", new Vector3(-0.957f, 0.4984f, 1.1267f), new Vector3(0, -61.7598f, 0), new Vector3(0, 20.0f, 0), false);
		
		BindingSymbol binB = ScriptableObject.CreateInstance<BindingSymbol> ();
		binB.init("b", new Vector3(1.3871f, 0.7653f, 0.0029f), new Vector3(0, 0, 298.3136f), Vector3.zero, true);
		
		StructureSymbol adpRibose = ScriptableObject.CreateInstance<StructureSymbol> ();
		adpRibose.init("m", "adpRibose", null);
		
		CommunicationSymbol mainGrow = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		mainGrow.init("C", "G", new Vector3(-0.957f, 0.4984f, 1.1267f), Quaternion.Euler(new Vector3(0, -61.7598f, 0)), 0.0f, "adpRibose", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		
		CommunicationSymbol mainBranch = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		mainBranch.init("C", "B", new Vector3(1.3871f, 0.7653f, 0.0029f), Quaternion.Euler(new Vector3(0, 0, 298.3136f)), 0.0f, "adpRibose", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		
		///////////////////////////////////////////////////////////////////////
		
		CommunicationSymbol    RP  = null;
		ISymbol                RSS = null;
		List<ISymbol>          RS  = new List<ISymbol> ();
		CommunicationCondition RC  = null;
		Rule                   R   = null;
		
		///////////////////////////////////////////////////////////////////////
		
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
		
		///////////////////////////////////////////////////////////////////////
		
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
		
		///////////////////////////////////////////////////////////////////////
		
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
		((CommunicationSymbol)RSS).init (mainGrow);
		RS.Add (RSS);
		
		RC = ScriptableObject.CreateInstance<CommunicationCondition> ();
		RC.init (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		R = ScriptableObject.CreateInstance<Rule> ();
		R.init (RP, RS, RC, 1.0f);
		rules.Add (R);
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
		bDGlucoseBin.init("b", new Vector3(0.0f, 0.8550f, 0), new Vector3(0, 180f, 0), new Vector3(0, 10.0f, 10.0f), false);
		
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
		tubulinBin.init("t", new Vector3(-0.7555f, -0.8697f, -1.2740f), new Vector3(0, 27.6639f, 0), new Vector3(0, 5.0f, 0), false);
		
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
	
	void setTestRules6()
	{
		rules.Clear ();
		
		StructureSymbol struc = ScriptableObject.CreateInstance<StructureSymbol> ();
		struc.init ("s", "Sphere", null);
		
		StructureSymbol cube = ScriptableObject.CreateInstance<StructureSymbol> ();
		cube.init ("c", "Cube", null);
		
		StructureSymbol cylinder = ScriptableObject.CreateInstance<StructureSymbol> ();
		cylinder.init ("d", "Cylinder", null);
		
		BindingSymbol binG = ScriptableObject.CreateInstance<BindingSymbol> ();
		binG.init("g", new Vector3(-0.4008502f, 0.4984f, 0.8279926f), new Vector3(0, 298.2402f, 0), new Vector3(0, 0.0f, 0), false);
		
		BindingSymbol binB = ScriptableObject.CreateInstance<BindingSymbol> ();
		binB.init("b", new Vector3(0.7569166f, 0.4194877f, 0.3090816f), new Vector3(0, 0, 288.3136f), Vector3.zero, true);
		
		BindingSymbol star1 = ScriptableObject.CreateInstance<BindingSymbol> ();
		star1.init("s1", new Vector3(0.8802532f, 0.373954f, 0.469326f), new Vector3(0, -24.67035f, 15.86258f), new Vector3(0, 0.0f, 0), true);
		
		BindingSymbol star2 = ScriptableObject.CreateInstance<BindingSymbol> ();
		star2.init("s2", new Vector3(-0.8616089f, 0.5282927f, 0.5845833f), new Vector3(-14.20465f, -62.14069f, 5.536514f), Vector3.zero, true);
		
		BindingSymbol star3 = ScriptableObject.CreateInstance<BindingSymbol> ();
		star3.init("s3", new Vector3(-0.03932f, 0.51478f, -0.8856f), new Vector3(-30.1433f, -177.4573f, 0), Vector3.zero, true);
		
		BindingSymbol cuB = ScriptableObject.CreateInstance<BindingSymbol> ();
		cuB.init("sb", new Vector3(0.99f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0f), Vector3.zero, false);
		
		
		EndSymbol end = ScriptableObject.CreateInstance<EndSymbol> ();
		end.init("e");
		
		CommunicationSymbol mainGrow = ScriptableObject.CreateInstance<CommunicationSymbol>();
		mainGrow.init("C", "G", new Vector3(-0.4008502f, 0.4984f, 0.8279926f), Quaternion.Euler(new Vector3(0, 298.2402f, 0)), 0.0f, "Sphere", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		
		CommunicationSymbol mainBranch = ScriptableObject.CreateInstance<CommunicationSymbol>();
		mainBranch.init("C", "B", new Vector3(0.7569166f, 0.4194877f, 0.3090816f), Quaternion.Euler(new Vector3(0, 0, 288.3136f)), 0.0f, "Sphere", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		
		CommunicationSymbol branchGrow = ScriptableObject.CreateInstance<CommunicationSymbol>();
		branchGrow.init("C", "BG", new Vector3(-0.4008502f, 0.4984f, 0.8279926f), Quaternion.Euler(new Vector3(0, 298.2402f, 0)), 0.0f, "Sphere", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		
		CommunicationSymbol star1Branch = ScriptableObject.CreateInstance<CommunicationSymbol>();
		star1Branch.init("C", "SB1", new Vector3(0.8802532f, 0.373954f, 0.469326f), Quaternion.Euler(new Vector3(0, -24.67035f, 15.86258f)), 0.0f, "Cube", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		
		CommunicationSymbol star2Branch = ScriptableObject.CreateInstance<CommunicationSymbol>();
		star2Branch.init("C", "SB2", new Vector3(-0.8616089f, 0.5282927f, 0.5845833f), Quaternion.Euler(new Vector3(-14.20465f, -62.14069f, 5.536514f)), 0.0f, "Cube", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		
		CommunicationSymbol star3Branch = ScriptableObject.CreateInstance<CommunicationSymbol>();
		star3Branch.init("C", "SB3", new Vector3(-0.03932f, 0.51478f, -0.8856f), Quaternion.Euler(new Vector3(-30.1433f, -177.4573f, 0)), 0.0f, "Cube", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		
		CommunicationSymbol star1Grow = ScriptableObject.CreateInstance<CommunicationSymbol>();
		star1Grow.init("C", "SG1", new Vector3(0.99f, 0.0f, 0.0f), Quaternion.Euler(new Vector3(0.0f, 0.0f, 0f)), 0.0f, "Cube", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		
		CommunicationSymbol star2Grow = ScriptableObject.CreateInstance<CommunicationSymbol>();
		star2Grow.init("C", "SG2", new Vector3(0.99f, 0.0f, 0.0f), Quaternion.Euler(new Vector3(0.0f, 0.0f, 0f)), 0.0f, "Cube", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		
		CommunicationSymbol star3Grow = ScriptableObject.CreateInstance<CommunicationSymbol>();
		star3Grow.init("C", "SG3", new Vector3(0.99f, 0.0f, 0.0f), Quaternion.Euler(new Vector3(0.0f, 0.0f, 0f)), 0.0f, "Cube", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		
		CommunicationSymbol growB = ScriptableObject.CreateInstance<CommunicationSymbol>();
		growB.init("C", "grB", new Vector3(0.99f, 0.0f, 0.0f), Quaternion.Euler(new Vector3(0.0f, 0.0f, 0f)), 0.0f, "Cylinder", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		
		CommunicationSymbol growC = ScriptableObject.CreateInstance<CommunicationSymbol>();
		growC.init("C", "grC", new Vector3(0.99f, 0.0f, 0.0f), Quaternion.Euler(new Vector3(0.0f, 0.0f, 0f)), 0.0f, "Cube", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		
		CommunicationSymbol    RP  = null;
		ISymbol                RSS = null;
		List<ISymbol>          RS  = new List<ISymbol> ();
		CommunicationCondition RC  = null;
		Rule                   R   = null;
		///////////////////////////////////////////////////////////////////////
		
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
		
		///////////////////////////////////////////////////////////////////////
		
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
		
		///////////////////////////////////////////////////////////////////////
		
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
		
		RC = ScriptableObject.CreateInstance<CommunicationCondition> ();
		RC.init (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		R = ScriptableObject.CreateInstance<Rule> ();
		R.init (RP, RS, RC, 1.0f);
		rules.Add (R);
		
		///////////////////////////////////////////////////////////////////////
		
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
		
		///////////////////////////////////////////////////////////////////////
		
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
		
		///////////////////////////////////////////////////////////////////////
		
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
		
		RC = ScriptableObject.CreateInstance<CommunicationCondition> ();
		RC.init (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		R = ScriptableObject.CreateInstance<Rule> ();
		R.init (RP, RS, RC, 1.0f);
		rules.Add (R);
		
		///////////////////////////////////////////////////////////////////////
		
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
		
		///////////////////////////////////////////////////////////////////////
		
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
		
		RC = ScriptableObject.CreateInstance<CommunicationCondition> ();
		RC.init (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		R = ScriptableObject.CreateInstance<Rule> ();
		R.init (RP, RS, RC, 1.0f);
		rules.Add (R);
		
		///////////////////////////////////////////////////////////////////////
		
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
		
		///////////////////////////////////////////////////////////////////////
		
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
		
		RC = ScriptableObject.CreateInstance<CommunicationCondition> ();
		RC.init (CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		R = ScriptableObject.CreateInstance<Rule> ();
		R.init (RP, RS, RC, 1.0f);
		rules.Add (R);
		
		///////////////////////////////////////////////////////////////////////
		
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
		
		///////////////////////////////////////////////////////////////////////
		
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
		
		///////////////////////////////////////////////////////////////////////
		
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
	}
	
	void setTestRules7()
	{
		alphabet.Clear ();
		rules.Clear ();
		
		StructureSymbol sphere = ScriptableObject.CreateInstance<StructureSymbol> ();
		sphere.init ("s", "Sphere", null);
		alphabet.Add (sphere);
		
		StructureSymbol cube = ScriptableObject.CreateInstance<StructureSymbol> ();
		cube.init ("c", "Cube", null);
		alphabet.Add (cube);
		
		BindingSymbol bind = ScriptableObject.CreateInstance<BindingSymbol> ();
		bind.init("g", new Vector3(0.0f, 0.8f, 0.0f), new Vector3(0, 0, 0), new Vector3(0, 0.0f, 0), false);
		alphabet.Add (bind);
		
		CommunicationSymbol addCube = ScriptableObject.CreateInstance<CommunicationSymbol>();
		addCube.init("C", "GC", new Vector3(0.0f, 0.8f, 0.0f), Quaternion.Euler(new Vector3(0, 0, 0)), 0.0f, "Cube", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		alphabet.Add (addCube);
		
		CommunicationSymbol addSphere = ScriptableObject.CreateInstance<CommunicationSymbol>();
		addSphere.init("C", "GS", new Vector3(0.0f, 0.8f, 0.0f), Quaternion.Euler(new Vector3(0, 0, 288.3136f)), 0.0f, "Sphere", null, AnimationCurve.Linear(0.0f, 0.0f, 5.0f, 1.0f));
		alphabet.Add (addSphere);
		
		CommunicationSymbol    RP  = null;
		ISymbol                RSS = null;
		List<ISymbol>          RS  = new List<ISymbol> ();
		CommunicationCondition RC  = null;
		Rule                   R   = null;
		
		///////////////////////////////////////////////////////////////////////
		
		RP = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		RP.init(addCube);
		
		RS = new List<ISymbol> ();
		
		RSS = ScriptableObject.CreateInstance<BindingSymbol> ();
		((BindingSymbol)RSS).init (bind);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<StructureSymbol> ();
		((StructureSymbol)RSS).init (cube);
		RS.Add (RSS);
		
		RSS = ScriptableObject.CreateInstance<CommunicationSymbol> ();
		((CommunicationSymbol)RSS).init (addCube);
		RS.Add (RSS);
		
		RC = ScriptableObject.CreateInstance<CommunicationCondition> ();
		RC.init(CommunicationCondition.CommParameters.result, CommunicationCondition.CommOperation.notEqual, null);
		
		R = ScriptableObject.CreateInstance<Rule> ();
		R.init(RP, RS, RC, 1.0f);
		rules.Add (R);
	}
	
	GameObject addObject(ref Turtle turtle, string prefabName, float uncertainity, float branchFail)
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
				mol.renderer.material = basicWhite;
			}
			
			if(prefabName == "alpha-tubulin")
			{
				mol.renderer.material = diffTransLightBlue;
			}
			
			if(prefabName == "beta-tubulin")
			{
				mol.renderer.material = diffTransDarkBlue;
			}
		}

		//uncertainity color encoding
		print (branchFail);
		Color uncertainColor = new Color ( 1.0f, 1.0f  - uncertainity, 1.0f - uncertainity);

		uncertainColor.r = uncertainColor.r - branchFail;
		uncertainColor.g = uncertainColor.g - branchFail;

		mol.renderer.material.SetColor("_Color", uncertainColor);

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

			float uncertainity    = 0.0f;
			float branchFail      = 0.0f;
			float oldUncertainity = symbol.uncertainity;


			Rule rule = rules.Get (symbol, out uncertainity);
			if(rule != null)
			{
				DestroyImmediate(symbol.result);
				state.RemoveAt(activeSymbol.Key + indexOffset);

				if(uncertainity <= 1.0f)
				{
					uncertainity = branchingUncertainity(Mathf.Abs(0.95f - uncertainity));
					branchFail   = branchFailDistance(Mathf.Abs(0.95f - uncertainity));
				}
				else
				{
					uncertainity = 0.0f;
				}

				List<ISymbol> newSymbols = rule.successor;
				
				for(int i = 0; i < newSymbols.Count; i++)
				{
					int newIndex = activeSymbol.Key + indexOffset + i;
					
					ISymbol newSymbol = null;
					
					if(newSymbols[i].GetType() == typeof(CommunicationSymbol))
					{
						newSymbol = ScriptableObject.CreateInstance<CommunicationSymbol>();
						((CommunicationSymbol)newSymbol).init((CommunicationSymbol)newSymbols[i]);
						if(((CommunicationSymbol)newSymbol).process == "B")
						{
							((CommunicationSymbol)newSymbol).uncertainity = oldUncertainity + uncertainity;
						}
						else
						{
							((CommunicationSymbol)newSymbol).uncertainity = oldUncertainity;
						}
						
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

						if(rule.successor.Count > 3)
							((StructureSymbol)newSymbol).uncertainity = oldUncertainity + uncertainity;
						else
						{
							((StructureSymbol)newSymbol).uncertainity = oldUncertainity;
							((StructureSymbol)newSymbol).branchFail   = uncertainity;//branchFail;
						}
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
					((StructureSymbol)state[i]).structureObject = addObject(ref current, ((StructureSymbol)symbol).structurePrefabName, ((StructureSymbol)symbol).uncertainity, ((StructureSymbol)symbol).branchFail);
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
				if((((CommunicationSymbol)symbol).process == "G") || (((CommunicationSymbol)symbol).process == "BG")  || (((CommunicationSymbol)symbol).process == "grB")  || (((CommunicationSymbol)symbol).process == "grC"))
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
		CommunicationManager cql = communicationQueryObject.GetComponent<CommunicationManager> ();
		
		foreach(KeyValuePair<int, CommunicationSymbol> symbol in activeSymbols)
		{
			cql.Add(symbol.Key, symbol.Value);
		}
	}
	
	private void TimeStep()
	{
		preEnviromentStep ();
		
		Derive ();
		
		Interpret ();
		
		postEnviromentStep ();
	}
	
	void Update()
	{
		TimeStep ();
		//debugUncertainity ();
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

	void debugUncertainity()
	{
		string output = "";
		for (int i = 0; i < state.Count; i++)
		{
			output += state[i].name;
			
			if(state[i].GetType() == typeof(CommunicationSymbol))
			{
				output += "(" + ((CommunicationSymbol)state[i]).process + ")";
				output += "[" + ((CommunicationSymbol)state[i]).uncertainity + "]";
			}

			if(state[i].GetType() == typeof(StructureSymbol))
			{
				output += "[" + ((StructureSymbol)state[i]).uncertainity + "]";
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
				symbolStr += "StructureSymbol(" + ((StructureSymbol)symbol).structurePrefabName + ")";
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

	private float branchingUncertainity(float distance)
	{
		return 0.35f * (float)Math.Exp (-distance * distance / 0.0008 );
	}

	private float branchFailDistance(float distance)
	{
		return 0.35f * (float)Math.Exp (-distance * distance / 0.0008 );
	}
}