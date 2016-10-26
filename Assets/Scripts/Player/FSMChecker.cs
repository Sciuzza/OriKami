﻿using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

#region Finite State Machine enum Structure


public enum abilties { roll, jump, moveBlock, switchToDolp, switchByDolp, VFissure, HFissure, dolpJumpAb, dolpSwimBel, npcInter, menu };

public enum physicStates { onAir, onGround, onWater }

public enum playerStates { standingStill, moving, flying };

public enum control { totalControl, noMoveControl, noCameraControl, noSpecialInputsControl, noMoveAndCamera, noCameraAndSpecial, noMoveAndSpecial, noControl };



#endregion


public class FSMChecker : MonoBehaviour
{


    [System.Serializable]
    public struct playerCState
    {
        public string currentForm;
        public string previousForm;
        public List<GameObject> forms;
        public List<abilties> currentAbilities;
        public physicStates currentPhState;
        public playerStates currentPlState;
        public control currentControl;
    }

    [SerializeField]
    playerCState cPlayerState;

    #region Effects Request Observer Path
    [System.Serializable]
    public class formEffectsRequest : UnityEvent<string, string, List<GameObject>>
    {
    }

    public formEffectsRequest formChanged;


    [System.Serializable]
    public class abilityEffectsRequest : UnityEvent<abilties>
    {
    }

    public abilityEffectsRequest abilityUsed;

    [System.Serializable]
    public class physicStateEffectsRequest : UnityEvent<physicStates>
    {
    }

    public physicStateEffectsRequest phStateChanged;

    [System.Serializable]
    public class playerStateEffectsRequest : UnityEvent<playerStates>
    {
    }

    public playerStateEffectsRequest plStateChanged;

    [System.Serializable]
    public class controlEffectsRequest : UnityEvent<control>
    {
    }

    public controlEffectsRequest controlChanged;
    #endregion

    void Awake()
    {
        SettingPlayerInitialState();
        formChanged.Invoke(cPlayerState.currentForm, cPlayerState.previousForm, cPlayerState.forms);


        PlayerInputs plInputsTempLink = this.gameObject.GetComponent<PlayerInputs>();

        plInputsTempLink.jumpRequest.AddListener(CheckingJumpRequirements);

    }

    private void SettingPlayerInitialState()
    {
        cPlayerState.currentForm = "Standard Form";
        cPlayerState.previousForm = "Standard Form";

        cPlayerState.currentAbilities.Add(abilties.jump);
        cPlayerState.currentAbilities.Add(abilties.menu);

        cPlayerState.currentPhState = physicStates.onGround;

        cPlayerState.currentPlState = playerStates.standingStill;

        cPlayerState.currentControl = control.totalControl;
    }

    private void CheckingJumpRequirements()
    {

        List<string> availableForms = new List<string>();
        availableForms.Add("Standard Form");
        availableForms.Add("Frog Form");

        List<physicStates> possiblePhStates = new List<physicStates>();
        possiblePhStates.Add(physicStates.onGround);

        List<playerStates> possiblePlStates = new List<playerStates>();
        possiblePlStates.Add(playerStates.moving);
        possiblePlStates.Add(playerStates.standingStill);

        List<control> possibleControlStates = new List<control>();
        possibleControlStates.Add(control.noCameraControl);
        possibleControlStates.Add(control.noMoveAndCamera);
        possibleControlStates.Add(control.noMoveControl);
        possibleControlStates.Add(control.totalControl);

        if (RequirementsCheckHandler(availableForms, abilties.jump, possiblePhStates, possiblePlStates, possibleControlStates))
            abilityUsed.Invoke(abilties.jump);
        else
            Debug.Log("Requirements not met");

    }


    private bool RequirementsCheckHandler(List<string> availableForms, abilties abilityToSearch, List<physicStates> possiblePhStates,
        List<playerStates> possiblePlStates, List<control> possibleControlStates)
    {
        if (!GeneralFormRequirementCheck(availableForms))
            return false;
        else if (!GeneralAbilityRequirementCheck(abilityToSearch))
            return false;
        else if (!GeneralPhysicStateRequirement(possiblePhStates))
            return false;
        else if (!GeneralPlayerStateRequirement(possiblePlStates))
            return false;
        else if (!GeneralControlRequirement(possibleControlStates))
            return false;
        else
            return true;
    }

    private bool GeneralFormRequirementCheck(List<string> availableForms)
    {
        if (availableForms.Contains(cPlayerState.currentForm))
            return true;
        else
            return false;
    }

    private bool GeneralAbilityRequirementCheck(abilties abilityToSearch)
    {
        if (cPlayerState.currentAbilities.Contains(abilityToSearch))
            return true;
        else
            return false;

    }

    private bool GeneralPhysicStateRequirement(List<physicStates> possiblePhStates)
    {
        if (possiblePhStates.Contains(cPlayerState.currentPhState))
            return true;
        else
            return false;
    }

    private bool GeneralPlayerStateRequirement(List<playerStates> possiblePlStates)
    {
        if (possiblePlStates.Contains(cPlayerState.currentPlState))
            return true;
        else
            return false;
    }

    private bool GeneralControlRequirement(List<control> possibleControlStates)
    {
        if (possibleControlStates.Contains(cPlayerState.currentControl))
            return true;
        else
            return false;
    }


    
}

