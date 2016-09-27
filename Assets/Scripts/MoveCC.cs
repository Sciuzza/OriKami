using UnityEngine;
using System.Collections;






public class MoveCC : MonoBehaviour
{


    private CharacterController ccLink;
    private InterCC isLink;


    private bool isGrounded = false;
    private bool isJumping = false;
    private bool isRolling = false;
    private bool isFlying = false;

    private float rollingTime = 0.0f;




    private Vector3 jumpDirection, glideDirection;




    // Use this for initialization
    void Awake()
    {

        ccLink = GetComponent<CharacterController>();
        isLink = GameObject.FindGameObjectWithTag("Player").GetComponent<InterCC>();

    }

    // Update is called once per frame
    void Update()
    {

        Physics.gravity = DesignerT.instance.GeneralTweaks.globalGravity * Vector3.down;

        transform.Rotate(0, Input.GetAxis("Horizontal") * DesignerT.instance.GeneralTweaks.rotateSpeed, 0);
        Vector3 forward = transform.TransformDirection(Vector3.forward);

        if (isLink.currentForm == forms.standard && !isJumping && !isFlying)
        {
            float curSpeed = DesignerT.instance.GestioneMovimento.standMove.moveSpeed * Input.GetAxis("Vertical");
            ccLink.SimpleMove(forward * curSpeed);
        }
        else if (isLink.currentForm == forms.frog && !isJumping && !isFlying)
        {
            float curSpeed = DesignerT.instance.GestioneMovimento.frogMove.moveSpeed * Input.GetAxis("Vertical");
            ccLink.SimpleMove(forward * curSpeed);
        }
        else if (isLink.currentForm == forms.armadillo && !isJumping && !isFlying && !isRolling)
        {
            float curSpeed = DesignerT.instance.GestioneMovimento.armaMove.moveSpeed * Input.GetAxis("Vertical");
            ccLink.SimpleMove(forward * curSpeed);
        }
        else if (isLink.currentForm == forms.dragon)
        {
            glideDirection = this.transform.forward * DesignerT.instance.GestioneMovimento.craneMove.glideSpeed;
            glideDirection.y -= DesignerT.instance.GeneralTweaks.glideGravity * Time.deltaTime;
            ccLink.Move(glideDirection * Time.deltaTime);

        }

        if (ccLink.isGrounded)
            isGrounded = true;
        else
            isGrounded = false;

        if (ccLink.isGrounded && Input.GetButtonDown("Jump"))
        {
            if (isLink.currentForm == forms.frog)
            {
                jumpDirection = (this.transform.up + this.transform.forward) * DesignerT.instance.GestioneMovimento.frogMove.jumpStrength;
                isJumping = true;
            }
            else if (isLink.currentForm == forms.standard)
            {
                jumpDirection = (this.transform.up + this.transform.forward) * DesignerT.instance.GestioneMovimento.standMove.jumpStrength;
                isJumping = true;
            }
            else if (isLink.currentForm == forms.armadillo)
            {
                isRolling = true;
            }
        }

        if (isJumping)
        {
            jumpDirection.y -= DesignerT.instance.GeneralTweaks.jumpGravity * Time.deltaTime;
            ccLink.Move(jumpDirection * Time.deltaTime);

            if (ccLink.isGrounded)
                isJumping = false;
        }

        if (isRolling)
        {
            rollingTime += Time.deltaTime;
            ccLink.SimpleMove(this.transform.forward * DesignerT.instance.GestioneMovimento.armaMove.rollingStrength);

            if (rollingTime >= DesignerT.instance.GestioneMovimento.armaMove.rollingTime)
            {
                rollingTime = 0;
                isRolling = false;
            }

        }

        if (!ccLink.isGrounded && isLink.currentForm == forms.dragon)
        {
            if (isJumping)
                isJumping = false;
            if (!isFlying)
                isFlying = true;

        }

        if (ccLink.isGrounded && isLink.currentForm == forms.dragon)
        {
            isFlying = false;
            isLink.SwitchToStandard();
        }

    }





}
