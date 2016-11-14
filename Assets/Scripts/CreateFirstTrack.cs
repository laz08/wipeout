using UnityEngine;
using System.Collections;

public class CreateFirstTrack : MonoBehaviour {
	
	public GameObject mTrackPiece;

	// Use this for initialization
	void Start () {
		
		for (int j = 0; j < 4; j++) {
			
		
			GameObject obj = (GameObject)Instantiate (mTrackPiece, new Vector3 (0f, 0f, j*20f), transform.rotation);
			obj.transform.parent = transform;
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
