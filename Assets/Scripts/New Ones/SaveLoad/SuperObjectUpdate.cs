using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//using UnityEditor;

using UnityEngine.SceneManagement;

public class SuperObjectUpdate : MonoBehaviour
{
    private Transform memoryTarget = null;


    #region Edit Mode Methods
    public void OnValidateCustom()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<SuperDataManager>().UpdatingObjState(this.gameObject);
    }
    
    #endregion

    private void Awake()
    {
        var sdmTempLink = GameObject.FindGameObjectWithTag("GameController").GetComponent<SuperDataManager>();

        sdmTempLink.RequestLocalUpdateToRepo.AddListener(this.SavingCurrentState);
        sdmTempLink.RequestLocalUpdateByRepo.AddListener(this.LoadingCurrentState);
    }


    public void CustomAwake()
    {
        var sdmTempLink = GameObject.FindGameObjectWithTag("GameController").GetComponent<SuperDataManager>();

        sdmTempLink.RequestLocalUpdateToRepo.AddListener(this.SavingCurrentState);
        sdmTempLink.RequestLocalUpdateByRepo.AddListener(this.LoadingCurrentState);
    }

    public void UpdatingLocalTransform(Transform newTransf)
    {
        this.memoryTarget = newTransf;
    }

    private void SavingCurrentState()
    {
        var currentSceneData =
            GameObject.FindGameObjectWithTag("GameController")
                .GetComponent<SuperDataManager>()
                .EnvSensData.Find(x => x.GpSceneName == SceneManager.GetActiveScene().name);

        Transform thisTrans = this.gameObject.transform;

        ObjectsData objToUpdate = currentSceneData.ObjState.Find(x => x.ObjName == this.gameObject.name);


        if (objToUpdate != null && this.memoryTarget != null)
        {
            objToUpdate.ObjPosX = this.memoryTarget.position.x;
            objToUpdate.ObjPosY = this.memoryTarget.position.y;
            objToUpdate.ObjPosZ = this.memoryTarget.position.z;

            objToUpdate.ObjRotX = this.memoryTarget.eulerAngles.x;
            objToUpdate.ObjRotY = this.memoryTarget.eulerAngles.y;
            objToUpdate.ObjRotZ = this.memoryTarget.eulerAngles.z;

            objToUpdate.IsActive = this.gameObject.activeSelf;
            Debug.Log("Saved with memory" + " " + objToUpdate.ObjName + " " + objToUpdate.ObjPosX + objToUpdate.ObjPosY + objToUpdate.ObjPosZ);
            this.memoryTarget = null;
        }
        else if (objToUpdate != null && this.memoryTarget == null)
        {
            objToUpdate.ObjPosX = thisTrans.position.x;
            objToUpdate.ObjPosY = thisTrans.position.y;
            objToUpdate.ObjPosZ = thisTrans.position.z;

            objToUpdate.ObjRotX = thisTrans.eulerAngles.x;
            objToUpdate.ObjRotY = thisTrans.eulerAngles.y;
            objToUpdate.ObjRotZ = thisTrans.eulerAngles.z;

            objToUpdate.IsActive = this.gameObject.activeSelf;
            Debug.Log("Saved without memory" + " " + objToUpdate.ObjName + " " + objToUpdate.ObjPosX + objToUpdate.ObjPosY + objToUpdate.ObjPosZ);
        }
        else
        {
            Debug.Log(this.gameObject.name + " not present in Repo, saving problem");
        }
    }

    private void LoadingCurrentState()
    {
        var currentSceneData =
        GameObject.FindGameObjectWithTag("GameController")
        .GetComponent<SuperDataManager>()
        .EnvSensData.Find(x => x.GpSceneName == SceneManager.GetActiveScene().name);

        ObjectsData objToUpdate = currentSceneData.ObjState.Find(x => x.ObjName == this.gameObject.name);


        if (objToUpdate != null)
        {
            this.gameObject.transform.position = new Vector3(objToUpdate.ObjPosX, objToUpdate.ObjPosY, objToUpdate.ObjPosZ);
            this.gameObject.transform.rotation = Quaternion.Euler(objToUpdate.ObjRotX, objToUpdate.ObjRotY, objToUpdate.ObjRotZ);

            this.gameObject.SetActive(objToUpdate.IsActive);
        }
        else
        {
            Debug.Log(this.gameObject.name + " not present in Repo, loading problem");
        }
    }
}
