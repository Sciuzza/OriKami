using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class PlayerInputs : MonoBehaviour
{

    public inputSettings currentInputs;
    
    #region Event Classes
    [System.Serializable]
    public class moveInp : UnityEvent<abilties, Vector3>
    {
    }

    [System.Serializable]
    public class generalAbiInput : UnityEvent<abilties>
    {
    }

    [System.Serializable]
    public class sceneSwitchInput : UnityEvent<int>
    {
    }

    #endregion

    #region Events
    public UnityEvent rollStopped;
    public generalAbiInput genAbiRequest;
    public moveInp dirAbiRequest;
    public sceneSwitchInput mainMenuRequest;
    #endregion

    #region Private Use Variables
    private Vector3 moveDirection;

    private bool switchCooldown = false;
    private bool isRollPressed = false;

    private enum currentForm { std, frog, crane, arma, dolphin };

    private currentForm cForm = currentForm.std;
    #endregion

    #region Taking References and Linking Events
    private void Awake()
    {
        FSMChecker fsmCheckerTempLink = this.GetComponent<FSMChecker>();

        fsmCheckerTempLink.formChangedInp.AddListener(UpdatingCurrentFormInputs);
    } 
    #endregion

    #region Player Inputs Handler
    private void Update()
    {

        MovingInputHandler();
        genAbiInputs();
        ExtraInputsHandler();

    }

    #region Move Input
    private void MovingInputHandler()
    {


        MoveInput();


        if (moveDirection.sqrMagnitude != 0)
        {
            dirAbiRequest.Invoke(abilties.move, moveDirection);
            dirAbiRequest.Invoke(abilties.rotate, moveDirection);
        }

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
        }

        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);


        Quaternion inputRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up), Vector3.up);
        moveDirection = inputRotation * moveDirection;

    }
    #endregion

    #region General Abi Input
    private void genAbiInputs()
    {
        if (jumpPressed())
            genAbiRequest.Invoke(abilties.jump);
        else if (toFrogPressed())
            genAbiRequest.Invoke(abilties.toFrog);
        else if (toArmaPressed())
            genAbiRequest.Invoke(abilties.toArma);
        else if (toCranePressed())
            genAbiRequest.Invoke(abilties.toCrane);
        else if (toDolpPressed())
            genAbiRequest.Invoke(abilties.toDolp);
        else if (toStdPressed())
            genAbiRequest.Invoke(abilties.toStd);
        else if (rollPressed())
            genAbiRequest.Invoke(abilties.roll);
        else if (rollReleased())
            rollStopped.Invoke();
        else if (VFissurePressed())
            genAbiRequest.Invoke(abilties.VFissure);
        else if (HFissurePressed())
            genAbiRequest.Invoke(abilties.HFissure);
        else if (DolpSwimBPressed())
            genAbiRequest.Invoke(abilties.dolpSwimBel);


    }


    #region Jump Input
    private bool jumpPressed()
    {

        switch (cForm)
        {
            case currentForm.std: return stdJumpInput();

            case currentForm.frog: return frogJumpInput();

            case currentForm.dolphin: return dolphJumpInput();

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
        if (currentInputs.standardInputs.joyInputs.Jump.ToString() != "LT" &&
         currentInputs.standardInputs.joyInputs.Jump.ToString() != "RT")
        {

            if (Input.GetButtonDown(currentInputs.standardInputs.joyInputs.Jump.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (currentInputs.standardInputs.joyInputs.Jump.ToString() == "LT")
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
        if (Input.GetButtonDown(currentInputs.standardInputs.keyInputs.Jump.ToString()))
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
        if (currentInputs.frogInputs.joyInputs.Jump.ToString() != "LT" &&
        currentInputs.frogInputs.joyInputs.Jump.ToString() != "RT")
        {

            if (Input.GetButtonDown(currentInputs.frogInputs.joyInputs.Jump.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (currentInputs.frogInputs.joyInputs.Jump.ToString() == "LT")
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
        if (Input.GetButtonDown(currentInputs.frogInputs.keyInputs.Jump.ToString()))
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
        if (currentInputs.dolphinInputs.joyInputs.jump.ToString() != "LT" &&
         currentInputs.dolphinInputs.joyInputs.jump.ToString() != "RT")
        {

            if (Input.GetButtonDown(currentInputs.dolphinInputs.joyInputs.jump.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (currentInputs.dolphinInputs.joyInputs.jump.ToString() == "LT")
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
        if (Input.GetButtonDown(currentInputs.dolphinInputs.keyInputs.jump.ToString()))
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
            case currentForm.std: return toFrogbyStdInput();

            case currentForm.crane: return toFrogbyCraneInput();

            case currentForm.arma: return toFrogbyArmaInput();

            case currentForm.dolphin: return toFrogbyDolpInput();

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
        if (currentInputs.standardInputs.joyInputs.toFrog.ToString() != "LT" &&
         currentInputs.standardInputs.joyInputs.toFrog.ToString() != "RT")
        {

            if (Input.GetButtonDown(currentInputs.standardInputs.joyInputs.toFrog.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (currentInputs.standardInputs.joyInputs.toFrog.ToString() == "LT")
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
        if (Input.GetButtonDown(currentInputs.standardInputs.keyInputs.toFrog.ToString()))
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
        if (currentInputs.craneInputs.joyInputs.toFrog.ToString() != "LT" &&
         currentInputs.craneInputs.joyInputs.toFrog.ToString() != "RT")
        {

            if (Input.GetButtonDown(currentInputs.craneInputs.joyInputs.toFrog.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (currentInputs.craneInputs.joyInputs.toFrog.ToString() == "LT")
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
        if (Input.GetButtonDown(currentInputs.craneInputs.keyInputs.toFrog.ToString()))
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
        if (currentInputs.armaInputs.joyInputs.toFrog.ToString() != "LT" &&
         currentInputs.armaInputs.joyInputs.toFrog.ToString() != "RT")
        {

            if (Input.GetButtonDown(currentInputs.armaInputs.joyInputs.toFrog.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (currentInputs.armaInputs.joyInputs.toFrog.ToString() == "LT")
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
        if (Input.GetButtonDown(currentInputs.armaInputs.keyInputs.toFrog.ToString()))
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
        if (currentInputs.dolphinInputs.joyInputs.toFrog.ToString() != "LT" &&
         currentInputs.dolphinInputs.joyInputs.toFrog.ToString() != "RT")
        {

            if (Input.GetButtonDown(currentInputs.dolphinInputs.joyInputs.toFrog.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (currentInputs.dolphinInputs.joyInputs.toFrog.ToString() == "LT")
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
        if (Input.GetButtonDown(currentInputs.dolphinInputs.keyInputs.toFrog.ToString()))
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
            case currentForm.std: return toArmabyStdInput();

            case currentForm.crane: return toArmabyCraneInput();

            case currentForm.frog: return toArmabyFrogInput();

            case currentForm.dolphin: return toArmabyDolpInput();

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
        if (currentInputs.standardInputs.joyInputs.toArma.ToString() != "LT" &&
       currentInputs.standardInputs.joyInputs.toArma.ToString() != "RT")
        {

            if (Input.GetButtonDown(currentInputs.standardInputs.joyInputs.toArma.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (currentInputs.standardInputs.joyInputs.toArma.ToString() == "LT")
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
        if (Input.GetButtonDown(currentInputs.standardInputs.keyInputs.toArma.ToString()))
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
        if (currentInputs.craneInputs.joyInputs.toArma.ToString() != "LT" &&
        currentInputs.craneInputs.joyInputs.toArma.ToString() != "RT")
        {

            if (Input.GetButtonDown(currentInputs.craneInputs.joyInputs.toArma.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (currentInputs.craneInputs.joyInputs.toArma.ToString() == "LT")
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
        if (Input.GetButtonDown(currentInputs.craneInputs.keyInputs.toArma.ToString()))
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
        if (currentInputs.frogInputs.joyInputs.toArma.ToString() != "LT" &&
         currentInputs.frogInputs.joyInputs.toArma.ToString() != "RT")
        {

            if (Input.GetButtonDown(currentInputs.frogInputs.joyInputs.toArma.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (currentInputs.frogInputs.joyInputs.toArma.ToString() == "LT")
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
        if (Input.GetButtonDown(currentInputs.frogInputs.keyInputs.toArma.ToString()))
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
        if (currentInputs.dolphinInputs.joyInputs.toArma.ToString() != "LT" &&
        currentInputs.dolphinInputs.joyInputs.toArma.ToString() != "RT")
        {

            if (Input.GetButtonDown(currentInputs.dolphinInputs.joyInputs.toArma.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (currentInputs.dolphinInputs.joyInputs.toArma.ToString() == "LT")
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
        if (Input.GetButtonDown(currentInputs.dolphinInputs.keyInputs.toArma.ToString()))
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
            case currentForm.std: return toCranebyStdInput();

            case currentForm.frog: return toCranebyFrogInput();

            case currentForm.arma: return toCranebyArmaInput();

            case currentForm.dolphin: return toCranebyDolpInput();

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
        if (currentInputs.standardInputs.joyInputs.toCrane.ToString() != "LT" &&
        currentInputs.standardInputs.joyInputs.toCrane.ToString() != "RT")
        {

            if (Input.GetButtonDown(currentInputs.standardInputs.joyInputs.toCrane.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (currentInputs.standardInputs.joyInputs.toCrane.ToString() == "LT")
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
        if (Input.GetButtonDown(currentInputs.standardInputs.keyInputs.toCrane.ToString()))
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
        if (currentInputs.frogInputs.joyInputs.toCrane.ToString() != "LT" &&
       currentInputs.frogInputs.joyInputs.toCrane.ToString() != "RT")
        {

            if (Input.GetButtonDown(currentInputs.frogInputs.joyInputs.toCrane.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (currentInputs.frogInputs.joyInputs.toCrane.ToString() == "LT")
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
        if (Input.GetButtonDown(currentInputs.frogInputs.keyInputs.toCrane.ToString()))
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
        if (currentInputs.armaInputs.joyInputs.toCrane.ToString() != "LT" &&
        currentInputs.armaInputs.joyInputs.toCrane.ToString() != "RT")
        {

            if (Input.GetButtonDown(currentInputs.armaInputs.joyInputs.toCrane.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (currentInputs.armaInputs.joyInputs.toCrane.ToString() == "LT")
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
        if (Input.GetButtonDown(currentInputs.armaInputs.keyInputs.toCrane.ToString()))
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
        if (currentInputs.dolphinInputs.joyInputs.toCrane.ToString() != "LT" &&
         currentInputs.dolphinInputs.joyInputs.toCrane.ToString() != "RT")
        {

            if (Input.GetButtonDown(currentInputs.dolphinInputs.joyInputs.toCrane.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (currentInputs.dolphinInputs.joyInputs.toCrane.ToString() == "LT")
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
        if (Input.GetButtonDown(currentInputs.dolphinInputs.keyInputs.toCrane.ToString()))
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
            case currentForm.std: return toDolpbyStdInput();

            case currentForm.crane: return toDolpbyCraneInput();

            case currentForm.arma: return toDolpbyArmaInput();

            case currentForm.frog: return toDolpbyFrogInput();

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
        if (currentInputs.standardInputs.joyInputs.toDolphin.ToString() != "LT" &&
         currentInputs.standardInputs.joyInputs.toDolphin.ToString() != "RT")
        {

            if (Input.GetButtonDown(currentInputs.standardInputs.joyInputs.toDolphin.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (currentInputs.standardInputs.joyInputs.toDolphin.ToString() == "LT")
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
        if (Input.GetButtonDown(currentInputs.standardInputs.keyInputs.toDolphin.ToString()))
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
        if (currentInputs.craneInputs.joyInputs.toDolphin.ToString() != "LT" &&
        currentInputs.craneInputs.joyInputs.toDolphin.ToString() != "RT")
        {

            if (Input.GetButtonDown(currentInputs.craneInputs.joyInputs.toDolphin.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (currentInputs.craneInputs.joyInputs.toDolphin.ToString() == "LT")
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
        if (Input.GetButtonDown(currentInputs.craneInputs.keyInputs.toDolphin.ToString()))
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
        if (currentInputs.armaInputs.joyInputs.toDolphin.ToString() != "LT" &&
         currentInputs.armaInputs.joyInputs.toDolphin.ToString() != "RT")
        {

            if (Input.GetButtonDown(currentInputs.armaInputs.joyInputs.toDolphin.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (currentInputs.armaInputs.joyInputs.toDolphin.ToString() == "LT")
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
        if (Input.GetButtonDown(currentInputs.armaInputs.keyInputs.toDolphin.ToString()))
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
        if (currentInputs.frogInputs.joyInputs.toDolphin.ToString() != "LT" &&
         currentInputs.frogInputs.joyInputs.toDolphin.ToString() != "RT")
        {

            if (Input.GetButtonDown(currentInputs.frogInputs.joyInputs.toDolphin.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (currentInputs.frogInputs.joyInputs.toDolphin.ToString() == "LT")
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
        if (Input.GetButtonDown(currentInputs.frogInputs.keyInputs.toDolphin.ToString()))
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
            case currentForm.frog: return toStdbyFrogInput();

            case currentForm.crane: return toStdbyCraneInput();

            case currentForm.arma: return toStdbyArmaInput();

            case currentForm.dolphin: return toStdbyDolpInput();

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
        if (currentInputs.frogInputs.joyInputs.toStd.ToString() != "LT" &&
        currentInputs.frogInputs.joyInputs.toStd.ToString() != "RT")
        {

            if (Input.GetButtonDown(currentInputs.frogInputs.joyInputs.toStd.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (currentInputs.frogInputs.joyInputs.toStd.ToString() == "LT")
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
        if (Input.GetButtonDown(currentInputs.frogInputs.keyInputs.toStd.ToString()))
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
        if (currentInputs.craneInputs.joyInputs.toStd.ToString() != "LT" &&
         currentInputs.craneInputs.joyInputs.toStd.ToString() != "RT")
        {

            if (Input.GetButtonDown(currentInputs.craneInputs.joyInputs.toStd.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (currentInputs.craneInputs.joyInputs.toStd.ToString() == "LT")
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
        if (Input.GetButtonDown(currentInputs.craneInputs.keyInputs.toStd.ToString()))
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
        if (currentInputs.armaInputs.joyInputs.toStd.ToString() != "LT" &&
        currentInputs.armaInputs.joyInputs.toStd.ToString() != "RT")
        {

            if (Input.GetButtonDown(currentInputs.armaInputs.joyInputs.toStd.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (currentInputs.armaInputs.joyInputs.toStd.ToString() == "LT")
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
        if (Input.GetButtonDown(currentInputs.armaInputs.keyInputs.toStd.ToString()))
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
        if (currentInputs.dolphinInputs.joyInputs.toStd.ToString() != "LT" &&
         currentInputs.dolphinInputs.joyInputs.toStd.ToString() != "RT")
        {

            if (Input.GetButtonDown(currentInputs.dolphinInputs.joyInputs.toStd.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (currentInputs.dolphinInputs.joyInputs.toStd.ToString() == "LT")
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
        if (Input.GetButtonDown(currentInputs.dolphinInputs.keyInputs.toStd.ToString()))
            return true;
        else
            return false;
    }
    #endregion

    #region Roll Input
    private bool rollPressed()
    {
        if (cForm == currentForm.arma)
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
        if (currentInputs.armaInputs.joyInputs.roll.ToString() != "LT" &&
        currentInputs.armaInputs.joyInputs.roll.ToString() != "RT")
        {

            if (Input.GetButtonDown(currentInputs.armaInputs.joyInputs.roll.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (currentInputs.armaInputs.joyInputs.roll.ToString() == "LT" && !isRollPressed)
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
        if (Input.GetButtonDown(currentInputs.armaInputs.keyInputs.roll.ToString()))
            return true;
        else
            return false;
    }

    private bool rollReleased()
    {
        if (cForm == currentForm.arma)
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
        if (currentInputs.armaInputs.joyInputs.roll.ToString() != "LT" &&
        currentInputs.armaInputs.joyInputs.roll.ToString() != "RT")
        {

            if (Input.GetButtonUp(currentInputs.armaInputs.joyInputs.roll.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (currentInputs.armaInputs.joyInputs.roll.ToString() == "LT" && isRollPressed)
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
        if (Input.GetButtonUp(currentInputs.armaInputs.keyInputs.roll.ToString()))
            return true;
        else
            return false;
    }
    #endregion

    #region Vertical Fissure Input
    private bool VFissurePressed()
    {
        if (cForm == currentForm.std)
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
        if (currentInputs.standardInputs.joyInputs.VerticalFissure.ToString() != "LT" &&
        currentInputs.standardInputs.joyInputs.VerticalFissure.ToString() != "RT")
        {

            if (Input.GetButtonDown(currentInputs.standardInputs.joyInputs.VerticalFissure.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (currentInputs.standardInputs.joyInputs.VerticalFissure.ToString() == "LT")
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
        if (Input.GetButtonDown(currentInputs.standardInputs.keyInputs.VerticalFissure.ToString()))
            return true;
        else
            return false;
    }
    #endregion

    #region Horizontal Fissure Input
    private bool HFissurePressed()
    {
        if (cForm == currentForm.frog)
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
        if (currentInputs.frogInputs.joyInputs.HorizontalFissure.ToString() != "LT" &&
        currentInputs.frogInputs.joyInputs.HorizontalFissure.ToString() != "RT")
        {

            if (Input.GetButtonDown(currentInputs.frogInputs.joyInputs.HorizontalFissure.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (currentInputs.frogInputs.joyInputs.HorizontalFissure.ToString() == "LT")
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
        if (Input.GetButtonDown(currentInputs.frogInputs.keyInputs.HorizontalFissure.ToString()))
            return true;
        else
            return false;
    }
    #endregion

    #region Dolphin Swim Below Input
    private bool DolpSwimBPressed()
    {
        if (cForm == currentForm.dolphin)
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
        if (currentInputs.dolphinInputs.joyInputs.moveBelow.ToString() != "LT" &&
        currentInputs.dolphinInputs.joyInputs.moveBelow.ToString() != "RT")
        {

            if (Input.GetButtonDown(currentInputs.dolphinInputs.joyInputs.moveBelow.ToString()))
                return true;
            else
                return false;
        }
        else
        {
            if (currentInputs.dolphinInputs.joyInputs.moveBelow.ToString() == "LT")
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
        if (Input.GetButtonDown(currentInputs.dolphinInputs.keyInputs.moveBelow.ToString()))
            return true;
        else
            return false;
    }
    #endregion
    #endregion

    #region Extra Inputs
    private void ExtraInputsHandler()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            mainMenuRequest.Invoke(1);
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
                cForm = PlayerInputs.currentForm.std;
                StartCoroutine(SwitchingCooldown());
                break;
            case "Frog Form":
                cForm = PlayerInputs.currentForm.frog;
                StartCoroutine(SwitchingCooldown());
                break;
            case "Armadillo Form":
                cForm = PlayerInputs.currentForm.arma;
                StartCoroutine(SwitchingCooldown());
                break;
            case "Dragon Form":
                cForm = PlayerInputs.currentForm.crane;
                StartCoroutine(SwitchingCooldown());
                break;
            case "Dolphin Form":
                cForm = PlayerInputs.currentForm.dolphin;
                StartCoroutine(SwitchingCooldown());
                break;
        }
    }

    #endregion 
    #endregion

}
