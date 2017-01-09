﻿using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

#region Finite State Machine enum Structure


public enum abilties
{
    move, rotate, cameraMove, npcInter, menu,
    jump, roll, moveOnRoll, moveBlock, VFissure, HFissure, dolpSwimBel,
    toStd, toFrog, toArma, toCrane, toDolp
};

public enum physicStates { onAir, onGround, onWater }

public enum playerStates
{
    standingStill, moving, flying,
    rolling, movingBlock
};

public enum triggerGenAbiStates { onVFissure, onHFissure, onDolpSwimBel, onMoveBlock, npcTalk, noTrigger };

public enum triggerSpecial { cameraLimited, noTrigger };

public enum controlStates { totalControl, noCamera, noMove, noGenAbi, noCamAndMove, noMoveAndGenAbi, noCameraAndGenAbi, noControl };


#endregion

#region Event Classes
[System.Serializable]
public class event_ps : UnityEvent<physicStates>
{
}

[System.Serializable]
public class event_vfscript_string : UnityEvent<VFissure, string>
{
}

[System.Serializable]
public class event_abi_vector3 : UnityEvent<abilties, Vector3>
{
}

[System.Serializable]
public class event_abi : UnityEvent<abilties>
{
}

[System.Serializable]
public class event_int : UnityEvent<int>
{
}

[System.Serializable]
public class event_string_string_listGb : UnityEvent<string, string, List<GameObject>>
{
}

[System.Serializable]
public class event_vector3_string_ps : UnityEvent<Vector3, string, physicStates>
{
}

[System.Serializable]
public class event_vector3_ps : UnityEvent<Vector3, playerStates>
{
}

[System.Serializable]
public class event_abi_string : UnityEvent<abilties, string>
{
}

[System.Serializable]
public class event_pl : UnityEvent<playerStates>
{
}

[System.Serializable]
public class event_string : UnityEvent<string>
{
}

[System.Serializable]
public class event_vector3_float : UnityEvent<Vector3, float>
{
}

[System.Serializable]
public class event_float : UnityEvent<float>
{
}

[System.Serializable]
public class event_cs : UnityEvent<controlStates>
{
    
}
#endregion

public class FSMChecker : MonoBehaviour
{
    #region Public Variables
    public formsSettings abiUnlocked;
    public float drowTimerSetting;
    #endregion

    #region Private Variables
    public SaveSystem save;
    [System.Serializable]
    public struct playerCState
    {
        public string currentForm;
        public string previousForm;
        public List<GameObject> forms;
        public List<abilties> currentAbilities;
        public physicStates currentPhState;
        public playerStates currentPlState;
        public triggerGenAbiStates currentTRGState;
        public triggerSpecial currentTSState;
        public controlStates currentClState;

    }

    [SerializeField]
    playerCState cPlayerState;

    private CharacterController ccLink;
    private VFissure vfLink;
    private string vFissureEntrance;
    private bool dying = false;
    #endregion

    #region Events
    public event_string formChangedInp;
    public event_vfscript_string vFissureUsed;
    public UnityEvent stoppingRollLogic, enableGlideLogic, stopGlideLogic, deathRequest;
    public event_abi_string genAbiUsed;
    public event_vector3_ps rotationUsed;
    public event_string_string_listGb formChanged;
    public event_vector3_string_ps moveUsed;
    public event_ps phStateChanged;
    public event_pl plStateChanged; 
    #endregion

