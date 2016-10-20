using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

   // public Vector3 prova;
   // public Rigidbody rb;

    void Start()
    {
       // rb = GetComponent<Rigidbody>();
    }
	
	void Update () {

        //rb.MovePosition(transform.position + transform.forward * Time.deltaTime);
       

	}

    /* void OnCollisionEnter(Collision collision)
     {

         if (collision.gameObject.tag == "Player")
         {
             Debug.Log("Collisione");
             //rb.AddForce(Vector3.down, ForceMode.Impulse);
             transform.position = Vector3.down *2* Time.deltaTime;
         }
     }*/

    /*  void OnControllerColliderHit(ControllerColliderHit hit)
      {

          if (gameObject.tag == "Object")
          {
              Debug.Log("Controller hit");

          }
      }*/

    void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.tag == "Object")
        {
            Debug.Log("Collision detected");
           // this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

}
