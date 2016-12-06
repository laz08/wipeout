using UnityEngine;
using System.Collections;

public class MoveVehicle : MonoBehaviour {

	public float accZ = 6.0f;
	public float maxSpeedForward = 100.0f;
	public float turnSpeed = 1.0f;
	public float mXAxisApplyValue = 0.1f;

	public BaseCreateTrackWaypoints mWaypointsFactory;

	private bool previousStateGoForward = true;
	public float speedZ = 0.0f;
	private float actRotation = 0.0f;


	public bool mIsRotationFree = false;

	private float mXAxisOffset = 0.0f;

    public float trackRadius = 10.0f;
    private int actualWayPoint;

    void Start()
    {
        actualWayPoint = 0;
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

		moveForward();

		if (Input.GetKey(KeyCode.LeftArrow)) {

			applyOffsetOnXAxis (true);
		}

		if (Input.GetKey(KeyCode.RightArrow)) {

			applyOffsetOnXAxis (false);
		}
	}

	/**
	 * Applies offset on x Axis
	 */
	void applyOffsetOnXAxis(bool isLeft){
		
		float valueToApply;
		if (isLeft) {
			
			valueToApply = -mXAxisApplyValue;
		} else {
			
			valueToApply = mXAxisApplyValue;
		}
			
		mXAxisOffset += valueToApply;
		gameObject.transform.position = gameObject.transform.position 
			+ gameObject.transform.TransformDirection (new Vector3 (valueToApply, 0.0f, 0.0f));
	}

	/**
	 * Moves vehicle forward.
	 */
	void moveForward(){

		//TODO Laura: Keeping this method so ugly so we can apply constants, instead of moving from point to point.
		Vector3 nextPosition = Vector3.zero;
		if (Input.GetKey (KeyCode.UpArrow)) {

			nextPosition = mWaypointsFactory.getNextWaypoint (transform.position);
		}

		if (nextPosition != Vector3.zero){

			transform.position = nextPosition;

			//Recovering offset.
			//Very important...
			gameObject.transform.position = gameObject.transform.position 
				+ gameObject.transform.TransformDirection (new Vector3 (mXAxisOffset, 0.0f, 0.0f));
		}
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

	/**
	 *
	 * Changes acceleration Direction.
	 *
	 */
	void changeAccelerationDirection() {
		speedZ /= 10;
		accZ *= -1;
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
			speedZ += accZ * Time.deltaTime;
			speedZ = Mathf.Min (speedZ,maxSpeedForward);
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


	void OnTriggerEnter()
	{
		Debug.Log("ontriggerneter");
		actualWayPoint++;
	}


}
