using UnityEngine;
using System.Collections;

public class ProjectileItem : MonoBehaviour {

	public ParticleSystem explosion;
	public float speed = 60.0f;

	// Update is called once per frame
	void Update () {
		//Move towards
		transform.Translate (0.0f,0.0f,speed*Time.deltaTime,Space.Self);
	}

	void OnCollisionEnter() {
		//Auto destroy with explosion effect
		Instantiate (explosion,transform.position,transform.rotation);
		explosion.Play ();
		Destroy (gameObject);
	}
		
}
