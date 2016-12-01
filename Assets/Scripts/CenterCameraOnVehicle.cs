using UnityEngine;
using System.Collections;

public class CenterCameraOnVehicle : MonoBehaviour {

	public GameObject mUserVehicle;
	public float mDefaultXOffset = -15f;
	public float mOffsetY = 10f;

	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void LateUpdate () {

		//transform.position = mUserVehicle.transform.position;
	
		//transform.forward = -mUserVehicle.transform.forward;
		//transform.LookAt(mUserVehicle.transform.position);
	/*
		transform.localPosition = 
			new Vector3 (
				mUserVehicle.transform.localPosition.x - mDefaultXOffset, 
				mUserVehicle.transform.localPosition.y + mOffsetY, 
				mUserVehicle.transform.localPosition.z);
				*/
	}
}
