using System;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEditor.Animations;

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

    public AnimatorController[] StoryAni;
    public Animator[] AnimatorRefs;

    public List<GameObject> Forms;
    #endregion

    #region Events
    public event_vector3_float moveSelected, rotSelected, specialRotSelected;
    public event_float jumpSelected, rollSelected;
    public event_ps phChangeEffect;
    public UnityEvent vFissureAniEnded;
    #endregion

    #region Private Variables
    PlayerInputs playerref;
    FSMChecker fsmLinker;
    SoundManager soundManagerLinker;


    //Animator Manager variables
    private Vector3 finalMoveDirTemp;
    private List<bool> delayFlagRefFix = new List<bool>();
    private Animator animatorLink;
    private Coroutine dolphinFIx;
    private Coroutine specialRollAni;


    private Quaternion armaOriginarRot;

    private bool noControlStory;
    #endregion

    #region Taking References and linking Events
    void Awake()
    {
        playerref = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputs>();
        fsmLinker = GameObject.FindGameObjectWithTag("Player").GetComponent<FSMChecker>();
        soundManagerLinker = GameObject.FindGameObjectWithTag("Player").GetComponent<SoundManager>();

        var fsmCheckerTempLink = this.gameObject.GetComponent<FSMChecker>();

        fsmCheckerTempLink.formChanged.AddListener(this.ApplyingFormEffect);
        fsmCheckerTempLink.moveUsed.AddListener(this.ApplyingMoveAbiEffect);
        fsmCheckerTempLink.genAbiUsed.AddListener(this.ApplyingAbilityEffect);
        fsmCheckerTempLink.rotationUsed.AddListener(this.ApplyingRotationEffect);
        fsmCheckerTempLink.vFissureUsed.AddListener(this.ApplyingSpecialAniAbi);
        fsmCheckerTempLink.swallowOnGround.AddListener(this.SettingAniOnGround);
        fsmCheckerTempLink.dolphinOnGround.AddListener(this.SettingAniDolphinOnGround);
        fsmCheckerTempLink.armaRolling.AddListener(this.SettingRolling);
        fsmCheckerTempLink.aniStoryInitRequest.AddListener(this.SettingStoryAnimator);
        fsmCheckerTempLink.aniNormalInitRequest.AddListener(this.SettingNormalAnimators);

        var moveHandTempLink = this.gameObject.GetComponent<MoveHandler>();

        moveHandTempLink.UpdatedFinalMoveRequest.AddListener(this.TakingFinalMoveUpdated);

        this.delayFlagRefFix.Add(true);

        GameObject storyLineCheck = GameObject.FindGameObjectWithTag("StoryLine");

        if (storyLineCheck != null)
        {

            StoryLineInstance slTempLink =
                GameObject.FindGameObjectWithTag("StoryLine").GetComponent<StoryLineInstance>();

            if (slTempLink != null)
            {
                slTempLink.AniRequest.AddListener(this.SettingAnimation);
            }
        }
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

        playerref.FormSound();
        soundManagerLinker.PersistendAudio[1].AudioSourceRef.Stop();

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
            case "Armadillo Form":
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

        if (this.noControlStory) return;

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
                        if (moveDirInput.sqrMagnitude == 0 && fsmLinker.isWalkingSound)
                        {
                            Debug.Log("STANDARD FERMO");
                            playerref.StopStandardWalk();
                            fsmLinker.isWalkingSound = false;
                        }
                        else if (moveDirInput.sqrMagnitude > 0.1f && !fsmLinker.isWalkingSound)
                        {
                            Debug.Log("STANDARD MUOVO");
                            playerref.StandardWalk();
                            fsmLinker.isWalkingSound = true;
                        }
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
                        if (moveDirInput.sqrMagnitude == 0 && fsmLinker.isWalkingSound)
                        {

                            Debug.Log("magnitude" + moveDirInput.sqrMagnitude);
                            playerref.StopFrogWalk();
                            fsmLinker.isWalkingSound = false;

                        }
                        else if (moveDirInput.sqrMagnitude > 0.1f && !fsmLinker.isWalkingSound)
                        {
                            Debug.Log("magnitude2" + moveDirInput.sqrMagnitude);
                            playerref.FrogWalk();
                            fsmLinker.isWalkingSound = true;
                        }
                        if (Math.Abs(moveDirInput.sqrMagnitude) > Tolerance)
                        {
                            this.moveSelected.Invoke(moveDirInput, this.currentMoveValues.frogMove.moveSpeed);
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

                        if (Math.Abs(moveDirInput.sqrMagnitude) > Tolerance)
                        {
                            this.moveSelected.Invoke(moveDirInput, this.generalValues.moveInAir);
                        }
                        this.animatorLink.SetFloat("Moving", Mathf.InverseLerp(0, (float)Math.Pow(this.currentMoveValues.armaMove.moveSpeed, 2), (moveDirInput * this.generalValues.moveInAir).sqrMagnitude));
                        break;

                    case physicStates.onWater:

                        if (Math.Abs(moveDirInput.sqrMagnitude) > Tolerance)
                        {
                            this.moveSelected.Invoke(moveDirInput, this.generalValues.moveInWater);
                        }
                        this.animatorLink.SetFloat("Moving", Mathf.InverseLerp(0, (float)Math.Pow(this.currentMoveValues.armaMove.moveSpeed, 2), (moveDirInput * this.generalValues.moveInWater).sqrMagnitude));
                        break;

                    case physicStates.onGround:

                        if (moveDirInput.sqrMagnitude == 0 && fsmLinker.isWalkingSound)
                        {
                            Debug.Log("magnitude" + moveDirInput.sqrMagnitude);
                            playerref.StopStandardWalk();
                            fsmLinker.isWalkingSound = false;
                        }
                        else if (moveDirInput.sqrMagnitude > 0.1f && !fsmLinker.isWalkingSound)
                        {
                            Debug.Log("magnitude2" + moveDirInput.sqrMagnitude);
                            playerref.StandardWalk();
                            fsmLinker.isWalkingSound = true;
                        }
                        if (Math.Abs(moveDirInput.sqrMagnitude) > Tolerance)
                        {
                            this.moveSelected.Invoke(moveDirInput, this.currentMoveValues.armaMove.moveSpeed);
                        }
                        this.animatorLink.SetFloat("Moving", Mathf.InverseLerp(0, (float)Math.Pow(this.currentMoveValues.armaMove.moveSpeed, 2), (moveDirInput * this.currentMoveValues.armaMove.moveSpeed).sqrMagnitude));
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
                    case physicStates.onGround:
                        if (moveDirInput.sqrMagnitude == 0 && fsmLinker.isDolphinIdle)
                        {
                            Debug.Log("magnitude" + moveDirInput.sqrMagnitude);
                            playerref.DolphinIdle();
                            fsmLinker.isDolphinIdle = false;
                        }
                        
                        else if (moveDirInput.sqrMagnitude > 0.1f && !fsmLinker.isDolphinIdle)
                        {
                            Debug.Log("magnitude2" + moveDirInput.sqrMagnitude);
                            playerref.StopDolphinIdle();
                            fsmLinker.isDolphinIdle = true;
                        }
                        break;

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
                    case "Standard Form":
                        var aniTempStd = forms[0].GetComponent<Animator>();
                        aniTempStd.SetTrigger("Jumping");
                        playerref.JumpSound();
                        this.jumpSelected.Invoke(this.currentMoveValues.standMove.jumpStrength);
                        break;
                    case "Frog Form":
                        var aniTempFrog = forms[1].GetComponent<Animator>();
                        aniTempFrog.SetTrigger("Jumping");
                        playerref.JumpSound();
                        this.jumpSelected.Invoke(this.currentMoveValues.frogMove.jumpStrength);
                        break;
                    case "Dolphin Form":
                        var aniTempDolphin = forms[4].GetComponent<Animator>();
                        aniTempDolphin.SetTrigger("Jumping");
                        this.jumpSelected.Invoke(this.currentMoveValues.dolphinMove.jumpStrength);
                        break;
                }

                break;
            case abilties.roll:
                this.rollSelected.Invoke(this.currentMoveValues.armaMove.rollingStrength);
                break;
        }
    }

    private void ApplyingRotationEffect(Vector3 abiDirInput, playerStates currentPl)
    {
        switch (currentPl)
        {

            case playerStates.flying:

            case playerStates.movingBlock:
                this.rotSelected.Invoke(abiDirInput, this.generalValues.rotateSpeed / 3);
                break;
            case playerStates.rolling:
                this.specialRotSelected.Invoke(abiDirInput, this.generalValues.rotateSpeed / 3);
                break;
            default:
                this.rotSelected.Invoke(abiDirInput, this.generalValues.rotateSpeed);
                break;
        }
    }
    #endregion

    #region Special Animation Ability Handler Methods
    private void ApplyingSpecialAniAbi(VFissure vfTempLink, string vfTag)
    {
        bool vFissureAniOn = true;

        if (vfTag == "vAbilityta" || vfTag == "vAbilitytb")
            StartCoroutine(this.VFissureNewExecution(vFissureAniOn, vfTempLink, vfTag));
        else if (vfTag == "hAbilityta" || vfTag == "hAbilitytb")
            StartCoroutine(HFissureNewExecution(vFissureAniOn, vfTempLink, vfTag));
        else
            StartCoroutine(DolpSwimBExecution(vFissureAniOn, vfTempLink, vfTag));
    }

    private IEnumerator VFissureNewExecution(bool vFissureAniOn, VFissure vfTempLink, string vfEntrance)
    {
        CharacterController ccTempLink = this.gameObject.GetComponent<CharacterController>();

        float radius = ccTempLink.radius;

        ccTempLink.radius = 0;

        if (vfTempLink == null)
            Debug.Log("Cazzo");

        Quaternion vTriggerRotation, vGuidanceRotation;
        Vector3 vTriggerMidPosition, vGuidanceFinPosition;

        if (vfEntrance == "vAbilityta")
        {
            vTriggerRotation = vfTempLink.aTrigger.transform.rotation;
            vGuidanceRotation = vfTempLink.mGuidance.transform.rotation;


            vTriggerMidPosition = vfTempLink.aTrigger.transform.position;
            vTriggerMidPosition.y = this.transform.position.y;

            vGuidanceFinPosition = vfTempLink.exitA.transform.position;
            vGuidanceFinPosition.y = this.transform.position.y;
        }
        else
        {
            vTriggerRotation = vfTempLink.bTrigger.transform.rotation;
            vGuidanceRotation = vfTempLink.mGuidance.transform.rotation;


            vTriggerMidPosition = vfTempLink.bTrigger.transform.position;
            vTriggerMidPosition.y = this.transform.position.y;

            vGuidanceFinPosition = vfTempLink.exitB.transform.position;
            vGuidanceFinPosition.y = this.transform.position.y;
        }


        var timePassed = 0.0f;
        var timeTaken = 0.5f;
        var oriRot = this.gameObject.transform.rotation;
        var oriPos = this.gameObject.transform.position;


        while (timePassed <= 1)
        {
            timePassed += Time.deltaTime / timeTaken;
            this.transform.rotation = Quaternion.Slerp(oriRot, vTriggerRotation, timePassed);
            yield return null;
        }

        timePassed = 0.0f;
        oriPos = this.gameObject.transform.position;

        while (timePassed <= 1)
        {
            timePassed += Time.deltaTime / timeTaken;
            this.transform.position = Vector3.Lerp(oriPos, vTriggerMidPosition, timePassed);
            yield return null;
        }

        timePassed = 0.0f;
        oriRot = this.gameObject.transform.rotation;

        while (timePassed <= 1)
        {
            timePassed += Time.deltaTime / timeTaken;
            this.transform.rotation = Quaternion.Slerp(oriRot, vGuidanceRotation, timePassed);
            yield return null;
        }

        timePassed = 0.0f;
        timeTaken = 1f;
        oriPos = this.gameObject.transform.position;

        while (timePassed <= 1)
        {
            timePassed += Time.deltaTime / timeTaken;
            this.transform.position = Vector3.Lerp(oriPos, vGuidanceFinPosition, timePassed);
            yield return null;
        }

        this.vFissureAniEnded.Invoke();
        ccTempLink.radius = radius;
    }

    private IEnumerator HFissureNewExecution(bool vFissureAniOn, VFissure vfTempLink, string vfEntrance)
    {

        CharacterController ccTempLink = this.gameObject.GetComponent<CharacterController>();


        ccTempLink.enableOverlapRecovery = false;

        if (vfTempLink == null)
            yield break;

        Quaternion vTriggerRotation;
        Vector3 vTriggerMidPosition, vGuidanceFinPosition;

        if (vfEntrance == "hAbilityta")
        {
            vTriggerRotation = vfTempLink.aTrigger.transform.rotation;

            vTriggerMidPosition = vfTempLink.aTrigger.transform.position;
            vTriggerMidPosition.y = this.transform.position.y;

            vGuidanceFinPosition = vfTempLink.exitA.transform.position;
            vGuidanceFinPosition.y = this.transform.position.y;
        }
        else
        {
            vTriggerRotation = vfTempLink.bTrigger.transform.rotation;

            vTriggerMidPosition = vfTempLink.bTrigger.transform.position;
            vTriggerMidPosition.y = this.transform.position.y;

            vGuidanceFinPosition = vfTempLink.exitB.transform.position;
            vGuidanceFinPosition.y = this.transform.position.y;
        }

        var timePassed = 0.0f;
        var timeTaken = 0.5f;
        var oriRot = this.gameObject.transform.rotation;
        Vector3 oriPos;


        while (timePassed <= 1)
        {
            timePassed += Time.deltaTime / timeTaken;
            this.transform.rotation = Quaternion.Slerp(oriRot, vTriggerRotation, timePassed);
            yield return null;
        }

        timePassed = 0.0f;
        oriPos = this.gameObject.transform.position;
        vTriggerMidPosition = (vTriggerMidPosition + oriPos) / 2;

        while (timePassed <= 1)
        {
            timePassed += Time.deltaTime / timeTaken;
            this.transform.position = Vector3.Lerp(oriPos, vTriggerMidPosition, timePassed);
            yield return null;
        }

        timePassed = 0.0f;
        timeTaken = 1f;
        oriPos = this.gameObject.transform.position;

        while (timePassed <= 1)
        {
            timePassed += Time.deltaTime / timeTaken;
            this.transform.position = Vector3.Lerp(oriPos, vGuidanceFinPosition, timePassed);
            yield return null;
        }

        this.vFissureAniEnded.Invoke();

        ccTempLink.enableOverlapRecovery = true;
    }

    private IEnumerator DolpSwimBExecution(bool vFissureAniOn, VFissure vfTempLink, string vfEntrance)
    {

        CharacterController ccTempLink = this.gameObject.GetComponent<CharacterController>();

        float radius = ccTempLink.radius;

        ccTempLink.radius = 0;

        bool moveFinished = false, secondMoveIsOn = false;

        if (vfTempLink == null)
            Debug.Log("Cazzo");

        Quaternion vTriggerRotation;
        Vector3 vTriggerMidPosition, vGuidanceFinPosition;

        if (vfEntrance == "dAbilityta")
        {
            vTriggerRotation = vfTempLink.aTrigger.transform.rotation;



            vTriggerMidPosition = vfTempLink.aTrigger.transform.position;
            vTriggerMidPosition.y = this.transform.position.y;

            vGuidanceFinPosition = vfTempLink.exitA.transform.position;
            vGuidanceFinPosition.y = this.transform.position.y;

        }
        else
        {
            vTriggerRotation = vfTempLink.bTrigger.transform.rotation;



            vTriggerMidPosition = vfTempLink.bTrigger.transform.position;
            vTriggerMidPosition.y = this.transform.position.y;

            vGuidanceFinPosition = vfTempLink.exitB.transform.position;
            vGuidanceFinPosition.y = this.transform.position.y;

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

    private void SettingRolling(bool isRolling)
    {
        if (this.gameObject.GetComponentInChildren<Animator>().name == "Armadillo")
            this.animatorLink.SetBool("Rolling", isRolling);

        if (isRolling)
        {
            if (this.gameObject.GetComponentInChildren<Animator>().name == "Armadillo")
                this.specialRollAni = this.StartCoroutine(this.SpecialRollingAni());

        }
        else
        {
            if (this.gameObject.GetComponentInChildren<Animator>().name == "Armadillo" && this.specialRollAni != null)
            {
                this.StopCoroutine(this.specialRollAni);
                this.specialRollAni = null;
                this.StartCoroutine(this.SpecialRollingAniReturn());

            }
            else
            {
                if (this.specialRollAni != null)
                {
                    this.StopCoroutine(this.specialRollAni);
                    this.specialRollAni = null;
                }

            }
        }
    }

    private IEnumerator SpecialRollingAni()
    {
        var armadillo = GameObject.FindGameObjectWithTag("Armadillo Form");

        var armaTransf = armadillo.transform;

        this.armaOriginarRot = armaTransf.localRotation;

        while (true)
        {
            armaTransf.RotateAround(armaTransf.position, armaTransf.right, Time.deltaTime * 1440f);

            yield return null;
        }
    }

    private IEnumerator SpecialRollingAniReturn()
    {
        var armadillo = GameObject.FindGameObjectWithTag("Armadillo Form");

        var armaTransf = armadillo.transform;

        var startingRot = armaTransf.localRotation;

        var timePassed = 0f;

        var timeTaken = 0.05f;

        while (timePassed <= 1)
        {
            timePassed += Time.deltaTime / timeTaken;

            armaTransf.localRotation = Quaternion.Slerp(startingRot, this.armaOriginarRot, timePassed);
            yield return null;
        }

        armaTransf.localRotation = this.armaOriginarRot;
    }

    private void SettingStoryAnimator()
    {
        this.noControlStory = true;

        this.AnimatorRefs[0].runtimeAnimatorController = this.StoryAni[0];
        this.AnimatorRefs[1].runtimeAnimatorController = this.StoryAni[1];
        this.AnimatorRefs[2].runtimeAnimatorController = this.StoryAni[2];
        this.AnimatorRefs[3].runtimeAnimatorController = this.StoryAni[3];
        this.AnimatorRefs[4].runtimeAnimatorController = this.StoryAni[4];

    }

    private void SettingNormalAnimators()
    {

        this.AnimatorRefs[0].runtimeAnimatorController = this.StoryAni[5];
        this.AnimatorRefs[1].runtimeAnimatorController = this.StoryAni[6];
        this.AnimatorRefs[2].runtimeAnimatorController = this.StoryAni[7];
        this.AnimatorRefs[3].runtimeAnimatorController = this.StoryAni[8];
        this.AnimatorRefs[4].runtimeAnimatorController = this.StoryAni[9];

        this.noControlStory = false;
    }

    private void SettingAnimation(int index)
    {
        var cForm = this.Forms.Find(x => x.activeInHierarchy == true);

        var animator = cForm.GetComponent<Animator>();

        animator.SetInteger("AniIndex", index);
    }
    #endregion
}
