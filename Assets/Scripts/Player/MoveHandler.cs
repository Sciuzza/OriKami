using UnityEngine;
using System.Collections;

public class MoveHandler : MonoBehaviour {


	Vector3 currentDir = new Vector3(0,0,0);
	CharacterController ccLink;


	// Use this for initialization
	void Awake () {
	
		FSMExecutor fsmExecTempLink = this.gameObject.GetComponent<FSMExecutor> ();

		fsmExecTempLink.moveSelected.AddListener (HandlingMove);

		ccLink = this.gameObject.GetComponent<CharacterController> ();

	}

	void Start(){

		StartCoroutine ();
	}
	
	private void HandlingMove(Vector3 inputDir){

	  currentDir = inputDir;

	}

	IEnumerator Moving(){

		while (currentDir.sqrMagnitude != 0) {

			ccLink.Move (currentDir * Time.deltaTime);
			yield return WaitForSeconds (Time.deltaTime);
		}

	}
}
