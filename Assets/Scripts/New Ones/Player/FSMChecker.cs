using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

using UnityEngine.SceneManagement;

#region Finite State Machine enum Structure
public enum abilties
{
    move,
    rotate,
    cameraMove,
    npcInter,
    menu,
    jump,
    roll,
    moveOnRoll,
    moveBlock,
    VFissure,
    HFissure,
    dolpSwimBel,
    toStd,
    toFrog,
    toArma,
    toCrane,
    toDolp
}

public enum physicStates
{
    onAir,
    onGround,
    onWater
}

public enum playerStates
{
    standingStill, moving, flying,
    rolling, movingBlock
}

public enum triggerGenAbiStates
{
    onVFissure,
    onHFissure,
    onDolpSwimBel,
    onMoveBlock,
    npcTalk,
    noTrigger
}

public enum triggerSpecial
{
    cameraLimited,
    noTrigger
}

public enum controlStates
{
    totalControl,
    noCamera,
    noMove,
    noGenAbi,
    noCamAndMove,
    noMoveAndGenAbi,
    noCameraAndGenAbi,
    noControl
}

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
public class event_vector3_string_ps_listgb : UnityEvent<Vector3, string, physicStates, List<GameObject>>
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
public class event_abi_string_listGb : UnityEvent<abilties, string, List<GameObject>>
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
public class event_bool : UnityEvent<bool>
{

}

[System.Serializable]
public class event_Gb : UnityEvent<GameObject>
{
}

[System.Serializable]
public class event_int_int : UnityEvent<int, int>
{
}

[System.Serializable]
public class event_collider : UnityEvent<Collider>
{
}

[System.Serializable]
public class event_joy_pc : UnityEvent<buttonsJoy, buttonsPc>
{
}

[System.Serializable]
public class event_cs : UnityEvent<controlStates>
{
}

[System.Serializable]
public class event_vector3 : UnityEvent<Vector3>
{
}

[System.Serializable]
public class event_string_string_string : UnityEvent<string, string, string>
{
}

[System.Serializable]
public class event_float_float_float : UnityEvent<float, float, float>
{
}

[System.Serializable]
public class event_int_float : UnityEvent<int, float>
{
}

[System.Serializable]
public class event_joy_pc_story : UnityEvent<buttonsJoy, buttonsPc, SingleStory>
{
}

[System.Serializable]
public class event_story : UnityEvent<SingleStory>
{
}

[System.Serializable]
public class event_string_string_string_string : UnityEvent<string, string, string, string>
{
}

[System.Serializable]
public class event_listEnvSens_plNsSens : UnityEvent<List<EnvDatas>, PlayerNsData>
{
    
}

[System.Serializable]
public class event_transform : UnityEvent<Transform>
{

}

[System.Serializable]
public class event_int_gb : UnityEvent<int, GameObject>
{

}
#endregion

public class FSMChecker : MonoBehaviour
{
    #region Public Variables
    public formsSettings abiUnlocked;
    public playerLegends legUnlocked;
    public float drowTimerSetting;

    public ParticleSystem ArmaRollingPart;

    public int GoldenCrane = 0;
    public int DRocks = 0;
    public int BlackSmith = 0;
    public int V3 = 0;
    #endregion

    #region Private Variables

    private PlayerInputs playerTemp;

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
    public playerCState cPlayerState;

    private CharacterController ccLink;
    private VFissure vfLink;
    private string vFissureEntrance;
    private bool dying = false;

    // Variables needed to handle Designer Cam Query Situations in Story Mode
    private bool queryDesCamera;
    private GameObject queryDesCamGb;
    public static bool storyMode;

    private bool isGlidingSound = false;
    private bool isRollingSound = false;
    private bool isWalkingSound = false;

    private bool onGroundSwallow = false;
    private bool onGroundDolphin = false;
    #endregion

