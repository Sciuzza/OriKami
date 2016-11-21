using UnityEngine;
using System.Collections;

public class MoveHandler : MonoBehaviour
{



    private Vector3 finalMove = new Vector3(0,0,0), rollImpulse = new Vector3(0,0,0), rollDir;
    private float rollStrength;
    private bool rolling = false, gliding = false;

    CharacterController ccLink;

    private float verticalVelocity = 0.0f, gravityStr = Physics.gravity.y;

    CollisionFlags flags;

    // Use this for initialization
    void Awake()
    {

        FSMExecutor fsmExecTempLink = this.gameObject.GetComponent<FSMExecutor>();

        fsmExecTempLink.moveSelected.AddListener(HandlingMove);
        fsmExecTempLink.rotSelected.AddListener(HandlingRot);
        fsmExecTempLink.jumpSelected.AddListener(HandlingJump);
        fsmExecTempLink.rollSelected.AddListener(HandlingRoll);
        fsmExecTempLink.specialRotSelected.AddListener(HandlingSpecialRot);
    

        ccLink = this.gameObject.GetComponent<CharacterController>();

        FSMChecker fsmCheckTempLink = this.GetComponent<FSMChecker>();

        fsmCheckTempLink.stoppingRollLogic.AddListener(StoppingRoll);
        fsmCheckTempLink.enableGlideLogic.AddListener(SettingGlideGravity);
        fsmCheckTempLink.stopGlideLogic.AddListener(SettingNormalGravity);

    }


    void Update()
    {


        float timeTakeThisFrame = Time.deltaTime;

        if (rolling)
        {
            finalMove = Vector3.Slerp(finalMove.normalized, rollDir.normalized, timeTakeThisFrame);

            float height = finalMove.y;

            finalMove.y = 0;

            if ((ccLink.collisionFlags & CollisionFlags.Below) != 0)
                this.transform.rotation = Quaternion.LookRotation(finalMove, Vector3.up);

            finalMove.y = height;

            finalMove *= rollStrength;


        }

        

        finalMove *= timeTakeThisFrame;

        if (!gliding)
            verticalVelocity -= gravityStr * timeTakeThisFrame;
        else
            verticalVelocity = -1;

        finalMove.y = verticalVelocity * timeTakeThisFrame;




        flags = ccLink.Move(finalMove);

        if ((flags & CollisionFlags.Below) != 0)
            verticalVelocity = -3f;
        else if ((flags & CollisionFlags.Above) != 0)
            verticalVelocity = 0;
    }

    private void HandlingMove(Vector3 inputDir, float moveSpeed)
    {
        finalMove = inputDir * moveSpeed;
    }

    private void HandlingRot(Vector3 inputDir, float rotSpeed)
    {

        Quaternion rotation = Quaternion.LookRotation(inputDir, Vector3.up);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, Time.deltaTime * rotSpeed);

    }

    private void HandlingSpecialRot(Vector3 inputDir, float rotSpeed)
    {

        //Quaternion rotation = Quaternion.LookRotation(inputDir, Vector3.up);
        //this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, Time.deltaTime * rotSpeed);
       
        rollDir = inputDir;
       
    }

    private void HandlingJump(float jumpStrength)
    {

        verticalVelocity = jumpStrength;

    }

    private void HandlingRoll(float rollStr)
    {
        rollImpulse = this.transform.forward;
        rollDir = this.transform.forward;
        rollStrength = rollStr;
        finalMove = rollImpulse;

        rolling = true;
      
       
    }

    private void StoppingRoll()
    {
        rolling = false;
    }

    private void SettingGlideGravity()
    {
        gliding = true;
    }

    private void SettingNormalGravity()
    {
        gliding = false;
    }



}
