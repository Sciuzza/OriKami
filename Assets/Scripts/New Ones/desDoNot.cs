using UnityEngine;
using System.Collections;

public class desDoNot : MonoBehaviour {


    void Awake () {
    
        DontDestroyOnLoad(this.gameObject);
    }
    
}
