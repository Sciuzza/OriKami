using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine.SceneManagement;

public class SuperObjectUpdate : MonoBehaviour
{
    private Transform memoryTarget = null;


    #region Edit Mode Methods
    /*

    public void OnValidate()
    {
        var tempList = GameObject.FindGameObjectWithTag("InObjRepo").GetComponent<InObjRepo>().SObjInactive;

        if (!this.gameObject.activeSelf && !tempList.Contains(this.gameObject.GetComponent<SuperObjectUpdate>()))
          GameObject.FindGameObjectWithTag("InObjRepo").GetComponent<InObjRepo>().SObjInactive.Add(this.gameObject.GetComponent<SuperObjectUpdate>());
    }
    

    public void OnValidateCustom()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<SuperDataManager>().UpdatingObjState(this.gameObject);
    }
    */
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
            objToUpdate.ObjPosX = this.memoryTarget.localPosition.x;
            objToUpdate.ObjPosY = this.memoryTarget.localPosition.y;
            objToUpdate.ObjPosZ = this.memoryTarget.localPosition.z;

            objToUpdate.ObjRotX = this.memoryTarget.eulerAngles.x;
            objToUpdate.ObjRotY = this.memoryTarget.eulerAngles.y;
            objToUpdate.ObjRotZ = this.memoryTarget.eulerAngles.z;

            objToUpdate.IsActive = this.gameObject.activeSelf;

            this.memoryTarget = null;
        }
        else if (objToUpdate != null && this.memoryTarget == null)
        {
            objToUpdate.ObjPosX = thisTrans.localPosition.x;
            objToUpdate.ObjPosY = thisTrans.localPosition.y;
            objToUpdate.ObjPosZ = thisTrans.localPosition.z;

            objToUpdate.ObjRotX = thisTrans.eulerAngles.x;
            objToUpdate.ObjRotY = thisTrans.eulerAngles.y;
            objToUpdate.ObjRotZ = thisTrans.eulerAngles.z;

            objToUpdate.IsActive = this.gameObject.activeSelf;
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
            this.gameObject.transform.localPosition = new Vector3(objToUpdate.ObjPosX, objToUpdate.ObjPosY, objToUpdate.ObjPosZ);
            this.gameObject.transform.rotation = Quaternion.Euler(objToUpdate.ObjRotX, objToUpdate.ObjRotY, objToUpdate.ObjRotZ);

            this.gameObject.SetActive(objToUpdate.IsActive);
        }
        else
        {
            Debug.Log(this.gameObject.name + " not present in Repo, loading problem");
        }
    }
}
