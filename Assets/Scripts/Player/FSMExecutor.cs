using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FSMExecutor : MonoBehaviour {

    [HideInInspector]
    public moveValues currentMoveValues;

    [HideInInspector]
    public generalTweaks generalValues;

    CharacterController ccLink;

    void Awake()
    {
        FSMChecker fsmCheckerTempLink = this.gameObject.GetComponent<FSMChecker>();

        fsmCheckerTempLink.formChanged.AddListener(ApplyingFormEffect);
        fsmCheckerTempLink.abilityUsed.AddListener(ApplyingAbilityEffect);
        fsmCheckerTempLink.phStateChanged.AddListener(ApplyingPhStateEffect);
        fsmCheckerTempLink.plStateChanged.AddListener(ApplyingPlStateEffect);
      

        ccLink = this.gameObject.GetComponent<CharacterController>();
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

    private void ApplyingAbilityEffect(abilties abiUsed)
    {
        switch (abiUsed)
        {
            case abilties.jump:
                break;
            case abilties.roll:
                break;
            case abilties.moveBlock:
                break;
            case abilties.VFissure:
                break;
            case abilties.HFissure:
                break;
            case abilties.switchToDolp:
                break;
            case abilties.switchByDolp:
                break;
            case abilties.dolpJumpAb:
                break;
            case abilties.dolpSwimBel:
                break;
            case abilties.menu:
                break;
        }
    }

    private void ApplyingPhStateEffect(physicStates currentPhState)
    {
        switch (currentPhState)
        {
            case physicStates.onGround:
                break;
            case physicStates.onAir:
                break;
            case physicStates.onWater:
                break;
        }
    }

    private void ApplyingPlStateEffect(playerStates currentPlState)
    {
        switch (currentPlState)
        {
            case playerStates.standingStill:
                break;
            case playerStates.movingOnGround:
                break;
            case playerStates.flying:
                break;
        }
    }

  
}
