using UnityEngine;
using System.Collections;

public class GlobalVariables
{
	public static string[] examples     = {"none", "PARP", "Star", "Copolymer", "Cellulose", "Tubulin", "Showcase1", "Timer"};
	public static string[] monomerTypes = {"adpRibose", "molecule", "a-D-glucose", "b-D-glucose", "alpha-tubulin", "beta-tubulin", "tubulin", "PARP1ap", "Sphere", "Cube", "Cylinder", "NAD", "glucose"};
	public static string[] symbolTypes  = {"ISymbol", "StructureSymbol", "BindingSymbol", "CommunicationSymbol", "EndSymbol"};

	public static float timeDelta    = 0.1f;
	public static float monomerSpeed = timeDelta * 50.0f;
}
