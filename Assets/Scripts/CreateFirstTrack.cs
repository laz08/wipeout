using UnityEngine;
using System.Collections;

public class CreateFirstTrack : MonoBehaviour {
	
	public GameObject mStraightPiece;
	public int mStraightNum = 4;

	private float mLastItemSizeZ = 0f;
	private float mLastItemSizeX = 0f;

	// Use this for initialization
	void Start () {

		//Create first straight road
		GameObject obj;
		for (int j = 0; j < mStraightNum; j++) {

			//Instantiate object
			obj = (GameObject) Instantiate (mStraightPiece,
											new Vector3 (0f, 0f, j*mLastItemSizeZ), transform.rotation);
			//Place on parent
			obj.transform.parent = transform;

			//Save values for next item to be instantiated
			var renderer = obj.GetComponent<Renderer> ();
			mLastItemSizeZ = renderer.bounds.size.z;
			mLastItemSizeX = renderer.bounds.size.x;
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
