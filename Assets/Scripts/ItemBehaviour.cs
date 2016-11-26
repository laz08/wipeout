using UnityEngine;
using System.Collections;

public class ItemBehaviour : MonoBehaviour {

	private float timeToRespawn = 5.0f;
	private float countdownToRespawn = 0.0f;

	// Update is called once per frame
	void Update () {
		if (countdownToRespawn > 0.0f) {
			countdownToRespawn -= Time.deltaTime;
			//Start showing the object through transparency
			if (countdownToRespawn < timeToRespawn / 2.0f) {
				Color itemColor = gameObject.GetComponent<MeshRenderer>().material.color;
				itemColor.a += 0.007f;//Should be nice to put this as a function of timeToRespawn
				gameObject.GetComponent<MeshRenderer>().material.color = itemColor;
			}
		}
		else {
			countdownToRespawn = 0.0f;
			gameObject.GetComponent<Collider>().enabled = true;
		}
		transform.Rotate(0.0f, 50*Time.deltaTime, 0.0f);
	}

	void OnCollisionEnter(Collision collision){
		countdownToRespawn = timeToRespawn;
		Color itemColor = gameObject.GetComponent<MeshRenderer>().material.color;
		itemColor.a = 0.0f;
		gameObject.GetComponent<MeshRenderer> ().material.color = itemColor;
		gameObject.GetComponent<Collider>().enabled = false;
	}

}
