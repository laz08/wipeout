using UnityEngine;
using System.Collections;

public class ItemVehicle : MonoBehaviour {

    enum Items {
           NONE, TURBO, PROJECTILE, BOMB, SHIELD
    };

    Items actualItem;

	public GameObject projectile;
	public GameObject bomb;
	public GameObject shield;

	private Texture projectileText;
	private Texture bombText;
	private Texture shieldText;
	private Texture turboText;

	public bool isPlayer = true;

	public float turboTime = 1.0f;
	public float turboSpeed = 75.0f;
	private float turboCountDown = 0.0f;
	private bool turboActivate = false;

	public float shieldTime = 6.0f;
	public float shieldCountDown = 0.0f;
	private bool shieldDestroyed = true;
	GameObject shieldInstance;

	Vector3 boxSelf,boxProjectile;

	// Use this for initialization
	void Start () {
		projectileText = (Texture)Resources.Load ("missileObj");
		bombText = (Texture)Resources.Load ("bombObj");
		shieldText = (Texture)Resources.Load ("shieldObj");
		turboText = (Texture)Resources.Load ("turboObj");

        actualItem = Items.NONE;
		boxSelf = GetComponent<BoxCollider>().size;
		boxProjectile = projectile.GetComponent<BoxCollider> ().size;
	}

	void OnGUI() { //Show actual item
		if (!isPlayer)
			return;
		
		const int textXpos = 40; const int textYpos = 65;
		const int textXsize = 80; const int textYsize = 80;

		switch (actualItem) {
		case Items.TURBO:
			GUI.DrawTexture(new Rect(textXpos, textYpos, textXsize, textYsize), turboText, ScaleMode.StretchToFill, true, 10.0F);
			break;
		case Items.PROJECTILE:
			GUI.DrawTexture(new Rect(textXpos, textYpos, textXsize, textYsize), projectileText, ScaleMode.StretchToFill, true, 10.0F);
			break;
		case Items.BOMB:
			GUI.DrawTexture(new Rect(textXpos, textYpos, textXsize, textYsize), bombText, ScaleMode.StretchToFill, true, 10.0F);
			break;
		case Items.SHIELD:
			GUI.DrawTexture(new Rect(textXpos, textYpos, textXsize, textYsize), shieldText, ScaleMode.StretchToFill, true, 10.0F);
			break;
		default:
			break;
		}


	}

	// Update is called once per frame
	void Update () {

		if (GetComponent<MoveVehicle> ().timeDamagedCountdown > 0.0f)
			return;

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

		if (!shieldDestroyed) {
			if (shieldCountDown < 0.0f) {
				Destroy (shieldInstance);
				shieldDestroyed = true;
			} else {
				shieldCountDown -= Time.deltaTime;
                transform.Translate(0.0f, 0.0f, turboSpeed * Time.deltaTime/10.0f, Space.Self);//Add small turbo
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
		}
		if (actualItem == Items.PROJECTILE) {
			Vector3 offset = new Vector3 (0.0f,0.0f,boxSelf.z*2 + boxProjectile.z);
			offset = transform.TransformVector (offset);
			GameObject misil = (GameObject)Instantiate(projectile,transform.position + offset,transform.rotation);
			misil.GetComponent<ProjectileItem>().setVehicle (gameObject);
			//Increase missil size on second track
			if (GetComponent<MoveVehicle> ().mWaypointsFactory as CreateSecondTRackWayPoints) {
				misil.transform.localScale = new Vector3 (10.0f, 10.0f, 10.0f);
			}
			Physics.IgnoreCollision (misil.GetComponent<BoxCollider> (), GetComponent<BoxCollider> ());
		}
		if (actualItem == Items.BOMB) {
			Vector3 offset = new Vector3 (0.0f,0.0f,boxSelf.z/2 + boxProjectile.z);
			offset = transform.TransformVector (offset);
			GameObject b = (GameObject)Instantiate(bomb,transform.position - offset,transform.rotation);
			//Increase bomb size on second track
			if (GetComponent<MoveVehicle> ().mWaypointsFactory as CreateSecondTRackWayPoints) {
				b.transform.localScale = new Vector3 (40.0f, 40.0f, 40.0f);
			}
		}
		if (actualItem == Items.SHIELD) {
			if (!shieldDestroyed)
				Destroy (shieldInstance);
			shieldInstance = (GameObject)Instantiate (shield, transform.position, transform.rotation);
			shieldInstance.transform.SetParent (transform);
			float ScaleValue = 1.3f*Mathf.Max (Mathf.Max (boxSelf.x, boxSelf.y), boxSelf.z);
			shieldInstance.transform.localScale = new Vector3 (ScaleValue, ScaleValue, ScaleValue);
			Color itemColor = shieldInstance.GetComponent<MeshRenderer>().material.color;
			itemColor.a = 0.4f;
			shieldInstance.GetComponent<MeshRenderer>().material.color = itemColor;
			shieldCountDown = shieldTime;
			shieldDestroyed = false;
		}
		actualItem = Items.NONE;
		if (isPlayer) itemActivatedEffect();//Set to NONE
	}

    void OnCollisionEnter(Collision collision) {
        if (actualItem == Items.NONE && collision.gameObject.tag == "PowerUpItem") {
			int item = Random.Range (0, 4);
			switch (item) {
			case 0:
				actualItem = Items.TURBO;
				break;
			case 1:
				actualItem = Items.PROJECTILE;
				break;
			case 2:
				actualItem = Items.BOMB;
				break;
			case 3:
				actualItem = Items.SHIELD;
				break;
			default: 
				break;
			}

			if (isPlayer) itemActivatedEffect();
        }
    }

	//GUI effects

	//show actual item (actived) TODO
	void itemActivatedEffect() {

	}

}
