using UnityEngine;
using System.Collections;

public class CenterCameraOnVehicle : MonoBehaviour {

	public GameObject mUserVehicle;
	public float mDefaultZ = -10f;
	public float mOffsetY = 10f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {

		transform.position = 
			new Vector3 (
			mUserVehicle.transform.position.x + 10f, 
			mUserVehicle.transform.position.y + mOffsetY, 
			mUserVehicle.transform.position.z + mDefaultZ);
		
		transform.LookAt(mUserVehicle.transform.position);
	}
}
