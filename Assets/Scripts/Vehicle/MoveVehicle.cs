﻿using UnityEngine;
using System.Collections;

public class MoveVehicle : MonoBehaviour {

	public float accZ = 6.0f;
	public float maxSpeedForward = 100.0f;
	public float turnSpeed = 1.0f;

	public BaseCreateTrackWaypoints mWaypointsFactory;

	private bool previousStateGoForward = true;
	public float speedZ = 0.0f;
	private float actRotation = 0.0f;

	private bool mIsRotationFree = false;

    public float trackRadius = 10.0f;
    private int actualWayPoint;

    void Start()
    {
        actualWayPoint = 0;
    }

	// Update is called once per frame
	void Update () {
		//Movement
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
		}else { 
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
        
		if (mIsRotationFree) {
			
			gameObject.transform.Rotate (0.0f, -actRotation, 0.0f, Space.Self);
			actRotation += turn * turnSpeed;
			gameObject.transform.Rotate (0.0f, actRotation, 0.0f, Space.Self);
		} else {

			/*gameObject.transform.LookAt (
				mWaypointsFactory.getNextWaypoint(
					gameObject.transform.position));*/

            //Set the direction as the direction of the previous way point to the actual => The waypoint i+1 should start at waypoint i center ??
            Vector3 directionToLook = mWaypointsFactory.getWaypoint(actualWayPoint) - mWaypointsFactory.getWaypoint(actualWayPoint -1);
            Quaternion newRotation = Quaternion.LookRotation(directionToLook);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime);

  
                //First translate object to the center of the track
                gameObject.transform.Translate(-actRotation, 0.0f, 0.0f, Space.Self);
                //Update distance that vehicle can move
                //"Rotation", actually a left-right movement
                Debug.Log(actRotation);
                actRotation += turn * turnSpeed * Time.deltaTime;
                if (actRotation > 0){
                    actRotation = Mathf.Min(actRotation, trackRadius);
                }
                else if(actRotation < 0) {
                    actRotation = Mathf.Max(actRotation, -trackRadius);
                }

                //Translate it to the position it should be
                transform.Translate(actRotation, 0.0f, 0.0f, Space.Self);


		}
        //changeUpDirection();

	}


    void OnTriggerEnter()
    {
        Debug.Log("ontriggerneter");
        actualWayPoint++;
    }

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

	void changeAccelerationDirection() {
		speedZ /= 10;
		accZ *= -1;
	}
		
}

