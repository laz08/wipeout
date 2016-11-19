using UnityEngine;
using System.Collections;

public class CloseGame : MonoBehaviour {


	// Update is called once per frame
	void Update () {

		if (Input.GetKey (KeyCode.Escape)) {

			Application.Quit ();
		}
	}

	public void onClick(){
		
		Application.Quit ();
	}

}
	