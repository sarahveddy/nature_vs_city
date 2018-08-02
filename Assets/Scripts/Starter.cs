using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Starter : MonoBehaviour {
	// Use this for initialization
	private GameObject splash; 
	void Start () {
		splash = GameObject.Find("SplashScreen").gameObject; 
		if (PersistentManagerScript.Instance.randomLevel)
		{
			int sceneIndex = Random.Range(1, 3);
			SceneManager.LoadScene(sceneIndex);
//			FindObjectOfType<Logging>().SendLog("SCENE SELECTED", sceneIndex.ToString());
		}
		else
		{
			splash.GetComponent<Canvas>().enabled = false; 
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
