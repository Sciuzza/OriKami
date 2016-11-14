using UnityEngine;
using System.Collections;

public class MoveHandler : MonoBehaviour
{


  
    
   
    CharacterController ccLink;

    private float verticalVelocity, gravityStr = 9.8f;

    // Use this for initialization
    void Awake()
    {

        FSMExecutor fsmExecTempLink = this.gameObject.GetComponent<FSMExecutor>();

        fsmExecTempLink.moveSelected.AddListener(HandlingMove);
        fsmExecTempLink.rotSelected.AddListener(HandlingRot);
        fsmExecTempLink.jumpSelected.AddListener(HandlingJump);
      

        ccLink = this.gameObject.GetComponent<CharacterController>();

    }

  

    private void HandlingMove(Vector3 inputDir, float moveSpeed)
    {

       

        inputDir *= moveSpeed * Time.deltaTime;

        verticalVelocity -= gravityStr * Time.deltaTime;

       

        inputDir.y = verticalVelocity * Time.deltaTime;

       




        CollisionFlags flags = ccLink.Move(inputDir);

        if ((flags & CollisionFlags.Below) != 0)
            verticalVelocity = -3f; 
       

    }

    private void HandlingRot(Vector3 inputDir, float rotSpeed)
    {

        if (inputDir.sqrMagnitude != 0)
        {
            Quaternion rotation = Quaternion.LookRotation(inputDir, Vector3.up);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, Time.deltaTime * rotSpeed);
        }

    }

    private void HandlingJump(float jumpStrength)
    {

        verticalVelocity += jumpStrength;


    }







   
}
