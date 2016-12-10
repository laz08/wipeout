using UnityEngine;
using System.Collections;

public class CenterCameraOnVehicle : MonoBehaviour {

	public GameObject mUserVehicle;
	private float mDefaultXOffset = 0.0f;
	private float mOffsetY = 7.0f;
	public float mOffsetZ = -25.0f;
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

		transform.LookAt(mUserVehicle.transform.position);
	}
}
