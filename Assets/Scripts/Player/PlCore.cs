using UnityEngine;
using System.Collections;
using UnityEngine.Events;


public enum forms { standard, frog, crane, armadillo, dolphin };

public class PlCore : MonoBehaviour {


    public UnityEvent brokeSomething;
    public UnityEvent activateSomething;

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


    // to be placed on static script
    public string currentActForm = "Standard Form";
    public GameObject frog, standard, dragon, armadillo, dolphin;
    public forms currentForm = forms.standard;

    public bool isJumping = false;
    public bool isRolling = false;
    public bool isFlying = false;
    public float rollingTime = 0.0f;
    public bool isInWater = false;
    public bool isArmaMoving = false;

    public bool vFissureAbilityisOn = false, secondRotationisOn = false, secondMoveIsOn = false, moveFinished = false;
    public Quaternion vTriggerRotation, vGuidanceRotation;
    public Vector3 vTriggerMidPosition, vGuidanceFinPosition;


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

       

      
    }

    void OnTriggerStay(Collider objectHit)
    {
        if (objectHit.gameObject.name == "VAbility Trigger" && Input.GetButtonDown("XButton") && currentForm == forms.standard && !vFissureAbilityisOn)
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
}
