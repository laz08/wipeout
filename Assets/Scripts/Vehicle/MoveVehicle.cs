using UnityEngine;
using System.Collections;

public class MoveVehicle : MonoBehaviour {

	//Config.
	public float accZ = 6.0f;
	public float speedZ = 0.0f;
	public float maxSpeedForward = 100.0f;
	public float mXAxisApplyValue = 0.1f; //This configs how "maneagable" is the vehicle. 

	public PlayerLapsController lapsController;
	public BaseCreateTrackWaypoints mWaypointsFactory;

	//For debugging purposes.
	public bool mIsRotationFree = false; 


	//Free movement
	private float turnSpeed = 1.0f;
	private bool previousStateGoForward = true;
	private float actRotation = 0.0f;

	//Movement in X Axis.
	public float xAxisSpeed = 500.0f;
	public bool isAutomaticMove = false;

	private int lapsDone = 0;

	public int currentWayPoint;

    void Start()
    {
       
		currentWayPoint = 0;
		if (!mIsRotationFree) transform.position = mWaypointsFactory.getWaypoint (0);
    }

	// Update is called once per frame
	void Update () {

		if (isAutomaticMove) {

			manageAutomaticMovement ();
		} else {

			manageManualMovement ();
		}
			
		checkHasLapBeenDone ();
		changeUpDirection();
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

		if (mIsRotationFree) {

			applyFreeMovement ();

		} else {

			followTrackWaypoints ();
		}
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

	/**
	 *
	 * Follows track's waypoints.
	 *
	 */
	void followTrackWaypoints(){

		// Correct forward.

		applyDirToVehicle ();

		float forwardValue = moveForward();
		float xAxisValue = Input.GetAxis ("Horizontal") * xAxisSpeed * Time.deltaTime;

		gameObject.transform.position = gameObject.transform.position
			+ gameObject.transform.TransformDirection (new Vector3 (xAxisValue, 0.0f, forwardValue));

	}

	private void checkHasLapBeenDone(){

		int closestWaypoint = mWaypointsFactory.getCurrentWaypointIndex (gameObject.transform.position);
		if (closestWaypoint == 0 && currentWayPoint != 0) {

			++lapsDone;
			lapsController.setLapsDone (lapsDone);
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


	/**
	 *
	 *
	 *  Changes up direction.
	 *
	 */
	void changeUpDirection()
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
	 * Applies free movement.
	 *
	 */
	void applyFreeMovement(){

		if (Input.GetKey (KeyCode.UpArrow)) {
			if (!previousStateGoForward) {
				previousStateGoForward = true;
				changeAccelerationDirection ();
			}
			augmentSpeed ();
		} else if (Input.GetKey (KeyCode.DownArrow)) {
			if (previousStateGoForward) {
				previousStateGoForward = false;
				changeAccelerationDirection ();
			}
			speedZ += accZ* Time.deltaTime;
			speedZ = Mathf.Max (speedZ,-maxSpeedForward);
		} else {
			speedZ -= accZ * Time.deltaTime; //stop movement(go to opposite direction)
			if (!previousStateGoForward) {
				speedZ = Mathf.Min (speedZ, 0.0f);
			} else {
				speedZ = Mathf.Max (speedZ, 0.0f);
			}
		}

		gameObject.transform.Translate(0.0f, 0.0f, speedZ*Time.deltaTime /*+ (1/2)*accZ*Time.deltaTime*Time.deltaTime*/,Space.Self);

		//Tiling
		float turnSpeed = 50.0f;
		float turn = Input.GetAxis("Horizontal");
		/*
		gameObject.transform.Rotate (0.0f, -actRotation, 0.0f, Space.Self);
		actRotation += turn * turnSpeed;
		gameObject.transform.Rotate (0.0f, actRotation, 0.0f, Space.Self);
		*/
		gameObject.transform.Translate (turnSpeed*turn*Time.deltaTime,0.0f,0.0f);
	}


	/**
	 *
	 * Changes acceleration Direction.
	 *
	 */
	void changeAccelerationDirection() {
		speedZ /= 10;
		accZ *= -1;
	}

}
