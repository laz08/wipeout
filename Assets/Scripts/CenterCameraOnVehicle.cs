using UnityEngine;
using System.Collections;

public class CenterCameraOnVehicle : MonoBehaviour {

	public GameObject mUserVehicle;
	public float mDefaultXOffset = -15f;
	public float mOffsetY = 10f;

    public float roadRadius = 20.0f;
    public float horizMove = -10.0f;

    private float actualTransl;
	// Use this for initialization
	void Start () {
        actualTransl = 0.0f;

	}
	
	// Update is called once per frame
	void Update () {
        /*if (Input.GetKey(KeyCode.RightArrow))
        {
            actualTransl += horizMove;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            actualTransl -= horizMove;

        }
        else
        {
            if (actualTransl > 0)
                actualTransl += horizMove ;
            else if (actualTransl < 0)
            {
                actualTransl -= horizMove ;
            }
        }

        if (actualTransl > 0)
        {
            actualTransl = Mathf.Min(actualTransl, roadRadius);
        }
        else if (actualTransl < 0)
        {
            actualTransl = Mathf.Max(actualTransl, -roadRadius);
        }

        transform.Translate(actualTransl * Time.deltaTime, 0.0f, 0.0f);
        */


		/*transform.position = mUserVehicle.transform.position;
	
		transform.forward = -mUserVehicle.transform.forward;
		transform.LookAt(mUserVehicle.transform.position);
	
		transform.localPosition = 
			new Vector3 (
				mUserVehicle.transform.localPosition.x - mDefaultXOffset, 
				mUserVehicle.transform.localPosition.y + mOffsetY, 
				mUserVehicle.transform.localPosition.z);
				*/
	}
}
