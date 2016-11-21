using UnityEngine;
using System.Collections;

public class VehicleForces : MonoBehaviour {

	public float hoverForce = 65f;
	public float hoverHeight = 3.5f;

	float gravity = -9.8f;

	Rigidbody vehicleRigidBody;
    BoxCollider boxC;

    Vector3 lastHitPosition;
	void Awake() {
		vehicleRigidBody = GetComponent <Rigidbody>();
        boxC = GetComponent<BoxCollider>();
	}

	void FixedUpdate() {

		Ray ray = new Ray (transform.position, -transform.up);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, hoverHeight)) {
            lastHitPosition = transform.position;
			float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
			Vector3 appliedHoverForce = hit.normal * proportionalHeight * hoverForce;
			vehicleRigidBody.AddForce (appliedHoverForce, ForceMode.Acceleration);
			//Still need to improve this gravity change
			Physics.gravity = hit.normal*gravity;
		}

        //Maybe do more than one rayast per position(ex, no tjust at the center, to it to at the vehicle top and bottom)
        Ray rayLeft = new Ray(transform.position - new Vector3(boxC.size.x/2.0f, 0.0f), -transform.up);
        Ray rayRight = new Ray(transform.position + new Vector3( boxC.size.x/2.0f, 0.0f, 0.0f), -transform.up);

        if (!Physics.Raycast(rayLeft, out hit) || !Physics.Raycast(rayRight, out hit)) {
            Vector3 dirForce = (lastHitPosition-transform.position)*2;
            //vehicleRigidBody.AddForce(-5*dirforce);
            transform.Translate(dirForce, Space.World);
        }
	}

}
