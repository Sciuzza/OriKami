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
	}


	void Update(){

		if (ccLink.isGrounded)
			psChanged.Invoke (physicStates.onGround);
		else
			psChanged.Invoke (physicStates.onAir);


	}



	void OnTriggerEnter (Collider envTrigger){

		switch (envTrigger.gameObject.tag) {

		case "Water":
			break;
		case "Camera Constraint":
			break;
		case "": 
			break;
		case "": 
			break;
		case "": 
			break;
		case "": 
			break;
		case "": 
			break;
		case "": 
			break;


		}

	}


	void OnTriggerExit (Collider envTrigger){


		switch (envTrigger) {

		case "Water":
			break;
		case "Camera Constraint":
			break;
		case "": 
			break;
		case "": 
			break;
		case "": 
			break;
		case "": 
			break;
		case "": 
			break;
		case "": 
			break;


		}
	}

}
