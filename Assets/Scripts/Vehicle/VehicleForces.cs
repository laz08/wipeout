using UnityEngine;
using System.Collections;

public class VehicleForces : MonoBehaviour {

	private float hoverForce = 65f;
	private float hoverHeight = 3.5f;

	public bool addForceAsVelocity = true; //True for first track. False for torus.

	float gravity = -9.8f;
	Vector3 gravityDir = new Vector3(0.0f,1.0f,0.0f);
	Rigidbody vehicleRigidBody;
    BoxCollider boxC;

    Vector3 lastHitPosition;

	private Vector3 prevAngularVel;
	private Vector3 prevVel;


	void Awake() {
		vehicleRigidBody = GetComponent <Rigidbody>();
        boxC = GetComponent<BoxCollider>();
	}

	void FixedUpdate() {

		prevAngularVel = vehicleRigidBody.angularVelocity;
		prevVel = vehicleRigidBody.velocity;

		//Add gravity force

		if (addForceAsVelocity) {
			
			//vehicleRigidBody.AddForce (gravityDir * gravity*Time.deltaTime, ForceMode.VelocityChange);
		} else {
		
			vehicleRigidBody.AddForce (gravityDir * gravity*Time.deltaTime, ForceMode.Acceleration);
		}

		if (GetComponent<MoveVehicle> ().timeDamagedCountdown > 0.0f)
			return;

		Ray ray = new Ray (transform.position, -transform.up);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, hoverHeight)) {
            lastHitPosition = transform.position;
			float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
			Vector3 appliedHoverForce = hit.normal * proportionalHeight * hoverForce;
			//vehicleRigidBody.AddForce (appliedHoverForce, ForceMode.Acceleration);
			//Change gravity direction
			gravityDir = hit.normal;
			//Still need to improve this gravity change
			//Physics.gravity = hit.normal*gravity; //THIS CHANGES THE PYSHICS OF ALL THE SCENE!!!!
			//vehicleRigidBody.AddForce(hit.normal*gravity);
		}

		if (addForceAsVelocity) { //First track
			//Check not to go away the track
			Ray rayUpR = new Ray (transform.position + new Vector3 (boxC.size.x*(3/2), 0.0f, boxC.size.z), -transform.up);
			Ray rayUpL = new Ray (transform.position + new Vector3 (-boxC.size.x*(3/2), 0.0f, boxC.size.z), -transform.up);
			Ray rayDownR = new Ray (transform.position + new Vector3 (boxC.size.x*(3/2), 0.0f, -boxC.size.z), -transform.up);
			Ray rayDownL = new Ray (transform.position + new Vector3 (-boxC.size.x*(3/2), 0.0f, -boxC.size.z), -transform.up);
			RaycastHit hitUR, hitUL, hitDR, hitDL;

			if (!Physics.Raycast (rayUpR, out hitUR) || !Physics.Raycast (rayUpL, out hitUL) || !Physics.Raycast (rayDownR, out hitDR) || !Physics.Raycast (rayDownL, out hitDL)) {
				//Vector3 offset = new Vector3 ();
				Vector3 dirForce = (lastHitPosition - transform.position);
				//vehicleRigidBody.AddForce(-5*dirforce);
				transform.Translate (dirForce, Space.World);
			}
		}

	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "DamageItem" || collision.gameObject.tag == "Vehicle") { //Vehicle gets damaged
			vehicleRigidBody.angularVelocity = prevAngularVel;
			vehicleRigidBody.velocity = prevVel;
		}

	}

}
