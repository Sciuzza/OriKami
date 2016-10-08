using UnityEngine;
using System.Collections;
using UnityEngine.Events;



public enum forms { standard, frog, crane, armadillo, dolphin };

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
    public forms currentForm = forms.standard;

    public bool isJumping = false;
    public bool isRolling = false;
    public bool isFlying = false;
    public float rollingTime = 0.0f;
    public bool isInWater = false;
    public bool isArmaMoving = false;
    public bool dolphinInAbility = false;
    public bool dolphinOutAbility = false;
    public bool stMoveEnabled = true;

    public bool vFissureAbilityisOn = false, secondRotationisOn = false, secondMoveIsOn = false, moveFinished = false;
    public Quaternion vTriggerRotation, vGuidanceRotation;
    public Vector3 vTriggerMidPosition, vGuidanceFinPosition;


    public Vector3 jumpOutFw, jumpOutUp, jumpInFw, jumpInUp;
    public Quaternion jumpInRot, jumpOutRot;

    MoveCC moveLink;

    private void Awake()
    {
        SettingDefaultValues();
        moveLink = this.GetComponent<MoveCC>();
        SettingStandardForm();

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

        /*
        if (hit.gameObject.tag == "Water")
            dolphinInAbility = false;

    */

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

            vGuidanceFinPosition = objectHit.GetComponentInParent<VFissure>().mGuidance.transform.position;
            vGuidanceFinPosition.y = 0.0f;
            vGuidanceFinPosition.z += objectHit.GetComponentInParent<VFissure>().mGuidance.GetComponent<BoxCollider>().size.x / 3;

        }

        if (objectHit.gameObject.tag == "vAbilitytb" && Input.GetButtonDown("XButton") && currentForm == forms.standard && !vFissureAbilityisOn)
        {
            vFissureAbilityisOn = true;


            vTriggerRotation = objectHit.transform.rotation;
            vGuidanceRotation = objectHit.GetComponentInParent<VFissure>().mGuidance.transform.rotation;


            vTriggerMidPosition = objectHit.transform.position;
            vTriggerMidPosition.y = 0.0f;

            vGuidanceFinPosition = objectHit.GetComponentInParent<VFissure>().mGuidance.transform.position;
            vGuidanceFinPosition.y = 0.0f;
            vGuidanceFinPosition.z -= objectHit.GetComponentInParent<VFissure>().mGuidance.GetComponent<BoxCollider>().size.x / 3;

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
}
