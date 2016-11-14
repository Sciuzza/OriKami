using UnityEngine;
using System.Collections;

public class MFS : MonoBehaviour
{

    private CharacterController ccLink;

    float curDirZ, curDirX, speed = 3;

   


    private float gravityStr = 9f;
    private float jumpSpeed = 10f;
    private bool canJump = false;
    private float verticalV;
    Vector3 forward, right;
    Vector3 moveDirection = new Vector3(0, 0, 0);

    void Awake()
    {
        ccLink = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

       

        /*
        curDirZ = -Input.GetAxis("LJVer");
        curDirX = Input.GetAxis("LJHor");

       

        forward = Camera.main.transform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;
        right = new Vector3(forward.z, 0, -forward.x);
        
        moveDirection = (curDirX * right + curDirZ * forward).normalized;
        */

        moveDirection.x = Input.GetAxis("LJHor");
        moveDirection.z = -Input.GetAxis("LJVer");

        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

        moveDirection *= speed * Time.deltaTime;

        
        Quaternion inputRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up), Vector3.up);
        moveDirection = inputRotation * moveDirection;

    
        

        verticalV -= gravityStr * Time.deltaTime;

        Debug.Log(ccLink.velocity.magnitude);

        if (Input.GetButtonDown("XButton") && canJump)
        {
           
            verticalV += jumpSpeed;

        }

        

        moveDirection.y = verticalV * Time.deltaTime;

        CollisionFlags flags = ccLink.Move(moveDirection);

        if ((flags & CollisionFlags.Below) != 0)
        {
            canJump = true;
            verticalV = -3f;
        }
        else
            canJump = false;

    }
}
