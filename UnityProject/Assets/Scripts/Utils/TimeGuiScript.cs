using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TimeGuiScript : MonoBehaviour
{
	static public float time         = 0.0f;
	static public float seconds      = 0.0f;
	static public float miliSeconds  = 0.0f;
	static public float microSeconds = 0.0f;
	static public float nanoSeconds  = 0.0f;

	private GUIStyle backgroundStyle = null;
	private GUIStyle activeStyle     = null;
	private GUIStyle inactiveStyle   = null;

	private float microNanoAccumulate = 0.0f;

	private void InitStyles()
	{
		if (backgroundStyle == null)
		{
			backgroundStyle = new GUIStyle (GUI.skin.box);
			backgroundStyle.border.top = 2;
			backgroundStyle.border.bottom = 2;
			backgroundStyle.border.left = 2;
			backgroundStyle.border.right = 2;

			backgroundStyle.normal.background = (Texture2D)Resources.Load("Images/boxBackground", typeof(Texture2D));
		}

		if (inactiveStyle == null)
		{
			inactiveStyle = new GUIStyle(GUI.skin.box);
			inactiveStyle.normal.background = null;
		}

		if (activeStyle == null)
		{
			activeStyle = new GUIStyle(GUI.skin.box);
			activeStyle.border.top    = 1;
			activeStyle.border.bottom = 1;
			activeStyle.border.left   = 1;
			activeStyle.border.right  = 1;
			activeStyle.normal.background = (Texture2D)Resources.Load("Images/boxActive", typeof(Texture2D));;
		}
	}

	void OnGUI ()
	{
		InitStyles ();

		string strSeconds      = "";
		string strMiliSeconds  = "";
		string strMicroSeconds = "";
		string strNanoSeconds  = "";

		GUIStyle secondStyle      = inactiveStyle;
		GUIStyle miliSecondStyle  = inactiveStyle;
		GUIStyle microSecondStyle = inactiveStyle;
		GUIStyle nanoSecondStyle  = inactiveStyle;

		if (Movement.speed >= 30)
		{
			secondStyle = activeStyle;
			time += Time.deltaTime;

			microSeconds = UnityEngine.Random.Range(0, 999);
			nanoSeconds  = UnityEngine.Random.Range(0, 999);
		}
		else if (NewAgentSystem.bindingMotion && (Movement.bindingTimerSaved == 0.0f))
		{
			nanoSecondStyle = activeStyle;

			microNanoAccumulate += Time.deltaTime;

			if(microNanoAccumulate > 1.0f)
			{
				nanoSeconds += 1;
				microNanoAccumulate = 0.0f;
			}
		}
		else
		{
			microSecondStyle = activeStyle;

			microNanoAccumulate += Time.deltaTime;

			if(microNanoAccumulate > 1.0f)
			{
				microSeconds += 1;
				microNanoAccumulate = 0.0f;
			}

			nanoSeconds  = UnityEngine.Random.Range(0, 999);
		}

		seconds     = (float)Math.Truncate(time);
		miliSeconds = (float)Math.Truncate((time * 1000) % 1000 );

		strSeconds      = seconds      + "s";
		strMiliSeconds  = miliSeconds  + "ms";
		strMicroSeconds = microSeconds + "µs";
		strNanoSeconds  = nanoSeconds  + "ns";

		GUI.Box (new Rect (5,   Screen.height - 45,  225, 35), "",           backgroundStyle);
		GUI.Box (new Rect (10,  Screen.height - 40,  50,  25), strSeconds,      secondStyle);
		GUI.Box (new Rect (65,  Screen.height - 40,  50,  25), strMiliSeconds,  miliSecondStyle);
		GUI.Box (new Rect (120, Screen.height - 40,  50,  25), strMicroSeconds, microSecondStyle);
		GUI.Box (new Rect (175, Screen.height - 40,  50,  25), strNanoSeconds,  nanoSecondStyle);
	}
}
