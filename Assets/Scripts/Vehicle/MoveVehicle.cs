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
	public bool isPlayerVehicle = false;

	public Vector3 offsetStartPosition = new Vector3 (0, 0, 0);

	private float speedZ = 0.0f;

	private float lapsCountDown = 0.0f;//In order to control laps countdown
	private float lapTime = 10.0f;//Minimum seconds to complete a lap?
	public int lapsDone = 0;
	public int currentWayPoint;
	public float actualPosition = 0.0f; //Position "score" depending on the waypoints and laps
	public int position = 0;//Position of the vehicle (first,second,..)

	private float timeDamaged = 3.0f;//Time damage animation takes
	public float timeDamagedCountdown = 0.0f;

    //For IA
    private float turnCountdown = 0.0f;
    private float turnTime = 1.2f;

    //Race beggining
    public float waitingStartTime = 4.2f;
    private Texture countDown3;
    private Texture countDown2;
    private Texture countDown1;
    private Texture countDownGo;
	private bool playedStart = false; //For controlling countdown sound

	//Race end 
	private bool isEnd = false;
	private Texture winText;
	private Texture looseText;
    private Texture pressSpace;
    private Texture loading;
	private bool ending = false;
	private bool firstPosition = false; //Has the player win?

    void Start()
    {
		applyDirToVehicle ();
		currentWayPoint = -1;
		transform.position = mWaypointsFactory.getWaypoint (0)
			+ /*transform.TransformDirection */(offsetStartPosition); //WHY TRANSFORM??? 
		if (isPlayerVehicle) { //Load textures
			winText = (Texture)Resources.Load ("YouWin");
			looseText = (Texture)Resources.Load ("YouLose");
            countDown3 = (Texture)Resources.Load("3");
            countDown2 = (Texture)Resources.Load("2");
            countDown1 = (Texture)Resources.Load("1");
            countDownGo = (Texture)Resources.Load("GO");
            pressSpace = (Texture)Resources.Load("press_spacebar");
            loading = (Texture)Resources.Load("loading");
		}
        turnTime = Random.value + Random.value + Random.value;
        Debug.Log(turnTime);
    }

	// Update is called once per frame
	void Update () {
        if (waitingStartTime > 0) {
            applyDirToVehicle();
            changeUpDirection();
            waitingStartTime -= Time.deltaTime;
			if (isPlayerVehicle) lapsController.setPositionText (position);
            return;
        }

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

	private bool checkFirstWayPoints(int closestWayPoint) {
		bool result = false;
		for (int i = 0; i < 6 && !result; ++i) {
			result = result || (closestWayPoint == i && currentWayPoint != i);
		}
		return result;
	}

	private void checkHasLapBeenDone(){

		int closestWaypoint = mWaypointsFactory.getCurrentWaypointIndex (gameObject.transform.position);
		if (lapsCountDown <= 0.0f && checkFirstWayPoints(closestWaypoint)) {
			lapsCountDown = lapTime;
			++lapsDone;
			if (isPlayerVehicle) {

				lapsController.setLapsDone (lapsDone);

				if (lapsDone == lapsController.maxLaps+ 1) {
					isEnd = true;
					isPlayerVehicle = false; //Let the IA controll the vehicle once ended
					firstPosition = position == 1;
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
        turnCountdown -= Time.deltaTime;
        if (turnCountdown > 0.0f) 
            return 0.0f;

        turnCountdown = turnTime;
		float shouldTurn = Random.value;
		if (shouldTurn <= 0.5f) {

			return 0;
		}
		if(shouldTurn > 0.65f && shouldTurn < 0.80f) {

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
				transform.rotation = Quaternion.Slerp (transform.rotation, aux.rotation, Time.deltaTime * 15);

			Destroy (tempGameObject);

        }
    }

	public float getSpeed(){
		return speedZ;
	}


	void OnGUI() {
		float Textwidth = (Screen.width / 2);
		float Textheight =  (Screen.height / 2);
        //For race beggining
		if (isPlayerVehicle && waitingStartTime > 0.0f) {

			if (!playedStart) {
				AudioSource audio = gameObject.AddComponent < AudioSource > ();
				audio.PlayOneShot ((AudioClip)Resources.Load ("start"));
				playedStart = true;
			}
			//Textures centered on screen
			if (waitingStartTime >= 2.5f) {
                GUI.DrawTexture(new Rect((Screen.width / 2) - (Textwidth / 2), (Screen.height / 2) - (Textheight / 2), Textwidth, Textheight), countDown3, ScaleMode.ScaleToFit);
			} else if (waitingStartTime >= 1.5f) {
                GUI.DrawTexture(new Rect((Screen.width / 2) - (Textwidth / 2), (Screen.height / 2) - (Textheight / 2), Textwidth, Textheight), countDown2, ScaleMode.ScaleToFit);
			} else if (waitingStartTime >= 0.5f) {
                GUI.DrawTexture(new Rect((Screen.width / 2) - (Textwidth / 2), (Screen.height / 2) - (Textheight / 2), Textwidth, Textheight), countDown1, ScaleMode.ScaleToFit);
			} else {
                GUI.DrawTexture(new Rect((Screen.width / 2) - (Textwidth / 2), (Screen.height / 2) - (Textheight / 2), Textwidth, Textheight), countDownGo, ScaleMode.ScaleToFit);
			}
		}

        //For race ending
		if (isEnd) {
			if (!ending) {
				ending = true;
				//stop all sounds
				AudioSource[] audios = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
				foreach (AudioSource aud in audios)
					aud.Stop ();

				AudioSource audio = gameObject.AddComponent < AudioSource > ();
				//Winner/looser sound
				if (firstPosition) 
					audio.PlayOneShot ((AudioClip)Resources.Load ("fanfare"));
				else
					audio.PlayOneShot ((AudioClip)Resources.Load ("defeat"));
			}

			if (Input.GetKey (KeyCode.Space)) {//Return to main menu
                GUI.DrawTexture(new Rect((Screen.width) - (Textwidth / 4), (Screen.height) - (Textheight / 4), Textwidth / 4, Textheight / 4), loading, ScaleMode.ScaleToFit, true, 10.0F);
                SceneManager.LoadScene("GameMenu");
			}

            GUI.DrawTexture(new Rect((Screen.width / 2) - (Textwidth / 4), (Screen.height) - (Textheight / 4), Textwidth / 4, Textheight / 3), pressSpace, ScaleMode.ScaleToFit, true, 10.0F);

			if (firstPosition)
                GUI.DrawTexture(new Rect((Screen.width / 2) - (Textwidth / 2), (Screen.height / 2) - (Textheight / 2), Textwidth, Textheight), winText, ScaleMode.ScaleToFit);
            else
                GUI.DrawTexture(new Rect((Screen.width / 2) - (Textwidth / 2), (Screen.height / 2) - (Textheight / 2), Textwidth, Textheight), looseText, ScaleMode.ScaleToFit);
        }
	}
}
