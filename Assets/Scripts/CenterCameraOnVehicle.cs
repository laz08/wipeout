using UnityEngine;
using System.Collections;

public class CenterCameraOnVehicle : MonoBehaviour {

	public GameObject mUserVehicle;
	private float mDefaultXOffset = -4.2f;
	private float mOffsetY = 15.58f;
	private float mOffsetZ = -38.7f;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		transform.position = mUserVehicle.transform.position;
	
		//transform.forward = -mUserVehicle.transform.forward;
		//transform.LookAt(mUserVehicle.transform.position);
		transform.localPosition = 
			new Vector3 (
				mDefaultXOffset, 
				mOffsetY, 
				mOffsetZ);

	}
}
