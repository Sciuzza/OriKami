using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class PlayerInputs : MonoBehaviour
{

    public inputSettings currentInputs;

    private float curDirZ = 0.0f, curDirX = 0.0f;
    private Vector3 forward, right, moveDirection;



    [System.Serializable]
    public class moveEvent : UnityEvent<Vector3>
    {
    }

    [HideInInspector]
    public moveEvent movingRequest;


    public UnityEvent jumpRequest;


    private void MovingInputHandler()
    {


        curDirZ = -Input.GetAxis("LJVer");
        curDirX = Input.GetAxis("LJHor");


        forward = Camera.main.transform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;
        right = new Vector3(forward.z, 0, -forward.x);

        moveDirection = (curDirX * right + curDirZ * forward).normalized;


        movingRequest.Invoke(moveDirection);


    }

    private void SpecialMoves()
    {

        if (jumpPressed())
           jumpRequest.Invoke();
    }

    private bool jumpPressed()
    {
        if (
               Input.GetButtonDown(currentInputs.standardInputs.joyInputs.Jump)
            || Input.GetButtonDown(currentInputs.frogInputs.joyInputs.Jump)
            || Input.GetButtonDown(currentInputs.standardInputs.keyInputs.Jump)
            || Input.GetButtonDown(currentInputs.frogInputs.keyInputs.Jump)
            )
        {
            Debug.Log("Jump Pressed");
            return true;
        }
        else
            return false;
    }
}
