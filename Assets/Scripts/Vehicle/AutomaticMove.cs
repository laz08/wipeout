using UnityEngine;
using System.Collections;

public class AutomaticMove : MonoBehaviour {
	
	public CreateFirstTrackWaypoints mWaypointsCreator;

	// Use this for initialization
	void Start () {
     
      //DO nothing.
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey (KeyCode.K)) {
		
			transform.position = mWaypointsCreator.getNextWaypoint (transform.position);
		}
	}
}
