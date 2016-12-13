using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CreateFirstTrackWaypoints : BaseCreateTrackWaypoints {


	// Use this for initialization
	void Awake () {

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

			Vector3[] hardcodedWaypoints = new [] {
				new Vector3 (34.76333f, -14.87549f, 7.331322f),
				new Vector3 (152.2041f, -14.94582f, 36.53478f),
				new Vector3 (287.482f, -14.95781f, 90.93034f),
				new Vector3 (382.0409f, -14.94468f, 169.1243f),
				new Vector3 (395.1613f, -14.93818f, 209.2503f),
				new Vector3 (393.2067f, -14.94338f, 283.765f),
				new Vector3 (385.3524f, -14.95887f, 332.5656f),
				new Vector3 (374.9921f, -14.95839f, 392.3707f),
				new Vector3 (376.7434f, -14.95545f, 483.208f),
				new Vector3 (398.3577f, -14.94036f, 548.9538f),
				new Vector3 (412.4839f, -14.94985f, 581.1032f),
				new Vector3 (430.588f, -14.93681f, 620.5297f),
				new Vector3 (443.4802f, -14.95494f, 649.9219f),
				new Vector3 (461.4415f, -14.93742f, 698.3375f),
				new Vector3 (470.5659f, -14.93071f, 749.9559f),
				new Vector3 (465.4489f, -14.94098f, 791.8664f),
				new Vector3 (452.3415f, -14.95633f, 819.9979f),
				new Vector3 (421.3679f, -14.92085f, 860.8126f),
				new Vector3 (380.4888f, -14.92765f, 900.5287f),
				new Vector3 (330.072f, -14.94908f, 940.6342f),
				new Vector3 (263.0796f, -14.9585f, 981.2732f),
				new Vector3 (169.5688f, -14.95842f, 1021.679f),
				new Vector3 (117.3839f, -14.95823f, 1035.065f),
				new Vector3 (67.83832f, -14.93898f, 1042.17f),
				new Vector3 (8.982924f, -14.95542f, 1042.963f),
				new Vector3 (-67.48635f, -14.95569f, 1037.718f),
				new Vector3 (-158.6126f, -14.95613f, 1024.324f),
				new Vector3 (-283.6396f, -14.95905f, 994.5119f),
				new Vector3 (-386.839f, -14.93214f, 955.9055f),
				new Vector3 (-428.4134f, -14.94942f, 931.5993f),
				new Vector3 (-460.6994f, -14.95702f, 900.2321f),
				new Vector3 (-480.8837f, -14.95175f, 839.6139f),
				new Vector3 (-479.7878f, -14.95357f, 794.3984f),
				new Vector3 (-462.1505f, -14.95846f, 703.0019f),
				new Vector3 (-450.7905f, -14.95687f, 646.2162f),
				new Vector3 (-446.2332f, -14.95397f, 584.2095f),
				new Vector3 (-452.9372f, -14.95465f, 535.7618f),
				new Vector3 (-472.4893f, -14.95569f, 464.0852f),
				new Vector3 (-484.7272f, -14.95181f, 427.0344f),
				new Vector3 (-498.9944f, -14.93979f, 383.8721f),
				new Vector3 (-517.3441f, -14.90854f, 316.0357f),
				new Vector3 (-522.1386f, -14.89231f, 254.6122f),
				new Vector3 (-497.3327f, -14.91823f, 184.9601f),
				new Vector3 (-482.4247f, -14.93765f, 166.2749f),
				new Vector3 (-462.4607f, -14.95204f, 146.4213f),
				new Vector3 (-431.7051f, -14.90139f, 122.0005f),
				new Vector3 (-408.315f, -14.87588f, 106.8957f),
				new Vector3 (-382.4938f, -14.90297f, 91.22688f),
				new Vector3 (-348.6157f, -14.94832f, 73.81383f),
				new Vector3 (-307.1418f, -14.91324f, 55.08204f),
				new Vector3 (-253.5882f, -14.94652f, 33.91164f),
				new Vector3 (-182.6246f, -14.8795f, 13.63374f),
				new Vector3 (-132.2278f, -14.95351f, 3.692588f),
				new Vector3 (-93.32132f, -14.8859f, -0.1721901f),
				new Vector3 (-52.84198f, -14.9443f, -1.594948f),
				new Vector3 (-31.01677f, -15.50537f, -1.868788f),
				new Vector3 (-6.904592f, -14.85552f, 1.75062f)
			};
		applyInterpolation = true;
		if (applyInterpolation) {
			
			//mWayPoints = BezierInterpolator.MakeSmoothCurve (hardcodedWaypoints, mSmoothnessInterpolation);
			mWayPoints = CatmullRomSpline.GetInterpolatedPoints(hardcodedWaypoints);

		} else {
			
			mWayPoints = hardcodedWaypoints;
		}
	}
}
