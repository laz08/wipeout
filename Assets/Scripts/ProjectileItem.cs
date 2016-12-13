using UnityEngine;
using System.Collections;

public class ProjectileItem : MonoBehaviour {

	public ParticleSystem explosion;
	private float speed = 200.0f;

	private GameObject vehicle;//Vehicle that has thrown this missile

	// Update is called once per frame
	void Update () {
		//Move towards
		float vSpeed = vehicle.GetComponent<MoveVehicle>().getSpeed();
		transform.Translate (0.0f,0.0f,(vSpeed*2+ speed)*Time.deltaTime,Space.Self);

		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit,4.0f) && hit.collider.tag != "PowerUpItem" && hit.collider.tag != "VelocityZone") {
			autoDestroy ();
		}
	}

	public void setVehicle(GameObject v) {
		vehicle = v;
	}

	void OnCollisionEnter() {
		autoDestroy ();
	}

	void autoDestroy() {
		//Auto destroy with explosion effect
		Instantiate (explosion,transform.position,transform.rotation);
		explosion.Play ();
		Destroy (gameObject);
	}
}
