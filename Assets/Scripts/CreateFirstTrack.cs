using UnityEngine;
using System.Collections;

public class CreateFirstTrack : MonoBehaviour {
	
	public GameObject mStraightPiece;
	public int mStraightNum = 4;

	public GameObject mRoadCurve;


	private float mLastItemSizeZ = 0f;
	private float mLastItemSizeX = 0f;
	private float mScale = 100f;

	// Use this for initialization
	void Start () {

		//Create first straight road
		GameObject obj;
		float lastZ = 0f;
		for (int j = 0; j < mStraightNum; j++) {

			//Instantiate object
			obj = (GameObject) Instantiate (mStraightPiece,
											new Vector3 (0f, 0f, j*mLastItemSizeZ), transform.rotation);
			//Place on parent
			obj.transform.parent = transform;
			obj.transform.localScale = new Vector3 (mScale, mScale, mScale);

			//Save values for next item to be instantiated
			var renderer = obj.GetComponent<Renderer> ();
			mLastItemSizeZ = renderer.bounds.size.z;
			mLastItemSizeX = renderer.bounds.size.x;
			lastZ = j * mLastItemSizeZ;
		}


		//First curve
		int rotateAngle = -180;
		for (int i = 0; i < 2; i++) {

			//Instantiate object
			obj = (GameObject) Instantiate (mRoadCurve,
				new Vector3 (-mLastItemSizeX*i, 0f, lastZ), 
				transform.rotation);
			
			obj.transform.Rotate (0, rotateAngle, 0);

			//Place on parent
			obj.transform.parent = transform;
			obj.transform.localScale = new Vector3 (mScale, mScale, mScale);

			//Save values for next item to be instantiated
			var renderer = obj.GetComponent<Renderer> ();
			mLastItemSizeZ = renderer.bounds.size.z;
			mLastItemSizeX = renderer.bounds.size.x;
			lastZ += mLastItemSizeZ;
			rotateAngle = -270;
		}

	}

	// Update is called once per frame
	void Update () {
	
	}
}
