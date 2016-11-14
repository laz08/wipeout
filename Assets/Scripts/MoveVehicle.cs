using UnityEngine;
using System.Collections;

public class MoveVehicle : MonoBehaviour {

	public float accZ = 6.0f;
	public float maxSpeedForward = 100.0f;
	public float turnSpeed = 1.0f;
	public float hoovingHeight = 2.0f;
	public float hoovingForce = 10.0f;

	private bool previousStateGoForward = true;
	private float speedZ = 0.0f;
	private float actRotation = 0.0f;
	private Rigidbody vehicleRigidBody;

	void Awake() {
		vehicleRigidBody = GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void Update () {
		//Movement
		if (Input.GetKey (KeyCode.UpArrow)) {
			if (!previousStateGoForward) {
				previousStateGoForward = true;
				changeAccelerationDirection ();
			}
			speedZ += accZ * Time.deltaTime;
			speedZ = Mathf.Min (speedZ,maxSpeedForward);
		} else if (Input.GetKey (KeyCode.DownArrow)) {
			if (previousStateGoForward) {
				previousStateGoForward = false;
				changeAccelerationDirection ();
			}
			speedZ += accZ* Time.deltaTime;
			speedZ = Mathf.Max (speedZ,-maxSpeedForward);
		}else { 
			speedZ -= accZ * Time.deltaTime; //stop movement(go to opposite direction)
			if (!previousStateGoForward) {
				speedZ = Mathf.Min (speedZ, 0.0f);
			} else {
				speedZ = Mathf.Max (speedZ, 0.0f);
			}
		}
			
		//gameObject.transform.Translate(0.0f, 0.0f, speedZ*Time.deltaTime + (1/2)*accZ*Time.deltaTime*Time.deltaTime,Space.Self);

		//Tiling
		/*float turn = Input.GetAxis("Horizontal");
		gameObject.transform.Rotate(0.0f, -actRotation,0.0f,Space.Self);
		actRotation += turn*turnSpeed;
		gameObject.transform.Rotate(0.0f, actRotation,0.0f,Space.Self);*/
	}
		

	void changeAccelerationDirection() {
		speedZ = -speedZ;
		accZ *= -1;
	}

	void FixedUpdate() {
		//vehicleRigidBody.AddRelativeForce (new Vector3 (0.0f, 0.0f, speedZ), ForceMode.VelocityChange);
		vehicleRigidBody.AddRelativeForce(new Vector3 (0.0f, 0.0f, speedZ),ForceMode.Force);
		//vehicleRigidBody.velocity = new Vector3 (0.0f, 0.0f, speedZ);
		float turn = Input.GetAxis("Horizontal");
		vehicleRigidBody.AddRelativeTorque (new Vector3(0.0f,turn*turnSpeed,0.0f));

		Vector3 size = GetComponent<BoxCollider> ().bounds.size;
		//Hooving physics
		Ray ray = new Ray (transform.position + new Vector3(0, 0, size.z/2) , -transform.up);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, hoovingHeight)) {
			float relativeHeight =(hoovingHeight- hit.distance) / hoovingHeight;
			Vector3 forceToApply =transform.up* relativeHeight * hoovingForce;
			vehicleRigidBody.AddRelativeForce (forceToApply, ForceMode.Acceleration);

			Vector3 normal = Vector3.Normalize (hit.normal);
			Debug.Log ("normal" + normal);
			transform.up = normal;
			Debug.Log ("up" + transform.up);
			//transform.up= normal;
			/*if (hit.distance <=hoovingHeight) {//when is higher than the hooving height, not apply hooving force
				Vector3 forceToApply = new Vector3(transform.up.x *Physics.gravity.x,
				transform.up.y *Physics.gravity.y,transform.up.z *Physics.gravity.z)*-2*Time.deltaTime;
				//vehicleRigidBody.AddForce (forceToApply, ForceMode.VelocityChange);
				vehicleRigidBody.AddRelativeForce (forceToApply, ForceMode.VelocityChange);
			}*/
		}
	}
		
}

