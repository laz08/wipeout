using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class DebugPoints : MonoBehaviour {

    private string fileName = "points_track_0";

    private string fileNameEnd = ".txt";
    private StreamWriter sr;

    // Use this for initialization
	void Start () {

        int i = 0;
        while (File.Exists(fileName + fileNameEnd))
        {

            Debug.Log(fileName + " already exists.");
            fileName = fileName + i;
            i++;
        }

        sr = new StreamWriter(fileName + fileNameEnd, true);
		sr.WriteLine ("Vector3[] hardcodedWaypoints = new [] {");
	}
	
	// Update is called once per frame
	void Update () {
     
        if (Input.GetKey(KeyCode.P))
        {

            Debug.Log(transform.position);

            sr.WriteLine(
				"new Vector3 (" + transform.position.x + "f, " 
				+ transform.position.y + "f, " 
				+ transform.position.z + "f),"
			);

        }
		if (Input.GetKey(KeyCode.Q))
        {
			sr.WriteLine ("};");
            sr.Close();
        }
	}


}
