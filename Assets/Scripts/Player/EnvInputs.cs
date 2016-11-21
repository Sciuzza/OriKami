using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class EnvInputs : MonoBehaviour
{


    CharacterController ccLink;

    bool onWaterFlag = false, onAir = false, onWater = false, onGround = false;


    [System.Serializable]
    public class physicChangeRequest : UnityEvent<physicStates>
    {
    }

    public physicChangeRequest psChanged;



    [System.Serializable]
    public class vFissureAbility : UnityEvent<VFissure, string>
    {
    }

    public vFissureAbility vFissureRequestOn;

    public UnityEvent vFissureRequestOff, playerIsDead;


    void Awake()
    {

        ccLink = this.GetComponent<CharacterController>();
        //StartCoroutine(DebugGrounded());
    }


    void Update()
    {


        PhysicStateInput();



    }





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
}
