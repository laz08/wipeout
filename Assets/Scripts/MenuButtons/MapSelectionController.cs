using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;

public class MapSelectionController : MonoBehaviour {

	public GameObject normalTrack;
	public GameObject torusTrack;

	private static string FIRST_TRACK_NAME = "FirstTrack";
	private static string SECOND_TRACK_NAME = "TorusTrack";

	private static string VEHICLE_SELECT_SCREEN_NAME = "VehicleSelectScreen";

	public float rotationSpeed = 75;


	void Awake() {
		
	}

	void Update(){

		normalTrack.transform.Rotate (Vector3.up * Time.deltaTime * rotationSpeed, Space.Self);
		torusTrack.transform.Rotate (Vector3.up * Time.deltaTime * -rotationSpeed, Space.Self);
	}

	public void onNormalSelected(){

		SceneManager.LoadScene (FIRST_TRACK_NAME);
	}

	public void onTorusSelected(){


		SceneManager.LoadScene (SECOND_TRACK_NAME);
	}

	public void onBackSelected(){

		SceneManager.LoadScene(VEHICLE_SELECT_SCREEN_NAME);
	}
}
