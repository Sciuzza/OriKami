using UnityEngine;
using System.Collections;

public class MoveHandler : MonoBehaviour
{



    private Vector3 finalMove = new Vector3(0,0,0), rollImpulse, rollDir;
    private float rollStrength;
    private bool rolling = false;

    CharacterController ccLink;

    private float verticalVelocity = 0.0f, gravityStr = 9.8f;

    // Use this for initialization
    void Awake()
    {

        FSMExecutor fsmExecTempLink = this.gameObject.GetComponent<FSMExecutor>();

        fsmExecTempLink.moveSelected.AddListener(HandlingMove);
        fsmExecTempLink.rotSelected.AddListener(HandlingRot);
        fsmExecTempLink.jumpSelected.AddListener(HandlingJump);
        fsmExecTempLink.StopMoveLogic.AddListener(StoppingMove);
        fsmExecTempLink.EnableMoveLogic.AddListener(EnablingMove);
        fsmExecTempLink.rollSelected.AddListener(HandlingRoll);
        fsmExecTempLink.specialRotSelected.AddListener(HandlingSpecialRot);
    

        ccLink = this.gameObject.GetComponent<CharacterController>();

        FSMChecker fsmCheckTempLink = this.GetComponent<FSMChecker>();

        fsmCheckTempLink.StoppingRollCoroutine.AddListener(StoppingRoll);


    }

    void Start()
    {
        StartCoroutine(Moving());
        
       // StopCoroutine(Moving());
        //StartCoroutine(Moving());
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

        verticalVelocity += jumpStrength;

    }

    private void HandlingRoll(float rollStr)
    {
        rollImpulse = this.transform.forward;
        rollDir = this.transform.forward;
        rollStrength = rollStr;

        rolling = true;
        StartCoroutine(Rolling());
       
    }

    

    private void StoppingMove()
    {
        Debug.Log("Move Stopped");
        
        StopCoroutine(Moving());
    }

    private void EnablingMove()
    {
        Debug.Log("Move Enabled");
        finalMove = new Vector3(0, 0, 0);
        StartCoroutine(Moving());
    }

    private void StoppingRoll()
    {
        rolling = false;
    }

    IEnumerator Moving()
    {
        while (true)
        {
            finalMove *= Time.deltaTime;

            verticalVelocity -= gravityStr * Time.deltaTime;


            finalMove.y = verticalVelocity * Time.deltaTime;


            CollisionFlags flags = ccLink.Move(finalMove);

            if ((flags & CollisionFlags.Below) != 0)
                verticalVelocity = -3f;

           // Debug.Log(verticalVelocity);

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    IEnumerator Rolling()
    {
        finalMove = rollImpulse;

        while (rolling)
        {
            finalMove = Vector3.Slerp(finalMove.normalized, rollDir.normalized, Time.deltaTime);

            float height = finalMove.y;

            finalMove.y = 0;
            this.transform.rotation = Quaternion.LookRotation(finalMove, Vector3.up);

            finalMove.y = height;

            finalMove *= rollStrength;


            yield return new WaitForSeconds(Time.deltaTime);
        }
    }





}
