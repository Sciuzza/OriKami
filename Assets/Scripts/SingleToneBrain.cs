﻿using UnityEngine;
using System.Collections;

public class SingleToneBrain : MonoBehaviour {

	// Use this for initialization
	void Awake () {


        DontDestroyOnLoad(this.gameObject);

    }
	
	
}
