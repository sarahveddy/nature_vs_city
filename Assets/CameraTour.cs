using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTour : MonoBehaviour {

	// Use this for initialization
	Vector3 originalLoc;
	private float startTime;
	private float speed;
	private Transform go; 
	
	void Start ()
	{
		originalLoc = transform.position;
		startTime = Time.time;
		speed = 10;
		go = gameObject.transform.GetChild(0); 

	}
	
	// Update is called once per frame
	void Update ()
	{
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, go.transform.position, step); 
		if (Time.timeSinceLevelLoad - startTime > 10)
		{
			transform.position = originalLoc;
			startTime = Time.time; 
		}
	}
}
