using UnityEngine;
using System.Collections;

public class FallingRocks : MonoBehaviour {

   public Rigidbody rocksBody;
    public Rigidbody rocksBody2;
    public Rigidbody rocksBody3;
    public Rigidbody rocksBody4;
    public Rigidbody rocksBody5;

    // Use this for initialization
    void Start () {
         
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider other)
    {
        //Da implementare poi con l'array
        if (other.tag == "Player")
            rocksBody.useGravity = true;
            rocksBody2.useGravity = true;
            rocksBody3.useGravity = true;
            rocksBody4.useGravity = true;
            rocksBody5.useGravity = true;
        

    }
}
