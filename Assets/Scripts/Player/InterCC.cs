/*
using UnityEngine;
using System.Collections;
using UnityEngine.Events;




public class InterCC : MonoBehaviour
{

    

     

    private PlCore coreLink;
    private CharacterController ccLink;
    private bool switchCooldown = false;


    public UnityEvent standardForm, frogForm, craneForm, armaForm, dolphinForm;

    private void Awake()
    {
        coreLink = this.GetComponent<PlCore>();
        ccLink = this.GetComponent<CharacterController>();

      
    }
   
    private void Update()
    {

        if (!coreLink.vFissureAbilityisOn)
        {
          
            InputManager();
        }
        else if (coreLink.vFissureAbilityisOn)
        {
            VFissureAbility();
        }


    }


   

    private void InputManager()
    {

        if ((Input.GetAxis("LRTButton") > 0 || Input.GetKeyDown("1")) && !switchCooldown)
        {
            switchCooldown = true;
            frogForm.Invoke();
            StartCoroutine(SwitchingCooldown());
        }

        else if ((Input.GetAxis("LRTButton") < 0 || Input.GetKeyDown("2")) && !switchCooldown) 
        {
            switchCooldown = true;
            craneForm.Invoke();
            StartCoroutine(SwitchingCooldown());
        }

        else if ((Input.GetButtonDown("LBButton") || Input.GetKeyDown("3")) && !switchCooldown)
        {
            switchCooldown = true;
            armaForm.Invoke();
            StartCoroutine(SwitchingCooldown());
        }

        else if ((Input.GetButtonDown("RBButton") || Input.GetKeyDown("4")) && !switchCooldown)
        {
            switchCooldown = true;
            dolphinForm.Invoke();
            StartCoroutine(SwitchingCooldown());
        }

    }

 

    private void VFissureAbility()
    {
        if (!coreLink.vFissureAbilityisOn)
        {
            
            InputManager();
        }
        else if (coreLink.vFissureAbilityisOn)
        {
            if (Quaternion.Angle(this.transform.rotation, coreLink.vTriggerRotation) > 0.1f && !coreLink.secondRotationisOn)
            {

                this.transform.localRotation = Quaternion.Slerp(this.transform.rotation, coreLink.vTriggerRotation, Time.deltaTime * coreLink.GeneralValues.rotateSpeed);
                //Debug.Log(Quaternion.Angle(this.transform.rotation, coreLink.vTriggerRotation));
            }
            else if (coreLink.moveFinished)
            {
                coreLink.vFissureAbilityisOn = false;
                coreLink.moveFinished = false;
                coreLink.secondMoveIsOn = false;
            }
            else
            {



                Vector3 distance = coreLink.vTriggerMidPosition - this.transform.position;

                if (distance.sqrMagnitude >= 0.71f && !coreLink.secondMoveIsOn)
                {
                    //Debug.Log(distance.sqrMagnitude);
                    Vector3 direction = (coreLink.vTriggerMidPosition - this.transform.position).normalized;
                    direction.y = 0;
                    this.transform.position += direction * Time.deltaTime * 3;
                }
                else
                {
                    coreLink.secondRotationisOn = true;

                    if (Quaternion.Angle(this.transform.rotation, coreLink.vGuidanceRotation) > 0.1f)

                        this.transform.rotation = Quaternion.Slerp(this.transform.localRotation, coreLink.vGuidanceRotation, Time.deltaTime * coreLink.GeneralValues.rotateSpeed);
                    else
                    {
                        coreLink.secondMoveIsOn = true;

                        distance = coreLink.vGuidanceFinPosition - this.transform.position;

                        if (distance.sqrMagnitude >= 0.8f)
                        {

                            // Vector3 direction = (coreLink.vGuidanceFinPosition - this.transform.position).normalized;
                            Vector3 direction = coreLink.vGuidanceDir.normalized;
                            direction.y = 0;
                            this.transform.position += direction * Time.deltaTime * 3;
                        }
                        else
                        {
                            coreLink.moveFinished = true;
                            coreLink.secondRotationisOn = false;
                        }

                    }
                }


            }
        }

    }

  
    private IEnumerator SwitchingCooldown()
    {
        yield return new WaitForSeconds(1f);
        switchCooldown = false;
    }

   
    

}
*/