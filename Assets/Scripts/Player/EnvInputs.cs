﻿using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class EnvInputs : MonoBehaviour
{

    #region Event Classes
    [System.Serializable]
    public class physicChangeRequest : UnityEvent<physicStates>
    {
    }

    [System.Serializable]
    public class vFissureAbility : UnityEvent<VFissure, string>
    {
    }
    #endregion

    #region Events
    public vFissureAbility vFissureRequestOn;
    public physicChangeRequest psChanged;
    public UnityEvent vFissureRequestOff, playerIsDead;
    #endregion

    #region Private Variables
    private CharacterController ccLink;
    private bool onWaterFlag = false, onAir = false, onWater = false, onGround = false;
    #endregion

    #region Taking References
    void Awake()
    {
        ccLink = this.GetComponent<CharacterController>();
    }
    #endregion

    #region Physic State Inputs Handler
    void Update()
    {
        PhysicStateInput();
    }

    private void PhysicStateInput()
    {
        if ((ccLink.collisionFlags & CollisionFlags.Below) != 0)
        {
            if (onAir)
            {

                if (!onWaterFlag && !onWater)
                {
                    Debug.Log("Terra");
                    psChanged.Invoke(physicStates.onGround);
                    onWater = false;
                    onGround = true;
                }
                else if (onWaterFlag && !onWater)
                {
                    Debug.Log("Acqua");
                    psChanged.Invoke(physicStates.onWater);
                    onWater = true;
                    onGround = false;
                }

                onAir = false;
            }
            else
            {
                if (!onWaterFlag && onWater && !onGround)
                {
                    Debug.Log("Terra");
                    psChanged.Invoke(physicStates.onGround);
                    onWater = false;
                    onGround = true;

                }
                else if (onWaterFlag && !onWater && onGround)
                {
                    Debug.Log("Acqua");
                    psChanged.Invoke(physicStates.onWater);
                    onWater = true;
                    onGround = false;
                }
            }

        }
        else if ((ccLink.collisionFlags & CollisionFlags.Below) == 0 && !onAir)
        {
            Debug.LogWarning("Aria");
            psChanged.Invoke(physicStates.onAir);

            onWater = false;
            onGround = false;
            onAir = true;
        }
    }
    #endregion

    #region Triggers Handler
    void OnTriggerEnter(Collider envTrigger)
    {

        switch (envTrigger.gameObject.tag)
        {

            case "vAbilityta":
            case "vAbilitytb":
                vFissureRequestOn.Invoke(envTrigger.gameObject.GetComponentInParent<VFissure>(), envTrigger.gameObject.tag);
                break;
            case "hAbilityta":
            case "hAbilitytb":
                vFissureRequestOn.Invoke(envTrigger.gameObject.GetComponentInParent<VFissure>(), envTrigger.gameObject.tag);
                break;
            case "dAbilityta":
            case "dAbilitytb":
                vFissureRequestOn.Invoke(envTrigger.gameObject.GetComponentInParent<VFissure>(), envTrigger.gameObject.tag);
                break;
            case "Water":
                onWaterFlag = true;
                break;
            case "Death":
                playerIsDead.Invoke();
                break;

        }

    }

    void OnTriggerExit(Collider envTrigger)
    {


        switch (envTrigger.gameObject.tag)
        {

            case "vAbilityta":
            case "vAbilitytb":
                vFissureRequestOff.Invoke();
                break;
            case "hAbilityta":
            case "hAbilitytb":
                vFissureRequestOff.Invoke();
                break;
            case "dAbilityta":
            case "dAbilitytb":
                vFissureRequestOff.Invoke();
                break;
            case "Water":
                onWaterFlag = false;
                break;
        }
    } 
    #endregion

}
