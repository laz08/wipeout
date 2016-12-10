using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ElapsedTimeController : MonoBehaviour {

	public Text timeText; 
	private float startTimestamp;

	// Use this for initialization
	void Awake () {
	
		startTimestamp = Time.time*1000;
	}
	
	// Update is called once per frame
	void Update () {
	
		float elapsedTime = Time.time*1000 - startTimestamp;
		timeText.text  = string.Format ("{0:00}:{1:00}:{2:000}", elapsedTime/(60*1000), (elapsedTime/1000)%60, (elapsedTime)%1000)	;
	}
}