    #region Events
    public event_string formChangedInp;
    public event_vfscript_string vFissureUsed;
    public UnityEvent stoppingRollLogic, enableGlideLogic, stopGlideLogic, deathRequest;
    public event_abi_string_listGb genAbiUsed;
    public event_vector3_ps rotationUsed;
    public event_string_string_listGb formChanged;
    public event_vector3_string_ps_listgb moveUsed;
    public event_ps phStateChanged;
    public event_pl plStateChanged;
    public event_Gb switchingCameraControlToOFF;
    public UnityEvent switchingCameraControlToOn;
    public UnityEvent switchingCameraToStoryRequest;
    public UnityEvent switchingCameraToPlayer;
    public event_float_float_float plCameraMoveUsed;
    public event_bool swallowOnGround;
    public event_bool dolphinOnGround;
    public event_bool armaRolling;
    public UnityEvent aniStoryInitRequest, aniNormalInitRequest;
    public event_int updateUiCollRequest;
    #endregion







    #region Initialization Methods
    void Awake()
    {

        ccLink = this.gameObject.GetComponent<CharacterController>();

        this.SettingPlayerInitialState();



        PlayerInputs plInputsTempLink = this.gameObject.GetComponent<PlayerInputs>();


        plInputsTempLink.dirAbiRequest.AddListener(CheckingDirAbiRequirements);
        plInputsTempLink.genAbiRequest.AddListener(CheckingAbiRequirements);
        plInputsTempLink.rollStopped.AddListener(EnablingMove);
        plInputsTempLink.camMoveRequest.AddListener(this.CheckingCamMoveReq);

        EnvInputs enInputsTempLink = this.gameObject.GetComponent<EnvInputs>();

        enInputsTempLink.psChanged.AddListener(ChangingPHStates);
        enInputsTempLink.vFissureRequestOn.AddListener(AddingVFissureAvailability);
        enInputsTempLink.vFissureRequestOff.AddListener(RemovingVFissureAvailability);
        enInputsTempLink.cameraOffRequest.AddListener(this.ChangingCameraToOff);
        enInputsTempLink.cameraOnRequest.AddListener(this.ChangingCameraToOn);
        enInputsTempLink.wallDCheckRequest.AddListener(this.WallDestructibleHandler);
        enInputsTempLink.IncrementNCollRequest.AddListener(this.NormalCollectiblesHandler);

        FSMExecutor fsmExeTempLink = this.gameObject.GetComponent<FSMExecutor>();

        fsmExeTempLink.vFissureAniEnded.AddListener(VFissureAniEndEffects);

        GameObject storyLineCheck = GameObject.FindGameObjectWithTag("StoryLine");

        if (storyLineCheck != null)
        {

            StoryLineInstance slTempLink =
                GameObject.FindGameObjectWithTag("StoryLine").GetComponent<StoryLineInstance>();

            if (slTempLink != null)
            {
                slTempLink.FormUnlockRequest.AddListener(this.UnlockingAbility);
                slTempLink.ChangeCsEnterRequest.AddListener(this.StoryCsChange);
                slTempLink.ChangeCsExitRequest.AddListener(this.StoryCsExit);
                slTempLink.IsStoryMode.AddListener(this.SettingStoryMode);
                slTempLink.LegendsUpdateRequest.AddListener(this.UnlockingLegend);
                slTempLink.TransfRequest.AddListener(this.CallingTransformation);
            }
        }

        var sdmTempLink = GameObject.FindGameObjectWithTag("GameController").GetComponent<SuperDataManager>();

        sdmTempLink.RequestLocalUpdateToRepo.AddListener(this.SavingCurrentState);
        sdmTempLink.RequestLocalUpdateByRepo.AddListener(this.LoadingCurrentState);
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

        SettingCapsuleCollider(0.15f, 0.3f, 0.2f);

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
                    moveUsed.Invoke(abiDir, cPlayerState.currentForm, cPlayerState.currentPhState, this.cPlayerState.forms);

                    if (this.cPlayerState.currentForm == "Dragon Form" && this.onGroundSwallow) this.onGroundSwallow = false;
                    if (this.cPlayerState.currentForm == "Dolphin Form" && this.onGroundDolphin) this.onGroundDolphin = false;
                    break;
                case abilties.rotate:
                    rotationUsed.Invoke(abiDir, cPlayerState.currentPlState);
                    break;
                case abilties.moveOnRoll:
                    rotationUsed.Invoke(abiDir, cPlayerState.currentPlState);
                    break;
            }

        }
        else if (abiReceived == abilties.move && this.cPlayerState.currentForm == "Dragon Form" && !this.onGroundSwallow)
        {
            this.onGroundSwallow = true;
            this.swallowOnGround.Invoke(this.onGroundSwallow);
        }
        else if (abiReceived == abilties.move && this.cPlayerState.currentForm == "Dolphin Form" && !this.onGroundDolphin)
        {
            this.onGroundDolphin = true;
            this.dolphinOnGround.Invoke(this.onGroundDolphin);
        }



    }

    private void CheckingAbiRequirements(abilties abiReceived)
    {
        if (this.cPlayerState.currentAbilities.Contains(abiReceived))
        {
            switch (abiReceived)
            {

                case abilties.jump:
                    if (!MoveHandler.sliding)
                    this.genAbiUsed.Invoke(abiReceived, this.cPlayerState.currentForm, this.cPlayerState.forms);
                    break;
                case abilties.roll:
                    this.cPlayerState.currentPlState = playerStates.rolling;
                    this.UpdatingAbilityList();
                    this.armaRolling.Invoke(true);
                    this.ArmaRollingPart.Play();
                    this.genAbiUsed.Invoke(abiReceived, this.cPlayerState.currentForm, this.cPlayerState.forms);
                    break;
                case abilties.VFissure:
                    Debug.Log("VFissure Pressed");
                    this.cPlayerState.currentClState = controlStates.noMoveAndGenAbi;
                    this.UpdatingAbilityList();
                    this.vFissureUsed.Invoke(this.vfLink, this.vFissureEntrance);
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
                    this.onGroundSwallow = false;
                    this.onGroundDolphin = false;
                    formChangedInp.Invoke(cPlayerState.currentForm);
                    if (cPlayerState.currentPlState == playerStates.rolling)
                        EnablingMove();
                    else
                        UpdatingAbilityList();
                    formChanged.Invoke(cPlayerState.currentForm, cPlayerState.previousForm, cPlayerState.forms);
                    if (cPlayerState.previousForm == "Dragon Form")
                        stopGlideLogic.Invoke();
                    SettingCapsuleCollider(0.15f, 0.3f, 0.2f);
                    break;
                case abilties.toFrog:
                    cPlayerState.previousForm = cPlayerState.currentForm;
                    cPlayerState.currentForm = "Frog Form";
                    this.onGroundSwallow = false;
                    this.onGroundDolphin = false;
                    formChangedInp.Invoke(cPlayerState.currentForm);
                    if (cPlayerState.currentPlState == playerStates.rolling)
                        EnablingMove();
                    else
                        UpdatingAbilityList();
                    formChanged.Invoke(cPlayerState.currentForm, cPlayerState.previousForm, cPlayerState.forms);
                    if (cPlayerState.previousForm == "Dragon Form")
                        stopGlideLogic.Invoke();
                    SettingCapsuleCollider(0.15f, 0.3f, 0.2f);
                    break;
                case abilties.toCrane:
                    cPlayerState.previousForm = cPlayerState.currentForm;
                    cPlayerState.currentForm = "Dragon Form";
                    this.onGroundSwallow = false;
                    this.onGroundDolphin = false;
                    formChangedInp.Invoke(cPlayerState.currentForm);
                    if (cPlayerState.currentPlState == playerStates.rolling)
                        EnablingMove();
                    else
                        UpdatingAbilityList();
                    formChanged.Invoke(cPlayerState.currentForm, cPlayerState.previousForm, cPlayerState.forms);
                    enableGlideLogic.Invoke();
                    SettingCapsuleCollider(0.15f, 0.3f, 0.2f);
                    break;
                case abilties.toArma:
                    cPlayerState.previousForm = cPlayerState.currentForm;
                    cPlayerState.currentForm = "Armadillo Form";
                    this.onGroundSwallow = false;
                    this.onGroundDolphin = false;
                    formChangedInp.Invoke(cPlayerState.currentForm);
                    UpdatingAbilityList();
                    formChanged.Invoke(cPlayerState.currentForm, cPlayerState.previousForm, cPlayerState.forms);
                    if (cPlayerState.previousForm == "Dragon Form")
                        stopGlideLogic.Invoke();
                    SettingCapsuleCollider(0.15f, 0.3f, 0.2f);
                    break;
                case abilties.toDolp:
                    cPlayerState.previousForm = cPlayerState.currentForm;
                    cPlayerState.currentForm = "Dolphin Form";
                    this.onGroundSwallow = false;
                    this.onGroundDolphin = false;
                    formChangedInp.Invoke(cPlayerState.currentForm);
                    if (cPlayerState.currentPlState == playerStates.rolling)
                        EnablingMove();
                    else
                        UpdatingAbilityList();
                    formChanged.Invoke(cPlayerState.currentForm, cPlayerState.previousForm, cPlayerState.forms);
                    if (cPlayerState.previousForm == "Dragon Form")
                        stopGlideLogic.Invoke();
                    SettingCapsuleCollider(0.15f, 0.3f, 0.2f);
                    break;
            }
        }
        else
            Debug.Log("Requirements not met");
    }


    private void CheckingCamMoveReq(float currentX, float currentY, float currentDistance)
    {
        if (this.cPlayerState.currentAbilities.Contains(abilties.cameraMove))
        {
            this.plCameraMoveUsed.Invoke(currentX, currentY, currentDistance);
        }
        else
        {
            //GameController.Debugging("Camera Move Not Possible");
        }
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

    public void UpdatingAbilityList()
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
    #endregion

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

        if (this.cPlayerState.currentPlState != playerStates.rolling)
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

        if (this.cPlayerState.currentPlState != playerStates.rolling)
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

        if (this.cPlayerState.currentPlState != playerStates.rolling)
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

        if (this.cPlayerState.currentPlState != playerStates.rolling)
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
        //cPlayerState.currentClState = controlStates.totalControl;
        UpdatingAbilityList();
        this.armaRolling.Invoke(false);
        this.ArmaRollingPart.Stop();
        stoppingRollLogic.Invoke();
    }

    private void SettingCapsuleCollider(float rad, float height, float skinW)
    {
        ccLink.radius = rad;
        ccLink.height = height;
        this.ccLink.skinWidth = skinW;
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

    private void UnlockingAbility(string whichAbility)
    {
        switch (whichAbility)
        {
            case "Frog Form":
                this.abiUnlocked.frogUnlocked = true;
                break;
            case "Dragon Form":
                this.abiUnlocked.craneUnlocked = true;
                break;
            case "Arma Form":
                this.abiUnlocked.armaUnlocked = true;
                break;
            case "Dolphin Form":
                this.abiUnlocked.dolphinUnlocked = true;
                break;
        }

        this.UpdatingAbilityList();
    }

    private void UnlockingLegend(int LegendIndex)
    {
        switch (LegendIndex)
        {
            case 0:
                this.legUnlocked.Legend1 = true;
                break;
            case 1:
                this.legUnlocked.Legend2 = true;
                break;
            case 2:
                this.legUnlocked.Legend3 = true;
                break;
            case 3:
                this.legUnlocked.Legend4 = true;
                break;
            case 4:
                this.legUnlocked.Legend5 = true;
                break;
        }
    }
    #endregion

    #region Designer Camera Handler
    private void ChangingCameraToOff(GameObject cameraDir)
    {

        this.queryDesCamera = true;
        this.queryDesCamGb = cameraDir;


        switch (this.cPlayerState.currentClState)
        {
            case controlStates.noGenAbi:
                this.cPlayerState.currentClState = controlStates.noCameraAndGenAbi;
                this.UpdatingAbilityList();
                this.switchingCameraControlToOFF.Invoke(cameraDir);
                break;
            case controlStates.noMove:
                this.cPlayerState.currentClState = controlStates.noCamAndMove;
                this.UpdatingAbilityList();
                this.switchingCameraControlToOFF.Invoke(cameraDir);
                break;
            case controlStates.noMoveAndGenAbi:
                this.cPlayerState.currentClState = controlStates.noControl;
                this.UpdatingAbilityList();
                this.switchingCameraControlToOFF.Invoke(cameraDir);
                break;
            case controlStates.totalControl:
                this.cPlayerState.currentClState = controlStates.noCamera;
                this.UpdatingAbilityList();
                this.switchingCameraControlToOFF.Invoke(cameraDir);
                break;
            default:
                Debug.Log("Camera Already Off");
                break;
        }

    }

    private void ChangingCameraToOn()
    {

        this.queryDesCamera = false;
        this.queryDesCamGb = null;

        switch (this.cPlayerState.currentClState)
        {
            case controlStates.noCameraAndGenAbi:
                this.cPlayerState.currentClState = controlStates.noGenAbi;
                this.UpdatingAbilityList();
                this.switchingCameraControlToOn.Invoke();
                break;
            case controlStates.noCamAndMove:
                this.cPlayerState.currentClState = controlStates.noMove;
                this.UpdatingAbilityList();
                this.switchingCameraControlToOn.Invoke();
                break;
            case controlStates.noControl:
                this.cPlayerState.currentClState = controlStates.noMoveAndGenAbi;
                this.UpdatingAbilityList();
                this.switchingCameraControlToOn.Invoke();
                break;
            case controlStates.noCamera:
                this.cPlayerState.currentClState = controlStates.totalControl;
                this.UpdatingAbilityList();
                this.switchingCameraControlToOn.Invoke();
                break;
            default:
                Debug.Log("Camera Already On");
                break;
        }

    }
    #endregion

    #region Story Input Handler
    private void StoryCsChange(controlStates newCs)
    {

        this.cPlayerState.currentClState = newCs;

        if (this.cPlayerState.currentClState == controlStates.noCamAndMove
            || this.cPlayerState.currentClState == controlStates.noCamera
            || this.cPlayerState.currentClState == controlStates.noCameraAndGenAbi
            || this.cPlayerState.currentClState == controlStates.noControl)
            this.switchingCameraToStoryRequest.Invoke();

        if (this.cPlayerState.currentClState == controlStates.noControl)
        {
            this.EnablingMove();
        }

        this.UpdatingAbilityList();

        if (this.cPlayerState.currentClState == controlStates.noControl)
        this.aniStoryInitRequest.Invoke();

    }

    private void StoryCsExit(controlStates exitCs)
    {
        this.cPlayerState.currentClState = exitCs;

        if (this.queryDesCamera)
        {
            this.ChangingCameraToOff(this.queryDesCamGb);
        }
        else
        {
            this.switchingCameraToPlayer.Invoke();
        }

        this.UpdatingAbilityList();

        this.aniNormalInitRequest.Invoke();
    }

    private void SettingStoryMode(bool state)
    {
        storyMode = state;
    }

     private void CallingTransformation(abilties abiReceived)
    {
            switch (abiReceived)
            {
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
                    SettingCapsuleCollider(0.15f, 0.3f, 0.2f);
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
                    SettingCapsuleCollider(0.15f, 0.3f, 0.2f);
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
                    SettingCapsuleCollider(0.15f, 0.3f, 0.2f);
                    break;
                case abilties.toArma:
                    cPlayerState.previousForm = cPlayerState.currentForm;
                    cPlayerState.currentForm = "Armadillo Form";
                    formChangedInp.Invoke(cPlayerState.currentForm);
                    UpdatingAbilityList();
                    formChanged.Invoke(cPlayerState.currentForm, cPlayerState.previousForm, cPlayerState.forms);
                    if (cPlayerState.previousForm == "Dragon Form")
                        stopGlideLogic.Invoke();
                    SettingCapsuleCollider(0.15f, 0.3f, 0.2f);
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
                    SettingCapsuleCollider(0.15f, 0.3f, 0.2f);
                    break;
            }
    }
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

        #region SoundForms
        if (cPlayerState.currentForm == "Dragon Form" && cPlayerState.currentPhState == physicStates.onAir && !isGlidingSound)
        {
            this.GetComponent<PlayerInputs>().CraneGlide();
            isGlidingSound = true;
        }
        else if ((cPlayerState.currentForm != "Dragon Form" || cPlayerState.currentPhState != physicStates.onAir) && isGlidingSound)
        {
            this.GetComponent<PlayerInputs>().StopCraneGlide();
            isGlidingSound = false;
        }

        if (cPlayerState.currentForm == "Armadillo Form" && cPlayerState.currentPhState == physicStates.onGround && this.GetComponent<PlayerInputs>().rollPressed() && !isRollingSound)
        {
            this.GetComponent<PlayerInputs>().RollingSound();
            isRollingSound = true;
        }
        else if (cPlayerState.currentForm == "Armadillo Form" && cPlayerState.currentPhState == physicStates.onGround && this.GetComponent<PlayerInputs>().rollReleased() && isRollingSound)
        {
            this.GetComponent<PlayerInputs>().StopRollingSound();
            isRollingSound = false;
        }
        else if ((cPlayerState.currentForm != "Armadillo Form" || cPlayerState.currentPhState != physicStates.onGround) && isRollingSound)
        {
            this.GetComponent<PlayerInputs>().StopRollingSound();
            isRollingSound = false;
        }

        //if ((cPlayerState.currentForm == "Standard Form" || cPlayerState.currentForm == "Armadillo Form") && cPlayerState.currentPhState == physicStates.onGround && cPlayerState.currentPlState == playerStates.moving &&!isWalkingSound)
        //{
        //    this.GetComponent<PlayerInputs>().StandardWalk();
        //    isWalkingSound = true;
        //}
        //else if ((cPlayerState.currentForm != "Standard Form" || cPlayerState.currentForm != "Armadillo Form") || cPlayerState.currentPhState != physicStates.onGround || cPlayerState.currentPlState != playerStates.moving && isWalkingSound)
        //{
        //    this.GetComponent<PlayerInputs>().StopStandardWalk();
        //    isWalkingSound = false;
        //}


        #endregion


    }

    void OnTriggerEnter(Collider deathZone)
    {
        if (deathZone.gameObject.tag == "Death")
        {
            Debug.Log("Morto");

            this.deathRequest.Invoke();


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

    #region Collectible Methods
    private void NormalCollectiblesHandler(int collIndex, GameObject collTaken)
    {
        switch (collIndex)
        {
            case 0:
                this.GoldenCrane++;
                this.updateUiCollRequest.Invoke(this.GoldenCrane);
                break;
            case 2:
                this.BlackSmith++;
                break;
            case 3:
                this.V3++;
                break;
        }
     
      Destroy(collTaken);
}

    private void WallDestructibleHandler(GameObject wall)
    {
        if (this.cPlayerState.currentPlState == playerStates.rolling)
        {
            Destroy(wall);
            this.DRocks++;
        }
    }
    #endregion

    #region Saving and Loading State Methods
    private void SavingCurrentState()
    {
        var dataToUpdate =
            GameObject.FindGameObjectWithTag("GameController")
                .GetComponent<SuperDataManager>()
                .EnvSensData.Find(x => x.GpSceneName == SceneManager.GetActiveScene().name)
                .PlState;

        var plTrans = this.gameObject.transform;

        dataToUpdate.PlayerPosX = plTrans.position.x;
        dataToUpdate.PlayerPosY = plTrans.position.y;
        dataToUpdate.PlayerPosZ = plTrans.position.z;

        dataToUpdate.PlayerRotX = plTrans.eulerAngles.x;
        dataToUpdate.PlayerRotY = plTrans.eulerAngles.y;
        dataToUpdate.PlayerRotZ = plTrans.eulerAngles.z;

        var plNsDataToUpdate =
            GameObject.FindGameObjectWithTag("GameController").GetComponent<SuperDataManager>().PlNsData;

        plNsDataToUpdate.Collectible1 = this.GoldenCrane;
        plNsDataToUpdate.Collectible2 = this.DRocks;
        plNsDataToUpdate.Collectible3 = this.BlackSmith;
        plNsDataToUpdate.Collectible4 = this.V3;

        plNsDataToUpdate.FrogUnlocked = this.abiUnlocked.frogUnlocked;
        plNsDataToUpdate.ArmaUnlocked = this.abiUnlocked.armaUnlocked;
        plNsDataToUpdate.CraneUnlocked = this.abiUnlocked.craneUnlocked;
        plNsDataToUpdate.DolphinUnlocked = this.abiUnlocked.dolphinUnlocked;

        plNsDataToUpdate.Legend1Unlocked = this.legUnlocked.Legend1;
        plNsDataToUpdate.Legend2Unlocked = this.legUnlocked.Legend2;
        plNsDataToUpdate.Legend3Unlocked = this.legUnlocked.Legend3;
        plNsDataToUpdate.Legend4Unlocked = this.legUnlocked.Legend4;
        plNsDataToUpdate.Legend5Unlocked = this.legUnlocked.Legend5;

        plNsDataToUpdate.SceneToLoad = SceneManager.GetActiveScene().name;

    }

    private void LoadingCurrentState()
    {
        var dataToUpdate =
            GameObject.FindGameObjectWithTag("GameController")
                .GetComponent<SuperDataManager>()
                .EnvSensData.Find(x => x.GpSceneName == SceneManager.GetActiveScene().name)
                .PlState;

        var plTrans = this.gameObject.transform;

        plTrans.position = new Vector3(dataToUpdate.PlayerPosX, dataToUpdate.PlayerPosY, dataToUpdate.PlayerPosZ);
        plTrans.rotation = Quaternion.Euler(dataToUpdate.PlayerRotX, dataToUpdate.PlayerRotY, dataToUpdate.PlayerRotZ);

        var plNsDataToUpdate =
            GameObject.FindGameObjectWithTag("GameController").GetComponent<SuperDataManager>().PlNsData;


        this.GoldenCrane = plNsDataToUpdate.Collectible1;
        this.DRocks = plNsDataToUpdate.Collectible2;
        this.BlackSmith = plNsDataToUpdate.Collectible3;
        this.V3 = plNsDataToUpdate.Collectible4;

        this.abiUnlocked.frogUnlocked = plNsDataToUpdate.FrogUnlocked;
        this.abiUnlocked.armaUnlocked = plNsDataToUpdate.ArmaUnlocked;
        this.abiUnlocked.craneUnlocked = plNsDataToUpdate.CraneUnlocked;
        this.abiUnlocked.dolphinUnlocked = plNsDataToUpdate.DolphinUnlocked;

        this.UpdatingAbilityList();

        this.legUnlocked.Legend1 = plNsDataToUpdate.Legend1Unlocked;
        this.legUnlocked.Legend2 = plNsDataToUpdate.Legend2Unlocked;
        this.legUnlocked.Legend3 = plNsDataToUpdate.Legend3Unlocked;
        this.legUnlocked.Legend4 = plNsDataToUpdate.Legend4Unlocked;
        this.legUnlocked.Legend5 = plNsDataToUpdate.Legend5Unlocked;

    }
    #endregion

    #region Edit Mode Methods

    public void OnValidate()
    {
        //GameObject.FindGameObjectWithTag("GameController").GetComponent<SuperDataManager>().UpdatingPlState(this.gameObject);
    }
    #endregion
}

