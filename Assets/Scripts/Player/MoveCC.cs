using UnityEngine;
using System.Collections;






public class MoveCC : MonoBehaviour
{





    private bool isGrounded = false;
    private bool isJumping = false;
    public bool isRolling = false;
    private bool isFlying = false;

    private float rollingTime = 0.0f;




    private Vector3 jumpDirection, glideDirection;

    private PlCore coreLink;
    private CharacterController ccLink;

    void Awake()
    {
        coreLink = this.GetComponent<PlCore>();
        ccLink = this.GetComponent<CharacterController>();
    }


    // Update is called once per frame

    void Update()
    {

        Physics.gravity = coreLink.GeneralValues.globalGravity * Vector3.down;

        transform.Rotate(0, Input.GetAxis("Horizontal") * coreLink.GeneralValues.rotateSpeed, 0);
        transform.Rotate(0, Input.GetAxis("LJHor") * coreLink.GeneralValues.rotateSpeed, 0);
        Vector3 forward = transform.TransformDirection(Vector3.forward);

        if (coreLink.currentForm == forms.standard && !isJumping && !isFlying)
        {
            float curSpeed;

            if (Input.GetAxis("Vertical") != 0)
                curSpeed = coreLink.CurrentMoveValues.standMove.moveSpeed * Input.GetAxis("Vertical");
            else
                curSpeed = coreLink.CurrentMoveValues.standMove.moveSpeed * -Input.GetAxis("LJVer");

            ccLink.SimpleMove(forward * curSpeed);
        }
        else if (coreLink.currentForm == forms.frog && !isJumping && !isFlying)
        {

            float curSpeed;

            if (Input.GetAxis("Vertical") != 0)
                curSpeed = coreLink.CurrentMoveValues.frogMove.moveSpeed * Input.GetAxis("Vertical");
            else
                curSpeed = coreLink.CurrentMoveValues.frogMove.moveSpeed * -Input.GetAxis("LJVer");

            ccLink.SimpleMove(forward * curSpeed);


        }
        else if (coreLink.currentForm == forms.armadillo && !isJumping && !isFlying && !isRolling)
        {
            float curSpeed;

            if (Input.GetAxis("Vertical") != 0)
                curSpeed = coreLink.CurrentMoveValues.armaMove.moveSpeed * Input.GetAxis("Vertical");
            else
                curSpeed = coreLink.CurrentMoveValues.armaMove.moveSpeed * -Input.GetAxis("LJVer");

            ccLink.SimpleMove(forward * curSpeed);


        }
        else if (coreLink.currentForm == forms.dragon)
        {
            glideDirection = this.transform.forward * coreLink.CurrentMoveValues.craneMove.glideSpeed;
            glideDirection.y -= coreLink.GeneralValues.glideGravity * Time.deltaTime;
            ccLink.Move(glideDirection * Time.deltaTime);

        }

        if (ccLink.isGrounded)
            isGrounded = true;
        else
            isGrounded = false;

        if (ccLink.isGrounded && (Input.GetButtonDown("Jump") || Input.GetButtonDown("AButton")))
        {



            if (coreLink.currentForm == forms.frog)
            {
                jumpDirection = (this.transform.up + this.transform.forward) * coreLink.CurrentMoveValues.frogMove.jumpStrength;
                isJumping = true;
            }
            else if (coreLink.currentForm == forms.standard)
            {
                jumpDirection = (this.transform.up + this.transform.forward) * coreLink.CurrentMoveValues.standMove.jumpStrength;
                isJumping = true;
            }
            else if (coreLink.currentForm == forms.armadillo)
            {
                isRolling = true;
            }

        }

        if (isJumping)
        {
            jumpDirection.y -= coreLink.GeneralValues.jumpGravity * Time.deltaTime;
            ccLink.Move(jumpDirection * Time.deltaTime);

            if (ccLink.isGrounded)
                isJumping = false;
        }

        if (isRolling)
        {
            rollingTime += Time.deltaTime;
            ccLink.SimpleMove(this.transform.forward * coreLink.CurrentMoveValues.armaMove.rollingStrength);

            if (rollingTime >= coreLink.CurrentMoveValues.armaMove.rollingTime)
            {
                rollingTime = 0;
                isRolling = false;
            }

        }

        if (!ccLink.isGrounded && coreLink.currentForm == forms.dragon)
        {
            if (isJumping)
                isJumping = false;
            if (!isFlying)
                isFlying = true;

        }

        if (ccLink.isGrounded && coreLink.currentForm == forms.dragon)
        {
            isFlying = false;
            coreLink.SwitchToStandard();
        }

    }




}




