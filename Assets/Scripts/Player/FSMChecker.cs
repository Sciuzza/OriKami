using UnityEngine;
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
        public triggerGenAbiStates currentTRGState;
        public triggerSpecial currentTSState;
        public controlStates currentClState;

    }

    [SerializeField]
    playerCState cPlayerState;

    private CharacterController ccLink;

    #region Effects Request Observer Path
    [System.Serializable]
    public class formEffectsRequest : UnityEvent<string, string, List<GameObject>>
    {
    }

    public formEffectsRequest formChanged;


    [System.Serializable]
    public class dirAbilityUsed : UnityEvent<abilties, Vector3, string, physicStates>
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

    [System.Serializable]
    public class vFissureUse : UnityEvent<VFissure, string>
    {
    }

    public vFissureUse vFissureUsed;
    public UnityEvent stoppingRollLogic, enableGlideLogic, stopGlideLogic;

    #endregion

    private VFissure vfLink;
    private string vFissureEntrance;

    #region Initialization Methods
    void Awake()
    {

        ccLink = this.gameObject.GetComponent<CharacterController>();

        SettingPlayerInitialState();



        PlayerInputs plInputsTempLink = this.gameObject.GetComponent<PlayerInputs>();


        plInputsTempLink.dirAbiRequest.AddListener(CheckingAbiRequirements);
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

        cPlayerState.currentAbilities.Add(abilties.toArma);
        cPlayerState.currentAbilities.Add(abilties.toCrane);
        cPlayerState.currentAbilities.Add(abilties.toDolp);
        cPlayerState.currentAbilities.Add(abilties.toFrog);

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
    private void CheckingAbiRequirements(abilties abiReceived, Vector3 abiDir)
    {

        if (cPlayerState.currentAbilities.Contains(abiReceived))
        {
            switch (abiReceived)
            {

                case abilties.move:
                    dirAbiUsed.Invoke(abiReceived, abiDir, cPlayerState.currentForm, cPlayerState.currentPhState);
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
                    UpdatingAbilityList();
                    Debug.Log("GG");
                    formChanged.Invoke(cPlayerState.currentForm, cPlayerState.previousForm, cPlayerState.forms);
                    if (cPlayerState.previousForm == "Dragon Form")
                        stopGlideLogic.Invoke();
                    SettingCapsuleCollider(0.15f, 1);
                    break;
                case abilties.toFrog:
                    cPlayerState.previousForm = cPlayerState.currentForm;
                    cPlayerState.currentForm = "Frog Form";
                    formChangedInp.Invoke(cPlayerState.currentForm);
                    UpdatingAbilityList();
                    formChanged.Invoke(cPlayerState.currentForm, cPlayerState.previousForm, cPlayerState.forms);
                    if (cPlayerState.previousForm == "Dragon Form")
                        stopGlideLogic.Invoke();
                    SettingCapsuleCollider(0.3f, 0.7f);
                    break;
                case abilties.toCrane:
                    cPlayerState.previousForm = cPlayerState.currentForm;
                    cPlayerState.currentForm = "Dragon Form";
                    formChangedInp.Invoke(cPlayerState.currentForm);
                    UpdatingAbilityList();
                    Debug.Log("GG");
                    formChanged.Invoke(cPlayerState.currentForm, cPlayerState.previousForm, cPlayerState.forms);
                    enableGlideLogic.Invoke();
                    SettingCapsuleCollider(0.3f, 0.7f);
                    break;
                case abilties.toArma:
                    cPlayerState.previousForm = cPlayerState.currentForm;
                    cPlayerState.currentForm = "Armadillo Form";
                    formChangedInp.Invoke(cPlayerState.currentForm);
                    UpdatingAbilityList();
                    formChanged.Invoke(cPlayerState.currentForm, cPlayerState.previousForm, cPlayerState.forms);
                    if (cPlayerState.previousForm == "Dragon Form")
                        stopGlideLogic.Invoke();
                    SettingCapsuleCollider(0.3f, 0.7f);
                    break;
                case abilties.toDolp:
                    cPlayerState.previousForm = cPlayerState.currentForm;
                    cPlayerState.currentForm = "Dolphin Form";
                    formChangedInp.Invoke(cPlayerState.currentForm);
                    UpdatingAbilityList();
                    formChanged.Invoke(cPlayerState.currentForm, cPlayerState.previousForm, cPlayerState.forms);
                    if (cPlayerState.previousForm == "Dragon Form")
                        stopGlideLogic.Invoke();
                    SettingCapsuleCollider(0.3f, 0.7f);
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

        AddAbility(abilties.toCrane);
        AddAbility(abilties.toFrog);
        AddAbility(abilties.toArma);
        AddAbility(abilties.toDolp);

		if (cPlayerState.currentPhState == physicStates.onGround) {
			
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

        AddAbility(abilties.toCrane);
        AddAbility(abilties.toFrog);
        AddAbility(abilties.toArma);
        AddAbility(abilties.toDolp);



		if (cPlayerState.currentPhState == physicStates.onGround) {

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

        AddAbility(abilties.toCrane);
        AddAbility(abilties.toFrog);
        AddAbility(abilties.toArma);
        AddAbility(abilties.toDolp);



		if (cPlayerState.currentPhState == physicStates.onGround) {

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

        AddAbility(abilties.toCrane);
        AddAbility(abilties.toFrog);
        AddAbility(abilties.toArma);
        AddAbility(abilties.toDolp);



		if (cPlayerState.currentPhState == physicStates.onGround) {

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

        AddAbility(abilties.toCrane);
        AddAbility(abilties.toStd);
        AddAbility(abilties.toArma);
        AddAbility(abilties.toDolp);

		if (cPlayerState.currentPhState == physicStates.onGround) {

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

        AddAbility(abilties.toCrane);
        AddAbility(abilties.toStd);
        AddAbility(abilties.toArma);
        AddAbility(abilties.toDolp);
	

		if (cPlayerState.currentPhState == physicStates.onGround) {

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

        AddAbility(abilties.toCrane);
        AddAbility(abilties.toStd);
        AddAbility(abilties.toArma);
        AddAbility(abilties.toDolp);


		if (cPlayerState.currentPhState == physicStates.onGround) {

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

        AddAbility(abilties.toCrane);
        AddAbility(abilties.toStd);
        AddAbility(abilties.toArma);
        AddAbility(abilties.toDolp);


		if (cPlayerState.currentPhState == physicStates.onGround) {

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

        AddAbility(abilties.toCrane);
        AddAbility(abilties.toFrog);
        AddAbility(abilties.toStd);
        AddAbility(abilties.toDolp);


		if (cPlayerState.currentPhState == physicStates.onGround) {

			if (cPlayerState.currentTRGState == triggerGenAbiStates.onMoveBlock)
				AddAbility (abilties.moveBlock);
			else if (cPlayerState.currentTRGState == triggerGenAbiStates.npcTalk)
				AddAbility (abilties.npcInter);
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

        AddAbility(abilties.toCrane);
        AddAbility(abilties.toFrog);
        AddAbility(abilties.toStd);
        AddAbility(abilties.toDolp);


		if (cPlayerState.currentPhState == physicStates.onGround) {

			if (cPlayerState.currentTRGState == triggerGenAbiStates.onMoveBlock)
				AddAbility (abilties.moveBlock);
			else if (cPlayerState.currentTRGState == triggerGenAbiStates.npcTalk)
				AddAbility (abilties.npcInter);
			AddAbility(abilties.roll);
		}
    }

    private void ArmaNoMove()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();

       
        AddAbility(abilties.cameraMove);

        AddAbility(abilties.menu);

        AddAbility(abilties.toCrane);
        AddAbility(abilties.toFrog);
        AddAbility(abilties.toStd);
        AddAbility(abilties.toDolp);

		if (cPlayerState.currentPhState == physicStates.onGround) {

			if (cPlayerState.currentTRGState == triggerGenAbiStates.onMoveBlock)
				AddAbility (abilties.moveBlock);
			else if (cPlayerState.currentTRGState == triggerGenAbiStates.npcTalk)
				AddAbility (abilties.npcInter);
			if (cPlayerState.currentPlState == playerStates.rolling)
				AddAbility (abilties.moveOnRoll);
			else if (cPlayerState.currentPlState == playerStates.movingBlock)
				Debug.Log ("Something");
			else
				AddAbility (abilties.roll);
		} else if (cPlayerState.currentPhState == physicStates.onWater &&
		         cPlayerState.currentPlState == playerStates.rolling)
			EnablingMove ();
    
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

        AddAbility(abilties.toCrane);
        AddAbility(abilties.toFrog);
        AddAbility(abilties.toStd);
        AddAbility(abilties.toDolp);

		if (cPlayerState.currentPhState == physicStates.onGround) {

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
        AddAbility(abilties.toFrog);
        AddAbility(abilties.toArma);
        AddAbility(abilties.toDolp);

		if (cPlayerState.currentPhState == physicStates.onAir)
			AddAbility (abilties.move);
    
    }

    private void CraneNoCamera()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();


        AddAbility(abilties.rotate);
       

        AddAbility(abilties.menu);

        AddAbility(abilties.toStd);
        AddAbility(abilties.toFrog);
        AddAbility(abilties.toArma);
        AddAbility(abilties.toDolp);

		if (cPlayerState.currentPhState == physicStates.onAir)
			AddAbility (abilties.move);
    }

    private void CraneNoMove()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();


      
        AddAbility(abilties.cameraMove);

        AddAbility(abilties.menu);

        AddAbility(abilties.toStd);
        AddAbility(abilties.toFrog);
        AddAbility(abilties.toArma);
        AddAbility(abilties.toDolp);


    }

    private void CraneNoGenAbi()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();


        AddAbility(abilties.rotate);
        AddAbility(abilties.cameraMove);

        AddAbility(abilties.menu);


		if (cPlayerState.currentPhState == physicStates.onAir)
			AddAbility (abilties.move);
    }

    private void CraneNoCamAndMove()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();


        AddAbility(abilties.menu);

        AddAbility(abilties.toStd);
        AddAbility(abilties.toFrog);
        AddAbility(abilties.toArma);
        AddAbility(abilties.toDolp);



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
			AddAbility (abilties.move);
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

        AddAbility(abilties.toCrane);
        AddAbility(abilties.toFrog);
        AddAbility(abilties.toArma);
        AddAbility(abilties.toStd);



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

        AddAbility(abilties.toCrane);
        AddAbility(abilties.toFrog);
        AddAbility(abilties.toArma);
        AddAbility(abilties.toStd);



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

        AddAbility(abilties.toCrane);
        AddAbility(abilties.toFrog);
        AddAbility(abilties.toArma);
        AddAbility(abilties.toStd);


		if (cPlayerState.currentPhState == physicStates.onWater) {

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
			AddAbility (abilties.move);

    }

    private void DolpNoCamAndMove()
    {
        cPlayerState.currentAbilities.Clear();
        cPlayerState.currentAbilities.TrimExcess();

      

        AddAbility(abilties.menu);

        AddAbility(abilties.toCrane);
        AddAbility(abilties.toFrog);
        AddAbility(abilties.toArma);
        AddAbility(abilties.toStd);

		if (cPlayerState.currentPhState == physicStates.onWater) {

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
			AddAbility (abilties.move);

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
    #endregion
    #endregion







}

