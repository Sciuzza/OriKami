using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class PlayerInputs : MonoBehaviour
{

    public inputSettings currentInputs;

    private float curDirZ = 0.0f, curDirX = 0.0f;
    private Vector3 forward, right, moveDirection;

    
	[System.Serializable]
	public class abiInput : UnityEvent<abilties, Vector3>
	{
	}

	public abiInput abiRequest;



    private void Update(){
        
        MovingInputHandler();
        SpecialMoves();
    }

    private void MovingInputHandler()
    {


        curDirZ = -Input.GetAxis("LJVer");
        curDirX = Input.GetAxis("LJHor");


        forward = Camera.main.transform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;
        right = new Vector3(forward.z, 0, -forward.x);

        moveDirection = (curDirX * right + curDirZ * forward).normalized;


		abiRequest.Invoke(abilties.move,moveDirection);


    }

    private void SpecialMoves()
    {

        if (jumpPressed())
			abiRequest.Invoke(abilties.jump, moveDirection);
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
