using UnityEngine;
using System.Collections;

public class MoveVehicle : MonoBehaviour {

	public float accZ = 6.0f;
	public float maxSpeedForward = 100.0f;

	float speedZ = 0.0f;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.UpArrow)) {
			speedZ += accZ * Time.deltaTime;
			speedZ = Mathf.Min (speedZ,maxSpeedForward);
		} else {
			speedZ -= accZ * Time.deltaTime;
			speedZ = Mathf.Max (speedZ,0.0f);
		}

		if (Input.GetKey (KeyCode.DownArrow)) {
			speedZ = -accZ;
		}
			
		gameObject.transform.Translate(0.0f, 0.0f, speedZ*Time.deltaTime + (1/2)*accZ*Time.deltaTime*Time.deltaTime);

	}
		
}

