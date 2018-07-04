using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{

	public bool quit; 
	public bool nature;
	public bool city;

	void OnMouseUp()
	{
		if (quit)
		{
			Debug.Log("Quit");
			//Application.Quit();
		}

		if (nature)
		{
			Debug.Log("Nature");
			//SceneManager.LoadScene("NatureScene");
		}

		if (city)
		{
			Debug.Log("City");
			//SceneManager.LoadScene("CityScene");
		}
	}
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
