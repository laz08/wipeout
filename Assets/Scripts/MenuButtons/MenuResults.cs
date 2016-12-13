using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuResults : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		//https://forum.unity3d.com/threads/unity-beginner-loadlevel-with-arguments.180925/
		Dictionary<string,string> arguments = AssemblyCSharp.SceneController.getSceneParameters();
		foreach (KeyValuePair<string,string> a in arguments) {
			Debug.Log (a.Value);

		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
