using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {


    #region SingleTone
    [HideInInspector]
    public static SceneManager instance = null;

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
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
