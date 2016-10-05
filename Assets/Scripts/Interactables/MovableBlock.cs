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
}
