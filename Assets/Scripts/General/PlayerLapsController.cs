using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerLapsController : MonoBehaviour {

	public Text lapsText;
	public Text positionText;
	public RawImage itemImage;

	public int maxLaps = 3;

	void Start(){

		lapsText.text = getLapsText(0);
	}

	private string getLapsText(int currentLap){
	
		return currentLap + "/" + maxLaps;
	}

	public void setLapsDone(int lapsDone){

		lapsText.text = getLapsText (lapsDone);
	}

	public void setPositionText(int pos){
	
		//Do sth
	}
		
}
