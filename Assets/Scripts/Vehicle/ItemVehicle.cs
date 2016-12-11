using UnityEngine;
using System.Collections;

public class ItemVehicle : MonoBehaviour {

    enum Items {
           NONE, TURBO, PROJECTILE
    };

    Items actualItem;

	public GameObject projectile;

	public bool isPlayer = true;

	public float turboTime = 1.0f;
	public float turboSpeed = 150.0f;
	private float turboCountDown = 0.0f;
	private bool turboActivate = false;

	Vector3 boxSelf,boxProjectile;

	// Use this for initialization
	void Start () {
        actualItem = Items.NONE;
		boxSelf = GetComponent<BoxCollider>().size;
		boxProjectile = projectile.GetComponent<BoxCollider> ().size;
	}
	
	// Update is called once per frame
	void Update () {

		if (actualItem != Items.NONE) {
			if (isPlayer) {
				//Check if user activates actual item
				if (Input.GetKey ("e")) {
					activateItem ();
				}
			} else {
				float shouldThrow = Random.value;
				if (shouldThrow < 0.15f) 
					activateItem();
			}
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

	void activateItem() { //Throws actual item
		if (actualItem == Items.TURBO) {
			turboActivate = true;
			turboCountDown = turboTime;
			actualItem = Items.NONE;
		}
		if (actualItem == Items.PROJECTILE) {
			Vector3 offset = new Vector3 (0.0f,0.0f,boxSelf.z + boxProjectile.z);
			offset = transform.TransformVector (offset);
			GameObject misil = (GameObject)Instantiate(projectile,transform.position + offset,transform.rotation);
			misil.GetComponent<ProjectileItem>().setVehicle (gameObject);
			actualItem = Items.NONE;
		}
		itemActivatedEffect();//Set to NONE
	}

    void OnCollisionEnter(Collision collision) {
        if (actualItem == Items.NONE && collision.gameObject.tag == "PowerUpItem") {
			//int item = Random.Range (0, 2);
			int item  = 1;
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
			itemActivatedEffect();
        }
    }

	//GUI effects

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
