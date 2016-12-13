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
		/*if (!isSecondTrack) {
			mDefaultXOffset = 0.0f;
			mOffsetY = 15.0f;
			mOffsetZ = -35.0f;
		} else {
			mDefaultXOffset = 0.0f;
			mOffsetY = 15.0f;
			mOffsetZ = -45.0f;
		}*/

		//Initialize position and rotation
		int wayPointV = mUserVehicle.GetComponent<MoveVehicle> ().currentWayPoint;
		Vector3 prevPoint = mWaypointsFactory.getWaypoint (wayPointV - wayPointsOffset);
		Vector3 nextPoint = mWaypointsFactory.getWaypoint (wayPointV);

		if (isSecondTrack) {
			//Vector3 camToUser = mUserVehicle.transform.position -  mWaypointsFactory.getWaypoint (wayPointV);
			//prevPoint += camToUser * camToUserOffset;
			//nextPoint += camToUser * camToUserOffset;
		} else {
			prevPoint = mWaypointsFactory.getWaypoint (wayPointV - wayPointsOffset*3) +  new Vector3 (0.0f, 10.0f, 0.0f) ; //Small y offset
			nextPoint +=/*  mWaypointsFactory.getWaypoint (wayPointV + wayPointsOffset*2) +*/ new Vector3 (0.0f, 10.0f, 0.0f);
		}

		transform.position = prevPoint;
		transform.LookAt (nextPoint);

	}
	
	// Update is called once per frame
	void Update () {
		
		float speed = Mathf.Log10(mUserVehicle.GetComponent<MoveVehicle> ().getSpeed ())*Time.deltaTime;//Lerp speed
		int wayPointV = mUserVehicle.GetComponent<MoveVehicle> ().currentWayPoint;
		Vector3 prevPoint = mWaypointsFactory.getWaypoint (wayPointV - wayPointsOffset);

		if (isSecondTrack) {
			Vector3 camToUser = mUserVehicle.transform.position - mWaypointsFactory.getWaypoint (wayPointV);
			prevPoint += camToUser * camToUserOffset;
			if (mUserVehicle.GetComponent<MoveVehicle> ().timeDamagedCountdown > 0.0f) //being damaged, slow speed
				speed /= 3.0f;
		} else {
			speed *= 2.2f;
			prevPoint = mWaypointsFactory.getWaypoint (wayPointV - wayPointsOffset*2) +  new Vector3 (0.0f, 10.0f, 0.0f) ; //Small y offset
		}

		//Change camera parameters
		transform.position = Vector3.Lerp (transform.position, prevPoint, speed);
		if (prevPoint -transform.position != new Vector3(0.0f,0.0f,0.0f)) {
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation (prevPoint - transform.position), speed);
		}

		/*
		 * OLD
		 * 
		transform.position = mUserVehicle.transform.position;
	
		transform.localPosition = 
			new Vector3 (
				mDefaultXOffset, 
				mOffsetY, 
				mOffsetZ);
		if (!isSecondTrack)
			transform.LookAt (mUserVehicle.transform.position);
		else {
			Vector3 offsetLookAt = mUserVehicle.transform.InverseTransformVector (new Vector3 (0.0f, mOffsetY/1.8f,0.0f));
			//transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation(mUserVehicle.transform.position+offsetLookAt-transform.position),Time.deltaTime*15);
			//transform.LookAt (mUserVehicle.transform.position +offsetLookAt);
		}

		*/

	}
}
