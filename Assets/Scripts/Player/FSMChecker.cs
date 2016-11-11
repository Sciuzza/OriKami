using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

#region Finite State Machine enum Structure


public enum abilties
{
    move, rotate, cameraMove, jump,
    roll, moveBlock,
    toStd, toFrog, toArma, toCrane, toDolp,
    VFissure, HFissure, dolpSwimBel,
    npcInter, menu
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
    public class abilityEffectsRequest : UnityEvent<abilties, Vector3, string>
    {
    }

    public abilityEffectsRequest abilityUsed;

	[System.Serializable]
	public class rotEffectsRequest : UnityEvent<Vector3, playerStates>
	{
	}

	public abilityEffectsRequest rotationUsed;



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



    #endregion

    void Awake()
    {
        SettingPlayerInitialState();
        formChanged.Invoke(cPlayerState.currentForm, cPlayerState.previousForm, cPlayerState.forms);


        PlayerInputs plInputsTempLink = this.gameObject.GetComponent<PlayerInputs>();


        plInputsTempLink.abiRequest.AddListener(CheckingAbiRequirements);

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
			switch (abiReceived) {

			case abilties.move:
			case abilties.jump: abilityUsed.Invoke(abiReceived, abiDir, cPlayerState.currentForm);
				break;
			case abilties.rotate: rotationUsed.Invoke(abiDir, cPlayerState.currentPlState);
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
        else
            Debug.Log("Requirements not met");
    }



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





}

