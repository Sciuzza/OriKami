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


   

    /*
	void OnTriggerEnter (Collider envTrigger){

		switch (envTrigger.gameObject.tag) {

		case "Water":
			break;
		case "Camera Constraint":
			break;
		case "a": 
			break;
		case "b": 
			break;
		case "c": 
			break;
		case "d": 
			break;
		case "e": 
			break;
		case "f": 
			break;


		}

	}


	void OnTriggerExit (Collider envTrigger){


		switch (envTrigger.gameObject.tag) {

		case "Water":
			break;
		case "Camera Constraint":
			break;
		case "a": 
			break;
		case "b": 
			break;
		case "c": 
			break;
		case "d": 
			break;
		case "e": 
			break;
		case "f": 
			break;


		}
	}
    */
}
