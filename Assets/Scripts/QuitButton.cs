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
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false; 
		#else
			Application.Quit(); 
		#endif
	}
	
	
}
