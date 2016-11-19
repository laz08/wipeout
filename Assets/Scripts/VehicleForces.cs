using UnityEngine;
using System.Collections;

public class VehicleForces : MonoBehaviour {

	public float hoverForce = 65f;
	public float hoverHeight = 3.5f;

	float gravity = -9.8f;

	Rigidbody vehicleRigidBody;

	void Awake() {
		vehicleRigidBody = GetComponent <Rigidbody>();
	}

	void FixedUpdate() {

		Ray ray = new Ray (transform.position, -transform.up);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, hoverHeight)) {
			float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
			Vector3 appliedHoverForce = hit.normal * proportionalHeight * hoverForce;
			vehicleRigidBody.AddForce (appliedHoverForce, ForceMode.Acceleration);
			//Still need to improve this gravity change
			Physics.gravity = hit.normal*gravity;
		}
	}

}
