using UnityEngine;
using System.Collections;

public class AutomaticMove : MonoBehaviour {

    public float duration;

    public bool lookForward;

    public SplineWalkerMode mode;

    private float progress;
    public BezierSpline mSpline;

	// Use this for initialization
	void Start () {
     
      //DO nothing.
	}
	
	// Update is called once per frame
	void Update () {

        float velocity = GetComponent<MoveVehicle>().speedZ;
        progress += Time.deltaTime * 0.001f*velocity;
        //progress += Time.deltaTime / duration;
        if (progress > 1f)
        {
            if (mode == SplineWalkerMode.Once)
            {
                progress = 1f;
            }
            else if (mode == SplineWalkerMode.Loop)
            {
                progress -= 1f;
            }
            else
            {
                progress = 2f - progress;
            }
        }
        
        Debug.Log(progress);

        Vector3 position = mSpline.GetPoint(progress);
        //transform.localPosition = position;
        if (lookForward && progress != 0)
        {
            transform.LookAt(position + mSpline.GetDirection(progress));
        }
	
	}
}
