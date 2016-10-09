using UnityEngine;
using System.Collections;

public class MovableBlock : MonoBehaviour {

 

   

 
    void OnTriggerExit(Collider player)
    {
        if (player.gameObject.tag == "Player")
        {
            Debug.Log("Collision detected");
            this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    }
    }
 
}
