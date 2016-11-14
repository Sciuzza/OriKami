using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class PlayerInputs : MonoBehaviour
{

    public inputSettings currentInputs;


    #region Private Use Variables
    private Vector3 moveDirection;

    private bool switchCooldown = false;

    private enum currentForm { std, frog, crane, arma, dolphin };

    private currentForm cForm = currentForm.std;
    #endregion

    #region Events Invoked
    [System.Serializable]
    public class moveInp : UnityEvent<abilties, Vector3>
    {
    }

    public moveInp dirAbiRequest;

    [System.Serializable]
    public class generalAbiInput : UnityEvent<abilties>
    {
    }

    public generalAbiInput genAbiRequest;
    #endregion


    private void Awake()
    {
        FSMChecker fsmCheckerTempLink = this.GetComponent<FSMChecker>();

        fsmCheckerTempLink.formChangedInp.AddListener(UpdatingCurrentFormInputs);
    }

    private void Update()
    {

        MovingInputHandler();
        genAbiInputs();

    }

    #region Move Input
    private void MovingInputHandler()
    {


        MoveInput();

        dirAbiRequest.Invoke(abilties.move, moveDirection);
        dirAbiRequest.Invoke(abilties.rotate, moveDirection);

    }

    private void MoveInput()
    {

        moveDirection.x = Input.GetAxis("LJHor");
        moveDirection.z = -Input.GetAxis("LJVer");

        if (moveDirection.x < 0)
            moveDirection.x = -1f;
        else if (moveDirection.x > 0)
            moveDirection.x = 1f;

        if (moveDirection.z < 0)
            moveDirection.z = -1f;
        else if (moveDirection.z > 0)
            moveDirection.z = 1f;

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
        if (toFrogPressed())
            genAbiRequest.Invoke(abilties.toFrog);
        if (toArmaPressed())
            genAbiRequest.Invoke(abilties.toArma);
        if (toCranePressed())
            genAbiRequest.Invoke(abilties.toCrane);
        if (toDolpPressed())
            genAbiRequest.Invoke(abilties.toDolp);
        if (toStdPressed())
            genAbiRequest.Invoke(abilties.toStd);
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

    private bool frogJumpInput()
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

    private bool dolphJumpInput()
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

    private bool toFrogbyCraneInput()
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

    private bool toFrogbyArmaInput()
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

    private bool toFrogbyDolpInput()
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

    private bool toArmabyCraneInput()
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

    private bool toArmabyFrogInput()
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

    private bool toArmabyDolpInput()
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

    private bool toCranebyFrogInput()
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

    private bool toCranebyArmaInput()
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

    private bool toCranebyDolpInput()
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

    private bool toDolpbyCraneInput()
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

    private bool toDolpbyArmaInput()
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

    private bool toDolpbyFrogInput()
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

    private bool toStdbyCraneInput()
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

    private bool toStdbyArmaInput()
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

    private bool toStdbyDolpInput()
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
    #endregion

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
                break;
            case "Frog Form":
                cForm = PlayerInputs.currentForm.frog;
                break;
            case "Armadillo Form":
                cForm = PlayerInputs.currentForm.arma;
                break;
            case "Dragon Form":
                cForm = PlayerInputs.currentForm.crane;
                break;
            case "Dolphin Form":
                cForm = PlayerInputs.currentForm.dolphin;
                break;
        }
    }

    #endregion











}
