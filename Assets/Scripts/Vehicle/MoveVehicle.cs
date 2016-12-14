using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MoveVehicle : MonoBehaviour {

	//Config.
	public float accZ = 6.0f;
	public float maxSpeedForward = 100.0f;

	public PlayerLapsController lapsController;
	public BaseCreateTrackWaypoints mWaypointsFactory;

	//Movement in X Axis.
	public float xAxisSpeed = 15.0f;
	public bool isPlayerVehicle = true;

	public Vector3 offsetStartPosition = new Vector3 (0, 0, 0);

	private float speedZ = 0.0f;

	private float lapsCountDown = 0.0f;//In order to control laps countdown
	private float lapTime = 20.0f;//Minimum seconds to complete a lap?
	public int lapsDone = 0;
	public int currentWayPoint;
	public float actualPosition = 0.0f; //Position "score" depending on the waypoints and laps
	public int position = 0;//Position of the vehicle (first,second,..)

	private float timeDamaged = 3.0f;//Time damage animation takes
	public float timeDamagedCountdown = 0.0f;

    //Race beggining

	//Race end 
	private bool isEnd = false;
	private Texture winText;
	private Texture looseText;
	private bool ending = false;

    void Start()
    {
		applyDirToVehicle ();
		currentWayPoint = -1;
		transform.position = mWaypointsFactory.getWaypoint (0)
			+ /*transform.TransformDirection */(offsetStartPosition); //WHY TRANSFORM???
		if (isPlayerVehicle) { //Load win/loose textures
			winText = (Texture)Resources.Load ("missileObj");
			looseText = (Texture)Resources.Load ("shieldObj");
		}
    }

	// Update is called once per frame
	void Update () {
		if (lapsCountDown > 0)
			lapsCountDown -= Time.deltaTime;
		if (timeDamagedCountdown > 0.0f) { //Not move, damage animation
		
			timeDamagedCountdown -= Time.deltaTime;
			transform.Rotate (0.0f, 240.0f*Time.deltaTime, 0.0f);
		} else { //Can move
		
			applyDirToVehicle ();
			if (!isPlayerVehicle) {

				manageAutomaticMovement ();
			} else {

				manageManualMovement ();
			}
		}
		checkHasLapBeenDone ();
		changeUpDirection();

		actualPosition = lapsDone * 10000f + currentWayPoint;
		if (isPlayerVehicle) lapsController.setPositionText (position);
	}

	public int getLapsDone(){

		return lapsDone;
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "DamageItem" && GetComponent<ItemVehicle>().shieldCountDown <= 0.0f) { //Vehicle gets damaged
			timeDamagedCountdown = timeDamaged;
		}
	}

	/*
	 * Manages automatic movement
	 */
	private void manageAutomaticMovement(){

		float forwardValue = moveForwardAutomatic ();

		float xAxisValue = turnVehicleAutomatic ();

		gameObject.transform.position = gameObject.transform.position
			+ gameObject.transform.TransformDirection (new Vector3 (xAxisValue, 0.0f, forwardValue));

	}

	/**
	 * Manages manual movement
	 */
	private void manageManualMovement(){
		
		float forwardValue = moveForward();
		float xAxisValue = Input.GetAxis ("Horizontal") * xAxisSpeed * Time.deltaTime;

		gameObject.transform.position = gameObject.transform.position
			+ gameObject.transform.TransformDirection (new Vector3 (xAxisValue, 0.0f, forwardValue));
	}


	private void applyDirToVehicle(){
		
		Vector3 newForward = (mWaypointsFactory.getDir (
			gameObject.transform.position));


		if (mWaypointsFactory as CreateFirstTrackWaypoints) {
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (newForward), Time.deltaTime*20);
			//transform.rotation = Quaternion.LookRotation(newForward);

		}
		else if(mWaypointsFactory as CreateSecondTRackWayPoints) {
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (newForward), Time.deltaTime*0.5f);
			//transform.rotation = Quaternion.LookRotation(newForward);
		}
	}

	private void checkHasLapBeenDone(){

		int closestWaypoint = mWaypointsFactory.getCurrentWaypointIndex (gameObject.transform.position);
		if (closestWaypoint == 0 && currentWayPoint != 0 && lapsCountDown <= 0.0f) {
			lapsCountDown = lapTime;
			++lapsDone;
			if (isPlayerVehicle) {

				lapsController.setLapsDone (lapsDone);

				if (lapsDone == /*lapsController.maxLaps+ 1*/2) {
					isEnd = true;
					isPlayerVehicle = false; //Let the IA controll the vehicle once ended
					//ADD sleep??
					/*Dictionary<string,string> arguments = new Dictionary<string,string>();
					arguments.Add ("position", position.ToString());
					AssemblyCSharp.SceneController.Load (RESULT_SCENE,arguments);*/
				}
			}
		}
		if (currentWayPoint != closestWaypoint)
		currentWayPoint = closestWaypoint;

	}


	/**
	 * Moves vehicle forward.
	 */
	float moveForward(){
		
		if (Input.GetKey (KeyCode.UpArrow)) {

			augmentSpeed ();
		} else {
		
			decrementSpeed ();
		}
		return  speedZ * Time.deltaTime;
	}

	/**
	 * Moves vehicle forward.
	 */
	float moveForwardAutomatic(){

		float shouldAccelerate = Random.value;
		if (shouldAccelerate < 0.95f) {

			augmentSpeed ();
		} else {

			decrementSpeed ();
		}
		return  speedZ * Time.deltaTime;
	}

	/**
	 * Moves vehicle forward.
	 */
	float turnVehicleAutomatic(){

		float shouldTurn = Random.value;
		if (shouldTurn <= 0.6f) {

			return 0;
		}
		if(shouldTurn > 0.64f && shouldTurn < 0.65f) {

			return xAxisSpeed * Time.deltaTime;
		}
		return  -xAxisSpeed * Time.deltaTime;
	}


	private void augmentSpeed(){

		speedZ += accZ * Time.deltaTime;
		speedZ = Mathf.Min (speedZ,maxSpeedForward);
	}

	private void decrementSpeed(){

		speedZ -= accZ * Time.deltaTime;
		speedZ = Mathf.Max (speedZ, 0);
	}

	/**
	 *
	 *
	 *  Changes up direction.
	 *
	 */
	private void changeUpDirection()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {

			//Create new object in order to have a new transform to handle rotation as quaternions
			GameObject tempGameObject = new GameObject();
			Transform aux = tempGameObject.transform;
			//Rotation to go from actual position to hit result normal
			aux.rotation = Quaternion.LookRotation(hit.normal, -transform.forward);
			aux.Rotate (Vector3.right, 90f);
			//Apply an smooth rotation

			if (mWaypointsFactory as CreateFirstTrackWaypoints) 
				transform.rotation = Quaternion.Slerp (transform.rotation, aux.rotation, Time.deltaTime * 20);
			else if (mWaypointsFactory as CreateSecondTRackWayPoints)
				transform.rotation = Quaternion.Slerp (transform.rotation, aux.rotation, Time.deltaTime * 10);

			Destroy (tempGameObject);

        }
    }

	public float getSpeed(){
		return speedZ;
	}

	void OnGUI() {
		if (isEnd) {
			if (!ending) {
				ending = true;
				//stop all sounds
				AudioSource[] audios = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
				foreach (AudioSource aud in audios)
					aud.Stop ();

				AudioSource audio = gameObject.AddComponent < AudioSource > ();
				//Winner/looser sound
				if (position ==1) 
					audio.PlayOneShot ((AudioClip)Resources.Load ("fanfare"));

			}

			if (Input.GetKey (KeyCode.Space)) {//Return to main menu
				GUI.Label (new Rect (500, 100, 100, 100), "Loading . . .");
				SceneManager.LoadScene ("GameMenu");
			}

			const int textXpos = 300;
			const int textYpos = 300;
			const int textXsize = 200;
			const int textYsize = 200;

			GUI.Label (new Rect (textXpos, textYpos, textXsize, textYsize), "Press space to return nto menu");
			/*
			switch (position) {
			case 1:
				GUI.DrawTexture (new Rect (textXpos, textYpos, textXsize, textYsize), winText, ScaleMode.StretchToFill, true, 10.0F);
				break;
			default :
				GUI.DrawTexture (new Rect (textXpos, textYpos, textXsize, textYsize), looseText, ScaleMode.StretchToFill, true, 10.0F);
				break;
			}*/
		}
	}
}
