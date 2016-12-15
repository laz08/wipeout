using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ElapsedTimeController : MonoBehaviour {

	public Text timeText; 
	private int startTimestamp;
	private bool hasStarted = false;

	// Use this for initialization
	void Awake () {
	
		startTimestamp = (int) Time.time;
	}



	// Update is called once per frame
	void Update () {
	
		float elapsedTime = (int) Time.time - startTimestamp;
		if (!hasStarted && elapsedTime >= 4) {
		
			startTimestamp = (int) Time.time;
			hasStarted = true;
		} else if (hasStarted) {
		
			timeText.text = string.Format ("{0:00}:{1:00}", elapsedTime / 60, elapsedTime % 60);
		}
	}
}
