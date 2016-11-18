using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class FSMExecutor : MonoBehaviour
{

    [HideInInspector]
    public moveValues currentMoveValues;

    [HideInInspector]
    public generalTweaks generalValues;

    #region Events
    [System.Serializable]
    public class dirAbiHandling : UnityEvent<Vector3, float>
    {
    }

    public dirAbiHandling moveSelected;

    [System.Serializable]
    public class genAbiHandling : UnityEvent<float>
    {
    }

    public genAbiHandling jumpSelected;

    [System.Serializable]
    public class rotHandling : UnityEvent<Vector3, float>
    {
    }

    public rotHandling rotSelected;

    [System.Serializable]
    public class phHandling : UnityEvent<physicStates>
    {
    }

    public phHandling phChangeEffect;

    public UnityEvent vFissureAniEnded;
    public UnityEvent StopMoveLogic, EnableMoveLogic;
    #endregion

    void Awake()
    {
        FSMChecker fsmCheckerTempLink = this.gameObject.GetComponent<FSMChecker>();

        fsmCheckerTempLink.formChanged.AddListener(ApplyingFormEffect);
        fsmCheckerTempLink.dirAbiUsed.AddListener(ApplyingAbilityEffect);
        fsmCheckerTempLink.genAbiUsed.AddListener(ApplyingAbilityEffect);
        fsmCheckerTempLink.rotationUsed.AddListener(ApplyingRotationEffect);
        fsmCheckerTempLink.vFissureUsed.AddListener(ApplyingVFissure);
      
     
    }

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

    }

    private void ApplyingAbilityEffect(abilties abiUsed, Vector3 moveDirInput, string currentForm)
    {
        switch (abiUsed)
        {
            case abilties.move:
                switch (currentForm)
                {
                    case ("Standard Form"):
                        moveSelected.Invoke(moveDirInput, currentMoveValues.standMove.moveSpeed);
                        break;
                    case ("Frog Form"):
                        moveSelected.Invoke(moveDirInput, currentMoveValues.frogMove.moveSpeed);
                        break;
                    case ("Dragon Form"):
                        moveSelected.Invoke(moveDirInput, currentMoveValues.craneMove.glideSpeed);
                        break;
                    case ("Armadillo Form"):
                        moveSelected.Invoke(moveDirInput, currentMoveValues.armaMove.moveSpeed);
                        break;
                    case ("Dolphin Form"):
                        moveSelected.Invoke(moveDirInput, currentMoveValues.dolphinMove.swimSpeed);
                        break;
                }


                break;

         
        }
    }

    private void ApplyingAbilityEffect(abilties abiUsed, string currentForm)
    {
        switch (abiUsed)
        {
            
            case abilties.jump:
                switch (currentForm)
                {
                    case ("Standard Form"):
                        jumpSelected.Invoke(currentMoveValues.standMove.jumpStrength);
                        break;
                    case ("Frog Form"):
                        jumpSelected.Invoke(currentMoveValues.frogMove.jumpStrength);
                        break;
                    case ("Dolphin Form"):
                        jumpSelected.Invoke(currentMoveValues.dolphinMove.jumpStrength);
                        break;
                }
                break;
            case abilties.roll:
                break;
       
        }
    }

    private void ApplyingVFissure(VFissure vfTempLink, string vfTag)
    {
        bool vFissureAniOn = true;
        StartCoroutine(VFissureExecution(vFissureAniOn, vfTempLink, vfTag));
    }

    private void ApplyingRotationEffect(Vector3 abiDirInput, playerStates currentPl)
    {

        switch (currentPl)
        {

            case playerStates.flying:
            case playerStates.movingBlock:
            case playerStates.rolling:
                rotSelected.Invoke(abiDirInput, generalValues.rotateSpeed / 3);
                break;
            default:
                rotSelected.Invoke(abiDirInput, generalValues.rotateSpeed);
                break;


        }

    }

    private IEnumerator VFissureExecution(bool vFissureAniOn, VFissure vfTempLink, string vfEntrance)
    {
        StopMoveLogic.Invoke();

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

                if (distance.sqrMagnitude >= 0.001f && !secondMoveIsOn)
                {
                    Debug.Log(distance.sqrMagnitude);
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

                        distance = vGuidanceFinPosition - this.transform.position;

                        if (distance.sqrMagnitude >= 0.001f)
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
        EnableMoveLogic.Invoke();
        ccTempLink.radius = radius;
    }


}
