﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class FSMExecutor : MonoBehaviour {

    [HideInInspector]
    public moveValues currentMoveValues;

    [HideInInspector]
    public generalTweaks generalValues;

    CharacterController ccLink;


	[System.Serializable]
	public class moveHandling : UnityEvent<Vector3>
	{
	}

	public moveHandling moveSelected;

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

	private void ApplyingAbilityEffect(abilties abiUsed, Vector3 moveDirInput)
    {
        switch (abiUsed)
        {
		case abilties.move:
			moveSelected.Invoke (moveDirInput);
			    break;
		    case abilties.cameraMove:
			    break;
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
            case abilties.dolpSwimBel:
                break;
		    case abilties.toStd:
			    break;
		    case abilties.toFrog:
			    break;
		    case abilties.toCrane:
			    break;
		    case abilties.toArma:
			    break;
		    case abilties.toDolp:
			    break;
		    case abilties.npcInter:
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
