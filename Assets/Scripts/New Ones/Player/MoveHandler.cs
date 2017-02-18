using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MoveHandler : MonoBehaviour
{
    #region Public Variables
    public float hitImpactVel;
    public float gravityStr;
    #endregion

    #region Private Variables
    private Vector3 finalMove = new Vector3(0, -1, 0), rollImpulse = new Vector3(0, 0, 0), rollDir;
    private float rollStrength;
    private bool rolling = false, gliding = false;
    private CharacterController ccLink;
    private float verticalVelocity = 0.0f;
    private CollisionFlags flags;
    private bool roofHit = false;

    private RaycastHit hitInfo;
    private Vector3 plCurrentPos;
    private Ray rayTest;
    public static bool sliding;
    public LayerMask Env;
    #endregion

    #region Events
    public UnityEvent deathRequest;
    public event_vector3 UpdatedFinalMoveRequest;
    #endregion

    #region Taking References and Linking Events
    void Awake()
    {

        FSMExecutor fsmExecTempLink = this.gameObject.GetComponent<FSMExecutor>();

        fsmExecTempLink.moveSelected.AddListener(HandlingMove);
        fsmExecTempLink.rotSelected.AddListener(HandlingRot);
        fsmExecTempLink.jumpSelected.AddListener(HandlingJump);
        fsmExecTempLink.rollSelected.AddListener(HandlingRoll);
        fsmExecTempLink.specialRotSelected.AddListener(HandlingSpecialRot);


        ccLink = this.gameObject.GetComponent<CharacterController>();

        FSMChecker fsmCheckTempLink = this.GetComponent<FSMChecker>();

        fsmCheckTempLink.stoppingRollLogic.AddListener(StoppingRoll);
        fsmCheckTempLink.enableGlideLogic.AddListener(SettingGlideGravity);
        fsmCheckTempLink.stopGlideLogic.AddListener(SettingNormalGravity);
        fsmCheckTempLink.deathRequest.AddListener(this.ResettingVerticalVelocity);

        this.deathRequest.AddListener(this.ResettingVerticalVelocity);
    }
    #endregion

    private void ResettingVerticalVelocity()
    {
        this.verticalVelocity = 0;
    }

    #region Move and Rotation Handling Methods
    public IEnumerator MoveHandlerUpdate()
    {
        while (this.ccLink != null)
        {
            this.plCurrentPos = this.gameObject.transform.position;
            this.rayTest = new Ray(this.plCurrentPos, -Vector3.up);

            var timeTakeThisFrame = Time.deltaTime;

            if (this.rolling)
            {
                this.finalMove = Vector3.Slerp(this.finalMove.normalized, this.rollDir.normalized, timeTakeThisFrame);

                float height = this.finalMove.y;

                this.finalMove.y = 0;

                if ((this.ccLink.collisionFlags & CollisionFlags.Below) != 0)
                    this.transform.rotation = Quaternion.LookRotation(this.finalMove, Vector3.up);

                this.finalMove.y = height;

                this.finalMove *= this.rollStrength;


            }

            this.finalMove *= timeTakeThisFrame;

            if (!this.gliding) this.verticalVelocity += this.gravityStr * timeTakeThisFrame;
            else this.verticalVelocity = -1;

            this.finalMove.y = this.verticalVelocity * timeTakeThisFrame;

            this.flags = this.ccLink.Move(this.finalMove);

            if ((this.flags & CollisionFlags.Below) != 0)
            {
                if (Physics.SphereCast(this.rayTest, 0.1f, out this.hitInfo, 100, this.Env.value))
                {

                    Debug.Log(Vector3.Dot(this.hitInfo.normal, Vector3.up));
                    Debug.Log(this.hitInfo.transform.name);

                    if (Vector3.Dot(this.hitInfo.normal, Vector3.up) >= 0 && Vector3.Dot(this.hitInfo.normal, Vector3.up) < 0.75f)
                    {
                        sliding = true;
                        //this.finalMove = -this.gameObject.transform.forward * 3;
                        var virtualGb = new GameObject();
                        virtualGb.transform.position = this.plCurrentPos;
                        virtualGb.transform.forward = -this.hitInfo.normal;
                        this.finalMove = -virtualGb.transform.up * 3;

                        Destroy(virtualGb);
                    }
                    else
                    {
                        sliding = false;
                    }
                }
                else
                {
                    sliding = false;
                }


                if (this.verticalVelocity <= -this.hitImpactVel) this.deathRequest.Invoke();
                else
                {
                    this.verticalVelocity = -3f;
                    this.finalMove.y = 0;
                }

                if (this.roofHit) this.roofHit = false;

            }
            else if ((this.flags & CollisionFlags.Above) != 0 && !this.roofHit)
            {
                this.roofHit = true;
                this.verticalVelocity = 0;
            }

            this.UpdatedFinalMoveRequest.Invoke(this.finalMove);

            yield return null;
        }
    }


    private void StoppingCoroutines()
    {
        this.StopCoroutine(this.MoveHandlerUpdate());
    }

    private void HandlingMove(Vector3 inputDir, float moveSpeed)
    {
        if (!sliding) this.finalMove = inputDir * moveSpeed;
    }

    private void HandlingRot(Vector3 inputDir, float rotSpeed)
    {
        if (inputDir.sqrMagnitude != 0)
        {
            Quaternion rotation = Quaternion.LookRotation(inputDir, Vector3.up);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, Time.deltaTime * rotSpeed);
        }
    }

    private void HandlingSpecialRot(Vector3 inputDir, float rotSpeed)
    {
        rollDir = inputDir;
    }

    private void HandlingJump(float jumpStrength)
    {
        verticalVelocity = jumpStrength;
    }

    private void HandlingRoll(float rollStr)
    {
        rollImpulse = this.transform.forward;
        rollDir = this.transform.forward;
        rollStrength = rollStr;
        finalMove = rollImpulse;
        rolling = true;
    }

    private void StoppingRoll()
    {
        rolling = false;
    }

    private void SettingGlideGravity()
    {
        gliding = true;
    }

    private void SettingNormalGravity()
    {
        gliding = false;
    }
    #endregion
}
