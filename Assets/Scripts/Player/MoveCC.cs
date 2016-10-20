using UnityEngine;
using System.Collections;
using UnityEngine.Events;






public class MoveCC : MonoBehaviour
{




    public Vector3 jumpDirection, glideDirection, forward, right, moveDirection;

    private PlCore coreLink;
    private CharacterController ccLink;


    [System.Serializable]
    public class moveEvent : UnityEvent<Vector3>
    {
    }



    [HideInInspector]
    public moveEvent Moving;

    public UnityEvent priAbilityInput, secAbilityInput, terAbilityInput, quaAbilityInput, isNotMoving; 

    

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

            MovingInputHandler();
            //MovingOldStyle();

            SpecialMoves();
        }

    }


    private void MovingOldStyle()
    {
        Physics.gravity = coreLink.GeneralValues.globalGravity * Vector3.down;

        transform.Rotate(0, Input.GetAxis("Horizontal") * coreLink.GeneralValues.rotateSpeed, 0);
        transform.Rotate(0, Input.GetAxis("LJHor") * coreLink.GeneralValues.rotateSpeed, 0);
        Vector3 forward = transform.TransformDirection(Vector3.forward);

        if (coreLink.currentForm == forms.standard && !coreLink.isInAir && !coreLink.isFlying)
        {
            float curSpeed;

            if (Input.GetAxis("Vertical") != 0)
                curSpeed = coreLink.CurrentMoveValues.standMove.moveSpeed * Input.GetAxis("Vertical");
            else
                curSpeed = coreLink.CurrentMoveValues.standMove.moveSpeed * -Input.GetAxis("LJVer");

            ccLink.SimpleMove(forward * curSpeed);

        }
        else if (coreLink.currentForm == forms.frog && !coreLink.isInAir && !coreLink.isFlying)
        {

            float curSpeed;

            if (Input.GetAxis("Vertical") != 0)
                curSpeed = coreLink.CurrentMoveValues.frogMove.moveSpeed * Input.GetAxis("Vertical");
            else
                curSpeed = coreLink.CurrentMoveValues.frogMove.moveSpeed * -Input.GetAxis("LJVer");

            ccLink.SimpleMove(forward * curSpeed);


        }
        else if (coreLink.currentForm == forms.armadillo && !coreLink.isInAir && !coreLink.isFlying && !coreLink.isRolling)
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


    private void MovingInputHandler()
    {
        

        curDirZ = -Input.GetAxis("LJVer");
        curDirX = Input.GetAxis("LJHor");
       

        forward = Camera.main.transform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;
        right = new Vector3(forward.z, 0, -forward.x);

        moveDirection = (curDirX * right + curDirZ * forward).normalized;

        
            Moving.Invoke(moveDirection);
     
  
    }

    private void SpecialMoves()
    {


        if (Input.GetButtonDown("Jump") || Input.GetButtonDown("AButton"))
            priAbilityInput.Invoke();

        /*
        if (ccLink.isGrounded && (Input.GetButtonDown("Jump") || Input.GetButtonDown("AButton")))
        {



            if (coreLink.currentForm == forms.frog)
            {
                jumpDirection = (this.transform.up + this.transform.forward) * coreLink.CurrentMoveValues.frogMove.jumpStrength;
                coreLink.isInAir = true;
            }
            else if (coreLink.currentForm == forms.standard)
            {
                jumpDirection = (this.transform.up + this.transform.forward) * coreLink.CurrentMoveValues.standMove.jumpStrength;
                coreLink.isInAir = true;
            }
            else if (coreLink.currentForm == forms.armadillo)
            {
                coreLink.isRolling = true;
            }


        }

        if (coreLink.isInAir)
        {
            jumpDirection.y -= coreLink.GeneralValues.jumpGravity * Time.deltaTime;
            ccLink.Move(jumpDirection * Time.deltaTime);

            if (ccLink.isGrounded)
            {
                coreLink.isInAir = false;
            }
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
                if (coreLink.isInAir)
                    coreLink.isInAir = false;
                if (!coreLink.isFlying)
                    coreLink.isFlying = true;
            }
            else
            {
                coreLink.isInAir = true;
            }

        }

        if (ccLink.isGrounded && coreLink.currentForm == forms.crane)
        {
            coreLink.isFlying = false;
            coreLink.SwitchToStandard();
        }

        if (coreLink.currentForm == forms.dolphin && coreLink.dolphinInAbility)
        {

            this.transform.rotation = coreLink.jumpInRot;
            jumpDirection = (coreLink.jumpInUp / 2 + coreLink.jumpInFw) * coreLink.CurrentMoveValues.dolphinMove.jumpStrength;

            coreLink.isInAir = true;

        }

        if (coreLink.dolphinOutAbility && coreLink.currentForm != forms.dolphin && coreLink.currentForm != forms.crane && coreLink.isInWater && coreLink.previousForm == forms.dolphin)
        {
            this.transform.rotation = coreLink.jumpOutRot;
            jumpDirection = (coreLink.jumpOutUp + coreLink.jumpOutFw) * coreLink.CurrentMoveValues.dolphinMove.jumpStrength;

            coreLink.isInAir = true;
            coreLink.isInWater = false;
        }
        */
    }

}




