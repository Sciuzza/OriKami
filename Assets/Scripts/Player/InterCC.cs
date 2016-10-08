﻿using UnityEngine;
using System.Collections;





public class InterCC : MonoBehaviour
{




    private PlCore coreLink;
    private CharacterController ccLink;

    private void Awake()
    {
        coreLink = this.GetComponent<PlCore>();
        ccLink = this.GetComponent<CharacterController>();
    }
   
    private void Update()
    {

        if (!coreLink.vFissureAbilityisOn)
        {
            KMInputsManager();
            JoyInputManager();
        }
        else if (coreLink.vFissureAbilityisOn)
        {
            VFissureAbility();
        }




    }


    private void KMInputsManager()
    {
        if (Input.GetKeyDown("1") && coreLink.currentForm != forms.standard)
        {
            coreLink.standard.SetActive(true);
            GameObject.FindGameObjectWithTag(coreLink.currentActForm).SetActive(false);
            coreLink.currentActForm = "Standard Form";
            coreLink.currentForm = forms.standard;

        }
        else if (Input.GetKeyDown("2") && coreLink.currentForm != forms.frog)
        {
            coreLink.frog.SetActive(true);
            GameObject.FindGameObjectWithTag(coreLink.currentActForm).SetActive(false);
            coreLink.currentActForm = "Frog Form";
            coreLink.currentForm = forms.frog;

        }
        else if (Input.GetKeyDown("3") && coreLink.currentForm != forms.crane && !ccLink.isGrounded)
        {
            coreLink.dragon.SetActive(true);
            GameObject.FindGameObjectWithTag(coreLink.currentActForm).SetActive(false);
            coreLink.currentActForm = "Dragon Form";
            coreLink.currentForm = forms.crane;

        }
        else if (Input.GetKeyDown("4") && coreLink.currentForm != forms.armadillo)
        {
            coreLink.armadillo.SetActive(true);
            GameObject.FindGameObjectWithTag(coreLink.currentActForm).SetActive(false);
            coreLink.currentActForm = "Armadillo Form";
            coreLink.currentForm = forms.armadillo;

        }
    }

    private void JoyInputManager()
    {
        if (StandardFormJoy())
        {
            coreLink.standard.SetActive(true);
            GameObject.FindGameObjectWithTag(coreLink.currentActForm).SetActive(false);
            coreLink.currentActForm = "Standard Form";
            coreLink.currentForm = forms.standard;

        }
        else if (Input.GetAxis("LRTButton") > 0 && coreLink.currentForm != forms.frog)
        {
            coreLink.frog.SetActive(true);
            GameObject.FindGameObjectWithTag(coreLink.currentActForm).SetActive(false);
            coreLink.currentActForm = "Frog Form";
            coreLink.currentForm = forms.frog;

        }
        else if (Input.GetAxis("LRTButton") < 0 && coreLink.currentForm != forms.crane && !ccLink.isGrounded)
        {
            coreLink.dragon.SetActive(true);
            GameObject.FindGameObjectWithTag(coreLink.currentActForm).SetActive(false);
            coreLink.currentActForm = "Dragon Form";
            coreLink.currentForm = forms.crane;

        }
        else if (Input.GetButtonDown("LBButton") && coreLink.currentForm != forms.armadillo)
        {
            coreLink.armadillo.SetActive(true);
            GameObject.FindGameObjectWithTag(coreLink.currentActForm).SetActive(false);
            coreLink.currentActForm = "Armadillo Form";
            coreLink.currentForm = forms.armadillo;

        }
        else if (Input.GetButtonDown("RBButton") && coreLink.currentForm != forms.dolphin && coreLink.dolphinInAbility)
        {
            coreLink.dolphin.SetActive(true);
            GameObject.FindGameObjectWithTag(coreLink.currentActForm).SetActive(false);
            coreLink.currentActForm = "Dolphin Form";
            coreLink.currentForm = forms.dolphin;
            coreLink.stMoveEnabled = false;
           

        }



    }

    private bool StandardFormJoy()
    {
        if ((Input.GetAxis("LRTButton") > 0 && coreLink.currentForm == forms.frog) || (Input.GetAxis("LRTButton") < 0 && coreLink.currentForm == forms.crane && !ccLink.isGrounded)
            || (Input.GetButtonDown("LBButton") && coreLink.currentForm == forms.armadillo))
            return true;
        else
            return false;
    }

    private void VFissureAbility()
    {
        if (!coreLink.vFissureAbilityisOn)
        {
            KMInputsManager();
            JoyInputManager();
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

                if (distance.sqrMagnitude >= 0.625f && !coreLink.secondMoveIsOn)
                {

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

                        if (distance.sqrMagnitude >= 0.625f)
                        {

                            Vector3 direction = (coreLink.vGuidanceFinPosition - this.transform.position).normalized;
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

  
}
