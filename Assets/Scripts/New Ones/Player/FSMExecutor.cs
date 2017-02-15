﻿using System;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class FSMExecutor : MonoBehaviour
{
    #region Private Constants
    private const float Tolerance = 0.1f;
    #endregion


    #region Public Variables
    [HideInInspector]
    public moveValues currentMoveValues;

    [HideInInspector]
    public generalTweaks generalValues;
    #endregion

    #region Events
    public event_vector3_float moveSelected, rotSelected, specialRotSelected;
    public event_float jumpSelected, rollSelected;
    public event_ps phChangeEffect;
    public UnityEvent vFissureAniEnded;
    #endregion

    #region Private Variables
    PlayerInputs playerref;

    //Animator Manager variables
    private Vector3 finalMoveDirTemp;
    private List<bool> delayFlagRefFix = new List<bool>();
    private Animator animatorLink;
    private Coroutine dolphinFIx;
    #endregion

    #region Taking References and linking Events
    void Awake()
    {
        var fsmCheckerTempLink = this.gameObject.GetComponent<FSMChecker>();

        fsmCheckerTempLink.formChanged.AddListener(this.ApplyingFormEffect);
        fsmCheckerTempLink.moveUsed.AddListener(this.ApplyingMoveAbiEffect);
        fsmCheckerTempLink.genAbiUsed.AddListener(this.ApplyingAbilityEffect);
        fsmCheckerTempLink.rotationUsed.AddListener(this.ApplyingRotationEffect);
        fsmCheckerTempLink.vFissureUsed.AddListener(this.ApplyingSpecialAniAbi);
        fsmCheckerTempLink.swallowOnGround.AddListener(this.SettingAniOnGround);
        fsmCheckerTempLink.dolphinOnGround.AddListener(this.SettingAniDolphinOnGround);

        var moveHandTempLink = this.gameObject.GetComponent<MoveHandler>();

        moveHandTempLink.UpdatedFinalMoveRequest.AddListener(this.TakingFinalMoveUpdated);

        this.delayFlagRefFix.Add(true);
    }
    #endregion

    #region Normal Ability Handler Methods
    private void ApplyingFormEffect(string newForm, string previousForm, List<GameObject> formReferences)
    {
        formReferences.Find(x => x.tag == newForm).SetActive(false);

        bool allDisabled = false;

        while (!allDisabled)
        {
            if (formReferences.Find(x => x.activeInHierarchy) == null)
                allDisabled = true;
            else
                formReferences.Find(x => x.activeInHierarchy).SetActive(false);
        }

        formReferences.Find(x => x.tag == newForm).SetActive(true);

        switch (newForm)
        {
            case "Standard Form":
                this.animatorLink = formReferences[0].GetComponent<Animator>();
                break;
            case "Frog Form":
                this.animatorLink = formReferences[1].GetComponent<Animator>();
                break;
            case "Dragon Form":
                this.animatorLink = formReferences[2].GetComponent<Animator>();
                break;
            case "Arma Form":
                this.animatorLink = formReferences[3].GetComponent<Animator>();
                break;
            case "Dolphin Form":
                this.animatorLink = formReferences[4].GetComponent<Animator>();
                this.dolphinFIx = this.StartCoroutine(this.FixingDolphinStuckAni());
                break;
        }
       
    }

    private IEnumerator FixingDolphinStuckAni()
    {
        var timer = 0.05f;
        var currentTime = 0.0f;

        while (currentTime <= timer)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
        this.dolphinFIx = null;
    }

    private void ApplyingMoveAbiEffect(Vector3 moveDirInput, string currentForm, physicStates currentPHState, List<GameObject> forms)
    {

        switch (currentForm)
        {
            case "Standard Form":


                //this.animatorLink = forms[0].GetComponent<Animator>();

                this.animatorLink.SetFloat("VerticalMove", this.finalMoveDirTemp.normalized.y);

                if (this.animatorLink.GetFloat("VerticalMove") == 0 && !this.animatorLink.GetBool("Ground"))
                {
                    this.animatorLink.SetBool("Ground", true);
                    this.animatorLink.SetTrigger("Landing");
                    this.StartCoroutine(this.EnablingVariableSpeed(this.delayFlagRefFix));
                }
                else if (this.animatorLink.GetFloat("VerticalMove") != 0 && this.animatorLink.GetBool("Ground"))
                {
                    this.animatorLink.SetBool("Ground", false);
                    this.delayFlagRefFix[0] = false;
                }

                switch (currentPHState)
                {
                    case physicStates.onAir:
                        this.moveSelected.Invoke(moveDirInput, this.generalValues.moveInAir);
                        break;
                    case physicStates.onWater:
                        if (Math.Abs(moveDirInput.sqrMagnitude) > Tolerance)
                        {
                            this.moveSelected.Invoke(moveDirInput, this.generalValues.moveInWater);
                        }
                        this.animatorLink.SetFloat("Moving", Mathf.InverseLerp(0, (float)Math.Pow(this.currentMoveValues.standMove.moveSpeed, 2), (moveDirInput * this.generalValues.moveInWater).sqrMagnitude));
                        break;
                    case physicStates.onGround:
                        if (Math.Abs(moveDirInput.sqrMagnitude) > Tolerance)
                        {
                            this.moveSelected.Invoke(moveDirInput, this.currentMoveValues.standMove.moveSpeed);
                        }
                        this.animatorLink.SetFloat("Moving", Mathf.InverseLerp(0, (float)Math.Pow(this.currentMoveValues.standMove.moveSpeed, 2), (moveDirInput * this.currentMoveValues.standMove.moveSpeed).sqrMagnitude));
                        break;
                }

                if (this.animatorLink.GetBool("Ground") && this.delayFlagRefFix[0])
                {
                    this.animatorLink.speed = Math.Abs(moveDirInput.sqrMagnitude) > Tolerance ? this.animatorLink.GetFloat("Moving") : 1;
                }
                else
                {
                    this.animatorLink.speed = 1;
                }

                break;

            case "Frog Form":

                //this.animatorLink = forms[1].GetComponent<Animator>();

                this.animatorLink.SetFloat("VerticalMove", this.finalMoveDirTemp.normalized.y);

                if (this.animatorLink.GetFloat("VerticalMove") == 0 && !this.animatorLink.GetBool("Ground"))
                {
                    this.animatorLink.SetBool("Ground", true);
                    this.animatorLink.SetTrigger("Landing");
                    this.StartCoroutine(this.EnablingVariableSpeed(this.delayFlagRefFix));
                }
                else if (this.animatorLink.GetFloat("VerticalMove") != 0 && this.animatorLink.GetBool("Ground"))
                {
                    this.animatorLink.SetBool("Ground", false);
                    this.delayFlagRefFix[0] = false;
                }

                switch (currentPHState)
                {
                    case physicStates.onAir:
                        this.moveSelected.Invoke(moveDirInput, this.generalValues.moveInAir);
                        break;
                    case physicStates.onWater:
                        if (Math.Abs(moveDirInput.sqrMagnitude) > Tolerance)
                        {
                            this.moveSelected.Invoke(moveDirInput, this.generalValues.moveInWater);
                        }
                        this.animatorLink.SetFloat("Moving", Mathf.InverseLerp(0, (float)Math.Pow(this.currentMoveValues.frogMove.moveSpeed, 2), (moveDirInput * this.generalValues.moveInWater).sqrMagnitude));
                        break;
                    case physicStates.onGround:
                        if (Math.Abs(moveDirInput.sqrMagnitude) > Tolerance)
                        {
                            this.moveSelected.Invoke(moveDirInput, this.currentMoveValues.standMove.moveSpeed);
                        }
                        this.animatorLink.SetFloat("Moving", Mathf.InverseLerp(0, (float)Math.Pow(this.currentMoveValues.frogMove.moveSpeed, 2), (moveDirInput * this.currentMoveValues.frogMove.moveSpeed).sqrMagnitude));
                        break;
                }

                if (this.animatorLink.GetBool("Ground") && this.delayFlagRefFix[0])
                {
                    this.animatorLink.speed = Math.Abs(moveDirInput.sqrMagnitude) > Tolerance ? this.animatorLink.GetFloat("Moving") : 1;
                }
                else
                {
                    this.animatorLink.speed = 1;
                }

                break;

            case "Dragon Form":

                //this.animatorLink = forms[2].GetComponent<Animator>();

                if (this.animatorLink.GetBool("Ground"))
                    this.animatorLink.SetBool("Ground", false);

                this.moveSelected.Invoke(moveDirInput, this.currentMoveValues.craneMove.glideSpeed);

                this.animatorLink.SetFloat("Moving", Mathf.InverseLerp(0, (float)Math.Pow(this.currentMoveValues.craneMove.glideSpeed, 2), (moveDirInput * this.currentMoveValues.craneMove.glideSpeed).sqrMagnitude));

                break;
            case "Armadillo Form":

                switch (currentPHState)
                {
                    case physicStates.onAir:
                        moveSelected.Invoke(moveDirInput, generalValues.moveInAir);
                        break;
                    case physicStates.onWater:
                        moveSelected.Invoke(moveDirInput, generalValues.moveInWater);
                        break;
                    case physicStates.onGround:
                        moveSelected.Invoke(moveDirInput, currentMoveValues.armaMove.moveSpeed);
                        break;
                }

                break;
            case "Dolphin Form":

                if (this.dolphinFIx != null) return;

                //this.animatorLink = forms[4].GetComponent<Animator>();

                this.animatorLink.SetFloat("VerticalMove", this.finalMoveDirTemp.normalized.y);

                if (this.animatorLink.GetFloat("VerticalMove") == 0 && !this.animatorLink.GetBool("Water") && !this.animatorLink.GetBool("Ground"))
                {
                    this.animatorLink.SetBool("Water", true);
                    this.animatorLink.SetTrigger("Landing");
                    this.StartCoroutine(this.EnablingVariableSpeed(this.delayFlagRefFix));
                }
                else if (this.animatorLink.GetFloat("VerticalMove") != 0 && this.animatorLink.GetBool("Water"))
                {
                    this.animatorLink.SetBool("Water", false);
                    this.delayFlagRefFix[0] = false;
                }

                switch (currentPHState)
                {
                    case physicStates.onAir:
                        this.moveSelected.Invoke(moveDirInput, this.generalValues.moveInAir);
                        break;
                    case physicStates.onWater:

                        if (!this.animatorLink.GetBool("Water"))
                            this.animatorLink.SetBool("Water", true);

                        if (Math.Abs(moveDirInput.sqrMagnitude) > Tolerance)
                        {
                            this.moveSelected.Invoke(moveDirInput, this.currentMoveValues.dolphinMove.swimSpeed);
                        }
                        this.animatorLink.SetFloat("Moving", Mathf.InverseLerp(0, (float)Math.Pow(this.currentMoveValues.dolphinMove.swimSpeed, 2), (moveDirInput * this.currentMoveValues.dolphinMove.swimSpeed).sqrMagnitude));
                        break;
                }

                if (this.animatorLink.GetBool("Water") && this.delayFlagRefFix[0])
                {
                    this.animatorLink.speed = Math.Abs(moveDirInput.sqrMagnitude) > Tolerance ? this.animatorLink.GetFloat("Moving") : 1;
                }
                else
                {
                    this.animatorLink.speed = 1;
                }

                break;

        }
    }

    private void ApplyingAbilityEffect(abilties abiUsed, string currentForm, List<GameObject> forms)
    {
        switch (abiUsed)
        {

            case abilties.jump:
                switch (currentForm)
                {
                    case ("Standard Form"):
                        var aniTempStd = forms[0].GetComponent<Animator>();
                        aniTempStd.SetTrigger("Jumping");
                        jumpSelected.Invoke(currentMoveValues.standMove.jumpStrength);
                        break;
                    case ("Frog Form"):
                        var aniTempFrog = forms[1].GetComponent<Animator>();
                        aniTempFrog.SetTrigger("Jumping");
                        jumpSelected.Invoke(currentMoveValues.frogMove.jumpStrength);
                        break;
                    case ("Dolphin Form"):
                        var aniTempDolphin = forms[4].GetComponent<Animator>();
                        aniTempDolphin.SetTrigger("Jumping");
                        jumpSelected.Invoke(currentMoveValues.dolphinMove.jumpStrength);
                        break;
                }
                break;
            case abilties.roll:
                rollSelected.Invoke(currentMoveValues.armaMove.rollingStrength);
                break;

        }
    }

    private void ApplyingRotationEffect(Vector3 abiDirInput, playerStates currentPl)
    {

        switch (currentPl)
        {

            case playerStates.flying:

            case playerStates.movingBlock:
                rotSelected.Invoke(abiDirInput, generalValues.rotateSpeed / 3);
                break;
            case playerStates.rolling:
                specialRotSelected.Invoke(abiDirInput, generalValues.rotateSpeed / 3);
                break;
            default:
                rotSelected.Invoke(abiDirInput, generalValues.rotateSpeed);
                break;


        }

    }
    #endregion

    #region Special Animation Ability Handler Methods
    private void ApplyingSpecialAniAbi(VFissure vfTempLink, string vfTag)
    {
        bool vFissureAniOn = true;

        if (vfTag == "vAbilityta" || vfTag == "vAbilitytb")
            StartCoroutine(VFissureExecution(vFissureAniOn, vfTempLink, vfTag));
        else if (vfTag == "hAbilityta" || vfTag == "hAbilitytb")
            StartCoroutine(HFissureExecution(vFissureAniOn, vfTempLink, vfTag));
        else
            StartCoroutine(DolpSwimBExecution(vFissureAniOn, vfTempLink, vfTag));
    }

    private IEnumerator VFissureExecution(bool vFissureAniOn, VFissure vfTempLink, string vfEntrance)
    {

        CharacterController ccTempLink = this.gameObject.GetComponent<CharacterController>();

        float radius = ccTempLink.radius;

        ccTempLink.radius = 0;

        bool secondRotationisOn = false, moveFinished = false, secondMoveIsOn = false;

        if (vfTempLink == null)
            Debug.Log("Cazzo");

        Quaternion vTriggerRotation, vGuidanceRotation;
        Vector3 vTriggerMidPosition, vGuidanceFinPosition, vGuidanceDir;

        if (vfEntrance == "vAbilityta")
        {
            vTriggerRotation = vfTempLink.aTrigger.transform.rotation;
            vGuidanceRotation = vfTempLink.mGuidance.transform.rotation;


            vTriggerMidPosition = vfTempLink.aTrigger.transform.position;
            vTriggerMidPosition.y = this.transform.position.y;

            vGuidanceFinPosition = vfTempLink.exitA.transform.position;
            vGuidanceFinPosition.y = this.transform.position.y;


            vGuidanceDir = vfTempLink.mGuidance.transform.right;
        }
        else
        {
            vTriggerRotation = vfTempLink.bTrigger.transform.rotation;
            vGuidanceRotation = vfTempLink.mGuidance.transform.rotation;


            vTriggerMidPosition = vfTempLink.bTrigger.transform.position;
            vTriggerMidPosition.y = this.transform.position.y;

            vGuidanceFinPosition = vfTempLink.exitB.transform.position;
            vGuidanceFinPosition.y = this.transform.position.y;


            vGuidanceDir = -vfTempLink.mGuidance.transform.right;
        }

        while (vFissureAniOn)
        {
            if (Quaternion.Angle(this.transform.rotation, vTriggerRotation) > 0.1f && !secondRotationisOn)
            {

                this.transform.localRotation = Quaternion.Slerp(this.transform.rotation, vTriggerRotation, Time.deltaTime * 5);
                //Debug.Log(Quaternion.Angle(this.transform.rotation, coreLink.vTriggerRotation));
            }
            else if (moveFinished)
            {
                vFissureAniOn = false;
                moveFinished = false;
                secondMoveIsOn = false;
            }
            else
            {



                Vector3 distance = vTriggerMidPosition - this.transform.position;
                //Debug.Log(distance.sqrMagnitude);
                if (distance.sqrMagnitude >= 0.05f && !secondMoveIsOn)
                {

                    Vector3 direction = (vTriggerMidPosition - this.transform.position).normalized;
                    direction.y = 0;
                    this.transform.position += direction * Time.deltaTime * 4;
                }
                else
                {
                    secondRotationisOn = true;

                    if (Quaternion.Angle(this.transform.rotation, vGuidanceRotation) > 0.1f)

                        this.transform.rotation = Quaternion.Slerp(this.transform.localRotation, vGuidanceRotation, Time.deltaTime * 5);
                    else
                    {
                        secondMoveIsOn = true;

                        vGuidanceFinPosition.y = this.transform.position.y;
                        distance = vGuidanceFinPosition - this.transform.position;
                        //Debug.Log(distance.sqrMagnitude);

                        if (distance.sqrMagnitude >= 0.1f)
                        {

                            // Vector3 direction = (coreLink.vGuidanceFinPosition - this.transform.position).normalized;
                            Vector3 direction = vGuidanceDir.normalized;
                            direction.y = 0;
                            this.transform.position += direction * Time.deltaTime * 4;
                        }
                        else
                        {
                            moveFinished = true;
                            secondRotationisOn = false;
                        }

                    }
                }
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        vFissureAniOn = false;
        vFissureAniEnded.Invoke();
        ccTempLink.radius = radius;
    }

    private IEnumerator HFissureExecution(bool vFissureAniOn, VFissure vfTempLink, string vfEntrance)
    {

        CharacterController ccTempLink = this.gameObject.GetComponent<CharacterController>();

        float radius = ccTempLink.radius;

        ccTempLink.enableOverlapRecovery = false;

        //ccTempLink.radius = 0;
        //ccTempLink.height = 0;

        bool moveFinished = false, secondMoveIsOn = false;

        if (vfTempLink == null)
            Debug.Log("Cazzo");

        Quaternion vTriggerRotation, vGuidanceRotation;
        Vector3 vTriggerMidPosition, vGuidanceFinPosition, vGuidanceDir;

        if (vfEntrance == "hAbilityta")
        {
            vTriggerRotation = vfTempLink.aTrigger.transform.rotation;
            vGuidanceRotation = vfTempLink.mGuidance.transform.rotation;


            vTriggerMidPosition = vfTempLink.aTrigger.transform.position;
            vTriggerMidPosition.y = this.transform.position.y;

            vGuidanceFinPosition = vfTempLink.exitA.transform.position;
            vGuidanceFinPosition.y = this.transform.position.y;


            vGuidanceDir = vfTempLink.mGuidance.transform.right;
        }
        else
        {
            vTriggerRotation = vfTempLink.bTrigger.transform.rotation;
            vGuidanceRotation = vfTempLink.mGuidance.transform.rotation;


            vTriggerMidPosition = vfTempLink.bTrigger.transform.position;
            vTriggerMidPosition.y = this.transform.position.y;

            vGuidanceFinPosition = vfTempLink.exitB.transform.position;
            vGuidanceFinPosition.y = this.transform.position.y;


            vGuidanceDir = -vfTempLink.mGuidance.transform.right;
        }

        while (vFissureAniOn)
        {
            if (Quaternion.Angle(this.transform.rotation, vTriggerRotation) > 0.1f)
            {

                this.transform.localRotation = Quaternion.Slerp(this.transform.rotation, vTriggerRotation, Time.deltaTime * 5);
                //Debug.Log(Quaternion.Angle(this.transform.rotation, coreLink.vTriggerRotation));
            }
            else if (moveFinished)
            {
                vFissureAniOn = false;
                moveFinished = false;
                secondMoveIsOn = false;
            }
            else
            {



                Vector3 distance = vTriggerMidPosition - this.transform.position;

                if (distance.sqrMagnitude >= 0.5f && !secondMoveIsOn)
                {
                    //Debug.Log(distance.sqrMagnitude);
                    Vector3 direction = (vTriggerMidPosition - this.transform.position).normalized;
                    direction.y = 0;
                    this.transform.position += direction * Time.deltaTime * 4;
                }
                else
                {

                    secondMoveIsOn = true;

                    distance = vGuidanceFinPosition - this.transform.position;

                    if (distance.sqrMagnitude >= 0.55f)
                    {

                        // Vector3 direction = (coreLink.vGuidanceFinPosition - this.transform.position).normalized;
                        Vector3 direction = vGuidanceDir.normalized;
                        direction.y = 0;
                        this.transform.position += direction * Time.deltaTime * 4;
                    }
                    else
                    {
                        moveFinished = true;

                    }


                }
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        vFissureAniOn = false;
        vFissureAniEnded.Invoke();

        ccTempLink.enableOverlapRecovery = true;
        //ccTempLink.radius = radius;
        //ccTempLink.height = 0.7f;
    }

    private IEnumerator DolpSwimBExecution(bool vFissureAniOn, VFissure vfTempLink, string vfEntrance)
    {

        CharacterController ccTempLink = this.gameObject.GetComponent<CharacterController>();

        float radius = ccTempLink.radius;

        ccTempLink.radius = 0;

        bool moveFinished = false, secondMoveIsOn = false;

        if (vfTempLink == null)
            Debug.Log("Cazzo");

        Quaternion vTriggerRotation, vGuidanceRotation;
        Vector3 vTriggerMidPosition, vGuidanceFinPosition, vGuidanceDir;

        if (vfEntrance == "dAbilityta")
        {
            vTriggerRotation = vfTempLink.aTrigger.transform.rotation;
            vGuidanceRotation = vfTempLink.mGuidance.transform.rotation;


            vTriggerMidPosition = vfTempLink.aTrigger.transform.position;
            vTriggerMidPosition.y = this.transform.position.y;

            vGuidanceFinPosition = vfTempLink.exitA.transform.position;
            vGuidanceFinPosition.y = this.transform.position.y;


            vGuidanceDir = -vfTempLink.mGuidance.transform.right;
        }
        else
        {
            vTriggerRotation = vfTempLink.bTrigger.transform.rotation;
            vGuidanceRotation = vfTempLink.mGuidance.transform.rotation;


            vTriggerMidPosition = vfTempLink.bTrigger.transform.position;
            vTriggerMidPosition.y = this.transform.position.y;

            vGuidanceFinPosition = vfTempLink.exitB.transform.position;
            vGuidanceFinPosition.y = this.transform.position.y;


            vGuidanceDir = vfTempLink.mGuidance.transform.right;
        }

        while (vFissureAniOn)
        {
            if (Quaternion.Angle(this.transform.rotation, vTriggerRotation) > 0.1f)
            {

                this.transform.localRotation = Quaternion.Slerp(this.transform.rotation, vTriggerRotation, Time.deltaTime * 5);
                //Debug.Log(Quaternion.Angle(this.transform.rotation, coreLink.vTriggerRotation));
            }
            else if (moveFinished)
            {
                vFissureAniOn = false;
                moveFinished = false;
                secondMoveIsOn = false;
            }
            else
            {


                Vector3 distance = vTriggerMidPosition - this.transform.position;

                if (distance.sqrMagnitude >= 0.5f && !secondMoveIsOn)
                {
                    //Debug.Log(distance.sqrMagnitude);
                    Vector3 direction = (vTriggerMidPosition - this.transform.position).normalized;
                    direction.y = 0;
                    this.transform.position += direction * Time.deltaTime * 4;
                }
                else
                {

                    secondMoveIsOn = true;
                    this.transform.position = vGuidanceFinPosition;
                    moveFinished = true;

                    /*
                    distance = vGuidanceFinPosition - this.transform.position;

                    if (distance.sqrMagnitude >= 0.55f)
                    {

                        // Vector3 direction = (coreLink.vGuidanceFinPosition - this.transform.position).normalized;
                        Vector3 direction = vGuidanceDir.normalized;
                        direction.y = 0;
                        this.transform.position += direction * Time.deltaTime * 4;
                    }
                    else
                    {
                        moveFinished = true;

                    }
                    */

                }
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        vFissureAniOn = false;
        vFissureAniEnded.Invoke();
        ccTempLink.radius = radius;
    }
    #endregion

    #region Animator Handler
    private IEnumerator EnablingVariableSpeed(List<bool> ciccio)
    {
        var timer = 0.0f;
        while (timer < 0.2f)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        ciccio[0] = true;
    }

    private void TakingFinalMoveUpdated(Vector3 currentMove)
    {
        this.finalMoveDirTemp = currentMove;
    }

    private void SettingAniOnGround(bool isOnGround)
    {
        this.animatorLink.SetBool("Ground", isOnGround);
    }

    private void SettingAniDolphinOnGround(bool isOnGround)
    {
        this.animatorLink.SetBool("Ground", isOnGround);
        this.animatorLink.SetBool("Water", false);
    }
    #endregion
}
