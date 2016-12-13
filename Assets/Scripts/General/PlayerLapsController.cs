using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerLapsController : MonoBehaviour {

	public Text lapsText;
	public Text positionText;

	public int maxLaps = 3;
	private float positionWaitTime = 0.15f;
	private float positionCountDown = 0.0f;

	void Start(){

		lapsText.text = getLapsText(0);
	}

	private string getLapsText(int currentLap){
	
		return Mathf.Max(1,Mathf.Min(currentLap,maxLaps)) + "/" + maxLaps;
	}

	public void setLapsDone(int lapsDone){

		lapsText.text = getLapsText (lapsDone);
	}

	public void setPositionText(int pos){
		//Not do it every frame in order to not have epilepsy
		if (positionCountDown <= 0.0f) {
			string position = pos.ToString ();
			switch (pos) {
			case 1:
				positionText.color = Color.yellow;
				position += "st";
				break;
			case 2:
				positionText.color = Color.green;
				position += "nd";
				break;
			case 3:
				positionText.color = Color.cyan;
				position += "nd";
				break;
			default:
				positionText.color = Color.red;
				position += "th";
				break;
			}
			positionText.text = position;
			positionCountDown = positionWaitTime;
		} else
			positionCountDown -= Time.deltaTime;
	}
		
}
