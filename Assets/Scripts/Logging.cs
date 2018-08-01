using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Logging : MonoBehaviour
{
	long frames = 0;
	float startTime;

	// Use this for initialization
	void Start()
	{
//		startTime = Time.time;
//		SendLog("SceneStarted");
		SaveTrial();
	}

	// Update is called once per frame
//	void Update()
//	{
//		frames++;
//	}
//
//	void OnDestroy()
//	{
//		double fps = frames / (double) (Time.time - startTime);
//
//		SendLog("AvgFPS", fps.ToString());
//		SendLog("SceneEnded");
//	}
//
//	public void SendLog(string message, string value = "")
//	{
//		float elapsed = Time.time;
//		string scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
//
//		WWWForm frm = new WWWForm();
//		frm.AddField("elapsed", elapsed.ToString());
//		frm.AddField("scene", scene);
//		frm.AddField("message", message);
//		frm.AddField("value", value);
//
//		var www = new WWW("#", frm);
//
//		if (!string.IsNullOrEmpty(www.error))
//		{
//			UnityEngine.Debug.Log("Could not submit log");
//		}
//		else
//		{
//			UnityEngine.Debug.Log("Logged successfully: " + message + " " + value);
//		}
//	}



	public void SaveTrial()
	{
//		float framerate = trialFrames / timerTrial.value;

		WWWForm frm = new WWWForm();
//		if (participantID != 0)
		frm.AddField("participantID", 0);
		frm.AddField("level", (SceneManager.GetActiveScene().buildIndex) - 1);
//		frm.AddField("duration", timerTrial.value.ToString("#.00"));
//		frm.AddField("avgFps", framerate.ToString("#.00"));
//		frm.AddField("trialNumber", trialNumber);
//		frm.AddField("sessionNumber", sessionNumber);
//		frm.AddField("difficultyRotation", levelManager.diffSettings.rotationDifficulty.GetValue().ToString("#.00000"));
//		frm.AddField("difficultySpawning", levelManager.diffSettings.spawningDifficulty.GetValue().ToString("#.00000"));
//		frm.AddField("movements", StringCompressor.CompressString(movementsCSV()));

		try
		{
			var request = UnityWebRequest.Post("/game_bejeweled", frm);
			request.SendWebRequest();
		}
		catch (Exception ex)
		{
			Debug.Log("Error in SaveTrial(): " + ex.Message);
		}
	}
}