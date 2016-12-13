using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CreateSecondTRackWayPoints : BaseCreateTrackWaypoints {


	void Awake(){
	
		initializeWaypointsArray ();
		if(shouldWaypointsBeInstantiated){

			instantiateWayPoints ();
		}
	}

	void Update(){
		if(drawWaypointsAndLines){

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
		Vector3 center = new Vector3 (0,-centerOffsetY,-centerOffsetZ);//Center of torus
		float radius = 3330.0f;
		int numberOfPoints = 1000;
		float deltaAlpha = (2 * Mathf.PI) / numberOfPoints;
		float alphaOffset = 1.2f;//Where the race starts

		Vector3[] hardcodedWaypoints = new Vector3[numberOfPoints];
		for (int i = 0; i < numberOfPoints; ++i) {
			float alpha = i * deltaAlpha-alphaOffset;
			hardcodedWaypoints [i] = center + new Vector3 (0,Mathf.Sin(alpha)*radius,Mathf.Cos(alpha)*radius);
		}

		if (applyInterpolation) {

			//mWayPoints = BezierInterpolator.MakeSmoothCurve (hardcodedWaypoints, mSmoothnessInterpolation);
			mWayPoints = CatmullRomSpline.GetInterpolatedPoints(hardcodedWaypoints);

		} else {

			mWayPoints = hardcodedWaypoints;
		}
	}
}
