using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuButtonsController : MonoBehaviour {

	public GameObject playButton;
	public GameObject instructionsButton;
	public GameObject creditsButton;

	public GameObject okButton;

	public GameObject instructionsText;
	public GameObject creditsText;

	private bool isInSubMenu = false;
	private bool isCredits = false;
	private bool isInstructions = false;

	void Awake(){
	

		setVisibility ();
	}


	public void onPlayClick(){
	
		//Application.LoadLevel (1);
	}

	public void onInstructionsClick(){
	
		isInSubMenu = true;
		isInstructions = true;
		setVisibility ();
	}

	public void onCreditsClick(){
	
		isInSubMenu = true;
		isCredits = true;

		setVisibility ();
	}

	public void onOKButtonClick(){

		isInSubMenu = false;
		isCredits = isInstructions = false;

		setVisibility ();
	}

	private void setVisibility(){

		playButton.SetActive (!isInSubMenu);
		instructionsButton.SetActive (!isInSubMenu);
		creditsButton.SetActive (!isInSubMenu);

		okButton.SetActive (isInSubMenu);

		instructionsText.SetActive (isInstructions);
		creditsText.SetActive (isCredits);
	}
}

