using UnityEngine;
using System.Collections;

public class ItemVehicle : MonoBehaviour {

    enum Items {
           NONE, TURBO
    };

    Items actualItem;

	public float turboTime = 2.0f;
	private float turboCountDown;

	// Use this for initialization
	void Start () {
        actualItem = Items.NONE;
	}
	
	// Update is called once per frame
	void Update () {
	    if (actualItem!=Items.NONE && (Input.GetKey ("a")) ) {
			if (actualItem == Items.TURBO) {
				Debug.Log ("trhworing item");
				//http://answers.unity3d.com/questions/400977/changing-a-variable-in-one-script-using-another-sc.html
				//GetComponent<MoveVehicle> (MoveVehicle);
				actualItem = Items.NONE;
			}
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        if (actualItem == Items.NONE && collision.gameObject.tag == "PowerUpItem")
        {
            actualItem = Items.TURBO;
			turboCountDown = turboTime;
        }
    }
}
