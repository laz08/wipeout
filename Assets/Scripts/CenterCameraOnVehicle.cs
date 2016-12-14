using UnityEngine;
using System.Collections;

public class CenterCameraOnVehicle : MonoBehaviour {

	public GameObject mUserVehicle;

	/*private float mDefaultXOffset;
	private float mOffsetY;
	private float mOffsetZ;*/
	public bool isSecondTrack = false; //True for first track. False for torus.
	private int wayPointsOffset = 2;
	private float camToUserOffset = 0.6f; //Used only for torus track, how much of the distance betwen waypoint and user take
	public BaseCreateTrackWaypoints mWaypointsFactory;

	// Use this for initialization
	void Start () {
		//Initialize position and rotation
		int wayPointV = mUserVehicle.GetComponent<MoveVehicle> ().currentWayPoint;
		Vector3 prevPoint = mWaypointsFactory.getWaypoint (wayPointV - wayPointsOffset);
		Vector3 nextPoint = mWaypointsFactory.getWaypoint (wayPointV);

		if (isSecondTrack) {
			//Vector3 camToUser = mUserVehicle.transform.position -  mWaypointsFactory.getWaypoint (wayPointV);
            prevPoint = mWaypointsFactory.getWaypoint(wayPointV - wayPointsOffset * 3);
			//prevPoint += camToUser * camToUserOffset;
			//nextPoint += camToUser * camToUserOffset;
            //nextPoint = mUserVehicle.transform.position;
		} else {
			prevPoint = mWaypointsFactory.getWaypoint (wayPointV - wayPointsOffset*20) +  new Vector3 (0.0f, 10.0f, 0.0f) ; //Small y offset
			nextPoint =  mWaypointsFactory.getWaypoint (wayPointV - wayPointsOffset*15) + new Vector3 (0.0f, 10.0f, 0.0f);
		}

		transform.position = prevPoint;
		transform.LookAt (nextPoint);

	}
	
	// Update is called once per frame
	void Update () {

        if (mUserVehicle.GetComponent<MoveVehicle>().waitingStartTime > 0.0f) return;

		float speed = Mathf.Log10(mUserVehicle.GetComponent<MoveVehicle> ().getSpeed ())*Time.deltaTime;//Lerp speed
		int wayPointV = mUserVehicle.GetComponent<MoveVehicle> ().currentWayPoint;
		Vector3 prevPoint = mWaypointsFactory.getWaypoint (wayPointV - wayPointsOffset);

		if (isSecondTrack) {
			Vector3 camToUser = mUserVehicle.transform.position - mWaypointsFactory.getWaypoint (wayPointV);
			prevPoint += camToUser * camToUserOffset;	
		} else {
			speed *= 2.2f;
			prevPoint = mWaypointsFactory.getWaypoint (wayPointV - wayPointsOffset*3) +  new Vector3 (0.0f, 10.0f, 0.0f) ; //Small y offset
		}

        if (mUserVehicle.GetComponent<MoveVehicle>().timeDamagedCountdown > 0.0f) //being damaged, slow speed
            speed /= 3.0f;

		//Change camera parameters
		transform.position = Vector3.Lerp (transform.position, prevPoint, speed);
		if (prevPoint -transform.position != new Vector3(0.0f,0.0f,0.0f)) {
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation (prevPoint - transform.position), speed);
		}

	}
}
