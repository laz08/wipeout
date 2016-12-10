using UnityEngine;
using System.Collections;

/**
 * Base object to create track waypoints.
 */
public class BaseCreateTrackWaypoints : MonoBehaviour {

	public GameObject mWaypointSphere;

	public float smoothnessInterpolation = 2.0f; 

	protected Vector3[] mWayPoints;

	public bool drawWaypointsAndLines = true;
	public bool applyInterpolation = false;
	public bool shouldWaypointsBeInstantiated= false;

	// Use this for initialization
	public virtual void Start (){

		// In derived classes:
		// 1.- Initialize waypoints
		// 2.- call instantiateWaypoints() method.
	}



	// ------------------------------------------------
	// -----------      NEW       ---------------------
	// ------------------------------------------------
	public Vector3 getWaypointPosition(int currentWaypoint){

		return mWayPoints [currentWaypoint];
	}

	public int getNextWaypoint(int currentWaypoint){

		if (currentWaypoint == mWayPoints.Length - 1) {

			return 0;
		} else {

			return currentWaypoint+1;
		}
	}

	public Vector3 getDir(int currentWaypoint){

		if (currentWaypoint == mWayPoints.Length - 1) {

			return (mWayPoints [0] - mWayPoints [currentWaypoint]).normalized;
		} else {

			//Return next waypoint dir
			return (mWayPoints[currentWaypoint+1] -  mWayPoints[currentWaypoint]).normalized;
		}
	}


	// ------------------------------------------------
	// -----------      OLD       ---------------------
	// ------------------------------------------------
	public Vector3 getNextWaypoint(Vector3 currentPosition){

		int closestWaypointIndex = 0;
		//float closestDistance = Mathf.Abs(Vector3.Distance (currentPosition, mWayPoints [0]));
		float closestDistance = Vector3.Distance (currentPosition, mWayPoints [0]);
		float tmpDistance;
		for (int i = 1; i < mWayPoints.Length; i++) {

			tmpDistance = Vector3.Distance (currentPosition, mWayPoints [i]);
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

	public Vector3 getDir(Vector3 currentPosition){

		int closestWaypointIndex = 0;
		float closestDistance = Vector3.Distance (currentPosition, mWayPoints [0]);
		float tmpDistance;
		for (int i = 1; i < mWayPoints.Length; i++) {

			tmpDistance = Vector3.Distance (currentPosition, mWayPoints [i]);
			if (tmpDistance < closestDistance) {

				closestDistance = tmpDistance;
				closestWaypointIndex = i;
			}
		}

		//if(closestWaypointIndex == (mWayPoints.Length - 1)){
		if(closestWaypointIndex == 0){

			//Its last waypoint, so the next one would be the first in the circuit.
			return (mWayPoints [0] - mWayPoints[mWayPoints.Length-1]).normalized;
		} else {

			//Return next waypoint
			return (mWayPoints[closestWaypointIndex] -  mWayPoints[closestWaypointIndex-1]).normalized;
		}
	}

	public Vector3 getWaypoint(int index)
	{
		return mWayPoints[index % (mWayPoints.Length - 1)];
	}


	public int getCurrentWaypointIndex(Vector3 currentPosition){

		int closestWaypointIndex = 0;
		float closestDistance = Vector3.Distance (currentPosition, mWayPoints [0]);
		float tmpDistance;
		for (int i = 1; i < mWayPoints.Length; i++) {

			tmpDistance = Vector3.Distance (currentPosition, mWayPoints [i]);
			if (tmpDistance < closestDistance) {

				closestDistance = tmpDistance;
				closestWaypointIndex = i;
			}
		}
		return closestWaypointIndex;
	}


	// ------------------------------------------------
	// -----------    ON START    ---------------------
	// -----------     DEBUG      ---------------------
	// ------------------------------------------------
	protected void instantiateWayPoints() {

		GameObject wayPointObj;
		for (int i = 0; i < mWayPoints.Length; i++) {

			//Instantiate waypoint
			wayPointObj = (GameObject) Instantiate (mWaypointSphere, mWayPoints[i], transform.rotation);

			//Place waypoint on parent
			wayPointObj.transform.parent = transform;
		}
	}
		

	protected void DrawLines(){

		for (int i = 0; i < mWayPoints.Length - 1; i++) {
		

			Debug.DrawLine(mWayPoints[i], mWayPoints[i+1], Color.red);
		}
		Debug.DrawLine(mWayPoints[mWayPoints.Length-1], mWayPoints[0], Color.red);
	}

	void OnDrawGizmos(){

		Gizmos.color = Color.blue;
		if (mWayPoints != null && mWayPoints.Length > 0) {
			Gizmos.DrawSphere (mWayPoints [0], 3);

			Gizmos.color = Color.yellow;
			for (int i = 1; i < mWayPoints.Length; i++) {
		
				Gizmos.DrawSphere (mWayPoints [i], 1);
			}
		}
	}
}