    #region Initialization Methods
    void Awake()
    {

        ccLink = this.gameObject.GetComponent<CharacterController>();

        SettingPlayerInitialState();



        PlayerInputs plInputsTempLink = this.gameObject.GetComponent<PlayerInputs>();


        plInputsTempLink.dirAbiRequest.AddListener(CheckingDirAbiRequirements);
        plInputsTempLink.genAbiRequest.AddListener(CheckingAbiRequirements);
        plInputsTempLink.rollStopped.AddListener(EnablingMove);

        EnvInputs enInputsTempLink = this.gameObject.GetComponent<EnvInputs>();

        enInputsTempLink.psChanged.AddListener(ChangingPHStates);
        enInputsTempLink.vFissureRequestOn.AddListener(AddingVFissureAvailability);
        enInputsTempLink.vFissureRequestOff.AddListener(RemovingVFissureAvailability);

        FSMExecutor fsmExeTempLink = this.gameObject.GetComponent<FSMExecutor>();

        fsmExeTempLink.vFissureAniEnded.AddListener(VFissureAniEndEffects);



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

        HandlingAbiDiscovered();

        cPlayerState.currentAbilities.Add(abilties.rotate);


        cPlayerState.currentPhState = physicStates.onGround;

        cPlayerState.currentPlState = playerStates.standingStill;

        cPlayerState.currentTRGState = triggerGenAbiStates.noTrigger;

        cPlayerState.currentTSState = triggerSpecial.noTrigger;

        cPlayerState.currentClState = controlStates.totalControl;

        SettingCapsuleCollider(0.15f, 1);

    }
    #endregion

