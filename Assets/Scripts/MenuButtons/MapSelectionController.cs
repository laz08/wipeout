using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;

public class MapSelectionController : MonoBehaviour {

	public GameObject normalTrack;
	public GameObject torusTrack;

	private static string FIRST_TRACK_NAME = "FirstTrack";
	private static string SECOND_TRACK_NAME = "TorusTrack";

	private static string VEHICLE_SELECT_SCREEN_NAME = "VehicleSelectScreen";

	public float rotationSpeed = 75;

    private bool selectedMap = false;
    private Texture loading;

	void Awake() {
        loading = (Texture)Resources.Load("loading");

	}

	void Update(){

		normalTrack.transform.Rotate (Vector3.up * Time.deltaTime * rotationSpeed, Space.Self);
		torusTrack.transform.Rotate (Vector3.up * Time.deltaTime * -rotationSpeed, Space.Self);
	}

	public void onNormalSelected(){
        SceneManager.UnloadScene(FIRST_TRACK_NAME);//RESET SCENE!
		SceneManager.LoadScene (FIRST_TRACK_NAME);
        selectedMap = true;
	}

	public void onTorusSelected(){
        SceneManager.UnloadScene(SECOND_TRACK_NAME);//RESET SCENE!
		SceneManager.LoadScene (SECOND_TRACK_NAME);
        selectedMap = true;
    
    }

	public void onBackSelected(){

		SceneManager.LoadScene(VEHICLE_SELECT_SCREEN_NAME);
	}

    void OnGUI()
    {
        if (selectedMap)
        {
            float Textwidth = (Screen.width / 2);
            float Textheight = (Screen.height / 2);
            GUI.DrawTexture(new Rect((Screen.width) - (Textwidth / 4), (Screen.height) - (Textheight / 4), Textwidth / 4, Textheight / 4), loading, ScaleMode.ScaleToFit,true,10.0f);
        }
    }
}
