﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/**
 * Interpolation between points with a Catmull-Rom spline.
 * 
 * Adapted code.
 * Original Source code: 
 * http://www.habrador.com/tutorials/interpolation/1-catmull-rom-splines/
 * 
 **/
public class CatmullRomSpline : MonoBehaviour
{
	
	//Has to be at least 4 points
	public static Vector3[] controlPointsList;
	public static List<Vector3> resultPointList;

	//Are we making a line or a loop?
	public static bool isLooping = true;

	public static Vector3[] GetInterpolatedPoints(Vector3[] originalPoints) {

		controlPointsList = originalPoints;
		resultPointList = new List<Vector3> ();

		//Draw the Catmull-Rom spline between the points
		for (int i = 0; i < controlPointsList.Length; i++)
		{
			//Cant draw between the endpoints
			//Neither do we need to draw from the second to the last endpoint
			//...if we are not making a looping line
			if (!isLooping && (i == 0 || i == controlPointsList.Length - 2 || i == controlPointsList.Length - 1))
			{
				continue;
			}

			GeneratePointsForIndex(i);
		}

		return resultPointList.ToArray();
	}

	private static void GeneratePointsForIndex (int pos){
	
		//The 4 points we need to form a spline between p1 and p2
		Vector3 p0 = controlPointsList[ClampListPos(pos - 1)];
		Vector3 p1 = controlPointsList[pos];
		Vector3 p2 = controlPointsList [ClampListPos (pos + 1)];
		Vector3 p3 = controlPointsList[ClampListPos(pos + 2)];

		//The start position of the line
		Vector3 lastPos = p1;

		//The spline's resolution
		//Make sure it's is adding up to 1, so 0.3 will give a gap, but 0.2 will work
		float resolution = 0.1f;

		//How many times should we loop?
		int loops = Mathf.FloorToInt(1f / resolution);

		for (int i = 1; i <= loops; i++)
		{
			//Which t position are we at?
			float t = i * resolution;

			//Find the coordinate between the end points with a Catmull-Rom spline
			Vector3 newPos = GetCatmullRomPosition(t, p0, p1, p2, p3);
			resultPointList.Add (newPos);

			lastPos = newPos;
		}
	}


	//Clamp the list positions to allow looping
	static int ClampListPos(int pos)
	{
		if (pos < 0)
		{
			pos = controlPointsList.Length - 1;
		}

		if (pos > controlPointsList.Length)
		{
			pos = 1;
		}
		else if (pos > controlPointsList.Length - 1)
		{
			pos = 0;
		}

		return pos;
	}

	//Returns a position between 4 Vector3 with Catmull-Rom spline algorithm
	//http://www.iquilezles.org/www/articles/minispline/minispline.htm
	static Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		//The coefficients of the cubic polynomial (except the 0.5f * which I added later for performance)
		Vector3 a = 2f * p1;
		Vector3 b = p2 - p0;
		Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
		Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

		//The cubic polynomial: a + b * t + c * t^2 + d * t^3
		Vector3 pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

		return pos;
	}

	/*
	//Display without having to press play
	void OnDrawGizmos()
	{

		Gizmos.color = Color.white;

		//Draw the Catmull-Rom spline between the points
		for (int i = 0; i < controlPointsList.Length; i++)
		{
			//Cant draw between the endpoints
			//Neither do we need to draw from the second to the last endpoint
			//...if we are not making a looping line
			if (!isLooping && (i == 0 || i == controlPointsList.Length - 2 || i == controlPointsList.Length - 1))
			{
				continue;
			}

			DisplayCatmullRomSpline(i);
		}
	}

	//Display a spline between 2 points derived with the Catmull-Rom spline algorithm
	void DisplayCatmullRomSpline(int pos)
	{
		//The 4 points we need to form a spline between p1 and p2
		Vector3 p0 = controlPointsList[ClampListPos(pos - 1)];
		Vector3 p1 = controlPointsList[pos];
		Vector3 p2 = controlPointsList [ClampListPos (pos + 1)];
		Vector3 p3 = controlPointsList[ClampListPos(pos + 2)];

		//The start position of the line
		Vector3 lastPos = p1;

		//The spline's resolution
		//Make sure it's is adding up to 1, so 0.3 will give a gap, but 0.2 will work
		float resolution = 0.2f;

		//How many times should we loop?
		int loops = Mathf.FloorToInt(1f / resolution);

		for (int i = 1; i <= loops; i++)
		{
			//Which t position are we at?
			float t = i * resolution;

			//Find the coordinate between the end points with a Catmull-Rom spline
			Vector3 newPos = GetCatmullRomPosition(t, p0, p1, p2, p3);

			//Draw this line segment
			Gizmos.DrawLine(lastPos, newPos);


			//Save this pos so we can draw the next line segment
			lastPos = newPos;
		}
	}
	*/
}