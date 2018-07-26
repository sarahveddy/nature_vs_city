using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Starter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (PersistentManagerScript.Instance.randomLevel)
		{
			int sceneIndex = Random.Range(1, 3);
			SceneManager.LoadScene(sceneIndex);
			FindObjectOfType<Logging>().SendLog("SCENE SELECTED", sceneIndex.ToString());
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
