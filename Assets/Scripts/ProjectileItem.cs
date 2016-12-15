using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class ProjectileItem : MonoBehaviour {

	public ParticleSystem explosion;
	private float speed = 200.0f;

	private GameObject vehicle;//Vehicle that has thrown this missile

	AudioSource audioExpl;
	AudioClip clipExpl;
	float countDownToDestroy = 2.0f;
	bool willBeDestroyed = false;

	void Start() {
		audioExpl = gameObject.AddComponent<AudioSource> ();
		clipExpl = (AudioClip)Resources.Load ("Sounds/explosion");
	}

	// Update is called once per frame
	void Update () {
		if (willBeDestroyed) {
			if (countDownToDestroy <= 0.0f)
				Destroy (gameObject);
			else
				countDownToDestroy -= Time.deltaTime;
		}

		//Move towards
		float vSpeed = vehicle.GetComponent<MoveVehicle>().getSpeed();
		transform.Translate (0.0f,0.0f,(vSpeed*2+ speed)*Time.deltaTime,Space.Self);

		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit,(vSpeed*2+ speed)*Time.deltaTime) && hit.collider.tag != "PowerUpItem") {
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
		if (vehicle.GetComponent<MoveVehicle> ().isPlayerVehicle && !willBeDestroyed) {
			GetComponent<BoxCollider> ().enabled = false;
			GetComponent<MeshRenderer> ().enabled = false;
			countDownToDestroy = 2.0f;
			willBeDestroyed = true;
			audioExpl.PlayOneShot (clipExpl);
		}
		else
			Destroy (gameObject);
	}
}
