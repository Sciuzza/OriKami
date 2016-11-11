using UnityEngine;
using System.Collections;

public class MoveHandler : MonoBehaviour {


	Vector3 currentDir = new Vector3(0,0,0);
	Vector3 finalMove = new Vector3 (0, 0, 0);
	float fallingStrength = 0.0f;
	CharacterController ccLink;
	bool inAir = false;


	// Use this for initialization
	void Awake () {
	
		FSMExecutor fsmExecTempLink = this.gameObject.GetComponent<FSMExecutor> ();

		fsmExecTempLink.moveSelected.AddListener (HandlingMove);
		fsmExecTempLink.rotSelected.AddListener (HandlingRot);
		fsmExecTempLink.jumpSelected.AddListener (HandlingJump);

		ccLink = this.gameObject.GetComponent<CharacterController> ();

	}

	void Start(){

        StartCoroutine(Moving());
    }
	
	private void HandlingMove(Vector3 inputDir, float moveSpeed){

		if (!inAir) {
			currentDir = inputDir;
			finalMove = currentDir * moveSpeed;
		} 
		else {
			currentDir = new Vector3 (inputDir.x, currentDir.y, inputDir.z);
			finalMove = currentDir * fallingStrength;
		}
    }

	private void HandlingRot(Vector3 inputDir, float rotSpeed){

		Quaternion rotation = Quaternion.LookRotation(inputDir, Vector3.up);
		this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, Time.deltaTime * rotSpeed);
	

	}

	private void HandlingJump(Vector3 inputDir, float jumpStrength){

		if (currentDir.sqrMagnitude == 0)
			currentDir = this.transform.up;
		else
			currentDir = this.transform.up + this.transform.forward;
		
		finalMove = currentDir + jumpStrength;
		inAir = true;
		StartCoroutine (Falling ());
	}

    IEnumerator Moving(){

		while (true) {
			ccLink.Move (finalMove * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
		}

	}

	IEnumerator Falling(){

		while (inAir) {
			finalMove.y -= Time.deltaTime;
			currentDir = finalMove.normalized;
			fallingStrength = finalMove.Magnitude;
			yield return new WaitForSeconds (Time.deltaTime);
		}
	}
}
