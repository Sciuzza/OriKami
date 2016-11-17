using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class EnvInputs : MonoBehaviour {


	CharacterController ccLink;

	bool onWater = false, onAir = false;


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

    public UnityEvent vFissureRequestOff;


    void Awake(){

		ccLink = this.GetComponent<CharacterController> ();
        //StartCoroutine(DebugGrounded());
    }


	void Update(){


        if ((ccLink.collisionFlags & CollisionFlags.Below) != 0  && onAir)
        {
            Debug.Log("Terra");
            if (!onWater)
                psChanged.Invoke(physicStates.onGround);
            else
                psChanged.Invoke(physicStates.onWater);

            onAir = false;
        }
        else if ((ccLink.collisionFlags & CollisionFlags.Below) == 0 && !onAir) {
            Debug.LogWarning("Aria");
            psChanged.Invoke(physicStates.onAir);

            onAir = true;
        }

        

    }


   

    
	void OnTriggerEnter (Collider envTrigger){

		switch (envTrigger.gameObject.tag) {

		case "vAbilityta":
        case "vAbilitytb":
                vFissureRequestOn.Invoke(envTrigger.gameObject.GetComponentInParent<VFissure>(), envTrigger.gameObject.tag);
			break;
		


		}

	}

 

	void OnTriggerExit (Collider envTrigger){


		switch (envTrigger.gameObject.tag) {

            case "vAbilityta":
            case "vAbilitytb":
                vFissureRequestOff.Invoke();
                break;


        }
	}
    
}
