using UnityEngine;
using System.Collections;






public class MoveCC : MonoBehaviour
{


    private CharacterController ccLink;
    private InterCC isLink;


    private bool isGrounded = false;
    private bool isJumping = false;
    private bool isRolling = false;
    private bool isFlying = false;


    private float standSpeed = 50f;
    private float frogSpeed = 20f;
    private float dragSpeed = 70f;
    private float armaSpeed = 35f;
    private float rotateSpeed = 1f;

    private float jumpStrength = 20.0f;
    private float frogGravity = 10.0f;
    private Vector3 jumpDirection; 

    // Use this for initialization
    void Awake()
    {

        ccLink = GetComponent<CharacterController>();
        isLink = GameObject.FindGameObjectWithTag("Player").GetComponent<InterCC>();

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
        Vector3 forward = transform.TransformDirection(Vector3.forward);

        if (isLink.currentForm == forms.standard)
        {
            float curSpeed = standSpeed * Input.GetAxis("Vertical");
            ccLink.SimpleMove(forward * curSpeed);
        }
        else if (isLink.currentForm == forms.frog)
        {
            float curSpeed = frogSpeed * Input.GetAxis("Vertical");
            ccLink.SimpleMove(forward * curSpeed);
        }
        else if (isLink.currentForm == forms.armadillo)
        {
            float curSpeed = armaSpeed * Input.GetAxis("Vertical");
            ccLink.SimpleMove(forward * curSpeed);
        }
        else if (isLink.currentForm == forms.dragon && !ccLink.isGrounded)
        {
            float curSpeed = dragSpeed * Input.GetAxis("Vertical");
            ccLink.SimpleMove(forward * curSpeed);
        }

        if (ccLink.isGrounded)
            isGrounded = true;
        else
            isGrounded = false;

        if (ccLink.isGrounded && Input.GetButtonDown("Jump") && isLink.currentForm == forms.frog && !isJumping)
        {
            jumpDirection = (this.transform.up + this.transform.forward) * jumpStrength;
            //jumpDirection = transform.TransformDirection(jumpDirection);
            isJumping = true;
        }

        if (isJumping)
        {
           
                jumpDirection.y -= frogGravity * Time.deltaTime;
                ccLink.Move(jumpDirection * Time.deltaTime);
           
        }

    }


 


}
