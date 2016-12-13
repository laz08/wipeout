using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class VehicleSelectController : MonoBehaviour {

	public GameObject kirbyObj;
	public GameObject corvetteObj;
	public GameObject boatObj;

	private static string FIRST_TRACK_NAME = "TorusTrack";
	private static string GAME_MENU_NAME = "GameMenu";

	private static string SELECTED_VEHICLE_KEY = "selectedVehicle";

	private static string KIRBY_VEHICLE = "Kirby";
	private static string CORVETTE_VEHICLE = "Corvette";
	private static string BOAT_VEHICLE = "Boat";

	public float rotationSpeed = 75;

	void Update(){

		kirbyObj.transform.Rotate (Vector3.up * Time.deltaTime * rotationSpeed, Space.Self);
		corvetteObj.transform.Rotate (Vector3.up * Time.deltaTime * -rotationSpeed, Space.Self);
		boatObj.transform.Rotate (Vector3.up * Time.deltaTime * rotationSpeed, Space.Self);
	}

	public void onKirbySelected(){

		AssemblyCSharp.SceneController.Load (FIRST_TRACK_NAME, SELECTED_VEHICLE_KEY, KIRBY_VEHICLE);
	}

	public void onCorvetteSelected(){
	
		AssemblyCSharp.SceneController.Load (FIRST_TRACK_NAME, SELECTED_VEHICLE_KEY, CORVETTE_VEHICLE);
	}

	public void onBoatSelected(){
	
		AssemblyCSharp.SceneController.Load (FIRST_TRACK_NAME, SELECTED_VEHICLE_KEY, BOAT_VEHICLE);
	}

	public void onBackSelected(){

		SceneManager.LoadScene(GAME_MENU_NAME);
	}
}
