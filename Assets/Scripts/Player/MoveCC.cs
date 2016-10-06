using UnityEngine;
using System.Collections;






public class MoveCC : MonoBehaviour
{
    
   

  
    public  Vector3 jumpDirection, glideDirection, forward, right, moveDirection;

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

        


        if (!coreLink.vFissureAbilityisOn)
        {

            //MovingNewStyle();
            MovingOldStyle();

            SpecialMoves();
        }

       

    }


    private void MovingOldStyle()
    {
        Physics.gravity = coreLink.GeneralValues.globalGravity * Vector3.down;

        transform.Rotate(0, Input.GetAxis("Horizontal") * coreLink.GeneralValues.rotateSpeed, 0);
        transform.Rotate(0, Input.GetAxis("LJHor") * coreLink.GeneralValues.rotateSpeed, 0);
        Vector3 forward = transform.TransformDirection(Vector3.forward);

        if (coreLink.currentForm == forms.standard && !coreLink.isJumping && !coreLink.isFlying)
        {
            float curSpeed;

            if (Input.GetAxis("Vertical") != 0)
                curSpeed = coreLink.CurrentMoveValues.standMove.moveSpeed * Input.GetAxis("Vertical");
            else
                curSpeed = coreLink.CurrentMoveValues.standMove.moveSpeed * -Input.GetAxis("LJVer");

            ccLink.SimpleMove(forward * curSpeed);

        }
        else if (coreLink.currentForm == forms.frog && !coreLink.isJumping && !coreLink.isFlying)
        {

            float curSpeed;

            if (Input.GetAxis("Vertical") != 0)
                curSpeed = coreLink.CurrentMoveValues.frogMove.moveSpeed * Input.GetAxis("Vertical");
            else
                curSpeed = coreLink.CurrentMoveValues.frogMove.moveSpeed * -Input.GetAxis("LJVer");

            ccLink.SimpleMove(forward * curSpeed);


        }
        else if (coreLink.currentForm == forms.armadillo && !coreLink.isJumping && !coreLink.isFlying && !coreLink.isRolling)
        {
            float curSpeed;

            if (Input.GetAxis("Vertical") != 0)
                curSpeed = coreLink.CurrentMoveValues.armaMove.moveSpeed * Input.GetAxis("Vertical");
            else
                curSpeed = coreLink.CurrentMoveValues.armaMove.moveSpeed * -Input.GetAxis("LJVer");

            ccLink.SimpleMove(forward * curSpeed);


        }
        else if (coreLink.currentForm == forms.crane)
        {
            glideDirection = this.transform.forward * coreLink.CurrentMoveValues.craneMove.glideSpeed;
            glideDirection.y -= coreLink.GeneralValues.glideGravity * Time.deltaTime;
            ccLink.Move(glideDirection * Time.deltaTime);

        }
    }


    float curDirZ = 0.0f;
    float curDirX = 0.0f;
   

    private void MovingNewStyle()
    {
        Physics.gravity = coreLink.GeneralValues.globalGravity * Vector3.down;

        curDirZ = -Input.GetAxis("LJVer");
        curDirX = Input.GetAxis("LJHor");
        

        forward = Camera.main.transform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;
        right = new Vector3(forward.z, 0, -forward.x);

        moveDirection = (curDirX * right + curDirZ * forward).normalized;

        


        if (coreLink.currentForm == forms.standard && !coreLink.isJumping && !coreLink.isFlying)
        {
            if (moveDirection.sqrMagnitude >= 0.1)
            {
                Quaternion rotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                this.transform.localRotation = Quaternion.Slerp(this.transform.localRotation, rotation, Time.deltaTime  * coreLink.GeneralValues.rotateSpeed);
            }
            ccLink.SimpleMove(moveDirection * coreLink.CurrentMoveValues.standMove.moveSpeed);
            
        }
        else if (coreLink.currentForm == forms.frog && !coreLink.isJumping && !coreLink.isFlying)
        {
            if (moveDirection.sqrMagnitude >= 0.1)
            {
                Quaternion rotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                this.transform.localRotation = Quaternion.Slerp(this.transform.localRotation, rotation, Time.deltaTime * coreLink.GeneralValues.rotateSpeed);
            }
            ccLink.SimpleMove(moveDirection * coreLink.CurrentMoveValues.frogMove.moveSpeed);
            



        }
        else if (coreLink.currentForm == forms.armadillo && !coreLink.isJumping && !coreLink.isFlying && !coreLink.isRolling)
        {
            if (moveDirection.sqrMagnitude >= 0.1)
            {
                Quaternion rotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                this.transform.localRotation = Quaternion.Slerp(this.transform.localRotation, rotation, Time.deltaTime * coreLink.GeneralValues.rotateSpeed);
                coreLink.isArmaMoving = true;
            }
            else
                coreLink.isArmaMoving = false;

            ccLink.SimpleMove(moveDirection * coreLink.CurrentMoveValues.armaMove.moveSpeed);
            



        }
        else if (coreLink.currentForm == forms.crane)
        {
            glideDirection = moveDirection;
            glideDirection.y -= coreLink.GeneralValues.glideGravity * Time.deltaTime;

            if (moveDirection.sqrMagnitude >= 0.1)
            {
                Quaternion rotation = Quaternion.LookRotation(glideDirection, Vector3.up);
                this.transform.localRotation = Quaternion.Slerp(this.transform.localRotation, rotation, Time.deltaTime * coreLink.GeneralValues.rotateSpeed);
            }
            ccLink.Move(glideDirection * coreLink.CurrentMoveValues.craneMove.glideSpeed * Time.deltaTime);
            coreLink.isArmaMoving = true;
        }
    }

    private void SpecialMoves()
    {
        if (ccLink.isGrounded && (Input.GetButtonDown("Jump") || Input.GetButtonDown("AButton")))
        {



            if (coreLink.currentForm == forms.frog)
            {
                jumpDirection = (this.transform.up + this.transform.forward) * coreLink.CurrentMoveValues.frogMove.jumpStrength;
                coreLink.isJumping = true;
            }
            else if (coreLink.currentForm == forms.standard)
            {
                jumpDirection = (this.transform.up + this.transform.forward) * coreLink.CurrentMoveValues.standMove.jumpStrength;
                coreLink.isJumping = true;
            }
            else if (coreLink.currentForm == forms.armadillo)
            {
                coreLink.isRolling = true;
            }

        }

        if (coreLink.isJumping)
        {
            jumpDirection.y -= coreLink.GeneralValues.jumpGravity * Time.deltaTime;
            ccLink.Move(jumpDirection * Time.deltaTime);

            if (ccLink.isGrounded)
                coreLink.isJumping = false;
        }

        if (coreLink.isRolling)
        {
            coreLink.rollingTime += Time.deltaTime;
            ccLink.SimpleMove(this.transform.forward * coreLink.CurrentMoveValues.armaMove.rollingStrength);

            if (coreLink.rollingTime >= coreLink.CurrentMoveValues.armaMove.rollingTime)
            {
                coreLink.rollingTime = 0;
                coreLink.isRolling = false;
            }

        }

        if (!ccLink.isGrounded)
        {
            if (coreLink.currentForm == forms.crane)
            {
                if (coreLink.isJumping)
                    coreLink.isJumping = false;
                if (!coreLink.isFlying)
                    coreLink.isFlying = true;
            }
            else
            {
                coreLink.isJumping = true;
            }

        }

        if (ccLink.isGrounded && coreLink.currentForm == forms.crane)
        {
            coreLink.isFlying = false;
            coreLink.SwitchToStandard();
        }
    }

}




