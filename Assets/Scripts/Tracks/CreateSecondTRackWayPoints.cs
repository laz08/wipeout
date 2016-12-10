using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CreateSecondTRackWayPoints : BaseCreateTrackWaypoints {


	// Use this for initialization
	void Start () {

		initializeWaypointsArray ();
		if(mInstantiateWaypoints){

			instantiateWayPoints ();
		}
	}

	void Update(){
		if(mDrawWaypointsAndLines){

			DrawLines ();
		}
	}

	/**
	 *
	 * Initializes waypoints array
	 *
	 */
	void initializeWaypointsArray(){

		float centerOffsetZ = 1250.0f;
		float centerOffsetY = -50.0f;
		Vector3 center = new Vector3 (0,-centerOffsetY,-centerOffsetZ);
		float radius = 3330.0f;
		int numberOfPoints = 500;
		float deltaAlpha = (2 * Mathf.PI) / numberOfPoints;
		float alphaOffset = 1.2f;

		Vector3[] hardcodedWaypoints = new Vector3[numberOfPoints];
		for (int i = 0; i < numberOfPoints; ++i) {
			float alpha = i * deltaAlpha-alphaOffset;
			hardcodedWaypoints [i] = center + new Vector3 (0,Mathf.Sin(alpha)*radius,Mathf.Cos(alpha)*radius);
		}

		if (mApplyInterpolation) {

			//mWayPoints = BezierInterpolator.MakeSmoothCurve (hardcodedWaypoints, mSmoothnessInterpolation);
			mWayPoints = CatmullRomSpline.GetInterpolatedPoints(hardcodedWaypoints);

		} else {

			mWayPoints = hardcodedWaypoints;
		}
	}
}
