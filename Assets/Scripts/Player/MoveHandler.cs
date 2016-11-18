using UnityEngine;
using System.Collections;

public class MoveHandler : MonoBehaviour
{



    private Vector3 finalMove;

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

        ccLink = this.gameObject.GetComponent<CharacterController>();

    }

    void Start()
    {
        StartCoroutine(Moving());
        StopCoroutine(Moving());
        StartCoroutine(Moving());
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

    private void HandlingJump(float jumpStrength)
    {

        verticalVelocity += jumpStrength;

    }

    private void StoppingMove()
    {
        Debug.Log("Move Stopped");
        StopCoroutine(Moving());
    }

    private void EnablingMove()
    {
        Debug.Log("Move Enabled");
        StartCoroutine(Moving());
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

            Debug.Log(verticalVelocity);

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }





}
