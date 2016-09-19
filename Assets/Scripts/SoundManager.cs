using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {


    #region SingleTone
    [HideInInspector]
    public static SoundManager instance = null;

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
