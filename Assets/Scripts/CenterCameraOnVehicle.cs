using UnityEngine;
using System.Collections;

public class CenterCameraOnVehicle : MonoBehaviour {

	public GameObject mUserVehicle;

	private float mDefaultXOffset;
	private float mOffsetY;
	private float mOffsetZ;
	public bool isSecondTrack = false; //True for first track. False for torus.


	// Use this for initialization
	void Start () {
		if (!isSecondTrack) {
			mDefaultXOffset = 0.0f;
			mOffsetY = 15.0f;
			mOffsetZ = -25.0f;
		} else {
			mDefaultXOffset = 0.0f;
			mOffsetY = 15.0f;
			mOffsetZ = -45.0f;
		}
	}
	
	// Update is called once per frame
	void Update () {

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
			transform.LookAt (mUserVehicle.transform.position +offsetLookAt);
		}
	}
}
