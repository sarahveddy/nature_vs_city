using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*	FUNCITON:
*	- This script keeps track of the time when gameplay is started
*	- This is used to calculate the time the player spends in actual gameplay
*	- The singleton pattern allows this value to persist when the scene is reloaded 
*
*/

public class PersistentManagerScript : MonoBehaviour
{
	public float gameplayStart = 0.0f;
	public bool gameplayStarted = false; 
	
	public static PersistentManagerScript Instance
	{
		get;
		private set; 
	}
	
	// Use this for initialization
	void Awake () {
		if (Instance == null)
		{
			Instance = this; 
			Debug.Log("*** PM Created **** "+Time.realtimeSinceStartup);
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

}
