using UnityEngine;
using System.Collections;

public class FpsCounter : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        Debug.Log(1.0f / Time.deltaTime);
    }

}
