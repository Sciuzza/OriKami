using UnityEngine;
using System.Collections;

public class MovableBlock : MonoBehaviour {

    public bool hasToMove = true;
    public Vector3 dirToMove;

   

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        

	}
/*
    void OnTriggerStay(Collider objectHit)
    {
        if (objectHit.gameObject.tag == "Wall")
        {
            Debug.Log("Ciao");
            hasToMove = false;
        }
    }

    void OnTriggerExit(Collider objectHit)
    {
        if (objectHit.gameObject.tag == "Wall")
            hasToMove = true;
    }
*/
 
    void OnTriggerExit(Collider player)
    {
        if (player.gameObject.tag == "Player")
        {
            Debug.Log("Collision detected");
            this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    }
    }
 
}
