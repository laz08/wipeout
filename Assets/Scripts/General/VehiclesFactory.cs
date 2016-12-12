using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class VehiclesFactory : MonoBehaviour {

	public BaseCreateTrackWaypoints wayPointFactory;
	public PlayerLapsController sceneLapsController;
	public VehicleType selectedVehicle = VehicleType.Kirby;
	public int opponents = 0;
	public bool isTorusTrack = false;

	//private GameObject[] vehiclesList;
	private List<GameObject> vehiclesList;

	private static string KIRBY_VEHICLE = "Kirby";
	private static string CORVETTE_VEHICLE = "Corvette";
	private static string BOAT_VEHICLE = "Boat";

	private static string SELECTED_VEHICLE_KEY = "selectedVehicle";

	void Awake(){


		string paramVeh = AssemblyCSharp.SceneController.getParam (SELECTED_VEHICLE_KEY);
		Debug.Log ("Param veh: " + paramVeh);
		if (paramVeh != null) {

			if (paramVeh.Equals (CORVETTE_VEHICLE)) {

				selectedVehicle = VehicleType.Corvette;
			} else if(paramVeh.Equals (BOAT_VEHICLE)){

				selectedVehicle = VehicleType.JusticeBoat;
			} else {
				
				selectedVehicle = VehicleType.Kirby;
			}

		} 

		vehiclesList = new List<GameObject>();
		instantiateAllVehicles (0);
	}

	void Update() {
		//Sort from smaller to bigger
		vehiclesList.Sort(delegate(GameObject a, GameObject b) {
			return (a.GetComponent<MoveVehicle>().actualPosition).CompareTo(b.GetComponent<MoveVehicle>().actualPosition);
		});

		//Set position
		for (int i = 0; i < vehiclesList.Count; ++i) {
			vehiclesList [i].GetComponent<MoveVehicle> ().position = vehiclesList.Count - i;
		}
	}

	private  void instantiateAllVehicles(int playerPosition){

		float offsetZAxis = -15.0f;
		for (int i = 0; i < opponents + 1; i++) { //+1 Because we're placing our fav. vehicle here. That is, the player :) 

			bool isPlayer;
			VehicleType type;

			float offsetXAxis = 10.0f;
			if (i % 2 == 0) {
				//Offset on left
				offsetXAxis = -offsetXAxis;
			} 


			if (i == playerPosition) {
		
				isPlayer = true;
				type = selectedVehicle;
			} else {

				isPlayer = false;

				//Get random vehicle type.
				float randValue = Random.value;

				if (randValue < 0.3f) {

					type = VehicleType.Kirby;
				} else if(randValue >= 0.3f && randValue < 0.6f){

					type = VehicleType.JusticeBoat;
				} else {

					type = VehicleType.Corvette;
				}
			}

			//TODO: put different initial position on the second track!
			instantiateVehicle (type, isPlayer, new Vector3 (offsetXAxis, 0, i * offsetZAxis));
		}

		eliminateVehiclesCollision ();
	}

	private void instantiateVehicle(VehicleType type, bool isPlayer, Vector3 offset){
	

		GameObject obj;
		switch(type){

			case VehicleType.Corvette:

				wayPointFactory.getWaypoint (0);

				obj = (GameObject)Instantiate (Resources.Load("Corvette"));
			break;

		case VehicleType.JusticeBoat:

			obj = (GameObject)Instantiate (Resources.Load ("JusticeBoat"));
			break;
			default:
			case VehicleType.Kirby:

				obj = (GameObject)Instantiate (Resources.Load("KirbyVehicle"));
			break;
		}
			
		obj.GetComponent<MoveVehicle> ().isPlayerVehicle = isPlayer;
		obj.GetComponent<ItemVehicle> ().isPlayer = isPlayer;

		obj.GetComponent<MoveVehicle> ().mWaypointsFactory = wayPointFactory;
		obj.GetComponent<MoveVehicle> ().lapsController = sceneLapsController;
		obj.GetComponent<MoveVehicle> ().offsetStartPosition = offset;

		if (isTorusTrack) { //Special track done by someone special
			//Change some values in order to have better gameplay at this track
			obj.GetComponent<MoveVehicle> ().xAxisSpeed *= 10;
			obj.GetComponent<MoveVehicle> ().accZ *= 2;
			obj.GetComponent<MoveVehicle> ().maxSpeedForward *= 2;

			obj.GetComponent<BoxCollider> ().size += new Vector3 (2.0f, 9.0f, 0.0f);

			obj.GetComponent<VehicleForces> ().addForceAsVelocity = false;

			obj.GetComponent<ItemVehicle> ().turboSpeed = 150.0f;

		} else  {
			obj.GetComponent<Rigidbody> ().useGravity = true;

			if (type == VehicleType.Kirby) { //reescale kirby on first track
				obj.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
			}
		}

		if (isPlayer) {
		
			GameObject mainCamera = GameObject.Find ("Main Camera");
			mainCamera.transform.parent = obj.transform;
			mainCamera.GetComponent<CenterCameraOnVehicle> ().mUserVehicle = obj;
			mainCamera.GetComponent<CenterCameraOnVehicle> ().isSecondTrack = isTorusTrack; 
		}

		vehiclesList.Add (obj);
	}

	void eliminateVehiclesCollision () {
		for (int i = 0; i < vehiclesList.Count;++i) {
			for (int j = i + 1; j < vehiclesList.Count; ++j) {
				Physics.IgnoreCollision (vehiclesList [i].GetComponent<BoxCollider> (), vehiclesList [j].GetComponent<BoxCollider> ());
			}
		}

	}
}
