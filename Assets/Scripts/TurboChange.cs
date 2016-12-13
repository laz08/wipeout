using UnityEngine;
using System.Collections;

public class TurboChange : MonoBehaviour {

	private Renderer rend;
	private float offset;
	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		offset += Time.deltaTime;

		rend.material.SetTextureOffset("_MainTex", new Vector2(0,offset));

	}
}