    #region Player Inputs Handler
    private void CheckingDirAbiRequirements(abilties abiReceived, Vector3 abiDir)
    {

        if (cPlayerState.currentAbilities.Contains(abiReceived))
        {
            switch (abiReceived)
            {

                case abilties.move:
                    moveUsed.Invoke(abiDir, cPlayerState.currentForm, cPlayerState.currentPhState);
                    break;
                case abilties.rotate:
                    rotationUsed.Invoke(abiDir, cPlayerState.currentPlState);
                    break;
                case abilties.moveOnRoll:
                    rotationUsed.Invoke(abiDir, cPlayerState.currentPlState);
                    break;
            }

        }

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
                    cPlayerState.currentPlState = playerStates.rolling;
                    cPlayerState.currentClState = controlStates.noMove;
                    UpdatingAbilityList();
                    genAbiUsed.Invoke(abiReceived, cPlayerState.currentForm);
                    break;
                case abilties.moveBlock:
                    break;
                case abilties.VFissure:
                    Debug.Log("VFissure Pressed");
                    cPlayerState.currentClState = controlStates.noMoveAndGenAbi;
                    UpdatingAbilityList();
                    vFissureUsed.Invoke(vfLink, vFissureEntrance);
                    break;
                case abilties.HFissure:
                    Debug.Log("HFissure Pressed");
                    cPlayerState.currentClState = controlStates.noMoveAndGenAbi;
                    UpdatingAbilityList();
                    vFissureUsed.Invoke(vfLink, vFissureEntrance);
                    break;
                case abilties.dolpSwimBel:
                    Debug.Log("DolpSwimBelow Pressed");
                    cPlayerState.currentClState = controlStates.noMoveAndGenAbi;
                    UpdatingAbilityList();
                    vFissureUsed.Invoke(vfLink, vFissureEntrance);
                    break;
                case abilties.toStd:
                    cPlayerState.previousForm = cPlayerState.currentForm;
                    cPlayerState.currentForm = "Standard Form";
                    formChangedInp.Invoke(cPlayerState.currentForm);
                    if (cPlayerState.currentPlState == playerStates.rolling)
                        EnablingMove();
                    else
                        UpdatingAbilityList();
                    formChanged.Invoke(cPlayerState.currentForm, cPlayerState.previousForm, cPlayerState.forms);
                    if (cPlayerState.previousForm == "Dragon Form")
                        stopGlideLogic.Invoke();
                     SettingCapsuleCollider(0.15f, 1);
                    break;
                case abilties.toFrog:
                    cPlayerState.previousForm = cPlayerState.currentForm;
                    cPlayerState.currentForm = "Frog Form";
                    formChangedInp.Invoke(cPlayerState.currentForm);
                    if (cPlayerState.currentPlState == playerStates.rolling)
                        EnablingMove();
                    else
                        UpdatingAbilityList();
                    formChanged.Invoke(cPlayerState.currentForm, cPlayerState.previousForm, cPlayerState.forms);
                    if (cPlayerState.previousForm == "Dragon Form")
                        stopGlideLogic.Invoke();
                     SettingCapsuleCollider(0.15f, 0.7f);
                    break;
                case abilties.toCrane:
                    cPlayerState.previousForm = cPlayerState.currentForm;
                    cPlayerState.currentForm = "Dragon Form";
                    formChangedInp.Invoke(cPlayerState.currentForm);
                    if (cPlayerState.currentPlState == playerStates.rolling)
                        EnablingMove();
                    else
                        UpdatingAbilityList();
                    formChanged.Invoke(cPlayerState.currentForm, cPlayerState.previousForm, cPlayerState.forms);
                    enableGlideLogic.Invoke();
                    SettingCapsuleCollider(0.15f, 0.7f);
                    break;
                case abilties.toArma:
                    cPlayerState.previousForm = cPlayerState.currentForm;
                    cPlayerState.currentForm = "Armadillo Form";
                    formChangedInp.Invoke(cPlayerState.currentForm);
                    UpdatingAbilityList();
                    formChanged.Invoke(cPlayerState.currentForm, cPlayerState.previousForm, cPlayerState.forms);
                    if (cPlayerState.previousForm == "Dragon Form")
                        stopGlideLogic.Invoke();
                    SettingCapsuleCollider(0.15f, 0.7f);
                    break;
                case abilties.toDolp:
                    cPlayerState.previousForm = cPlayerState.currentForm;
                    cPlayerState.currentForm = "Dolphin Form";
                    formChangedInp.Invoke(cPlayerState.currentForm);
                    if (cPlayerState.currentPlState == playerStates.rolling)
                        EnablingMove();
                    else
                        UpdatingAbilityList();
                    formChanged.Invoke(cPlayerState.currentForm, cPlayerState.previousForm, cPlayerState.forms);
                    if (cPlayerState.previousForm == "Dragon Form")
                        stopGlideLogic.Invoke();
                    SettingCapsuleCollider(0.15f, 0.7f);
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

    private void AddingVFissureAvailability(VFissure vfTempLink, string vfTag)
    {
        vfLink = vfTempLink;
        vFissureEntrance = vfTag;

        switch (vFissureEntrance)
        {
            case "vAbilityta":
            case "vAbilitytb":
                cPlayerState.currentTRGState = triggerGenAbiStates.onVFissure;
                break;
            case "hAbilityta":
            case "hAbilitytb":
                cPlayerState.currentTRGState = triggerGenAbiStates.onHFissure;
                break;
            case "dAbilityta":
            case "dAbilitytb":
                cPlayerState.currentTRGState = triggerGenAbiStates.onDolpSwimBel;
                break;
        }

        UpdatingAbilityList();

    }

    private void RemovingVFissureAvailability()
    {
        vfLink = null;
        cPlayerState.currentTRGState = triggerGenAbiStates.noTrigger;
        UpdatingAbilityList();

    }

    private void VFissureAniEndEffects()
    {
        cPlayerState.currentClState = controlStates.totalControl;
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
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();

        AddAbility(abilties.move);
        AddAbility(abilties.rotate);
        AddAbility(abilties.cameraMove);

        AddAbility(abilties.menu);

        HandlingAbiDiscovered();

        if (cPlayerState.currentPhState == physicStates.onGround)
        {

            if (cPlayerState.currentTRGState == triggerGenAbiStates.onVFissure)
                AddAbility(abilties.VFissure);
            else if (cPlayerState.currentTRGState == triggerGenAbiStates.npcTalk)
                AddAbility(abilties.npcInter);
            AddAbility(abilties.jump);

        }

    }

    private void StdNoCamera()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();

        AddAbility(abilties.move);
        AddAbility(abilties.rotate);


        AddAbility(abilties.menu);

        HandlingAbiDiscovered();



        if (cPlayerState.currentPhState == physicStates.onGround)
        {

            if (cPlayerState.currentTRGState == triggerGenAbiStates.onVFissure)
                AddAbility(abilties.VFissure);
            else if (cPlayerState.currentTRGState == triggerGenAbiStates.npcTalk)
                AddAbility(abilties.npcInter);
            AddAbility(abilties.jump);

        }


    }

    private void StdNoMove()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();



        AddAbility(abilties.cameraMove);

        AddAbility(abilties.menu);

