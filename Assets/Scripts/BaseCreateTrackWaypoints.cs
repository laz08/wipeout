using UnityEngine;
using System.Collections;

/**
 * Base object to create track waypoints.
 */
public class BaseCreateTrackWaypoints : MonoBehaviour {

	public GameObject mWaypointSphere;

	protected Vector3[] mWayPoints;

	// Use this for initialization
	public virtual void Start () {
	
		// In derived classes:
		// 1.- Initialize waypoints
		// 2.- call instantiateWaypoints() method.
	}


	public Vector3 getNextWaypoint(Vector3 currentPosition){

		int closestWaypointIndex = 0;
		float closestDistance = Mathf.Abs(Vector3.Distance (currentPosition, mWayPoints [0]));
		float tmpDistance;
		for (int i = 1; i < mWayPoints.Length; i++) {

			tmpDistance = Mathf.Abs(Vector3.Distance (currentPosition, mWayPoints [i]));
			if (tmpDistance < closestDistance) {

				closestDistance = tmpDistance;
				closestWaypointIndex = i;
			}
		}

		if(closestWaypointIndex == (mWayPoints.Length - 1)){

			//Its last waypoint, so the next one would be the first in the circuit.
			return mWayPoints [0];
		} else {

			//Return next waypoint
			return mWayPoints[closestWaypointIndex+1];
		}
	}

    public Vector3 getWaypoint(int index)
    {
        return mWayPoints[index % (mWayPoints.Length - 1)];
    }


	protected void instantiateWayPoints() {

		GameObject wayPointObj;
		for (int i = 0; i < mWayPoints.Length; i++) {

			//Instantiate waypoint
			wayPointObj = (GameObject) Instantiate (mWaypointSphere, mWayPoints[i] - new Vector3(0.0f,13.0f,0.0f), transform.rotation);

			//Place waypoint on parent
			wayPointObj.transform.parent = transform;
		}
	}


	// Update is called once per frame
	void Update () {
	
	}
}
