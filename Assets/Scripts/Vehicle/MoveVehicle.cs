using UnityEngine;
using System.Collections;

public class MoveVehicle : MonoBehaviour {

	//Config.
	public float accZ = 6.0f;
	public float maxSpeedForward = 100.0f;

	public PlayerLapsController lapsController;
	public BaseCreateTrackWaypoints mWaypointsFactory;

	//Movement in X Axis.
	public float xAxisSpeed = 15.0f;
	public bool isPlayerVehicle = true;

	private float speedZ = 0.0f;
	private int lapsDone = 0;
	private int currentWayPoint;

	private float timeDamaged = 3.0f;//Time damage animation takes
	private float timeDamagedCountdown = 0.0f;

    void Start()
    {
       
		currentWayPoint = 0;
		transform.position = mWaypointsFactory.getWaypoint (0);
    }

	// Update is called once per frame
	void Update () {

		if (timeDamagedCountdown > 0.0f) { //Not move, damage animation
			timeDamagedCountdown -= Time.deltaTime;
			transform.Rotate (0.0f, 500.0f*Time.deltaTime, 0.0f);
		} else { //Can move
			if (!isPlayerVehicle) {

				manageAutomaticMovement ();
			} else {

				manageManualMovement ();
			}
		}
		checkHasLapBeenDone ();
		changeUpDirection();
	}


	public int getLapsDone(){

		return lapsDone;
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Missile") { //Vehicle gets damaged
			timeDamagedCountdown = timeDamaged;
		}
	}

	/*
	 * Manages automatic movement
	 */
	private void manageAutomaticMovement(){

		applyDirToVehicle ();
		float forwardValue = moveForwardAutomatic ();

		float xAxisValue = turnVehicleAutomatic ();

		gameObject.transform.position = gameObject.transform.position
			+ gameObject.transform.TransformDirection (new Vector3 (xAxisValue, 0.0f, forwardValue));

	}

	/**
	 * Manages manual movement
	 */
	private void manageManualMovement(){

		applyDirToVehicle ();

		float forwardValue = moveForward();
		float xAxisValue = Input.GetAxis ("Horizontal") * xAxisSpeed * Time.deltaTime;

		gameObject.transform.position = gameObject.transform.position
			+ gameObject.transform.TransformDirection (new Vector3 (xAxisValue, 0.0f, forwardValue));
	}


	private void applyDirToVehicle(){
		
		Vector3 newForward = (mWaypointsFactory.getDir (
			gameObject.transform.position));


		if (mWaypointsFactory as CreateFirstTrackWaypoints) {
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (newForward), 0.2f);
		}
		else if(mWaypointsFactory as CreateSecondTRackWayPoints) {
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (newForward), Time.deltaTime);
			//transform.rotation = Quaternion.LookRotation(newForward);
		}
	}

	private void checkHasLapBeenDone(){

		int closestWaypoint = mWaypointsFactory.getCurrentWaypointIndex (gameObject.transform.position);
		if (closestWaypoint == 0 && currentWayPoint != 0) {

			++lapsDone;
			if (isPlayerVehicle) {

				lapsController.setLapsDone (lapsDone);
			}
		}
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
		if (shouldAccelerate < 0.8f) {

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
		if(shouldTurn > 0.6f && shouldTurn < 0.65f) {

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
            /*Vector3 angleIncr = hit.normal - transform.up;
            //Vector3.Angle(hit.normal,transform.up)
            Debug.Log(hit.normal);
            transform.up = Vector3.Lerp(transform.up,hit.normal,Time.deltaTime*5.0f);
            */


            //http://answers.unity3d.com/questions/351899/rotation-lerp.html


			//Create new object in order to have a new transform to handle rotation as quaternions
			GameObject tempGameObject = new GameObject();
			Transform aux = tempGameObject.transform;
			//Rotation to go from actual position to hit result normal
			aux.rotation = Quaternion.LookRotation(hit.normal, -transform.forward);
			aux.Rotate (Vector3.right, 90f);
			//Apply an smooth rotation
			transform.rotation = Quaternion.Slerp (transform.rotation, aux.rotation, Time.deltaTime * 20);

			//transform.rotation = aux.rotation;
			Destroy (tempGameObject);

            //http://answers.unity3d.com/questions/1192454/bug-transformup-transformup-sets-y-rotation-to-0.html
            //transform.rotation = Quaternion.LookRotation(hit.normal, -transform.forward);
            //transform.Rotate(Vector3.right, 90f);
        }
    }


}
