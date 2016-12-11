using UnityEngine;
using System.Collections;

public class VehiclesFactory : MonoBehaviour {

	public BaseCreateTrackWaypoints wayPointFactory;
	public PlayerLapsController sceneLapsController;
	public VehicleType selectedVehicle = VehicleType.Kirby;
	public int opponents = 0;
	public bool isTorusTrack = false;

	void Awake(){

		instantiateAllVehicles (0);
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
				if (randValue < 0.5f) {

					type = VehicleType.Kirby;
				} else {

					type = VehicleType.Corvette;
				}
			}

			instantiateVehicle (type, isPlayer, new Vector3 (offsetXAxis, 0, i * offsetZAxis));
		}
	}

	private void instantiateVehicle(VehicleType type, bool isPlayer, Vector3 offset){
	

		GameObject obj;
		switch(selectedVehicle){

			case VehicleType.Corvette:

				wayPointFactory.getWaypoint (0);

				obj = (GameObject)Instantiate (Resources.Load("Corvette"));
			break;

			default:
			case VehicleType.Kirby:

				obj = (GameObject)Instantiate (Resources.Load("KirbyVehicle"));
			break;
		}
			
		obj.GetComponent<MoveVehicle> ().isPlayerVehicle = isPlayer;

		obj.GetComponent<MoveVehicle> ().mWaypointsFactory = wayPointFactory;
		obj.GetComponent<MoveVehicle> ().lapsController = sceneLapsController;
		obj.GetComponent<MoveVehicle> ().offsetStartPosition = offset;

		if (isPlayer) {
		
			GameObject mainCamera = GameObject.Find ("Main Camera");
			mainCamera.transform.parent = obj.transform;
			mainCamera.GetComponent<CenterCameraOnVehicle> ().mUserVehicle = obj;
			mainCamera.GetComponent<CenterCameraOnVehicle> ().isSecondTrack = isTorusTrack; 
		}
	}


}
