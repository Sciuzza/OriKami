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

        StartCoroutine(Moving());
    }
	
	private void HandlingMove(Vector3 inputDir, float moveSpeed){

	    currentDir = inputDir;
        currentDir *= moveSpeed;
     
    }



    IEnumerator Moving(){

		while (true) {

			ccLink.Move (currentDir * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
		}

	}
}
