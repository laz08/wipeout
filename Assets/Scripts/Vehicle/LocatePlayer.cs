using UnityEngine;
using System.Collections;

public class LocatePlayer : MonoBehaviour {

    public GameObject lightPilar;
    private GameObject lightInstance;

	// Use this for initialization
	void Start () {
        if (GetComponent<MoveVehicle>() == null) return;
		if (GetComponent<MoveVehicle>() != null && !GetComponent<MoveVehicle>().isPlayerVehicle) return;
        Vector3 boxSize = GetComponent<BoxCollider>().size;
        lightInstance = (GameObject)Instantiate(lightPilar,transform.position, transform.rotation);
        lightInstance.transform.parent = transform;
        lightInstance.transform.localScale = new Vector3(boxSize.x, 99999.0f, boxSize.z);
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<MoveVehicle> () == null)
			return;
        float startTime = GetComponent<MoveVehicle>().waitingStartTime;
        if (startTime <= 0.0f) Destroy(lightInstance);
	}
}
