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

public enum controlStates { totalControl, noCamera, noMove, noGenAbi, noCamAndMove, noMoveAndGenAbi, noCameraAndGenAbi, noControl };


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
        public controlStates currentClState;

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

    #region Initialization Methods
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

        cPlayerState.currentClState = controlStates.totalControl;


    } 
    #endregion

    #region Player Inputs Handler
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
                    UpdatingAbilityList();
                    formChanged.Invoke(cPlayerState.currentForm, cPlayerState.previousForm, cPlayerState.forms);
                    break;
                case abilties.toFrog:
                    cPlayerState.previousForm = cPlayerState.currentForm;
                    cPlayerState.currentForm = "Frog Form";
                    formChangedInp.Invoke(cPlayerState.currentForm);
                    UpdatingAbilityList();
                    formChanged.Invoke(cPlayerState.currentForm, cPlayerState.previousForm, cPlayerState.forms);
                    break;
                case abilties.toCrane:
                    cPlayerState.previousForm = cPlayerState.currentForm;
                    cPlayerState.currentForm = "Dragon Form";
                    formChangedInp.Invoke(cPlayerState.currentForm);
                    UpdatingAbilityList();
                    formChanged.Invoke(cPlayerState.currentForm, cPlayerState.previousForm, cPlayerState.forms);
                    break;
                case abilties.toArma:
                    cPlayerState.previousForm = cPlayerState.currentForm;
                    cPlayerState.currentForm = "Armadillo Form";
                    formChangedInp.Invoke(cPlayerState.currentForm);
                    UpdatingAbilityList();
                    formChanged.Invoke(cPlayerState.currentForm, cPlayerState.previousForm, cPlayerState.forms);
                    break;
                case abilties.toDolp:
                    cPlayerState.previousForm = cPlayerState.currentForm;
                    cPlayerState.currentForm = "Dolphin Form";
                    formChangedInp.Invoke(cPlayerState.currentForm);
                    UpdatingAbilityList();
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
    #endregion

    #region Environment Inputs Handler
    private void ChangingPHStates(physicStates stateToGo)
    {

        cPlayerState.currentPhState = stateToGo;
        UpdatingAbilityList();


    } 
    #endregion

    #region Ability List Handler
    //totalControl, noCamera, noMove, noGenAbi, noCamAndMove, noMoveAndGenAbi, noCameraAndGenAbi, noControl
    // move, rotate, cameraMove, npcInter, menu, jump, roll, moveBlock, VFissure, HFissure, dolpSwimBel, toStd, toFrog, toArma, toCrane, toDolp

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

    #region Standard Abilities Handler
    private void UpdatingStdAbilityList()
    {
        switch (cPlayerState.currentClState)
        {
            case controlStates.totalControl:
                StdTotalControl();
                break;
            case controlStates.noCamera:
                StdNoCamera();
                break;
            case controlStates.noMove:
                StdNoMove();
                break;
            case controlStates.noGenAbi:
                StdNoGenAbi();
                break;
            case controlStates.noCamAndMove:
                StdNoCamAndMove();
                break;
            case controlStates.noMoveAndGenAbi:
                StdNoMoveAndGenAbi();
                break;
            case controlStates.noCameraAndGenAbi:
                StdNoCameraAndGenAbi();
                break;
            case controlStates.noControl:
                NoControl();
                break;
        }


    }

    private void StdTotalControl()
    {

        AddAbility(abilties.move);
        AddAbility(abilties.rotate);

        AddAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.roll);
        RemoveAbility(abilties.moveBlock);
        RemoveAbility(abilties.HFissure);
        RemoveAbility(abilties.dolpSwimBel);


        RemoveAbility(abilties.toStd);
        AddAbility(abilties.toCrane);
        AddAbility(abilties.toFrog);
        AddAbility(abilties.toArma);
        AddAbility(abilties.toDolp);



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

    private void StdNoCamera()
    {
        AddAbility(abilties.move);
        AddAbility(abilties.rotate);

        RemoveAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.roll);
        RemoveAbility(abilties.moveBlock);
        RemoveAbility(abilties.HFissure);
        RemoveAbility(abilties.dolpSwimBel);


        RemoveAbility(abilties.toStd);
        AddAbility(abilties.toCrane);
        AddAbility(abilties.toFrog);
        AddAbility(abilties.toArma);
        AddAbility(abilties.toDolp);



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

    private void StdNoMove()
    {
        RemoveAbility(abilties.move);
        AddAbility(abilties.rotate);

        AddAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.roll);
        RemoveAbility(abilties.moveBlock);
        RemoveAbility(abilties.HFissure);
        RemoveAbility(abilties.dolpSwimBel);


        RemoveAbility(abilties.toStd);
        AddAbility(abilties.toCrane);
        AddAbility(abilties.toFrog);
        AddAbility(abilties.toArma);
        AddAbility(abilties.toDolp);



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

    private void StdNoGenAbi()
    {
        AddAbility(abilties.move);
        AddAbility(abilties.rotate);

        AddAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.roll);
        RemoveAbility(abilties.moveBlock);
        RemoveAbility(abilties.HFissure);
        RemoveAbility(abilties.dolpSwimBel);


        RemoveAbility(abilties.toStd);
        RemoveAbility(abilties.toCrane);
        RemoveAbility(abilties.toFrog);
        RemoveAbility(abilties.toArma);
        RemoveAbility(abilties.toDolp);



        RemoveAbility(abilties.jump);
        RemoveAbility(abilties.VFissure);
    }

    private void StdNoCamAndMove()
    {
        RemoveAbility(abilties.move);
        AddAbility(abilties.rotate);

        RemoveAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.roll);
        RemoveAbility(abilties.moveBlock);
        RemoveAbility(abilties.HFissure);
        RemoveAbility(abilties.dolpSwimBel);


        RemoveAbility(abilties.toStd);
        AddAbility(abilties.toCrane);
        AddAbility(abilties.toFrog);
        AddAbility(abilties.toArma);
        AddAbility(abilties.toDolp);



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

    private void StdNoMoveAndGenAbi()
    {
        RemoveAbility(abilties.move);
        AddAbility(abilties.rotate);

        AddAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.roll);
        RemoveAbility(abilties.moveBlock);
        RemoveAbility(abilties.HFissure);
        RemoveAbility(abilties.dolpSwimBel);


        RemoveAbility(abilties.toStd);
        RemoveAbility(abilties.toCrane);
        RemoveAbility(abilties.toFrog);
        RemoveAbility(abilties.toArma);
        RemoveAbility(abilties.toDolp);



        RemoveAbility(abilties.jump);
        RemoveAbility(abilties.VFissure);
    }

    private void StdNoCameraAndGenAbi()
    {
        AddAbility(abilties.move);
        AddAbility(abilties.rotate);

        RemoveAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.roll);
        RemoveAbility(abilties.moveBlock);
        RemoveAbility(abilties.HFissure);
        RemoveAbility(abilties.dolpSwimBel);


        RemoveAbility(abilties.toStd);
        RemoveAbility(abilties.toCrane);
        RemoveAbility(abilties.toFrog);
        RemoveAbility(abilties.toArma);
        RemoveAbility(abilties.toDolp);



        RemoveAbility(abilties.jump);
        RemoveAbility(abilties.VFissure);
    }
    #endregion

    #region Frog Abilities Handler
    private void UpdatingFrogAbilityList()
    {

        switch (cPlayerState.currentClState)
        {
            case controlStates.totalControl:
                FrogTotalControl();
                break;
            case controlStates.noCamera:
                FrogNoCamera();
                break;
            case controlStates.noMove:
                FrogNoMove();
                break;
            case controlStates.noGenAbi:
                FrogNoGenAbi();
                break;
            case controlStates.noCamAndMove:
                FrogNoCamAndMove();
                break;
            case controlStates.noMoveAndGenAbi:
                FrogNoMoveAndGenAbi();
                break;
            case controlStates.noCameraAndGenAbi:
                FrogNoCameraAndGenAbi();
                break;
            case controlStates.noControl:
                NoControl();
                break;
        }
    }

    private void FrogTotalControl()
    {

        AddAbility(abilties.move);
        AddAbility(abilties.rotate);

        AddAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.roll);
        RemoveAbility(abilties.moveBlock);
        RemoveAbility(abilties.VFissure);
        RemoveAbility(abilties.dolpSwimBel);


        AddAbility(abilties.toStd);
        AddAbility(abilties.toCrane);
        RemoveAbility(abilties.toFrog);
        AddAbility(abilties.toArma);
        AddAbility(abilties.toDolp);



        switch (cPlayerState.currentPhState)
        {
            case physicStates.onWater:
            case physicStates.onAir:
                RemoveAbility(abilties.jump);
                RemoveAbility(abilties.HFissure);
                break;
            case physicStates.onGround:
                AddAbility(abilties.jump);
                AddAbility(abilties.HFissure);
                break;
        }
    }

    private void FrogNoCamera()
    {
        AddAbility(abilties.move);
        AddAbility(abilties.rotate);

        RemoveAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.roll);
        RemoveAbility(abilties.moveBlock);
        RemoveAbility(abilties.VFissure);
        RemoveAbility(abilties.dolpSwimBel);


        AddAbility(abilties.toStd);
        AddAbility(abilties.toCrane);
        RemoveAbility(abilties.toFrog);
        AddAbility(abilties.toArma);
        AddAbility(abilties.toDolp);



        switch (cPlayerState.currentPhState)
        {
            case physicStates.onWater:
            case physicStates.onAir:
                RemoveAbility(abilties.jump);
                RemoveAbility(abilties.HFissure);
                break;
            case physicStates.onGround:
                AddAbility(abilties.jump);
                AddAbility(abilties.HFissure);
                break;
        }
    }

    private void FrogNoMove()
    {
        RemoveAbility(abilties.move);
        AddAbility(abilties.rotate);

        AddAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.roll);
        RemoveAbility(abilties.moveBlock);
        RemoveAbility(abilties.VFissure);
        RemoveAbility(abilties.dolpSwimBel);


        AddAbility(abilties.toStd);
        AddAbility(abilties.toCrane);
        RemoveAbility(abilties.toFrog);
        AddAbility(abilties.toArma);
        AddAbility(abilties.toDolp);



        switch (cPlayerState.currentPhState)
        {
            case physicStates.onWater:
            case physicStates.onAir:
                RemoveAbility(abilties.jump);
                RemoveAbility(abilties.HFissure);
                break;
            case physicStates.onGround:
                AddAbility(abilties.jump);
                AddAbility(abilties.HFissure);
                break;
        }
    }

    private void FrogNoGenAbi()
    {
        AddAbility(abilties.move);
        AddAbility(abilties.rotate);

        AddAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.roll);
        RemoveAbility(abilties.moveBlock);
        RemoveAbility(abilties.VFissure);
        RemoveAbility(abilties.dolpSwimBel);


        RemoveAbility(abilties.toStd);
        RemoveAbility(abilties.toCrane);
        RemoveAbility(abilties.toFrog);
        RemoveAbility(abilties.toArma);
        RemoveAbility(abilties.toDolp);


        RemoveAbility(abilties.jump);
        RemoveAbility(abilties.HFissure);
        
    }

    private void FrogNoCamAndMove()
    {
        RemoveAbility(abilties.move);
        AddAbility(abilties.rotate);

        RemoveAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.roll);
        RemoveAbility(abilties.moveBlock);
        RemoveAbility(abilties.VFissure);
        RemoveAbility(abilties.dolpSwimBel);


        AddAbility(abilties.toStd);
        AddAbility(abilties.toCrane);
        RemoveAbility(abilties.toFrog);
        AddAbility(abilties.toArma);
        AddAbility(abilties.toDolp);



        switch (cPlayerState.currentPhState)
        {
            case physicStates.onWater:
            case physicStates.onAir:
                RemoveAbility(abilties.jump);
                RemoveAbility(abilties.HFissure);
                break;
            case physicStates.onGround:
                AddAbility(abilties.jump);
                AddAbility(abilties.HFissure);
                break;
        }
    }

    private void FrogNoMoveAndGenAbi()
    {
        RemoveAbility(abilties.move);
        AddAbility(abilties.rotate);

        RemoveAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.roll);
        RemoveAbility(abilties.moveBlock);
        RemoveAbility(abilties.VFissure);
        RemoveAbility(abilties.dolpSwimBel);


        RemoveAbility(abilties.toStd);
        RemoveAbility(abilties.toCrane);
        RemoveAbility(abilties.toFrog);
        RemoveAbility(abilties.toArma);
        RemoveAbility(abilties.toDolp);


        RemoveAbility(abilties.jump);
        RemoveAbility(abilties.HFissure);
       
    }

    private void FrogNoCameraAndGenAbi()
    {
        AddAbility(abilties.move);
        AddAbility(abilties.rotate);

        RemoveAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.roll);
        RemoveAbility(abilties.moveBlock);
        RemoveAbility(abilties.VFissure);
        RemoveAbility(abilties.dolpSwimBel);


        RemoveAbility(abilties.toStd);
        RemoveAbility(abilties.toCrane);
        RemoveAbility(abilties.toFrog);
        RemoveAbility(abilties.toArma);
        RemoveAbility(abilties.toDolp);


        RemoveAbility(abilties.jump);
        RemoveAbility(abilties.HFissure);
        
    }
    #endregion

    #region Armadillo Abilities Handler
    private void UpdatingArmaAbilityList()
    {

        switch (cPlayerState.currentClState)
        {
            case controlStates.totalControl:
                ArmaTotalControl();
                break;
            case controlStates.noCamera:
                ArmaNoCamera();
                break;
            case controlStates.noMove:
                ArmaNoMove();
                break;
            case controlStates.noGenAbi:
                ArmaNoGenAbi();
                break;
            case controlStates.noCamAndMove:
                ArmaNoCamAndMove();
                break;
            case controlStates.noMoveAndGenAbi:
                ArmaNoMoveAndGenAbi();
                break;
            case controlStates.noCameraAndGenAbi:
                ArmaNoCameraAndGenAbi();
                break;
            case controlStates.noControl:
                NoControl();
                break;
        }
    }

    private void ArmaTotalControl()
    {

        AddAbility(abilties.move);
        AddAbility(abilties.rotate);

        AddAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.jump);
        RemoveAbility(abilties.VFissure);
        RemoveAbility(abilties.HFissure);
        RemoveAbility(abilties.dolpSwimBel);

        AddAbility(abilties.toStd);
        AddAbility(abilties.toCrane);
        AddAbility(abilties.toFrog);
        RemoveAbility(abilties.toArma);
        AddAbility(abilties.toDolp);



        switch (cPlayerState.currentPhState)
        {
            case physicStates.onWater:
            case physicStates.onAir:
                RemoveAbility(abilties.roll);
                RemoveAbility(abilties.moveBlock);
                break;
            case physicStates.onGround:
                AddAbility(abilties.roll);
                AddAbility(abilties.moveBlock);
                break;
        }
    }

    private void ArmaNoCamera()
    {
        AddAbility(abilties.move);
        AddAbility(abilties.rotate);

        RemoveAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.jump);
        RemoveAbility(abilties.VFissure);
        RemoveAbility(abilties.HFissure);
        RemoveAbility(abilties.dolpSwimBel);

        AddAbility(abilties.toStd);
        AddAbility(abilties.toCrane);
        AddAbility(abilties.toFrog);
        RemoveAbility(abilties.toArma);
        AddAbility(abilties.toDolp);



        switch (cPlayerState.currentPhState)
        {
            case physicStates.onWater:
            case physicStates.onAir:
                RemoveAbility(abilties.roll);
                RemoveAbility(abilties.moveBlock);
                break;
            case physicStates.onGround:
                AddAbility(abilties.roll);
                AddAbility(abilties.moveBlock);
                break;
        }
    }

    private void ArmaNoMove()
    {
        RemoveAbility(abilties.move);
        AddAbility(abilties.rotate);

        AddAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.jump);
        RemoveAbility(abilties.VFissure);
        RemoveAbility(abilties.HFissure);
        RemoveAbility(abilties.dolpSwimBel);

        AddAbility(abilties.toStd);
        AddAbility(abilties.toCrane);
        AddAbility(abilties.toFrog);
        RemoveAbility(abilties.toArma);
        AddAbility(abilties.toDolp);



        switch (cPlayerState.currentPhState)
        {
            case physicStates.onWater:
            case physicStates.onAir:
                RemoveAbility(abilties.roll);
                RemoveAbility(abilties.moveBlock);
                break;
            case physicStates.onGround:
                AddAbility(abilties.roll);
                AddAbility(abilties.moveBlock);
                break;
        }
    }

    private void ArmaNoGenAbi()
    {
        AddAbility(abilties.move);
        AddAbility(abilties.rotate);

        AddAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.jump);
        RemoveAbility(abilties.VFissure);
        RemoveAbility(abilties.HFissure);
        RemoveAbility(abilties.dolpSwimBel);

        RemoveAbility(abilties.toStd);
        RemoveAbility(abilties.toCrane);
        RemoveAbility(abilties.toFrog);
        RemoveAbility(abilties.toArma);
        RemoveAbility(abilties.toDolp);


        RemoveAbility(abilties.roll);
        RemoveAbility(abilties.moveBlock);
        
    }

    private void ArmaNoCamAndMove()
    {

        RemoveAbility(abilties.move);
        AddAbility(abilties.rotate);

        RemoveAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.jump);
        RemoveAbility(abilties.VFissure);
        RemoveAbility(abilties.HFissure);
        RemoveAbility(abilties.dolpSwimBel);

        AddAbility(abilties.toStd);
        AddAbility(abilties.toCrane);
        AddAbility(abilties.toFrog);
        RemoveAbility(abilties.toArma);
        AddAbility(abilties.toDolp);



        switch (cPlayerState.currentPhState)
        {
            case physicStates.onWater:
            case physicStates.onAir:
                RemoveAbility(abilties.roll);
                RemoveAbility(abilties.moveBlock);
                break;
            case physicStates.onGround:
                AddAbility(abilties.roll);
                AddAbility(abilties.moveBlock);
                break;
        }
    }

    private void ArmaNoMoveAndGenAbi()
    {

        RemoveAbility(abilties.move);
        AddAbility(abilties.rotate);

        AddAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.jump);
        RemoveAbility(abilties.VFissure);
        RemoveAbility(abilties.HFissure);
        RemoveAbility(abilties.dolpSwimBel);

        RemoveAbility(abilties.toStd);
        RemoveAbility(abilties.toCrane);
        RemoveAbility(abilties.toFrog);
        RemoveAbility(abilties.toArma);
        RemoveAbility(abilties.toDolp);


        RemoveAbility(abilties.roll);
        RemoveAbility(abilties.moveBlock);
       
    }

    private void ArmaNoCameraAndGenAbi()
    {

        AddAbility(abilties.move);
        AddAbility(abilties.rotate);

        RemoveAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.jump);
        RemoveAbility(abilties.VFissure);
        RemoveAbility(abilties.HFissure);
        RemoveAbility(abilties.dolpSwimBel);

        RemoveAbility(abilties.toStd);
        RemoveAbility(abilties.toCrane);
        RemoveAbility(abilties.toFrog);
        RemoveAbility(abilties.toArma);
        RemoveAbility(abilties.toDolp);


        RemoveAbility(abilties.roll);
        RemoveAbility(abilties.moveBlock);
        
    }
    #endregion

    #region Crane Abilities Handler
    private void UpdatingCraneAbilityList()
    {
        switch (cPlayerState.currentClState)
        {
            case controlStates.totalControl:
                CraneTotalControl();
                break;
            case controlStates.noCamera:
                CraneNoCamera();
                break;
            case controlStates.noMove:
                CraneNoMove();
                break;
            case controlStates.noGenAbi:
                CraneNoGenAbi();
                break;
            case controlStates.noCamAndMove:
                CraneNoCamAndMove();
                break;
            case controlStates.noMoveAndGenAbi:
                CraneNoMoveAndGenAbi();
                break;
            case controlStates.noCameraAndGenAbi:
                CraneNoCameraAndGenAbi();
                break;
            case controlStates.noControl:
                NoControl();
                break;
        }

    }

    private void CraneTotalControl()
    {
     
        AddAbility(abilties.rotate);

        AddAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.roll);
        RemoveAbility(abilties.moveBlock);
        RemoveAbility(abilties.HFissure);
        RemoveAbility(abilties.dolpSwimBel);
        RemoveAbility(abilties.jump);
        RemoveAbility(abilties.VFissure);

        AddAbility(abilties.toStd);
        RemoveAbility(abilties.toCrane);
        AddAbility(abilties.toFrog);
        AddAbility(abilties.toArma);
        AddAbility(abilties.toDolp);



        switch (cPlayerState.currentPhState)
        {
            case physicStates.onAir:
                AddAbility(abilties.move);
                break;
            case physicStates.onWater:
            case physicStates.onGround:
                RemoveAbility(abilties.move);
                break;
        }
    }

    private void CraneNoCamera()
    {
        AddAbility(abilties.rotate);

        RemoveAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.roll);
        RemoveAbility(abilties.moveBlock);
        RemoveAbility(abilties.HFissure);
        RemoveAbility(abilties.dolpSwimBel);
        RemoveAbility(abilties.jump);
        RemoveAbility(abilties.VFissure);

        AddAbility(abilties.toStd);
        RemoveAbility(abilties.toCrane);
        AddAbility(abilties.toFrog);
        AddAbility(abilties.toArma);
        AddAbility(abilties.toDolp);



        switch (cPlayerState.currentPhState)
        {
            case physicStates.onAir:
                AddAbility(abilties.move);
                break;
            case physicStates.onWater:
            case physicStates.onGround:
                RemoveAbility(abilties.move);
                break;
        }
    }

    private void CraneNoMove()
    {
        AddAbility(abilties.rotate);

        AddAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.roll);
        RemoveAbility(abilties.moveBlock);
        RemoveAbility(abilties.HFissure);
        RemoveAbility(abilties.dolpSwimBel);
        RemoveAbility(abilties.jump);
        RemoveAbility(abilties.VFissure);

        AddAbility(abilties.toStd);
        RemoveAbility(abilties.toCrane);
        AddAbility(abilties.toFrog);
        AddAbility(abilties.toArma);
        AddAbility(abilties.toDolp);


        RemoveAbility(abilties.move);

      
    }

    private void CraneNoGenAbi()
    {
        AddAbility(abilties.rotate);

        AddAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.roll);
        RemoveAbility(abilties.moveBlock);
        RemoveAbility(abilties.HFissure);
        RemoveAbility(abilties.dolpSwimBel);
        RemoveAbility(abilties.jump);
        RemoveAbility(abilties.VFissure);

        RemoveAbility(abilties.toStd);
        RemoveAbility(abilties.toCrane);
        RemoveAbility(abilties.toFrog);
        RemoveAbility(abilties.toArma);
        RemoveAbility(abilties.toDolp);



        switch (cPlayerState.currentPhState)
        {
            case physicStates.onAir:
                AddAbility(abilties.move);
                break;
            case physicStates.onWater:
            case physicStates.onGround:
                RemoveAbility(abilties.move);
                break;
        }
    }

    private void CraneNoCamAndMove()
    {
        AddAbility(abilties.rotate);

        RemoveAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.roll);
        RemoveAbility(abilties.moveBlock);
        RemoveAbility(abilties.HFissure);
        RemoveAbility(abilties.dolpSwimBel);
        RemoveAbility(abilties.jump);
        RemoveAbility(abilties.VFissure);

        AddAbility(abilties.toStd);
        RemoveAbility(abilties.toCrane);
        AddAbility(abilties.toFrog);
        AddAbility(abilties.toArma);
        AddAbility(abilties.toDolp);


        RemoveAbility(abilties.move);

       
    }

    private void CraneNoMoveAndGenAbi()
    {
        AddAbility(abilties.rotate);

        AddAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.roll);
        RemoveAbility(abilties.moveBlock);
        RemoveAbility(abilties.HFissure);
        RemoveAbility(abilties.dolpSwimBel);
        RemoveAbility(abilties.jump);
        RemoveAbility(abilties.VFissure);

        RemoveAbility(abilties.toStd);
        RemoveAbility(abilties.toCrane);
        RemoveAbility(abilties.toFrog);
        RemoveAbility(abilties.toArma);
        RemoveAbility(abilties.toDolp);


        RemoveAbility(abilties.move);

    }

    private void CraneNoCameraAndGenAbi()
    {
        AddAbility(abilties.rotate);

        RemoveAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.roll);
        RemoveAbility(abilties.moveBlock);
        RemoveAbility(abilties.HFissure);
        RemoveAbility(abilties.dolpSwimBel);
        RemoveAbility(abilties.jump);
        RemoveAbility(abilties.VFissure);

        RemoveAbility(abilties.toStd);
        RemoveAbility(abilties.toCrane);
        RemoveAbility(abilties.toFrog);
        RemoveAbility(abilties.toArma);
        RemoveAbility(abilties.toDolp);


        switch (cPlayerState.currentPhState)
        {
            case physicStates.onAir:
                AddAbility(abilties.move);
                break;
            case physicStates.onWater:
            case physicStates.onGround:
                RemoveAbility(abilties.move);
                break;
        }
    }
    #endregion

    #region Dolphin Abilities Handler
    private void UpdatingDolpAbilityList()
    {
        switch (cPlayerState.currentClState)
        {
            case controlStates.totalControl:
                DolpTotalControl();
                break;
            case controlStates.noCamera:
                DolpNoCamera();
                break;
            case controlStates.noMove:
                DolpNoMove();
                break;
            case controlStates.noGenAbi:
                DolpNoGenAbi();
                break;
            case controlStates.noCamAndMove:
                DolpNoCamAndMove();
                break;
            case controlStates.noMoveAndGenAbi:
                DolpNoMoveAndGenAbi();
                break;
            case controlStates.noCameraAndGenAbi:
                DolpNoCameraAndGenAbi();
                break;
            case controlStates.noControl:
                NoControl();
                break;
        }
    }

    private void DolpTotalControl()
    {
  
        AddAbility(abilties.rotate);

        AddAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.roll);
        RemoveAbility(abilties.moveBlock);
        RemoveAbility(abilties.HFissure);
        RemoveAbility(abilties.dolpSwimBel);
        RemoveAbility(abilties.VFissure);

        AddAbility(abilties.toStd);
        AddAbility(abilties.toCrane);
        AddAbility(abilties.toFrog);
        AddAbility(abilties.toArma);
        RemoveAbility(abilties.toDolp);



        switch (cPlayerState.currentPhState)
        {
            case physicStates.onWater:
                AddAbility(abilties.jump);
                AddAbility(abilties.move);
                break;
            case physicStates.onGround:
            case physicStates.onAir:
                RemoveAbility(abilties.jump);
                RemoveAbility(abilties.move);
                break;         
        }
    }

    private void DolpNoCamera()
    {
        AddAbility(abilties.rotate);

        RemoveAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.roll);
        RemoveAbility(abilties.moveBlock);
        RemoveAbility(abilties.HFissure);
        RemoveAbility(abilties.dolpSwimBel);
        RemoveAbility(abilties.VFissure);

        AddAbility(abilties.toStd);
        AddAbility(abilties.toCrane);
        AddAbility(abilties.toFrog);
        AddAbility(abilties.toArma);
        RemoveAbility(abilties.toDolp);



        switch (cPlayerState.currentPhState)
        {
            case physicStates.onWater:
                AddAbility(abilties.jump);
                AddAbility(abilties.move);
                break;
            case physicStates.onGround:
            case physicStates.onAir:
                RemoveAbility(abilties.jump);
                RemoveAbility(abilties.move);
                break;
        }
    }

    private void DolpNoMove()
    {
        AddAbility(abilties.rotate);

        AddAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.roll);
        RemoveAbility(abilties.moveBlock);
        RemoveAbility(abilties.HFissure);
        RemoveAbility(abilties.dolpSwimBel);
        RemoveAbility(abilties.VFissure);

        AddAbility(abilties.toStd);
        AddAbility(abilties.toCrane);
        AddAbility(abilties.toFrog);
        AddAbility(abilties.toArma);
        RemoveAbility(abilties.toDolp);


        RemoveAbility(abilties.move);

        switch (cPlayerState.currentPhState)
        {
            case physicStates.onWater:
                AddAbility(abilties.jump);
                break;
            case physicStates.onGround:
            case physicStates.onAir:
                RemoveAbility(abilties.jump);      
                break;
        }
    }

    private void DolpNoGenAbi()
    {
        AddAbility(abilties.rotate);

        AddAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.roll);
        RemoveAbility(abilties.moveBlock);
        RemoveAbility(abilties.HFissure);
        RemoveAbility(abilties.dolpSwimBel);
        RemoveAbility(abilties.VFissure);

        RemoveAbility(abilties.toStd);
        RemoveAbility(abilties.toCrane);
        RemoveAbility(abilties.toFrog);
        RemoveAbility(abilties.toArma);
        RemoveAbility(abilties.toDolp);


        RemoveAbility(abilties.jump);

        switch (cPlayerState.currentPhState)
        {
            case physicStates.onWater:
                AddAbility(abilties.move);
                break;
            case physicStates.onGround:
            case physicStates.onAir:
                RemoveAbility(abilties.move);
                break;
        }
    }

    private void DolpNoCamAndMove()
    {
        AddAbility(abilties.rotate);

        RemoveAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.roll);
        RemoveAbility(abilties.moveBlock);
        RemoveAbility(abilties.HFissure);
        RemoveAbility(abilties.dolpSwimBel);
        RemoveAbility(abilties.VFissure);

        AddAbility(abilties.toStd);
        AddAbility(abilties.toCrane);
        AddAbility(abilties.toFrog);
        AddAbility(abilties.toArma);
        RemoveAbility(abilties.toDolp);

        RemoveAbility(abilties.move);

        switch (cPlayerState.currentPhState)
        {
            case physicStates.onWater:
                AddAbility(abilties.jump);
                break;
            case physicStates.onGround:
            case physicStates.onAir:
                RemoveAbility(abilties.jump);
                break;
        }
    }

    private void DolpNoMoveAndGenAbi()
    {
        AddAbility(abilties.rotate);

        AddAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.roll);
        RemoveAbility(abilties.moveBlock);
        RemoveAbility(abilties.HFissure);
        RemoveAbility(abilties.dolpSwimBel);
        RemoveAbility(abilties.VFissure);

        RemoveAbility(abilties.toStd);
        RemoveAbility(abilties.toCrane);
        RemoveAbility(abilties.toFrog);
        RemoveAbility(abilties.toArma);
        RemoveAbility(abilties.toDolp);


        RemoveAbility(abilties.jump);
        RemoveAbility(abilties.move);
        
    }

    private void DolpNoCameraAndGenAbi()
    {
        AddAbility(abilties.rotate);

        RemoveAbility(abilties.cameraMove);

        AddAbility(abilties.npcInter);
        AddAbility(abilties.menu);

        RemoveAbility(abilties.roll);
        RemoveAbility(abilties.moveBlock);
        RemoveAbility(abilties.HFissure);
        RemoveAbility(abilties.dolpSwimBel);
        RemoveAbility(abilties.VFissure);

        RemoveAbility(abilties.toStd);
        RemoveAbility(abilties.toCrane);
        RemoveAbility(abilties.toFrog);
        RemoveAbility(abilties.toArma);
        RemoveAbility(abilties.toDolp);

        RemoveAbility(abilties.jump);

        switch (cPlayerState.currentPhState)
        {
            case physicStates.onWater:
                AddAbility(abilties.move);
                break;
            case physicStates.onGround:
            case physicStates.onAir:
                RemoveAbility(abilties.move);
                break;
        }
    }
    #endregion

    #region Generic Ability Handler Methods
    private void NoControl()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();
        AddAbility(abilties.menu);
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
    #endregion
    #endregion




  


}

