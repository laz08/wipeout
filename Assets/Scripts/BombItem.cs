using UnityEngine;
using System.Collections;

public class BombItem : MonoBehaviour {

	public ParticleSystem explosion;

	// Update is called once per frame
	void Update () {
		transform.Translate (0.0f, -Time.deltaTime, 0.0f);//Fall down
	}
		
	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Vehicle")
			autoDestroy (collision);
	}

	void autoDestroy(Collision collision) {
		
		//Auto destroy with explosion effect
		Instantiate (explosion,transform.position,transform.rotation);
		explosion.Play ();
		Destroy (gameObject);

	}
}
