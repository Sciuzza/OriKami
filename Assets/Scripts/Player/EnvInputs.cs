using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class EnvInputs : MonoBehaviour {


	CharacterController ccLink;

	bool onWater = false;


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

        
     if ((ccLink.collisionFlags & CollisionFlags.Below) != 0)
        {
            if (!onWater)
                psChanged.Invoke(physicStates.onGround);
            else
                psChanged.Invoke(physicStates.onWater);
        }
     else
            psChanged.Invoke(physicStates.onAir);




    }


    IEnumerator DebugGrounded()
    {
        while (true)
        {
            Debug.Log(ccLink.collisionFlags == CollisionFlags.None);
            yield return new WaitForSeconds(2);
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
