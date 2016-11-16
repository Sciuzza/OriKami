﻿using UnityEngine;
using System.Collections;

public class MoveHandler : MonoBehaviour
{



    private Vector3 finalMove;

    CharacterController ccLink;

    private float verticalVelocity, gravityStr = 9.8f;

    // Use this for initialization
    void Awake()
    {

        FSMExecutor fsmExecTempLink = this.gameObject.GetComponent<FSMExecutor>();

        fsmExecTempLink.moveSelected.AddListener(HandlingMove);
        fsmExecTempLink.rotSelected.AddListener(HandlingRot);
        fsmExecTempLink.jumpSelected.AddListener(HandlingJump);


        ccLink = this.gameObject.GetComponent<CharacterController>();

    }

    void Start()
    {
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

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }





}
