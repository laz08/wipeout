using UnityEngine;
using System.Collections;

public class ItemVehicle : MonoBehaviour {

    enum Items {
           NONE, TURBO, PROJECTILE
    };

    Items actualItem;

	public float turboTime = 1.0f;
	public float turboSpeed = 150.0f;
	private float turboCountDown = 0.0f;
	private bool turboActivate = false;

	// Use this for initialization
	void Start () {
        actualItem = Items.NONE;
	}
	
	// Update is called once per frame
	void Update () {

		//Check if user activates actual item
	    if (actualItem != Items.NONE && (Input.GetKey ("e")) ) {
			itemActivatedEffect();
			if (actualItem == Items.TURBO) {
				turboActivate = true;
				turboCountDown = turboTime;
				actualItem = Items.NONE;
			}
			itemNotActivatedEffect();//Set to NONE
        }

		//Check if the vehicle is in a velocity ramp
		Ray ray = new Ray(transform.position, -transform.up);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			if (hit.collider.gameObject.tag == "VelocityZone") {
				turboActivate = true;
				turboCountDown = turboTime;
			}
		}

		//Apply turbo "force"
		if (turboActivate) {
			if (turboCountDown <= 0.0f) {
				turboActivate = false;
				itemActivatedEffect ();//Set to NONE
			} else {
				turboCountDown -= Time.deltaTime;
				transform.Translate (0.0f,0.0f,turboSpeed*Time.deltaTime,Space.Self);
				//TODO:add velocity visual effect
			}
		}
	}

    void OnCollisionEnter(Collision collision) {
        if (actualItem == Items.NONE && collision.gameObject.tag == "PowerUpItem") {
			int item = Random.Range (0, 2);
			Debug.Log ("random number: " + item);
			switch (item) {
			case 0:
				actualItem = Items.TURBO;
				break;
			case 1:
				actualItem = Items.PROJECTILE;
				break;
			default:
				break;
			}
			itemNotActivatedEffect();
        }
    }

	//GUI effects

	//show actual item (not activated) TODO
	void itemNotActivatedEffect() {
		switch (actualItem) {
		case Items.NONE:

			break;
		case Items.TURBO:

			break;
		case Items.PROJECTILE:

			break;
		default:
			break;
		}
	}

	//show actual item (actived) TODO
	void itemActivatedEffect() {
		switch (actualItem) {
		case Items.NONE:

			break;
		case Items.TURBO:

			break;
		case Items.PROJECTILE:

			break;
		default:
			break;
		}
	}

}
