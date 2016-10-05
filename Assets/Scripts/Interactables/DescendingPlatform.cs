using UnityEngine;
using System.Collections;

public class DescendingPlatform : MonoBehaviour {

    public Transform paddleDown;
    public Transform paddleUp;

    Transform targetPaddle;

    public bool isFalling = false;
    public bool isDirectionDown = true;
    public bool isOnPaddle = true;

    public float downSpeed;
      	
    void Start()
    {
        targetPaddle = paddleDown;
    }


   	void Update () {

        //  FallingPlatform();

        Vector3 distance = targetPaddle.position - this.transform.position;
        Vector3 direction = (targetPaddle.position - this.transform.position).normalized;
        transform.position = transform.position + direction * downSpeed * Time.deltaTime;

        if (distance.magnitude < 0.1f)
        {
            if (targetPaddle == paddleDown)
            {
                targetPaddle = paddleUp;
            }

            else
            {
                targetPaddle = paddleDown;
            }
        }
	}


   /* void FallingPlatform()
    {
        if (isFalling)
        {
            
            transform.position = new Vector3(transform.position.x, transform.position.y - downSpeed, transform.position.z);
        }
    }*/

    void OnTriggerEnter(Collider coll)
    {
        Debug.Log("Scendo");
        if (coll.gameObject.tag == "Player")
        {

            downSpeed += Time.deltaTime / 20;
            
           //  isFalling = true;

        }
    }
}
