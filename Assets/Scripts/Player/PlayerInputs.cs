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

        Debug.LogWarning("Something Strange with Jump Input");
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
    #endregion

    #region General Methods

    private IEnumerator SwitchingCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        switchCooldown = false;
    } 
    #endregion











}
