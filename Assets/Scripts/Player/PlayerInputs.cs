using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerInputs : MonoBehaviour
{
    #region Public Variables
    public inputSettings CurrentInputs;
    public CameraPlayer CurrentPlCameraSettings;
    #endregion

    #region Private Variables
    private Vector3 moveDirection;

    private bool switchCooldown = false;
    private bool isRollPressed = false;


    private CurrentForm cForm = CurrentForm.Std;

    private bool storyModeInput = false;
    private buttonsJoy storyJoyInput = buttonsJoy.none;
    private buttonsPc storyPcInput = buttonsPc.none;
    private SingleStory ssTempRef;

    private float currentX, currentY, currentDistance = 6;

    private List<string> scenes = new List<string>()
    {
        "Main Menu", "Route 1", "Route 2", "Frogs' Village", "Armadillos' Village",
        "Route 3", "Dolphins and Swallows' Village", "Route 4", "Dragon's Spring Temple"
    };

    private enum CurrentForm
    {
        Std,
        Frog,
        Crane,
        Arma,
        Dolphin
    }

    #endregion

    #region Events

    public UnityEvent rollStopped, nextSceneRequest, previousSceneRequest, resettingSceneRequest;
    public event_story storyLivingRequest;
    public event_abi genAbiRequest;
    public event_abi_vector3 dirAbiRequest;
    public event_int mainMenuRequest;
    public event_float_float_float camMoveRequest;
    public event_string switchSceneRequest;
    #endregion

    #region Taking References and Linking Events
    private void Awake()
    {
        FSMChecker fsmCheckerTempLink = this.GetComponent<FSMChecker>();

        fsmCheckerTempLink.formChangedInp.AddListener(this.UpdatingCurrentFormInputs);

        GameObject storyLineCheck = GameObject.FindGameObjectWithTag("StoryLine");

        CameraManager cmTempLink = GameObject.FindGameObjectWithTag("GameController").GetComponent<CameraManager>();

        cmTempLink.reAdjustingCamValues.AddListener(this.FixingCamValues);

        if (storyLineCheck != null)
        {

            StoryLineInstance slTempLink =
                GameObject.FindGameObjectWithTag("StoryLine").GetComponent<StoryLineInstance>();

            slTempLink.ActivateStoryInputRequest.AddListener(this.SettingStoryInputs);
            slTempLink.eraseInputMemoryRequest.AddListener(this.DisablingStoryInputs);
        }
    }
    #endregion

    #region Player Inputs Update Check
    private void Update()
    {
        this.MovingInputHandler();
        this.CameraInputHandler();
        this.GenAbiInputHandler();
        this.ExtraInputsHandler();
        this.StoryInputsHandler();

    }
    #endregion

    #region Move Input
    private void MovingInputHandler()
    {


        MoveInput();



        dirAbiRequest.Invoke(abilties.move, moveDirection);
        dirAbiRequest.Invoke(abilties.rotate, moveDirection);


        dirAbiRequest.Invoke(abilties.moveOnRoll, moveDirection);

    }

    private void MoveInput()
    {

        moveDirection.x = Input.GetAxis("LJHor") + Input.GetAxis("Horizontal");
        moveDirection.z = -Input.GetAxis("LJVer") + Input.GetAxis("Vertical");


        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if (moveDirection.x < 0)
                moveDirection.x = -1f;
            else if (moveDirection.x > 0)
                moveDirection.x = 1f;

            if (moveDirection.z < 0)
                moveDirection.z = -1f;
            else if (moveDirection.z > 0)
                moveDirection.z = 1f;

            moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);
        }



        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);



        var inputRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up), Vector3.up);
        this.moveDirection = inputRotation * this.moveDirection;

    }
    #endregion

    #region Camera Input
    private void CameraInputHandler()
    {
        if (this.PcCamInputHandler() || this.JoyCamInputHandler())
            this.camMoveRequest.Invoke(this.currentX, this.currentY, this.currentDistance);
    }

    private bool PcCamInputHandler()
    {
        // Pc COde
        if (Input.GetMouseButton(0))
        {
            Cursor.visible = false;
            this.currentX += Input.GetAxis("Mouse X") * this.CurrentPlCameraSettings.sensitivityX;
            this.currentY -= Input.GetAxis("Mouse Y") * this.CurrentPlCameraSettings.sensitivityY;
            this.currentDistance -= Input.GetAxis("Mouse ScrollWheel") * this.CurrentPlCameraSettings.sensitivityZoom;

            this.currentDistance = Mathf.Clamp(this.currentDistance, this.CurrentPlCameraSettings.distanceMin, this.CurrentPlCameraSettings.distanceMax);
            this.currentY = Mathf.Clamp(this.currentY, this.CurrentPlCameraSettings.yAngleMin, this.CurrentPlCameraSettings.yAngleMax);

            return true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Cursor.visible = true;

            return false;
        }
        else
        {
            return false;
        }

    }

    private bool JoyCamInputHandler()
    {
        // Joy Code
        if (Input.GetAxis("RJHor") != 0 || Input.GetAxis("RJVer") != 0)
        {
            this.currentX += Input.GetAxis("RJHor") * this.CurrentPlCameraSettings.sensitivityX;
            this.currentY += Input.GetAxis("RJVer") * this.CurrentPlCameraSettings.sensitivityY;

            if (Input.GetButton("CamZjoy"))
                this.currentDistance += Input.GetAxis("RJVer") * this.CurrentPlCameraSettings.sensitivityZoom;

            this.currentDistance = Mathf.Clamp(this.currentDistance, this.CurrentPlCameraSettings.distanceMin, this.CurrentPlCameraSettings.distanceMax);
            this.currentY = Mathf.Clamp(this.currentY, this.CurrentPlCameraSettings.yAngleMin, this.CurrentPlCameraSettings.yAngleMax);

            return true;
        }
        else
        {
            return false;
        }
    }

    private void FixingCamValues(float xInput, float yInput, float distInput)
    {
        this.currentX = xInput;
        this.currentY = yInput;
        this.currentDistance = distInput;
    }
    #endregion

    #region General Abi Input
    private void GenAbiInputHandler()
    {
        if (jumpPressed())
        {
            genAbiRequest.Invoke(abilties.jump);
            JumpSound();
        }
        else if (toFrogPressed())
        {
            genAbiRequest.Invoke(abilties.toFrog);
            FormSound();
        }
        else if (toArmaPressed())
        {
            genAbiRequest.Invoke(abilties.toArma);
            FormSound();
        }
        else if (toCranePressed())
        {
            genAbiRequest.Invoke(abilties.toCrane);
            FormSound();
        }
        else if (toDolpPressed())
        {
            genAbiRequest.Invoke(abilties.toDolp);
            FormSound();
        }
        else if (toStdPressed())
        {
            genAbiRequest.Invoke(abilties.toStd);
            FormSound();
        }
        else if (rollPressed())
        {
            genAbiRequest.Invoke(abilties.roll);

        }
        else if (rollReleased())
        {
            rollStopped.Invoke();

        }
        else if (VFissurePressed())
            genAbiRequest.Invoke(abilties.VFissure);
        else if (HFissurePressed())
            genAbiRequest.Invoke(abilties.HFissure);
        else if (DolpSwimBPressed())
            genAbiRequest.Invoke(abilties.dolpSwimBel);


    }
    #region SoundAbilities
    public void JumpSound()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<SoundManager>().PlaySound(0, 0);
    }
    public void RollingSound()
    {

        GameObject.FindGameObjectWithTag("Player").GetComponent<SoundManager>().PlaySound(1, 0);
    }
    public void StopRollingSound()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<SoundManager>().StopSound(1, 0);
    }
    public void FormSound()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<SoundManager>().PlaySound(0, 1);
    }
    public void CraneGlide()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<SoundManager>().PlaySound(1, 1);
    }
    public void StopCraneGlide()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<SoundManager>().StopSound(1, 1);
    }
    public void StandardWalk()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<SoundManager>().PlaySound(1, 2);
    }
    public void StopStandardWalk()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<SoundManager>().StopSound(1, 2);
    }
    public void FrogWalk()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<SoundManager>().PlaySound(1, 3);
    }
    public void StopFrogWalk()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<SoundManager>().StopSound(1, 3);
    }

    #endregion

    #region Jump Input
    private bool jumpPressed()
    {

        switch (cForm)
        {
            case CurrentForm.Std: return stdJumpInput();

            case CurrentForm.Frog: return frogJumpInput();

            case CurrentForm.Dolphin: return dolphJumpInput();

        }


        return false;


    }

    private bool stdJumpInput()
    {
        if (stdJumpJoyI() || stdJumpPcI())
            return true;
        else
            return false;
    }

    private bool stdJumpJoyI()
    {
        if (this.CurrentInputs.standardInputs.joyInputs.Jump.ToString() != "LT" &&
         this.CurrentInputs.standardInputs.joyInputs.Jump.ToString() != "RT")
        {

            if (Input.GetButtonDown(this.CurrentInputs.standardInputs.joyInputs.Jump.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (this.CurrentInputs.standardInputs.joyInputs.Jump.ToString() == "LT")
            {
                if (Input.GetAxis("LRT") > 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (Input.GetAxis("LRT") < 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
        }
    }

    private bool stdJumpPcI()
    {
        if (Input.GetButtonDown(this.CurrentInputs.standardInputs.keyInputs.Jump.ToString()))
            return true;
        else
            return false;
    }

    private bool frogJumpInput()
    {
        if (frogJumpJoyI() || frogJumpPcI())
            return true;
        else
            return false;
    }

    private bool frogJumpJoyI()
    {
        if (this.CurrentInputs.frogInputs.joyInputs.Jump.ToString() != "LT" &&
        this.CurrentInputs.frogInputs.joyInputs.Jump.ToString() != "RT")
        {

            if (Input.GetButtonDown(this.CurrentInputs.frogInputs.joyInputs.Jump.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (this.CurrentInputs.frogInputs.joyInputs.Jump.ToString() == "LT")
            {
                if (Input.GetAxis("LRT") > 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (Input.GetAxis("LRT") < 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
        }
    }

    private bool frogJumpPcI()
    {
        if (Input.GetButtonDown(this.CurrentInputs.frogInputs.keyInputs.Jump.ToString()))
            return true;
        else
            return false;
    }

    private bool dolphJumpInput()
    {
        if (dolphJumpJoyI() || dolphJumpPcI())
            return true;
        else
            return false;
    }

    private bool dolphJumpJoyI()
    {
        if (this.CurrentInputs.dolphinInputs.joyInputs.jump.ToString() != "LT" &&
         this.CurrentInputs.dolphinInputs.joyInputs.jump.ToString() != "RT")
        {

            if (Input.GetButtonDown(this.CurrentInputs.dolphinInputs.joyInputs.jump.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (this.CurrentInputs.dolphinInputs.joyInputs.jump.ToString() == "LT")
            {
                if (Input.GetAxis("LRT") > 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (Input.GetAxis("LRT") < 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
        }
    }

    private bool dolphJumpPcI()
    {
        if (Input.GetButtonDown(this.CurrentInputs.dolphinInputs.keyInputs.jump.ToString()))
            return true;
        else
            return false;
    }

    #endregion

    #region To Frog Input
    private bool toFrogPressed()
    {
        switch (cForm)
        {
            case CurrentForm.Std: return toFrogbyStdInput();

            case CurrentForm.Crane: return toFrogbyCraneInput();

            case CurrentForm.Arma: return toFrogbyArmaInput();

            case CurrentForm.Dolphin: return toFrogbyDolpInput();

        }


        return false;
    }

    private bool toFrogbyStdInput()
    {
        if (toFrogbyStdJoyI() || toFrogbyStdPcI())
            return true;
        else
            return false;

    }

    private bool toFrogbyStdJoyI()
    {
        if (this.CurrentInputs.standardInputs.joyInputs.toFrog.ToString() != "LT" &&
         this.CurrentInputs.standardInputs.joyInputs.toFrog.ToString() != "RT")
        {

            if (Input.GetButtonDown(this.CurrentInputs.standardInputs.joyInputs.toFrog.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (this.CurrentInputs.standardInputs.joyInputs.toFrog.ToString() == "LT")
            {
                if (Input.GetAxis("LRT") > 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (Input.GetAxis("LRT") < 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
        }
    }

    private bool toFrogbyStdPcI()
    {
        if (Input.GetButtonDown(this.CurrentInputs.standardInputs.keyInputs.toFrog.ToString()))
            return true;
        else
            return false;
    }

    private bool toFrogbyCraneInput()
    {
        if (toFrogbyCraneJoyI() || toFrogbyCranePcI())
            return true;
        else
            return false;

    }

    private bool toFrogbyCraneJoyI()
    {
        if (this.CurrentInputs.craneInputs.joyInputs.toFrog.ToString() != "LT" &&
         this.CurrentInputs.craneInputs.joyInputs.toFrog.ToString() != "RT")
        {

            if (Input.GetButtonDown(this.CurrentInputs.craneInputs.joyInputs.toFrog.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (this.CurrentInputs.craneInputs.joyInputs.toFrog.ToString() == "LT")
            {
                if (Input.GetAxis("LRT") > 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (Input.GetAxis("LRT") < 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
        }
    }

    private bool toFrogbyCranePcI()
    {
        if (Input.GetButtonDown(this.CurrentInputs.craneInputs.keyInputs.toFrog.ToString()))
            return true;
        else
            return false;
    }

    private bool toFrogbyArmaInput()
    {
        if (toFrogbyArmaJoyI() || toFrogbyArmaPcI())
            return true;
        else
            return false;

    }

    private bool toFrogbyArmaJoyI()
    {
        if (this.CurrentInputs.armaInputs.joyInputs.toFrog.ToString() != "LT" &&
         this.CurrentInputs.armaInputs.joyInputs.toFrog.ToString() != "RT")
        {

            if (Input.GetButtonDown(this.CurrentInputs.armaInputs.joyInputs.toFrog.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (this.CurrentInputs.armaInputs.joyInputs.toFrog.ToString() == "LT")
            {
                if (Input.GetAxis("LRT") > 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (Input.GetAxis("LRT") < 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
        }
    }

    private bool toFrogbyArmaPcI()
    {
        if (Input.GetButtonDown(this.CurrentInputs.armaInputs.keyInputs.toFrog.ToString()))
            return true;
        else
            return false;
    }

    private bool toFrogbyDolpInput()
    {
        if (toFrogbyDolpJoyI() || toFrogbyDolpPcI())
            return true;
        else
            return false;

    }

    private bool toFrogbyDolpJoyI()
    {
        if (this.CurrentInputs.dolphinInputs.joyInputs.toFrog.ToString() != "LT" &&
         this.CurrentInputs.dolphinInputs.joyInputs.toFrog.ToString() != "RT")
        {

            if (Input.GetButtonDown(this.CurrentInputs.dolphinInputs.joyInputs.toFrog.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (this.CurrentInputs.dolphinInputs.joyInputs.toFrog.ToString() == "LT")
            {
                if (Input.GetAxis("LRT") > 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (Input.GetAxis("LRT") < 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
        }
    }

    private bool toFrogbyDolpPcI()
    {
        if (Input.GetButtonDown(this.CurrentInputs.dolphinInputs.keyInputs.toFrog.ToString()))
            return true;
        else
            return false;
    }
    #endregion

    #region To Arma Input
    private bool toArmaPressed()
    {
        switch (cForm)
        {
            case CurrentForm.Std: return toArmabyStdInput();

            case CurrentForm.Crane: return toArmabyCraneInput();

            case CurrentForm.Frog: return toArmabyFrogInput();

            case CurrentForm.Dolphin: return toArmabyDolpInput();

        }


        return false;
    }

    private bool toArmabyStdInput()
    {
        if (toArmabyStdJoyI() || toArmabyStdPcI())
            return true;
        else
            return false;

    }

    private bool toArmabyStdJoyI()
    {
        if (this.CurrentInputs.standardInputs.joyInputs.toArma.ToString() != "LT" &&
       this.CurrentInputs.standardInputs.joyInputs.toArma.ToString() != "RT")
        {

            if (Input.GetButtonDown(this.CurrentInputs.standardInputs.joyInputs.toArma.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (this.CurrentInputs.standardInputs.joyInputs.toArma.ToString() == "LT")
            {
                if (Input.GetAxis("LRT") > 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (Input.GetAxis("LRT") < 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
        }
    }

    private bool toArmabyStdPcI()
    {
        if (Input.GetButtonDown(this.CurrentInputs.standardInputs.keyInputs.toArma.ToString()))
            return true;
        else
            return false;
    }

    private bool toArmabyCraneInput()
    {
        if (toArmabyCraneJoyI() || toArmabyCranePcI())
            return true;
        else
            return false;

    }

    private bool toArmabyCraneJoyI()
    {
        if (this.CurrentInputs.craneInputs.joyInputs.toArma.ToString() != "LT" &&
        this.CurrentInputs.craneInputs.joyInputs.toArma.ToString() != "RT")
        {

            if (Input.GetButtonDown(this.CurrentInputs.craneInputs.joyInputs.toArma.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (this.CurrentInputs.craneInputs.joyInputs.toArma.ToString() == "LT")
            {
                if (Input.GetAxis("LRT") > 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (Input.GetAxis("LRT") < 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
        }
    }

    private bool toArmabyCranePcI()
    {
        if (Input.GetButtonDown(this.CurrentInputs.craneInputs.keyInputs.toArma.ToString()))
            return true;
        else
            return false;
    }

    private bool toArmabyFrogInput()
    {
        if (toArmabyFrogJoyI() || toArmabyFrogPcI())
            return true;
        else
            return false;

    }

    private bool toArmabyFrogJoyI()
    {
        if (this.CurrentInputs.frogInputs.joyInputs.toArma.ToString() != "LT" &&
         this.CurrentInputs.frogInputs.joyInputs.toArma.ToString() != "RT")
        {

            if (Input.GetButtonDown(this.CurrentInputs.frogInputs.joyInputs.toArma.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (this.CurrentInputs.frogInputs.joyInputs.toArma.ToString() == "LT")
            {
                if (Input.GetAxis("LRT") > 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (Input.GetAxis("LRT") < 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
        }
    }

    private bool toArmabyFrogPcI()
    {
        if (Input.GetButtonDown(this.CurrentInputs.frogInputs.keyInputs.toArma.ToString()))
            return true;
        else
            return false;
    }

    private bool toArmabyDolpInput()
    {
        if (toArmabyDolpJoyI() || toArmabyDolpPcI())
            return true;
        else
            return false;

    }

    private bool toArmabyDolpJoyI()
    {
        if (this.CurrentInputs.dolphinInputs.joyInputs.toArma.ToString() != "LT" &&
        this.CurrentInputs.dolphinInputs.joyInputs.toArma.ToString() != "RT")
        {

            if (Input.GetButtonDown(this.CurrentInputs.dolphinInputs.joyInputs.toArma.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (this.CurrentInputs.dolphinInputs.joyInputs.toArma.ToString() == "LT")
            {
                if (Input.GetAxis("LRT") > 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (Input.GetAxis("LRT") < 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
        }
    }

    private bool toArmabyDolpPcI()
    {
        if (Input.GetButtonDown(this.CurrentInputs.dolphinInputs.keyInputs.toArma.ToString()))
            return true;
        else
            return false;
    }
    #endregion

    #region To Crane Input
    private bool toCranePressed()
    {
        switch (cForm)
        {
            case CurrentForm.Std: return toCranebyStdInput();

            case CurrentForm.Frog: return toCranebyFrogInput();

            case CurrentForm.Arma: return toCranebyArmaInput();

            case CurrentForm.Dolphin: return toCranebyDolpInput();

        }


        return false;
    }

    private bool toCranebyStdInput()
    {
        if (toCranebyStdJoyI() || toCranebyStdPcI())
            return true;
        else
            return false;

    }

    private bool toCranebyStdJoyI()
    {
        if (this.CurrentInputs.standardInputs.joyInputs.toCrane.ToString() != "LT" &&
        this.CurrentInputs.standardInputs.joyInputs.toCrane.ToString() != "RT")
        {

            if (Input.GetButtonDown(this.CurrentInputs.standardInputs.joyInputs.toCrane.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (this.CurrentInputs.standardInputs.joyInputs.toCrane.ToString() == "LT")
            {
                if (Input.GetAxis("LRT") > 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (Input.GetAxis("LRT") < 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
        }
    }

    private bool toCranebyStdPcI()
    {
        if (Input.GetButtonDown(this.CurrentInputs.standardInputs.keyInputs.toCrane.ToString()))
            return true;
        else
            return false;
    }

    private bool toCranebyFrogInput()
    {
        if (toCranebyFrogJoyI() || toCranebyFrogPcI())
            return true;
        else
            return false;

    }

    private bool toCranebyFrogJoyI()
    {
        if (this.CurrentInputs.frogInputs.joyInputs.toCrane.ToString() != "LT" &&
       this.CurrentInputs.frogInputs.joyInputs.toCrane.ToString() != "RT")
        {

            if (Input.GetButtonDown(this.CurrentInputs.frogInputs.joyInputs.toCrane.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (this.CurrentInputs.frogInputs.joyInputs.toCrane.ToString() == "LT")
            {
                if (Input.GetAxis("LRT") > 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (Input.GetAxis("LRT") < 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
        }
    }

    private bool toCranebyFrogPcI()
    {
        if (Input.GetButtonDown(this.CurrentInputs.frogInputs.keyInputs.toCrane.ToString()))
            return true;
        else
            return false;
    }

    private bool toCranebyArmaInput()
    {
        if (toCranebyArmaJoyI() || toCranebyArmaPcI())
            return true;
        else
            return false;

    }

    private bool toCranebyArmaJoyI()
    {
        if (this.CurrentInputs.armaInputs.joyInputs.toCrane.ToString() != "LT" &&
        this.CurrentInputs.armaInputs.joyInputs.toCrane.ToString() != "RT")
        {

            if (Input.GetButtonDown(this.CurrentInputs.armaInputs.joyInputs.toCrane.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (this.CurrentInputs.armaInputs.joyInputs.toCrane.ToString() == "LT")
            {
                if (Input.GetAxis("LRT") > 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (Input.GetAxis("LRT") < 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
        }
    }

    private bool toCranebyArmaPcI()
    {
        if (Input.GetButtonDown(this.CurrentInputs.armaInputs.keyInputs.toCrane.ToString()))
            return true;
        else
            return false;
    }

    private bool toCranebyDolpInput()
    {
        if (toCranebyDolpJoyI() || toCranebyDolpPcI())
            return true;
        else
            return false;

    }

    private bool toCranebyDolpJoyI()
    {
        if (this.CurrentInputs.dolphinInputs.joyInputs.toCrane.ToString() != "LT" &&
         this.CurrentInputs.dolphinInputs.joyInputs.toCrane.ToString() != "RT")
        {

            if (Input.GetButtonDown(this.CurrentInputs.dolphinInputs.joyInputs.toCrane.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (this.CurrentInputs.dolphinInputs.joyInputs.toCrane.ToString() == "LT")
            {
                if (Input.GetAxis("LRT") > 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (Input.GetAxis("LRT") < 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
        }
    }

    private bool toCranebyDolpPcI()
    {
        if (Input.GetButtonDown(this.CurrentInputs.dolphinInputs.keyInputs.toCrane.ToString()))
            return true;
        else
            return false;
    }
    #endregion

    #region To Dolp Input
    private bool toDolpPressed()
    {
        switch (cForm)
        {
            case CurrentForm.Std: return toDolpbyStdInput();

            case CurrentForm.Crane: return toDolpbyCraneInput();

            case CurrentForm.Arma: return toDolpbyArmaInput();

            case CurrentForm.Frog: return toDolpbyFrogInput();

        }


        return false;
    }

    private bool toDolpbyStdInput()
    {
        if (toDolpbyStdJoyI() || toDolpbyStdPcI())
            return true;
        else
            return false;

    }

    private bool toDolpbyStdJoyI()
    {
        if (this.CurrentInputs.standardInputs.joyInputs.toDolphin.ToString() != "LT" &&
         this.CurrentInputs.standardInputs.joyInputs.toDolphin.ToString() != "RT")
        {

            if (Input.GetButtonDown(this.CurrentInputs.standardInputs.joyInputs.toDolphin.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (this.CurrentInputs.standardInputs.joyInputs.toDolphin.ToString() == "LT")
            {
                if (Input.GetAxis("LRT") > 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (Input.GetAxis("LRT") < 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
        }
    }

    private bool toDolpbyStdPcI()
    {
        if (Input.GetButtonDown(this.CurrentInputs.standardInputs.keyInputs.toDolphin.ToString()))
            return true;
        else
            return false;
    }

    private bool toDolpbyCraneInput()
    {
        if (toDolpbyCraneJoyI() || toDolpbyCranePcI())
            return true;
        else
            return false;

    }

    private bool toDolpbyCraneJoyI()
    {
        if (this.CurrentInputs.craneInputs.joyInputs.toDolphin.ToString() != "LT" &&
        this.CurrentInputs.craneInputs.joyInputs.toDolphin.ToString() != "RT")
        {

            if (Input.GetButtonDown(this.CurrentInputs.craneInputs.joyInputs.toDolphin.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (this.CurrentInputs.craneInputs.joyInputs.toDolphin.ToString() == "LT")
            {
                if (Input.GetAxis("LRT") > 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (Input.GetAxis("LRT") < 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
        }
    }

    private bool toDolpbyCranePcI()
    {
        if (Input.GetButtonDown(this.CurrentInputs.craneInputs.keyInputs.toDolphin.ToString()))
            return true;
        else
            return false;
    }

    private bool toDolpbyArmaInput()
    {
        if (toDolpbyArmaJoyI() || toDolpbyArmaPcI())
            return true;
        else
            return false;

    }

    private bool toDolpbyArmaJoyI()
    {
        if (this.CurrentInputs.armaInputs.joyInputs.toDolphin.ToString() != "LT" &&
         this.CurrentInputs.armaInputs.joyInputs.toDolphin.ToString() != "RT")
        {

            if (Input.GetButtonDown(this.CurrentInputs.armaInputs.joyInputs.toDolphin.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (this.CurrentInputs.armaInputs.joyInputs.toDolphin.ToString() == "LT")
            {
                if (Input.GetAxis("LRT") > 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (Input.GetAxis("LRT") < 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
        }
    }

    private bool toDolpbyArmaPcI()
    {
        if (Input.GetButtonDown(this.CurrentInputs.armaInputs.keyInputs.toDolphin.ToString()))
            return true;
        else
            return false;
    }

    private bool toDolpbyFrogInput()
    {
        if (toDolpbyFrogJoyI() || toDolpbyFrogPcI())
            return true;
        else
            return false;

    }

    private bool toDolpbyFrogJoyI()
    {
        if (this.CurrentInputs.frogInputs.joyInputs.toDolphin.ToString() != "LT" &&
         this.CurrentInputs.frogInputs.joyInputs.toDolphin.ToString() != "RT")
        {

            if (Input.GetButtonDown(this.CurrentInputs.frogInputs.joyInputs.toDolphin.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (this.CurrentInputs.frogInputs.joyInputs.toDolphin.ToString() == "LT")
            {
                if (Input.GetAxis("LRT") > 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (Input.GetAxis("LRT") < 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
        }
    }

    private bool toDolpbyFrogPcI()
    {
        if (Input.GetButtonDown(this.CurrentInputs.frogInputs.keyInputs.toDolphin.ToString()))
            return true;
        else
            return false;
    }
    #endregion

    #region To Std Input
    private bool toStdPressed()
    {
        switch (cForm)
        {
            case CurrentForm.Frog: return toStdbyFrogInput();

            case CurrentForm.Crane: return toStdbyCraneInput();

            case CurrentForm.Arma: return toStdbyArmaInput();

            case CurrentForm.Dolphin: return toStdbyDolpInput();

        }


        return false;
    }

    private bool toStdbyFrogInput()
    {
        if (toStdbyFrogJoyI() || toStdbyFrogPcI())
            return true;
        else
            return false;

    }

    private bool toStdbyFrogJoyI()
    {
        if (this.CurrentInputs.frogInputs.joyInputs.toStd.ToString() != "LT" &&
        this.CurrentInputs.frogInputs.joyInputs.toStd.ToString() != "RT")
        {

            if (Input.GetButtonDown(this.CurrentInputs.frogInputs.joyInputs.toStd.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (this.CurrentInputs.frogInputs.joyInputs.toStd.ToString() == "LT")
            {
                if (Input.GetAxis("LRT") > 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (Input.GetAxis("LRT") < 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
        }
    }

    private bool toStdbyFrogPcI()
    {
        if (Input.GetButtonDown(this.CurrentInputs.frogInputs.keyInputs.toStd.ToString()))
            return true;
        else
            return false;
    }

    private bool toStdbyCraneInput()
    {
        if (toStdbyCraneJoyI() || toStdbyCranePcI())
            return true;
        else
            return false;

    }

    private bool toStdbyCraneJoyI()
    {
        if (this.CurrentInputs.craneInputs.joyInputs.toStd.ToString() != "LT" &&
         this.CurrentInputs.craneInputs.joyInputs.toStd.ToString() != "RT")
        {

            if (Input.GetButtonDown(this.CurrentInputs.craneInputs.joyInputs.toStd.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (this.CurrentInputs.craneInputs.joyInputs.toStd.ToString() == "LT")
            {
                if (Input.GetAxis("LRT") > 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (Input.GetAxis("LRT") < 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
        }
    }

    private bool toStdbyCranePcI()
    {
        if (Input.GetButtonDown(this.CurrentInputs.craneInputs.keyInputs.toStd.ToString()))
            return true;
        else
            return false;
    }

    private bool toStdbyArmaInput()
    {
        if (toStdbyArmaJoyI() || toStdbyArmaPcI())
            return true;
        else
            return false;

    }

    private bool toStdbyArmaJoyI()
    {
        if (this.CurrentInputs.armaInputs.joyInputs.toStd.ToString() != "LT" &&
        this.CurrentInputs.armaInputs.joyInputs.toStd.ToString() != "RT")
        {

            if (Input.GetButtonDown(this.CurrentInputs.armaInputs.joyInputs.toStd.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (this.CurrentInputs.armaInputs.joyInputs.toStd.ToString() == "LT")
            {
                if (Input.GetAxis("LRT") > 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (Input.GetAxis("LRT") < 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
        }
    }

    private bool toStdbyArmaPcI()
    {
        if (Input.GetButtonDown(this.CurrentInputs.armaInputs.keyInputs.toStd.ToString()))
            return true;
        else
            return false;
    }

    private bool toStdbyDolpInput()
    {
        if (toStdbyDolpJoyI() || toStdbyDolpPcI())
            return true;
        else
            return false;

    }

    private bool toStdbyDolpJoyI()
    {
        if (this.CurrentInputs.dolphinInputs.joyInputs.toStd.ToString() != "LT" &&
         this.CurrentInputs.dolphinInputs.joyInputs.toStd.ToString() != "RT")
        {

            if (Input.GetButtonDown(this.CurrentInputs.dolphinInputs.joyInputs.toStd.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (this.CurrentInputs.dolphinInputs.joyInputs.toStd.ToString() == "LT")
            {
                if (Input.GetAxis("LRT") > 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (Input.GetAxis("LRT") < 0 && !switchCooldown)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
        }
    }

    private bool toStdbyDolpPcI()
    {
        if (Input.GetButtonDown(this.CurrentInputs.dolphinInputs.keyInputs.toStd.ToString()))
            return true;
        else
            return false;
    }
    #endregion

    #region Roll Input
    public bool rollPressed()
    {
        if (cForm == CurrentForm.Arma)
            return armaRollInput();
        else
            return false;
    }

    private bool armaRollInput()
    {
        if (armaRollJoyI() || armaRollPcI())
            return true;
        else
            return false;
    }

    private bool armaRollJoyI()
    {
        if (this.CurrentInputs.armaInputs.joyInputs.roll.ToString() != "LT" &&
        this.CurrentInputs.armaInputs.joyInputs.roll.ToString() != "RT")
        {

            if (Input.GetButtonDown(this.CurrentInputs.armaInputs.joyInputs.roll.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (this.CurrentInputs.armaInputs.joyInputs.roll.ToString() == "LT" && !isRollPressed)
            {
                if (Input.GetAxis("LRT") > 0)
                {
                    isRollPressed = true;
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (Input.GetAxis("LRT") < 0 && !isRollPressed)
                {
                    isRollPressed = true;
                    return true;
                }
                else
                    return false;
            }
        }
    }

    private bool armaRollPcI()
    {
        if (Input.GetButtonDown(this.CurrentInputs.armaInputs.keyInputs.roll.ToString()))
            return true;
        else
            return false;
    }

    public bool rollReleased()
    {
        if (cForm == CurrentForm.Arma)
            return armaRollRInput();
        else
            return false;
    }

    private bool armaRollRInput()
    {
        if (armaRollRJoyI() || armaRollRPcI())
            return true;
        else
            return false;
    }

    private bool armaRollRJoyI()
    {
        if (this.CurrentInputs.armaInputs.joyInputs.roll.ToString() != "LT" &&
        this.CurrentInputs.armaInputs.joyInputs.roll.ToString() != "RT")
        {

            if (Input.GetButtonUp(this.CurrentInputs.armaInputs.joyInputs.roll.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (this.CurrentInputs.armaInputs.joyInputs.roll.ToString() == "LT" && isRollPressed)
            {
                if (Input.GetAxis("LRT") == 0)
                {
                    isRollPressed = false;
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (Input.GetAxis("LRT") == 0 && isRollPressed)
                {
                    isRollPressed = false;
                    return true;
                }
                else
                    return false;
            }
        }
    }

    private bool armaRollRPcI()
    {
        if (Input.GetButtonUp(this.CurrentInputs.armaInputs.keyInputs.roll.ToString()))
            return true;
        else
            return false;
    }
    #endregion

    #region Vertical Fissure Input
    private bool VFissurePressed()
    {
        if (cForm == CurrentForm.Std)
            return StdVFissureInput();
        else
            return false;
    }

    private bool StdVFissureInput()
    {
        if (StdVFissureJoyI() || StdVFissurePcI())
            return true;
        else
            return false;
    }

    private bool StdVFissureJoyI()
    {
        if (this.CurrentInputs.standardInputs.joyInputs.VerticalFissure.ToString() != "LT" &&
        this.CurrentInputs.standardInputs.joyInputs.VerticalFissure.ToString() != "RT")
        {

            if (Input.GetButtonDown(this.CurrentInputs.standardInputs.joyInputs.VerticalFissure.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (this.CurrentInputs.standardInputs.joyInputs.VerticalFissure.ToString() == "LT")
            {
                if (Input.GetAxis("LRT") > 0)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (Input.GetAxis("LRT") < 0)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
        }
    }

    private bool StdVFissurePcI()
    {
        if (Input.GetButtonDown(this.CurrentInputs.standardInputs.keyInputs.VerticalFissure.ToString()))
            return true;
        else
            return false;
    }
    #endregion

    #region Horizontal Fissure Input
    private bool HFissurePressed()
    {
        if (cForm == CurrentForm.Frog)
            return StdVFissureInput();
        else
            return false;
    }

    private bool FrogHFissureInput()
    {
        if (FrogHFissureJoyI() || FrogHFissurePcI())
            return true;
        else
            return false;
    }

    private bool FrogHFissureJoyI()
    {
        if (this.CurrentInputs.frogInputs.joyInputs.HorizontalFissure.ToString() != "LT" &&
        this.CurrentInputs.frogInputs.joyInputs.HorizontalFissure.ToString() != "RT")
        {

            if (Input.GetButtonDown(this.CurrentInputs.frogInputs.joyInputs.HorizontalFissure.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (this.CurrentInputs.frogInputs.joyInputs.HorizontalFissure.ToString() == "LT")
            {
                if (Input.GetAxis("LRT") > 0)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (Input.GetAxis("LRT") < 0)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
        }
    }

    private bool FrogHFissurePcI()
    {
        if (Input.GetButtonDown(this.CurrentInputs.frogInputs.keyInputs.HorizontalFissure.ToString()))
            return true;
        else
            return false;
    }
    #endregion

    #region Dolphin Swim Below Input
    private bool DolpSwimBPressed()
    {
        if (cForm == CurrentForm.Dolphin)
            return DolpSwimBInput();
        else
            return false;
    }

    private bool DolpSwimBInput()
    {
        if (DolpSwimBJoyI() || DolpSwimBPcI())
            return true;
        else
            return false;
    }

    private bool DolpSwimBJoyI()
    {
        if (this.CurrentInputs.dolphinInputs.joyInputs.moveBelow.ToString() != "LT" &&
        this.CurrentInputs.dolphinInputs.joyInputs.moveBelow.ToString() != "RT")
        {

            if (Input.GetButtonDown(this.CurrentInputs.dolphinInputs.joyInputs.moveBelow.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (this.CurrentInputs.dolphinInputs.joyInputs.moveBelow.ToString() == "LT")
            {
                if (Input.GetAxis("LRT") > 0)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (Input.GetAxis("LRT") < 0)
                {
                    switchCooldown = true;
                    StartCoroutine(SwitchingCooldown());
                    return true;
                }
                else
                    return false;
            }
        }
    }

    private bool DolpSwimBPcI()
    {
        if (Input.GetButtonDown(this.CurrentInputs.dolphinInputs.keyInputs.moveBelow.ToString()))
            return true;
        else
            return false;
    }
    #endregion
    #endregion

    #region Extra Inputs
    private void ExtraInputsHandler()
    {

        if (SceneManager.GetActiveScene().buildIndex != 11)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                this.switchSceneRequest.Invoke("Main Menu");
            if (Input.GetKeyDown(KeyCode.PageUp))
                this.switchSceneRequest.Invoke(this.CalculatingScene(SceneManager.GetActiveScene().name, 1));
            if (Input.GetKeyDown(KeyCode.PageDown))
                this.switchSceneRequest.Invoke(this.CalculatingScene(SceneManager.GetActiveScene().name, -1));
            if (Input.GetKeyDown(KeyCode.End))
                this.switchSceneRequest.Invoke(this.CalculatingScene(SceneManager.GetActiveScene().name, 0));
        }
    }

    private string CalculatingScene(string currentScene, int codeLogic)
    {
        int i = 0;

        string currentSceneToFind = this.scenes.Find(x => x == currentScene);

        if (currentSceneToFind != null) i = this.scenes.IndexOf(currentSceneToFind);
        else return null;

        if (codeLogic == 1)
        {
            if (i + 1 < this.scenes.Count) return this.scenes[i + 1];
            else return null;
        }
        else if (codeLogic == -1)
        {
            if (i - 1 >= 0) return this.scenes[i - 1];
            else return null;
        }
        else
        {
            return currentScene;
        }
    }
    #endregion

    #region Story Inputs
    private void StoryInputsHandler()
    {
        if (StoryInputPressed() && !FSMChecker.storyMode)
        {
            storyLivingRequest.Invoke(this.ssTempRef);
        }
    }

    private bool StoryInputPressed()
    {
        if (storyModeInput)
        {
            if (storyJoyInput.ToString() != "LT" &&
         storyJoyInput.ToString() != "RT")
            {
                if (Input.GetButtonDown(storyJoyInput.ToString()))
                    return true;
                else
                    return false;
            }
            else
            {
                if (storyJoyInput.ToString() == "LT")
                {
                    if (Input.GetAxis("LRT") > 0 && !switchCooldown)
                    {
                        switchCooldown = true;
                        StartCoroutine(SwitchingCooldown());
                        return true;
                    }
                    else
                        return false;
                }
                else
                {
                    if (Input.GetAxis("LRT") < 0 && !switchCooldown)
                    {
                        switchCooldown = true;
                        StartCoroutine(SwitchingCooldown());
                        return true;
                    }
                    else
                        return false;
                }
            }
        }
        else
            return false;
    }

    private void SettingStoryInputs(buttonsJoy joyInput, buttonsPc pcInput, SingleStory ssToRemember)
    {
        storyJoyInput = joyInput;
        storyPcInput = pcInput;

        storyModeInput = true;

        this.ssTempRef = ssToRemember;
    }

    private void DisablingStoryInputs()
    {
        this.storyModeInput = false;
        this.storyJoyInput = buttonsJoy.none;
        this.storyPcInput = buttonsPc.none;
        this.ssTempRef = null;
    }
    #endregion

    #region General Methods

    private IEnumerator SwitchingCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        switchCooldown = false;
    }

    private void UpdatingCurrentFormInputs(string currentForm)
    {
        switch (currentForm)
        {
            case "Standard Form":
                cForm = PlayerInputs.CurrentForm.Std;
                StartCoroutine(SwitchingCooldown());
                break;
            case "Frog Form":
                cForm = PlayerInputs.CurrentForm.Frog;
                StartCoroutine(SwitchingCooldown());
                break;
            case "Armadillo Form":
                cForm = PlayerInputs.CurrentForm.Arma;
                StartCoroutine(SwitchingCooldown());
                break;
            case "Dragon Form":
                cForm = PlayerInputs.CurrentForm.Crane;
                StartCoroutine(SwitchingCooldown());
                break;
            case "Dolphin Form":
                cForm = PlayerInputs.CurrentForm.Dolphin;
                StartCoroutine(SwitchingCooldown());
                break;
        }
    }
    #endregion 
}
