using UnityEngine;
using System.Collections;

public class AutomaticMove : MonoBehaviour {
	
	public CreateFirstTrackWaypoints mWaypointsCreator;

    private int actualWayPoint;
	// Use this for initialization
	void Start () {
        actualWayPoint = 0;
      //DO nothing.
	}
	
	// Update is called once per frame
	void Update () {

        //transform.LookAt();
        /*if (Input.GetKey (KeyCode.K)) {
		
			transform.position = mWaypointsCreator.getNextWaypoint (transform.position);
		}*/
	}
 

 
}
