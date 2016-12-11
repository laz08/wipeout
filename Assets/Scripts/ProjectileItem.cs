using UnityEngine;
using System.Collections;

public class ProjectileItem : MonoBehaviour {

	public ParticleSystem explosion;
	public float speed = 120.0f;

	// Update is called once per frame
	void Update () {
		//Move towards
		transform.Translate (0.0f,0.0f,speed*Time.deltaTime,Space.Self);

		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit,4.0f) && hit.collider.tag != "PowerUpItem") {
			autoDestroy ();
		}
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
