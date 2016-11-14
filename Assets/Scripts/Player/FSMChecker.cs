using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

#region Finite State Machine enum Structure


public enum abilties
{
    move, rotate, cameraMove, npcInter, menu,
    jump, roll, moveBlock, VFissure, HFissure, dolpSwimBel,
    toStd, toFrog, toArma, toCrane, toDolp
};

public enum physicStates { onAir, onGround, onWater }

public enum playerStates
{
    standingStill, moving, flying,
    rolling, movingBlock
};




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
    public class dirAbilityUsed : UnityEvent<abilties, Vector3, string>
    {
    }

    public dirAbilityUsed dirAbiUsed;

    [System.Serializable]
    public class rotEffectsRequest : UnityEvent<Vector3, playerStates>
    {
    }

    public rotEffectsRequest rotationUsed;

    [System.Serializable]
    public class generalAbiUsed : UnityEvent<abilties, string>
    {
    }

    public generalAbiUsed genAbiUsed;



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
    public class availableInputs : UnityEvent<string>
    {
    }

    public availableInputs formChangedInp;

    #endregion

    void Awake()
    {
        SettingPlayerInitialState();



        PlayerInputs plInputsTempLink = this.gameObject.GetComponent<PlayerInputs>();


        plInputsTempLink.dirAbiRequest.AddListener(CheckingAbiRequirements);
        plInputsTempLink.genAbiRequest.AddListener(CheckingAbiRequirements);

        EnvInputs enInputsTempLink = this.gameObject.GetComponent<EnvInputs>();

        enInputsTempLink.psChanged.AddListener(ChangingPHStates);


    }

    void Start()
    {

        formChanged.Invoke(cPlayerState.currentForm, cPlayerState.previousForm, cPlayerState.forms);
    }


    private void SettingPlayerInitialState()
    {
        cPlayerState.currentForm = "Standard Form";
        cPlayerState.previousForm = "Standard Form";

        cPlayerState.currentAbilities.Add(abilties.jump);
        cPlayerState.currentAbilities.Add(abilties.menu);
        cPlayerState.currentAbilities.Add(abilties.cameraMove);
        cPlayerState.currentAbilities.Add(abilties.move);
        cPlayerState.currentAbilities.Add(abilties.npcInter);
        cPlayerState.currentAbilities.Add(abilties.toArma);
        cPlayerState.currentAbilities.Add(abilties.toCrane);
        cPlayerState.currentAbilities.Add(abilties.toDolp);
        cPlayerState.currentAbilities.Add(abilties.toFrog);
        cPlayerState.currentAbilities.Add(abilties.VFissure);
        cPlayerState.currentAbilities.Add(abilties.rotate);


        cPlayerState.currentPhState = physicStates.onGround;

        cPlayerState.currentPlState = playerStates.standingStill;


    }

    private void CheckingAbiRequirements(abilties abiReceived, Vector3 abiDir)
    {

        if (cPlayerState.currentAbilities.Contains(abiReceived))
        {
            switch (abiReceived)
            {

                case abilties.move:
                    dirAbiUsed.Invoke(abiReceived, abiDir, cPlayerState.currentForm);
                    break;
                case abilties.rotate:
                    rotationUsed.Invoke(abiDir, cPlayerState.currentPlState);
                    break;
               
            }


        }
        else
            Debug.Log("Requirements not met");
    }

    private void CheckingAbiRequirements(abilties abiReceived)
    {

        if (cPlayerState.currentAbilities.Contains(abiReceived))
        {
            switch (abiReceived)
            {
        
                case abilties.jump:
                     genAbiUsed.Invoke(abiReceived, cPlayerState.currentForm);
                    break;
                case abilties.cameraMove:
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
                    cPlayerState.previousForm = cPlayerState.currentForm;
                    cPlayerState.currentForm = "Standard Form";
                    formChangedInp.Invoke(cPlayerState.currentForm);
                    formChanged.Invoke(cPlayerState.currentForm, cPlayerState.previousForm, cPlayerState.forms);
                    break;
                case abilties.toFrog:
                    cPlayerState.previousForm = cPlayerState.currentForm;
                    cPlayerState.currentForm = "Frog Form";
                    formChangedInp.Invoke(cPlayerState.currentForm);
                    formChanged.Invoke(cPlayerState.currentForm, cPlayerState.previousForm, cPlayerState.forms);
                    break;
                case abilties.toCrane:
                    cPlayerState.previousForm = cPlayerState.currentForm;
                    cPlayerState.currentForm = "Dragon Form";
                    formChangedInp.Invoke(cPlayerState.currentForm);
                    formChanged.Invoke(cPlayerState.currentForm, cPlayerState.previousForm, cPlayerState.forms);
                    break;
                case abilties.toArma:
                    cPlayerState.previousForm = cPlayerState.currentForm;
                    cPlayerState.currentForm = "Armadillo Form";
                    formChangedInp.Invoke(cPlayerState.currentForm);
                    formChanged.Invoke(cPlayerState.currentForm, cPlayerState.previousForm, cPlayerState.forms);
                    break;
                case abilties.toDolp:
                    cPlayerState.previousForm = cPlayerState.currentForm;
                    cPlayerState.currentForm = "Dolphin Form";
                    formChangedInp.Invoke(cPlayerState.currentForm);
                    formChanged.Invoke(cPlayerState.currentForm, cPlayerState.previousForm, cPlayerState.forms);
                    break;
                case abilties.npcInter:
                    break;
                case abilties.menu:
                    break;


            }


        }
        else
            Debug.Log("Requirements not met");
    }

    private void ChangingPHStates(physicStates stateToGo)
    {

        cPlayerState.currentPhState = stateToGo;
        //phStateChanged.Invoke(stateToGo);

        UpdatingAbilityList();
       

    }


    private void UpdatingAbilityList()
    {
        switch (cPlayerState.currentForm)
        {
            case "Standard Form":
                UpdatingStdAbilityList();
                break;
            case "Frog Form":
                UpdatingFrogAbilityList();
                break;
            case "Armadillo Form":
                UpdatingArmaAbilityList();
                break;
            case "Dragon Form":
                UpdatingCraneAbilityList();
                break;
            case "Dolphin Form":
                UpdatingDolpAbilityList();
                break;
        }
    }

    private void UpdatingStdAbilityList()
    {

        RemoveAbility(abilties.HFissure);
        RemoveAbility(abilties.dolpSwimBel);
        RemoveAbility(abilties.moveBlock);
        RemoveAbility(abilties.roll);
        RemoveAbility(abilties.toStd);

        switch (cPlayerState.currentPhState)
        {
            case physicStates.onWater:
            case physicStates.onAir:
                RemoveAbility(abilties.jump);
                RemoveAbility(abilties.VFissure);       
                break;
            case physicStates.onGround:
                AddAbility(abilties.jump);
                AddAbility(abilties.VFissure);             
                break;
        }
    }

    private void UpdatingFrogAbilityList()
    {
        switch (cPlayerState.currentPhState)
        {
            case physicStates.onAir:
                RemoveAbility(abilties.jump);
                RemoveAbility(abilties.HFissure);
                break;
            case physicStates.onGround:
                AddAbility(abilties.jump);
                AddAbility(abilties.HFissure);
                break;
            case physicStates.onWater:
                RemoveAbility(abilties.jump);
                RemoveAbility(abilties.HFissure);
                break;
        }
    }

    private void UpdatingArmaAbilityList()
    {
        switch (cPlayerState.currentPhState)
        {
            case physicStates.onAir:
                RemoveAbility(abilties.roll);
                RemoveAbility(abilties.moveBlock);
                break;
            case physicStates.onGround:
                AddAbility(abilties.roll);
                AddAbility(abilties.moveBlock);
                break;
            case physicStates.onWater:
                RemoveAbility(abilties.roll);
                RemoveAbility(abilties.moveBlock);
                break;
        }
    }

    private void UpdatingCraneAbilityList()
    {

    }

    private void UpdatingDolpAbilityList()
    {

    }

    private void RemoveAbility(abilties abiToRemove)
    {
        if (cPlayerState.currentAbilities.Contains(abiToRemove))
            cPlayerState.currentAbilities.Remove(abiToRemove);
      
    }

    private void AddAbility(abilties abiToAdd)
    {
        if (!cPlayerState.currentAbilities.Contains(abiToAdd))
            cPlayerState.currentAbilities.Add(abiToAdd);
    }
    /*
    private bool RequirementsCheckHandler(List<string> availableForms, abilties abilityToSearch, List<physicStates> possiblePhStates,
        List<playerStates> possiblePlStates)
    {
        if (!GeneralFormRequirementCheck(availableForms))
            return false;
        else if (!GeneralAbilityRequirementCheck(abilityToSearch))
            return false;
        else if (!GeneralPhysicStateRequirement(possiblePhStates))
            return false;
        else if (!GeneralPlayerStateRequirement(possiblePlStates))
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


    */


}

