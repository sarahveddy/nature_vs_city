using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*	FUNCTION:
*	This script closes the game.
*
*/

public class QuitButton : MonoBehaviour {

	public void Quit() 
	{
		Debug.Log(Application.absoluteURL);
		string[] s = Application.absoluteURL.Split('/');
		Debug.Log(s[0] + "       "+ s[1]);
		string newURL = s[0] + "/redirect_next_page";
		Debug.Log(newURL);
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false; 
		#else
			Application.OpenURL(newURL); 
		#endif
	}
}
