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

    void Awake()
    {
        FSMChecker fsmCheckerTempLink = this.gameObject.GetComponent<FSMChecker>();

        fsmCheckerTempLink.formChanged.AddListener(ApplyingFormEffect);
        fsmCheckerTempLink.dirAbiUsed.AddListener(ApplyingAbilityEffect);
        fsmCheckerTempLink.genAbiUsed.AddListener(ApplyingAbilityEffect);
        fsmCheckerTempLink.rotationUsed.AddListener(ApplyingRotationEffect);
        fsmCheckerTempLink.phStateChanged.AddListener(ApplyingPhStateEffect);
        fsmCheckerTempLink.plStateChanged.AddListener(ApplyingPlStateEffect);


     
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
                    case ("Crane Form"):
                        moveSelected.Invoke(moveDirInput, currentMoveValues.craneMove.glideSpeed);
                        break;
                    case ("Arma Form"):
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
           
       
        }
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

    private void ApplyingPhStateEffect(physicStates currentPhState)
    {
        phChangeEffect.Invoke(currentPhState);
    }

    private void ApplyingPlStateEffect(playerStates currentPlState)
    {
        switch (currentPlState)
        {
            case playerStates.standingStill:
                break;
            case playerStates.moving:
                break;
            case playerStates.flying:
                break;
            case playerStates.movingBlock:
                break;
            case playerStates.rolling:
                break;
        }
    }


}
