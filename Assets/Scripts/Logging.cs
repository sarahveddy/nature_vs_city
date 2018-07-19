using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logging : MonoBehaviour 
{
	long frames = 0;
	float startTime;

	// Use this for initialization
	void Start () 
	{
		startTime = Time.time;
		SendLog("SceneStarted");
	}

	// Update is called once per frame
	void Update () 
	{
		frames++;
	}

	void OnDestroy()
	{
		double fps = frames / (double)(Time.time - startTime);

		SendLog("AvgFPS", fps.ToString());
		SendLog("SceneEnded");
	}

	public void SendLog(string message, string value = "")
	{
		float elapsed = Time.time;
		string scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

		WWWForm frm = new WWWForm();
		frm.AddField("elapsed", elapsed.ToString());
		frm.AddField("scene", scene);
		frm.AddField("message", message);
		frm.AddField("value", value);

		var www = new WWW("#", frm);

		if (!string.IsNullOrEmpty(www.error))
		{
			UnityEngine.Debug.Log("Could not submit log");
		}
		else
		{
			UnityEngine.Debug.Log("Logged successfully");
		}
	}
} 
