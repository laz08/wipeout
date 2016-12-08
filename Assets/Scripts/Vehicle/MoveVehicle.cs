using UnityEngine;
using System.Collections;

public class MoveVehicle : MonoBehaviour {

	//Config.
	public float accZ = 6.0f;
	public float speedZ = 0.0f;
	public float maxSpeedForward = 100.0f;
	public float mXAxisApplyValue = 0.1f; //This configs how "maneagable" is the vehicle. 

	public BaseCreateTrackWaypoints mWaypointsFactory;

	//For debugging purposes.
	public bool mIsRotationFree = false; 


	//Free movement
	private float turnSpeed = 1.0f;
	private bool previousStateGoForward = true;
	private float actRotation = 0.0f;

	//Movement in X Axis.
	public float mXAxisOffset = 0.0f;
	public float MaxXAxisOffset = 19.0f;

	public int currentWayPoint;

    void Start()
    {
       
		currentWayPoint = 0;
		transform.position = mWaypointsFactory.getWaypoint (0);
    }

	// Update is called once per frame
	void Update () {

		if (mIsRotationFree) {

			applyFreeMovement ();
			// Corrects up.
			changeUpDirection();

		} else {

			followTrackWaypoints ();
		}
	}


	/**
	 *
	 * Follows track's waypoints.
	 *
	 */
	void followTrackWaypoints(){

		// Correct forward.

		gameObject.transform.forward =  (
			mWaypointsFactory.getDir(
				gameObject.transform.position));

		float forwardValue = moveForward();
		float xAxisValue = 0;
		if (Input.GetKey(KeyCode.LeftArrow)) {

			xAxisValue = applyOffsetOnXAxis (true);
		}

		if (Input.GetKey(KeyCode.RightArrow)) {

			xAxisValue = applyOffsetOnXAxis (false);
		}

		gameObject.transform.position = gameObject.transform.position
			+ gameObject.transform.TransformDirection (new Vector3 (xAxisValue, 0.0f, forwardValue));
	}

	/**
	 * Applies offset on x Axis
	 */
	float applyOffsetOnXAxis(bool isLeft){
		
		float valueToApply;
		if (isLeft) {
		
			valueToApply = -mXAxisApplyValue;
			mXAxisOffset += valueToApply;
			mXAxisOffset =  Mathf.Max (mXAxisOffset, -MaxXAxisOffset);
			if (mXAxisOffset >= -MaxXAxisOffset) {
		
				return valueToApply;
			}
		} else {
		
			valueToApply = mXAxisApplyValue;
			mXAxisOffset += valueToApply;
			mXAxisOffset =  Mathf.Min (mXAxisOffset, MaxXAxisOffset);
			if (mXAxisOffset < MaxXAxisOffset) {

				return valueToApply;
				//gameObject.transform.position = gameObject.transform.position
				//	+ gameObject.transform.TransformDirection (new Vector3 (valueToApply, 0.0f, 0.0f));
			}
		}
		return 0;
	}

	/**
	 * Moves vehicle forward.
	 */
	float moveForward(){
		
		if (Input.GetKey (KeyCode.UpArrow)) {

			augmentSpeed ();
			return  speedZ * Time.deltaTime;

			//nextPosition = mWaypointsFactory.getNextWaypoint (transform.position);
			//transform.position = new Vector3(nextPosition.x, transform.position.y, nextPosition.z);				

		
			//transform.position = transform.position 
			//	+ gameObject.transform.Translate(0.0f, 0.0f, speedZ*Time.deltaTime,  /*+ (1/2)*accZ*Time.deltaTime*Time.deltaTime*/
			//	, Space.Self);


			//TODO: No need to recover offset for now.
			/*
			//Recovering offset.
			//Very important...
			transform.position = transform.position 
				+ gameObject.transform.TransformDirection (new Vector3 (mXAxisOffset, 0.0f, 0.0f));
			*/
		}
		return 0;
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
			transform.rotation = Quaternion.Slerp (transform.rotation,aux.rotation,Time.deltaTime);
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
		float turn = Input.GetAxis("Horizontal");

		gameObject.transform.Rotate (0.0f, -actRotation, 0.0f, Space.Self);
		actRotation += turn * turnSpeed;
		gameObject.transform.Rotate (0.0f, actRotation, 0.0f, Space.Self);
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
