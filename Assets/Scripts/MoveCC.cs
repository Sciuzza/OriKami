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

    private float jumpStrength = 50.0f;
    private float frogGravity = 100.0f;
    private Vector3 jumpDirection;

    private float craneGravity = 50.0f;
    private float moveSpeedInAir = 10.0f;


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

        if (isLink.currentForm == forms.standard && !isJumping && !isFlying)
        {
            float curSpeed = standSpeed * Input.GetAxis("Vertical");
            ccLink.SimpleMove(forward * curSpeed);
        }
        else if (isLink.currentForm == forms.frog && !isJumping && !isFlying)
        {
            float curSpeed = frogSpeed * Input.GetAxis("Vertical");
            ccLink.SimpleMove(forward * curSpeed);
        }
        else if (isLink.currentForm == forms.armadillo && !isJumping && !isFlying)
        {
            float curSpeed = armaSpeed * Input.GetAxis("Vertical");
            ccLink.SimpleMove(forward * curSpeed);
        }
        else if (isLink.currentForm == forms.dragon)
        {
            float curSpeed = dragSpeed * Input.GetAxis("Vertical");
            forward.y -= craneGravity * Time.deltaTime;
            ccLink.Move(forward * Time.deltaTime * dragSpeed);

        }

        if (ccLink.isGrounded)
            isGrounded = true;
        else
            isGrounded = false;

        if (ccLink.isGrounded && Input.GetButtonDown("Jump"))
        {
            if (isLink.currentForm == forms.frog)
                jumpDirection = (this.transform.up + this.transform.forward) * jumpStrength;
            else if (isLink.currentForm == forms.standard)
                jumpDirection = (this.transform.up + this.transform.forward) * jumpStrength / 2;

            isJumping = true;
        }

        if (isJumping)
        {


            jumpDirection.y -= frogGravity * Time.deltaTime;
            ccLink.Move(jumpDirection * Time.deltaTime);




            if (ccLink.isGrounded)
                isJumping = false;
        }

        if (!ccLink.isGrounded && isLink.currentForm == forms.dragon)
        {
            if (isJumping)
                isJumping = false;
            if (!isFlying)
                isFlying = true;

        }

        if (ccLink.isGrounded && isLink.currentForm == forms.dragon)
        {
            isFlying = false;
            isLink.SwitchToStandard();
        }

    }





}
