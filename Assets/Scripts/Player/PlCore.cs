using UnityEngine;
using System.Collections;
using UnityEngine.Events;



public enum forms { standard, frog, crane, armadillo, dolphin };

public enum physicStates { onAir, onGround, onWater}

public enum primaryAbilityStates { roll, dolJumpAbove, dolJumpBelow, Jump, fly, noAbility}

public enum secondaryAbilityStates { vFissure, hFissure, movingBlock, noAbility}

public enum tertiaryAbilityStates { switchToDolphin, switchByDolphin, noAbility}

public enum quaternaryAbilityStates { noAbility};

public enum states { standingStill, walking, falling, noState};

public enum control { totalControl, noMoveControl, noCameraControl, noSpecialInputsControl, noMoveAndCamera, noCameraAndSpecial, noMoveAndSpecial, noControl };

public struct playerState
{
    public forms currentForm;
    public physicStates currentPState;
    public primaryAbilityStates currentPAState;
    public secondaryAbilityStates currentSAState;
    public tertiaryAbilityStates currentTAState;
    public quaternaryAbilityStates currentQAState;
    public states currentState;
    public control currentControl;
}

public class PlCore : MonoBehaviour
{


    public UnityEvent brokeSomething;
    public UnityEvent activateSomething;

    [SerializeField]
    private Movement currentMoveValues;
    public Movement CurrentMoveValues
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



    public string currentActForm = "Standard Form";
    public GameObject frog, standard, dragon, armadillo, dolphin;
   



    playerState cPlayerState;

    // to be removed
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
    

    public bool vFissureAbilityisOn = false, secondRotationisOn = false, secondMoveIsOn = false, moveFinished = false;
    public Quaternion vTriggerRotation, vGuidanceRotation;
    public Vector3 vTriggerMidPosition, vGuidanceFinPosition;
    public Vector3 vGuidanceDir;

    public float rollingTime = 0.0f;

    public Vector3 jumpOutFw, jumpOutUp, jumpInFw, jumpInUp;
    public Quaternion jumpInRot, jumpOutRot;

    MoveCC moveLink;

   

    private void Awake()
    {
        SettingDefaultValues();
        moveLink = this.GetComponent<MoveCC>();
        SettingStandardForm();

        SettingCPState();

        InterCC interactionLink = this.gameObject.GetComponent<InterCC>();
        interactionLink.standardForm.AddListener(SwitchToStandardForm);
    }

    void OnTriggerEnter(Collider objectHit)
    {

        if (objectHit.gameObject.GetComponentInParent<DestroyableObjects>() != null && isRolling)
        {
            brokeSomething.Invoke();
        }

        if (objectHit.gameObject.GetComponent<ButtonActivator>() != null)
        {
            activateSomething.Invoke();
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

        if (hit.gameObject.tag == "Water")
            isInWater = true;

    }

   






    private void SettingDefaultValues()
    {
        Movement defaultMove;

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

        CurrentMoveValues = defaultMove;


        generalTweaks defaultGeneral;

        defaultGeneral.globalGravity = 9;
        defaultGeneral.jumpGravity = 20;
        defaultGeneral.glideGravity = 10;
        defaultGeneral.rotateSpeed = 2;
        defaultGeneral.currentInput = playMode.KMInput;

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



    public void SwitchToStandard()
    {
        standard.SetActive(true);
        GameObject.FindGameObjectWithTag(currentActForm).SetActive(false);
        currentActForm = "Standard Form";
        currentForm = forms.standard;
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


    private void SwitchToStandardForm()
    {

    }

    private void SettingCPState()
    {
        cPlayerState.currentForm = forms.standard;
        cPlayerState.currentPState = physicStates.onGround;
        cPlayerState.currentPAState = primaryAbilityStates.Jump;
        cPlayerState.currentSAState = secondaryAbilityStates.noAbility;
        cPlayerState.currentTAState = tertiaryAbilityStates.noAbility;
        cPlayerState.currentQAState = quaternaryAbilityStates.noAbility;
        cPlayerState.currentControl = control.totalControl;
    }

}
