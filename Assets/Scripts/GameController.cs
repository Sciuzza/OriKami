using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {


    #region SingleTone
    [HideInInspector]
    public static GameController instance = null;

    void Awake()
    {


        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);



       

    }
    #endregion


    // Use this for initialization
    void Start () {


        #region to be moved to OnLevelWasLoaded

        CameraManager.instance.SettingPlayerCamera();

        #endregion


    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
