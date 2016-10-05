using UnityEngine;
using System.Collections;

public class MovableBlock : MonoBehaviour {

    public float forza = 5.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void move(Vector3 moveDirection)
    {
       // this.transform.position += moveDirection * Time.deltaTime;

        this.GetComponent<Rigidbody>().AddForce(moveDirection*forza);
    }

    public float pushPower = 2.0F;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Ciao");

        
        /*
        if (hit.moveDirection.y < -0.3F)
            return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        

        this.transform.position += hit.moveDirection * Time.deltaTime;
        */
    }
}
