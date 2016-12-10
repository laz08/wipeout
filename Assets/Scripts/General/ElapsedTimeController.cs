using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ElapsedTimeController : MonoBehaviour {

	public Text timeText; 
	private int startTimestamp;

	// Use this for initialization
	void Awake () {
	
		startTimestamp = (int) Time.time;
	}
	
	// Update is called once per frame
	void Update () {
	
		float elapsedTime = (int) Time.time - startTimestamp;
		timeText.text  = string.Format ("{0:00}:{1:00}", elapsedTime/60, elapsedTime%60);
	}
}
