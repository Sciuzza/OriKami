using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;


/*
#region Finite State Machine Structure
public enum forms { standard, frog, crane, armadillo, dolphin };

public enum physicStates { onAir, onGround, onWater }

public enum primaryAbilityStates { roll, dolJumpAbove, dolJumpBelow, Jump, fly, noAbility }

public enum secondaryAbilityStates { vFissure, hFissure, movingBlock, noAbility }

public enum tertiaryAbilityStates { switchToDolphin, switchByDolphin, noAbility }

public enum quaternaryAbilityStates { noAbility };

public enum states { standingStill, walking, falling, abilityAnimation };

public enum control { totalControl, noMoveControl, noCameraControl, noSpecialInputsControl, noMoveAndCamera, noCameraAndSpecial, noMoveAndSpecial, noControl };


[System.Serializable]
public struct playerState
{
    public forms currentForm;
    public forms previousForm;
    public List<GameObject> forms;
    public physicStates currentPState;
    public primaryAbilityStates currentPAState;
    public secondaryAbilityStates currentSAState;
    public tertiaryAbilityStates currentTAState;
    public quaternaryAbilityStates currentQAState;
    public states currentState;
    public control currentControl;
}
#endregion
*/
/*
public class PlCore : MonoBehaviour
{


    public UnityEvent brokeSomething;
    public UnityEvent activateBridge;
    public UnityEvent activateStairs;
    public UnityEvent activateWaterStairs;
    public UnityEvent activateRamp;

    public float weight = 10.0f;




    [SerializeField]
    private moveValues currentMoveValues;
    public moveValues CurrentMoveValues
    {
        get
        {
            return currentMoveValues;
        }

        set
        {
            currentMoveValues = value;
        }
    }

    [SerializeField]
    private generalTweaks generalValues;
    public generalTweaks GeneralValues
    {
        get
        {
            return generalValues;
        }

        set
        {
            generalValues = value;
        }
    }

    [SerializeField]
    playerState cPlayerState;

    CharacterController ccLink;


    // to be removed
    public string currentActForm = "Standard Form";
    public GameObject frog, standard, dragon, armadillo, dolphin;
    public forms currentForm = forms.standard;
    public forms previousForm = forms.standard;
    public bool isInAir = false;  // false if it is on ground or in water, true if it is on air, could be replaced by isOnGround Character Controller
    public bool isInWater = false;
    public bool stMoveEnabled = true; // to be disabled on Special Abilities that will provide script controlled movement
    public bool isRolling = false;    // only when current form is Armadillo and 
    public bool isFlying = false;     // only true when isInAir is true and current form is crane
    public bool isArmaMoving = false;
    public bool dolphinInAbility = false;
    public bool dolphinOutAbility = false;

    // to be checked or removed or moved
    public bool vFissureAbilityisOn = false, secondRotationisOn = false, secondMoveIsOn = false, moveFinished = false;
    public Quaternion vTriggerRotation, vGuidanceRotation;
    public Vector3 vTriggerMidPosition, vGuidanceFinPosition;
    public Vector3 vGuidanceDir;
    public float rollingTime = 0.0f;
    public Vector3 jumpOutFw, jumpOutUp, jumpInFw, jumpInUp;
    public Quaternion jumpInRot, jumpOutRot;


    private Vector3 jumpDirection, fallDirection;

    private void Awake()
    {
        #region Initializing Player Data
       // SettingDefaultValues();

        SettingStandardForm();

        SettingCPState();
        #endregion

        #region Check for Missing Components and Initializing them if present
        InterCC interactionLink = this.gameObject.GetComponent<InterCC>();

        if (interactionLink != null)
        {

            interactionLink.frogForm.AddListener(SwitchToFrog);
            interactionLink.craneForm.AddListener(SwitchToCrane);
            interactionLink.armaForm.AddListener(SwitchToArma);
            interactionLink.dolphinForm.AddListener(SwitchToDolphin);

        }
        else
            Debug.LogWarning("Missing InterCC");


        MoveCC moveLink = this.gameObject.GetComponent<MoveCC>();


        if (moveLink != null)
        {
            moveLink.Moving.AddListener(MoveHandler);
            moveLink.priAbilityInput.AddListener(PrimaryAbilityHandler);
            moveLink.secAbilityInput.AddListener(SecondaryAbilityHandler);
            moveLink.terAbilityInput.AddListener(TertiaryAbilityHandler);
            moveLink.quaAbilityInput.AddListener(QuaternaryAbilityHandler);

            moveLink.isNotMoving.AddListener(ChangingStateToStandingStill);
        }
        else
            Debug.LogWarning("Missing MoveCC");


        ccLink = this.gameObject.GetComponent<CharacterController>();

        if (ccLink == null)
            Debug.LogWarning("Missing Character Controller");
        #endregion
    }



    // to be checked or removed or moved
    void OnTriggerEnter(Collider objectHit)
    {

        if (objectHit.gameObject.GetComponentInParent<DestroyableObjects>()) // && isRolling)
        {
            objectHit.gameObject.GetComponentInParent<DestroyableObjects>().DestroyingMySelf();
        }

        if (objectHit.gameObject.GetComponent<ButtonActivator>() != null)
        {
            activateBridge.Invoke();
        }

        if (objectHit.gameObject.GetComponent<ButtonStairs>() != null)
        {
           activateStairs.Invoke();
        }
        if (objectHit.gameObject.GetComponent<ButtonStairsWater>() != null)
        {
            activateWaterStairs.Invoke();
        }
        if (objectHit.gameObject.GetComponent<ButtonRamp>() != null)
        {
            activateWaterStairs.Invoke();
        }
        if (objectHit.gameObject.name == "DeathTrigger")
        {
            Application.LoadLevel(Application.loadedLevel);
        }



        DolphinTriggersActivation(objectHit);


    }

    void OnTriggerStay(Collider objectHit)
    {

        vAbilityTriggers(objectHit);



    }

    void OnTriggerExit(Collider objectHit)
    {
        DolphinTriggersDeactivation(objectHit);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {

        if (hit.gameObject.tag == "movable")
        {
            hit.gameObject.GetComponent<Rigidbody>().velocity = (hit.moveDirection * 2);
        }

        if ((hit.gameObject.tag == "Platform"))
        {
              
            hit.gameObject.GetComponent<Rigidbody>().AddForce(-hit.normal * weight);
        }

        if (hit.gameObject.tag == "Water")
            isInWater = true;

    }

    private void vAbilityTriggers(Collider objectHit)
    {
        if (objectHit.gameObject.tag == "vAbilityta" && Input.GetButtonDown("XButton") && currentForm == forms.standard && !vFissureAbilityisOn)
        {
            vFissureAbilityisOn = true;


            vTriggerRotation = objectHit.transform.rotation;
            vGuidanceRotation = objectHit.GetComponentInParent<VFissure>().mGuidance.transform.rotation;


            vTriggerMidPosition = objectHit.transform.position;
            vTriggerMidPosition.y = 0.0f;

            vGuidanceFinPosition = objectHit.GetComponentInParent<VFissure>().exitA.transform.position;
            vGuidanceFinPosition.y = 0.0f;


            vGuidanceDir = objectHit.GetComponentInParent<VFissure>().mGuidance.transform.right;

        }

        if (objectHit.gameObject.tag == "vAbilitytb" && Input.GetButtonDown("XButton") && currentForm == forms.standard && !vFissureAbilityisOn)
        {
            vFissureAbilityisOn = true;


            vTriggerRotation = objectHit.transform.rotation;
            vGuidanceRotation = objectHit.GetComponentInParent<VFissure>().mGuidance.transform.rotation;


            vTriggerMidPosition = objectHit.transform.position;
            vTriggerMidPosition.y = 0.0f;

            vGuidanceFinPosition = objectHit.GetComponentInParent<VFissure>().exitB.transform.position;
            vGuidanceFinPosition.y = 0.0f;


            vGuidanceDir = -objectHit.GetComponentInParent<VFissure>().mGuidance.transform.right;

        }
    }

    private void DolphinTriggersActivation(Collider objectHit)
    {
        if (objectHit.gameObject.tag == "EnterTrigger")
        {
            dolphinInAbility = true;
            jumpInUp = objectHit.gameObject.transform.up;
            jumpInFw = objectHit.gameObject.transform.forward;
            jumpInRot = objectHit.gameObject.transform.rotation;
        }

        if (objectHit.gameObject.tag == "ExitTrigger")
        {
            dolphinOutAbility = true;
            jumpOutUp = objectHit.gameObject.transform.up;
            jumpOutFw = objectHit.gameObject.transform.forward;
            jumpOutRot = objectHit.gameObject.transform.rotation;
        }

    }

    private void DolphinTriggersDeactivation(Collider objectHit)
    {
        if (objectHit.gameObject.tag == "EnterTrigger")
        {
            dolphinInAbility = false;
            stMoveEnabled = true;
        }
        if (objectHit.gameObject.tag == "ExitTrigger")
        {
            dolphinOutAbility = false;
            stMoveEnabled = true;
        }
    }



    #region Player Initialization
    private void SettingDefaultValues()
    {
        moveValues defaultMove;

        defaultMove.standMove.moveSpeed = 10;
        defaultMove.standMove.jumpStrength = 10;

        defaultMove.frogMove.moveSpeed = 5;
        defaultMove.frogMove.jumpStrength = 20;

        defaultMove.craneMove.glideSpeed = 15;

        defaultMove.armaMove.moveSpeed = 7.5f;
        defaultMove.armaMove.rollingStrength = 5;
        defaultMove.armaMove.rollingTime = 0.5f;

        defaultMove.dolphinMove.swimSpeed = 10;
        defaultMove.dolphinMove.jumpStrength = 8;

       


        generalTweaks defaultGeneral;

        defaultGeneral.globalGravity = 9;
        defaultGeneral.jumpGravity = 20;
        defaultGeneral.glideGravity = 10;
        defaultGeneral.rotateSpeed = 2;
        

        GeneralValues = defaultGeneral;

    }

    private void SettingStandardForm()
    {
        frog = GameObject.FindGameObjectWithTag("Frog Form");
        frog.SetActive(false);

        standard = GameObject.FindGameObjectWithTag("Standard Form");
        standard.SetActive(true);

        dragon = GameObject.FindGameObjectWithTag("Dragon Form");
        dragon.SetActive(false);

        armadillo = GameObject.FindGameObjectWithTag("Armadillo Form");
        armadillo.SetActive(false);
        

        dolphin = GameObject.FindGameObjectWithTag("Dolphin Form");
        dolphin.SetActive(false);
    }

    private void SettingCPState()
    {
        cPlayerState.currentForm = forms.standard;
        cPlayerState.previousForm = forms.standard;
        cPlayerState.currentPState = physicStates.onGround;
        cPlayerState.currentPAState = primaryAbilityStates.Jump;
        cPlayerState.currentSAState = secondaryAbilityStates.noAbility;
        cPlayerState.currentTAState = tertiaryAbilityStates.noAbility;
        cPlayerState.currentQAState = quaternaryAbilityStates.noAbility;
        cPlayerState.currentControl = control.totalControl;
    }
    #endregion

    #region Switch Handler Methods
    public void SwitchToStandard()
    {
        if (CheckStandardSwitchRequirements())
        {

            cPlayerState.forms.Find(prev => prev.activeInHierarchy).SetActive(false);
            cPlayerState.forms[0].SetActive(true);
            cPlayerState.previousForm = cPlayerState.currentForm;
            cPlayerState.currentForm = forms.standard;
            if (cPlayerState.previousForm == forms.crane)
                StartCoroutine(Falling(fallDirection));
        }
        else
            Debug.Log("Conditions not met");
    }

    private void SwitchToFrog()
    {
        if (CheckFrogSwitchRequirements())
        {

            cPlayerState.forms.Find(prev => prev.activeInHierarchy).SetActive(false);
            cPlayerState.forms[1].SetActive(true);
            cPlayerState.previousForm = cPlayerState.currentForm;
            cPlayerState.currentForm = forms.frog;
            if (cPlayerState.previousForm == forms.crane)
                StartCoroutine(Falling(fallDirection));
        }
        else
            SwitchToStandard();

    }

    private void SwitchToCrane()
    {
        if (CheckCraneSwitchRequirements())
        {

            cPlayerState.forms.Find(prev => prev.activeInHierarchy).SetActive(false);
            cPlayerState.forms[2].SetActive(true);
            cPlayerState.previousForm = cPlayerState.currentForm;
            cPlayerState.currentForm = forms.crane;

        }
        else
            SwitchToStandard();
    }

    private void SwitchToArma()
    {
        if (CheckArmaSwitchRequirements())
        {

            cPlayerState.forms.Find(prev => prev.activeInHierarchy).SetActive(false);
            cPlayerState.forms[3].SetActive(true);
            cPlayerState.previousForm = cPlayerState.currentForm;
            cPlayerState.currentForm = forms.armadillo;
            if (cPlayerState.previousForm == forms.crane)
                StartCoroutine(Falling(fallDirection));
        }
        else
            SwitchToStandard();
    }

    private void SwitchToDolphin()
    {
        if (CheckDolphinSwitchRequirements())
        {

            cPlayerState.forms.Find(prev => prev.activeInHierarchy).SetActive(false);
            cPlayerState.forms[4].SetActive(true);
            cPlayerState.previousForm = cPlayerState.currentForm;
            cPlayerState.currentForm = forms.dolphin;
            if (cPlayerState.previousForm == forms.crane)
                StartCoroutine(Falling(fallDirection));
        }
        else
            SwitchToStandard();
    }

    private bool CheckStandardSwitchRequirements()
    {
        if (cPlayerState.currentForm == forms.standard)
            return false;
        if (!GeneralSwitchControlRequirements())
            return false;
        return true;
    }

    private bool CheckFrogSwitchRequirements()
    {
        if (cPlayerState.currentForm == forms.frog)
            return false;
        if (!GeneralSwitchControlRequirements())
            return false;
        return true;
    }

    private bool CheckCraneSwitchRequirements()
    {
        if (cPlayerState.currentForm == forms.crane)
            return false;
        if (cPlayerState.currentPState != physicStates.onAir)
            return false;
        if (!GeneralSwitchControlRequirements())
            return false;
        return true;
    }

    private bool CheckArmaSwitchRequirements()
    {
        if (cPlayerState.currentForm == forms.armadillo)
            return false;
        if (!GeneralSwitchControlRequirements())
            return false;
        return true;
    }

    private bool CheckDolphinSwitchRequirements()
    {
        if (cPlayerState.currentForm == forms.dolphin)
            return false;
        if (cPlayerState.currentTAState != tertiaryAbilityStates.switchToDolphin)
            return false;
        if (!GeneralSwitchControlRequirements())
            return false;
        return true;
    }

    private bool GeneralSwitchControlRequirements()
    {
        if (cPlayerState.currentControl == control.noCameraAndSpecial)
            return false;
        if (cPlayerState.currentControl == control.noMoveAndSpecial)
            return false;
        if (cPlayerState.currentControl == control.noSpecialInputsControl)
            return false;
        if (cPlayerState.currentControl == control.noControl)
            return false;
        return true;
    }
    #endregion

    #region Move Handler Methods

    private void MoveHandler(Vector3 moveDir)
    {
        fallDirection = moveDir;

        switch (cPlayerState.currentForm)
        {
            case forms.standard:
                if (GeneralMoveControlRequirements())
                    StandardMoving(moveDir);
                else
                    Debug.Log("Cannot Move");
                break;

            case forms.frog:
                if (GeneralMoveControlRequirements())
                    FrogMoving(moveDir);
                else
                    Debug.Log("Cannot Move");
                break;

            case forms.crane:
                if (GeneralMoveControlRequirements())
                    CraneMoving(moveDir);
                else
                    Debug.Log("Cannot Move");
                break;

            case forms.armadillo:
                if (GeneralMoveControlRequirements())
                    ArmaMoving(moveDir);
                else
                    Debug.Log("Cannot Move");
                break;

            case forms.dolphin:
                if (GeneralMoveControlRequirements())
                    DolphinMoving(moveDir);
                else
                    Debug.Log("Cannot Move");
                break;
        }
    }

    private void StandardMoving(Vector3 moveDir)
    {
        if (moveDir.sqrMagnitude >= 0.1f)
        {
            RotationHandler(moveDir);

            if (CheckMoveStandardRequirements())
            {
                cPlayerState.currentState = states.walking;
                ccLink.SimpleMove(moveDir * CurrentMoveValues.standMove.moveSpeed);
            }
        }
    }

    private bool CheckMoveStandardRequirements()
    {
        if (cPlayerState.currentPState != physicStates.onGround)
            return false;
        return true;
    }

    private void FrogMoving(Vector3 moveDir)
    {
        if (moveDir.sqrMagnitude >= 0.1f)
        {
            RotationHandler(moveDir);

            if (CheckMoveFrogRequirements())
            {
                cPlayerState.currentState = states.walking;
                ccLink.SimpleMove(moveDir * CurrentMoveValues.frogMove.moveSpeed);
            }
        }
    }

    private bool CheckMoveFrogRequirements()
    {
        if (cPlayerState.currentPState != physicStates.onGround)
            return false;
        return true;
    }

    private void ArmaMoving(Vector3 moveDir)
    {
        if (moveDir.sqrMagnitude >= 0.1f)
        {
            RotationHandler(moveDir);

            if (CheckMoveArmaRequirements())
            {
                cPlayerState.currentState = states.walking;
                ccLink.SimpleMove(moveDir * CurrentMoveValues.armaMove.moveSpeed);
            }
        }
    }

    private bool CheckMoveArmaRequirements()
    {
        if (cPlayerState.currentPState != physicStates.onGround)
            return false;
        if (cPlayerState.currentState == states.abilityAnimation)
            return false;
        return true;
    }

    private void CraneMoving(Vector3 moveDir)
    {
        if (moveDir.sqrMagnitude >= 0.1f)
            RotationHandler(moveDir);
        Vector3 glideDirection = moveDir;
        glideDirection.y -= GeneralValues.glideGravity * Time.deltaTime;
        cPlayerState.currentState = states.walking;
        ccLink.Move(glideDirection * CurrentMoveValues.craneMove.glideSpeed * Time.deltaTime);
    }

    private void DolphinMoving(Vector3 moveDir)
    {
        if (moveDir.sqrMagnitude >= 0.1f)
        {
            RotationHandler(moveDir);
            cPlayerState.currentState = states.walking;
            ccLink.SimpleMove(moveDir * CurrentMoveValues.dolphinMove.swimSpeed * Time.deltaTime);
        }
    }

    private void RotationHandler(Vector3 moveDir)
    {
        Quaternion rotation = Quaternion.LookRotation(moveDir, Vector3.up);
        if (cPlayerState.currentForm == forms.crane || (cPlayerState.currentForm == forms.armadillo && cPlayerState.currentState == states.abilityAnimation))
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, Time.deltaTime * GeneralValues.rotateSpeed / 3);
        else
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, Time.deltaTime * GeneralValues.rotateSpeed);
    }

    private bool GeneralMoveControlRequirements()
    {
        if (cPlayerState.currentControl == control.noMoveAndCamera)
            return false;
        if (cPlayerState.currentControl == control.noMoveControl)
            return false;
        if (cPlayerState.currentControl == control.noMoveAndSpecial)
            return false;
        if (cPlayerState.currentControl == control.noControl)
            return false;
        return true;
    }

    #endregion

    #region Abilities Handler Methods
    private void PrimaryAbilityHandler()
    {
        switch (cPlayerState.currentForm)
        {
            case forms.standard:
                if (CheckPrimaryAbiStandardRequirements())
                {
                    StandardJumping();
                }
                else
                    Debug.Log("Cannot Jump");
                break;

            case forms.frog:
                if (CheckPrimaryAbiStandardRequirements())
                {
                    FrogJumping();
                }
                else
                    Debug.Log("Cannot Jump");
                break;

            case forms.crane:

                break;

            case forms.armadillo:
                if (CheckPrimaryAbiStandardRequirements())
                {
                    ArmaRolling();
                }
                else
                    Debug.Log("Cannot Roll");
                break;

            case forms.dolphin:

                break;
        }
    }

    private void StandardJumping()
    {


        if (cPlayerState.currentState == states.walking)
            jumpDirection = (this.transform.up + this.transform.forward) * CurrentMoveValues.standMove.jumpStrength;
        else if (cPlayerState.currentState == states.standingStill)
            jumpDirection = (this.transform.up) * CurrentMoveValues.standMove.jumpStrength;

        cPlayerState.currentPState = physicStates.onAir;
        StartCoroutine(Falling(jumpDirection));
    }

    private void FrogJumping()
    {
        if (cPlayerState.currentState == states.walking)
            jumpDirection = (this.transform.up + this.transform.forward) * CurrentMoveValues.frogMove.jumpStrength;
        else if (cPlayerState.currentState == states.standingStill)
            jumpDirection = (this.transform.up) * CurrentMoveValues.frogMove.jumpStrength;

        cPlayerState.currentPState = physicStates.onAir;
        StartCoroutine(Falling(jumpDirection));
    }

    private void ArmaRolling()
    {
        StartCoroutine(Rolling());
        cPlayerState.currentState = states.abilityAnimation;
    }

    private bool CheckPrimaryAbiStandardRequirements()
    {
        if (!GeneralSpecialInputsControlRequirements())
            return false;
        if (cPlayerState.currentPState == physicStates.onAir)
            return false;
        return true;
    }

    private void SecondaryAbilityHandler()
    {

    }

    private void TertiaryAbilityHandler()
    {

    }

    private void QuaternaryAbilityHandler()
    {

    }

    private bool GeneralSpecialInputsControlRequirements()
    {
        if (cPlayerState.currentControl == control.noCameraAndSpecial)
            return false;
        if (cPlayerState.currentControl == control.noSpecialInputsControl)
            return false;
        if (cPlayerState.currentControl == control.noMoveAndSpecial)
            return false;
        if (cPlayerState.currentControl == control.noControl)
            return false;
        return true;
    }
    #endregion

    #region Generale State Changing Methods

    private void ChangingStateToStandingStill()
    {
        if (cPlayerState.currentState == states.walking)
            cPlayerState.currentState = states.standingStill;
    }

    void Update()
    {
        if (cPlayerState.currentPState == physicStates.onAir && ccLink.isGrounded)
        {
            cPlayerState.currentPState = physicStates.onGround;

            if (cPlayerState.currentForm == forms.crane)
                SwitchToStandard();
        }

        if (cPlayerState.currentPState == physicStates.onGround && !ccLink.isGrounded)
        {
            cPlayerState.currentPState = physicStates.onAir;

            if (cPlayerState.currentForm != forms.crane)
                StartCoroutine(Falling(fallDirection));


        }

        if (fallDirection.sqrMagnitude < 0.1f && cPlayerState.currentState == states.walking)
            cPlayerState.currentState = states.standingStill;
        else if (fallDirection.sqrMagnitude > 0.5f && cPlayerState.currentForm != forms.crane)
            cPlayerState.currentState = states.walking;
    }

    #endregion

    private IEnumerator Falling(Vector3 jumpDir)
    {
        while (cPlayerState.currentPState != physicStates.onGround && cPlayerState.currentForm != forms.crane)
        {
            
            fallDirection.y = jumpDir.y;

            if (fallDirection.sqrMagnitude == 0)
            {
                jumpDir.y -= GeneralValues.jumpGravity * Time.deltaTime;
                ccLink.Move(jumpDir * Time.deltaTime);
            }
            else
            {
                

                jumpDir.x = fallDirection.x * currentMoveValues.standMove.moveSpeed;
                jumpDir.z = fallDirection.z * currentMoveValues.standMove.moveSpeed;

                jumpDir.y -= GeneralValues.jumpGravity * Time.deltaTime;
                ccLink.Move(jumpDir * Time.deltaTime);
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private IEnumerator Rolling()
    {
        while (rollingTime <= CurrentMoveValues.armaMove.rollingTime)
        {
            rollingTime += Time.deltaTime;
            ccLink.SimpleMove(this.transform.forward * CurrentMoveValues.armaMove.rollingStrength);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        rollingTime = 0;
        cPlayerState.currentState = states.standingStill;
    }






}
*/