        HandlingAbiDiscovered();



        if (cPlayerState.currentPhState == physicStates.onGround)
        {

            if (cPlayerState.currentTRGState == triggerGenAbiStates.onVFissure)
                AddAbility(abilties.VFissure);
            else if (cPlayerState.currentTRGState == triggerGenAbiStates.npcTalk)
                AddAbility(abilties.npcInter);
            AddAbility(abilties.jump);

        }


    }

    private void StdNoGenAbi()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();

        AddAbility(abilties.move);
        AddAbility(abilties.rotate);
        AddAbility(abilties.cameraMove);

        AddAbility(abilties.menu);

    }

    private void StdNoCamAndMove()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();


        AddAbility(abilties.rotate);


        AddAbility(abilties.menu);

        HandlingAbiDiscovered();



        if (cPlayerState.currentPhState == physicStates.onGround)
        {

            if (cPlayerState.currentTRGState == triggerGenAbiStates.onVFissure)
                AddAbility(abilties.VFissure);
            else if (cPlayerState.currentTRGState == triggerGenAbiStates.npcTalk)
                AddAbility(abilties.npcInter);
            AddAbility(abilties.jump);

        }


    }

    private void StdNoMoveAndGenAbi()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();



        AddAbility(abilties.cameraMove);
        AddAbility(abilties.menu);


    }

    private void StdNoCameraAndGenAbi()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();

        AddAbility(abilties.move);
        AddAbility(abilties.rotate);


        AddAbility(abilties.menu);


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

        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();

        AddAbility(abilties.move);
        AddAbility(abilties.rotate);
        AddAbility(abilties.cameraMove);

        AddAbility(abilties.menu);

        AddAbility(abilties.toStd);
       
        HandlingAbiDiscovered();

        if (cPlayerState.currentPhState == physicStates.onGround)
        {

            if (cPlayerState.currentTRGState == triggerGenAbiStates.onHFissure)
                AddAbility(abilties.HFissure);
            else if (cPlayerState.currentTRGState == triggerGenAbiStates.npcTalk)
                AddAbility(abilties.npcInter);
            AddAbility(abilties.jump);
        }

    }

    private void FrogNoCamera()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();

        AddAbility(abilties.move);
        AddAbility(abilties.rotate);


        AddAbility(abilties.menu);

        AddAbility(abilties.toStd);

        HandlingAbiDiscovered();


        if (cPlayerState.currentPhState == physicStates.onGround)
        {

            if (cPlayerState.currentTRGState == triggerGenAbiStates.onHFissure)
                AddAbility(abilties.HFissure);
            else if (cPlayerState.currentTRGState == triggerGenAbiStates.npcTalk)
                AddAbility(abilties.npcInter);
            AddAbility(abilties.jump);
        }
    }

    private void FrogNoMove()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();


        AddAbility(abilties.cameraMove);

        AddAbility(abilties.menu);

        AddAbility(abilties.toStd);

        HandlingAbiDiscovered();


        if (cPlayerState.currentPhState == physicStates.onGround)
        {

            if (cPlayerState.currentTRGState == triggerGenAbiStates.onHFissure)
                AddAbility(abilties.HFissure);
            else if (cPlayerState.currentTRGState == triggerGenAbiStates.npcTalk)
                AddAbility(abilties.npcInter);
            AddAbility(abilties.jump);
        }
    }

    private void FrogNoGenAbi()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();

        AddAbility(abilties.move);
        AddAbility(abilties.rotate);
        AddAbility(abilties.cameraMove);

        AddAbility(abilties.menu);



    }

    private void FrogNoCamAndMove()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();


        AddAbility(abilties.menu);

        AddAbility(abilties.toStd);

        HandlingAbiDiscovered();


        if (cPlayerState.currentPhState == physicStates.onGround)
        {

            if (cPlayerState.currentTRGState == triggerGenAbiStates.onHFissure)
                AddAbility(abilties.HFissure);
            else if (cPlayerState.currentTRGState == triggerGenAbiStates.npcTalk)
                AddAbility(abilties.npcInter);
            AddAbility(abilties.jump);
        }
    }

    private void FrogNoMoveAndGenAbi()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();


        AddAbility(abilties.cameraMove);

        AddAbility(abilties.menu);



    }

    private void FrogNoCameraAndGenAbi()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();

        AddAbility(abilties.move);
        AddAbility(abilties.rotate);


        AddAbility(abilties.menu);



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

        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();

        AddAbility(abilties.move);
        AddAbility(abilties.rotate);
        AddAbility(abilties.cameraMove);

        AddAbility(abilties.menu);

        AddAbility(abilties.toStd);

        HandlingAbiDiscovered();


        if (cPlayerState.currentPhState == physicStates.onGround)
        {

            if (cPlayerState.currentTRGState == triggerGenAbiStates.onMoveBlock)
                AddAbility(abilties.moveBlock);
            else if (cPlayerState.currentTRGState == triggerGenAbiStates.npcTalk)
                AddAbility(abilties.npcInter);
            AddAbility(abilties.roll);
        }



    }

    private void ArmaNoCamera()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();

        AddAbility(abilties.move);
        AddAbility(abilties.rotate);


        AddAbility(abilties.menu);

        AddAbility(abilties.toStd);

        HandlingAbiDiscovered();


        if (cPlayerState.currentPhState == physicStates.onGround)
        {

            if (cPlayerState.currentTRGState == triggerGenAbiStates.onMoveBlock)
                AddAbility(abilties.moveBlock);
            else if (cPlayerState.currentTRGState == triggerGenAbiStates.npcTalk)
                AddAbility(abilties.npcInter);
            AddAbility(abilties.roll);
        }
    }

    private void ArmaNoMove()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();


        AddAbility(abilties.cameraMove);

        AddAbility(abilties.menu);

        AddAbility(abilties.toStd);

        HandlingAbiDiscovered();

        if (cPlayerState.currentPhState == physicStates.onGround)
        {

            if (cPlayerState.currentTRGState == triggerGenAbiStates.onMoveBlock)
                AddAbility(abilties.moveBlock);
            else if (cPlayerState.currentTRGState == triggerGenAbiStates.npcTalk)
                AddAbility(abilties.npcInter);
            if (cPlayerState.currentPlState == playerStates.rolling)
                AddAbility(abilties.moveOnRoll);
            else if (cPlayerState.currentPlState == playerStates.movingBlock)
                Debug.Log("Something");
            else
                AddAbility(abilties.roll);
        }
        else if (cPlayerState.currentPhState == physicStates.onWater &&
               cPlayerState.currentPlState == playerStates.rolling)
            EnablingMove();

    }

    private void ArmaNoGenAbi()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();

        AddAbility(abilties.move);
        AddAbility(abilties.rotate);
        AddAbility(abilties.cameraMove);

        AddAbility(abilties.menu);



    }

    private void ArmaNoCamAndMove()
    {

        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();



        AddAbility(abilties.menu);

        AddAbility(abilties.toStd);

        HandlingAbiDiscovered();

        if (cPlayerState.currentPhState == physicStates.onGround)
        {

            if (cPlayerState.currentTRGState == triggerGenAbiStates.onMoveBlock)
                AddAbility(abilties.moveBlock);
            else if (cPlayerState.currentTRGState == triggerGenAbiStates.npcTalk)
                AddAbility(abilties.npcInter);
            AddAbility(abilties.roll);

        }

    }

    private void ArmaNoMoveAndGenAbi()
    {

        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();


        AddAbility(abilties.cameraMove);

        AddAbility(abilties.menu);


    }

    private void ArmaNoCameraAndGenAbi()
    {

        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();

        AddAbility(abilties.move);
        AddAbility(abilties.rotate);


        AddAbility(abilties.menu);


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

        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();


        AddAbility(abilties.rotate);
        AddAbility(abilties.cameraMove);

        AddAbility(abilties.menu);

        AddAbility(abilties.toStd);

        HandlingAbiDiscovered();

        if (cPlayerState.currentPhState == physicStates.onAir)
            AddAbility(abilties.move);

    }

    private void CraneNoCamera()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();


        AddAbility(abilties.rotate);


        AddAbility(abilties.menu);

        AddAbility(abilties.toStd);

        HandlingAbiDiscovered();

        if (cPlayerState.currentPhState == physicStates.onAir)
            AddAbility(abilties.move);
    }

    private void CraneNoMove()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();



        AddAbility(abilties.cameraMove);

        AddAbility(abilties.menu);

        AddAbility(abilties.toStd);

        HandlingAbiDiscovered();


    }

    private void CraneNoGenAbi()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();


        AddAbility(abilties.rotate);
        AddAbility(abilties.cameraMove);

        AddAbility(abilties.menu);


        if (cPlayerState.currentPhState == physicStates.onAir)
            AddAbility(abilties.move);
    }

    private void CraneNoCamAndMove()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();


        AddAbility(abilties.menu);

        AddAbility(abilties.toStd);

        HandlingAbiDiscovered();



    }

    private void CraneNoMoveAndGenAbi()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();

        AddAbility(abilties.cameraMove);

        AddAbility(abilties.menu);



    }

    private void CraneNoCameraAndGenAbi()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();


        AddAbility(abilties.rotate);


        AddAbility(abilties.menu);


        if (cPlayerState.currentPhState == physicStates.onAir)
            AddAbility(abilties.move);
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

        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();


        AddAbility(abilties.rotate);
        AddAbility(abilties.cameraMove);

        AddAbility(abilties.menu);

        AddAbility(abilties.toStd);

        HandlingAbiDiscovered();



        switch (cPlayerState.currentPhState)
        {
            case physicStates.onAir:
                AddAbility(abilties.move);
                break;
            case physicStates.onWater:
                if (cPlayerState.currentTRGState == triggerGenAbiStates.onDolpSwimBel)
                    AddAbility(abilties.dolpSwimBel);
                else if (cPlayerState.currentTRGState == triggerGenAbiStates.npcTalk)
                    AddAbility(abilties.npcInter);
                AddAbility(abilties.jump);
                AddAbility(abilties.move);
                break;
        }
    }

    private void DolpNoCamera()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();


        AddAbility(abilties.rotate);


        AddAbility(abilties.menu);

        AddAbility(abilties.toStd);

        HandlingAbiDiscovered();



        switch (cPlayerState.currentPhState)
        {
            case physicStates.onAir:
                AddAbility(abilties.move);
                break;
            case physicStates.onWater:
                if (cPlayerState.currentTRGState == triggerGenAbiStates.onDolpSwimBel)
                    AddAbility(abilties.dolpSwimBel);
                else if (cPlayerState.currentTRGState == triggerGenAbiStates.npcTalk)
                    AddAbility(abilties.npcInter);
                AddAbility(abilties.jump);
                AddAbility(abilties.move);
                break;
        }
    }

    private void DolpNoMove()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();


        AddAbility(abilties.cameraMove);

        AddAbility(abilties.menu);

        AddAbility(abilties.toStd);

        HandlingAbiDiscovered();


        if (cPlayerState.currentPhState == physicStates.onWater)
        {

            if (cPlayerState.currentTRGState == triggerGenAbiStates.onDolpSwimBel)
                AddAbility(abilties.dolpSwimBel);
            else if (cPlayerState.currentTRGState == triggerGenAbiStates.npcTalk)
                AddAbility(abilties.npcInter);
            AddAbility(abilties.jump);
        }

    }

    private void DolpNoGenAbi()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();


        AddAbility(abilties.rotate);
        AddAbility(abilties.cameraMove);

        AddAbility(abilties.menu);


        if (cPlayerState.currentPhState == physicStates.onWater)
            AddAbility(abilties.move);

    }

    private void DolpNoCamAndMove()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();



        AddAbility(abilties.menu);

        AddAbility(abilties.toStd);

        HandlingAbiDiscovered();

        if (cPlayerState.currentPhState == physicStates.onWater)
        {

            if (cPlayerState.currentTRGState == triggerGenAbiStates.onDolpSwimBel)
                AddAbility(abilties.dolpSwimBel);
            else if (cPlayerState.currentTRGState == triggerGenAbiStates.npcTalk)
                AddAbility(abilties.npcInter);
            AddAbility(abilties.jump);

        }

    }

    private void DolpNoMoveAndGenAbi()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();


        AddAbility(abilties.cameraMove);

        AddAbility(abilties.menu);


    }

    private void DolpNoCameraAndGenAbi()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();

        AddAbility(abilties.rotate);

        AddAbility(abilties.menu);

        if (cPlayerState.currentPhState == physicStates.onWater)
            AddAbility(abilties.move);

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

    private void EnablingMove()
    {
        cPlayerState.currentPlState = playerStates.standingStill;
        cPlayerState.currentClState = controlStates.totalControl;
        UpdatingAbilityList();
        stoppingRollLogic.Invoke();
    }

    private void SettingCapsuleCollider(float rad, float height)
    {
        ccLink.radius = rad;
        ccLink.height = height;
    }

    private void HandlingAbiDiscovered()
    {

        switch (cPlayerState.currentForm)
        {

            case "Standard Form":
                if (abiUnlocked.armaUnlocked)
                    AddAbility(abilties.toArma);
                if (abiUnlocked.frogUnlocked)
                    AddAbility(abilties.toFrog);
                if (abiUnlocked.craneUnlocked)
                    AddAbility(abilties.toCrane);
                if (abiUnlocked.dolphinUnlocked)
                    AddAbility(abilties.toDolp);
                break;
            case "Frog Form":
                if (abiUnlocked.armaUnlocked)
                    AddAbility(abilties.toArma);
                if (abiUnlocked.craneUnlocked)
                    AddAbility(abilties.toCrane);
                if (abiUnlocked.dolphinUnlocked)
                    AddAbility(abilties.toDolp);
                break;
            case "Armadillo Form":
                if (abiUnlocked.frogUnlocked)
                    AddAbility(abilties.toFrog);
                if (abiUnlocked.craneUnlocked)
                    AddAbility(abilties.toCrane);
                if (abiUnlocked.dolphinUnlocked)
                    AddAbility(abilties.toDolp);
                break;
            case "Dragon Form":
                if (abiUnlocked.frogUnlocked)
                    AddAbility(abilties.toFrog);
                if (abiUnlocked.armaUnlocked)
                    AddAbility(abilties.toArma);
                if (abiUnlocked.dolphinUnlocked)
                    AddAbility(abilties.toDolp);
                break;
            case "Dolphin Form":
                if (abiUnlocked.frogUnlocked)
                    AddAbility(abilties.toFrog);
                if (abiUnlocked.armaUnlocked)
                    AddAbility(abilties.toArma);
                if (abiUnlocked.craneUnlocked)
                    AddAbility(abilties.toCrane);
                break;

        }

    }
    #endregion
    #endregion

    #region Death Conditions Methods
    void Update()
    {
        if (cPlayerState.currentForm != "Dolphin Form" && cPlayerState.currentPhState == physicStates.onWater && !dying)
        {
            dying = true;
            StartCoroutine(Drowning());
        }


        #region Cheat Code 

        if (Input.GetKeyDown(KeyCode.Y))
        {
            abiUnlocked.frogUnlocked = true;
            UpdatingAbilityList();
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            abiUnlocked.armaUnlocked = true;
            UpdatingAbilityList();
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            abiUnlocked.craneUnlocked = true;
            UpdatingAbilityList();
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            abiUnlocked.dolphinUnlocked = true;
            UpdatingAbilityList();
        }

        #endregion

    }

    void OnTriggerEnter(Collider deathZone)
    {
        if (deathZone.gameObject.tag == "Death")
        {
            Debug.Log("Morto");
            save.LoadState();
            // deathRequest.Invoke();

        }

    }

    private IEnumerator Drowning()
    {
        float drowningTimer = drowTimerSetting;

        while (cPlayerState.currentForm != "Dolphin Form" && cPlayerState.currentPhState == physicStates.onWater && drowningTimer > 0)
        {
            drowningTimer -= Time.deltaTime;
            yield return null;
        }

        if (drowningTimer <= 0)
            deathRequest.Invoke();
        else
            dying = false;
    }
    #endregion
}

