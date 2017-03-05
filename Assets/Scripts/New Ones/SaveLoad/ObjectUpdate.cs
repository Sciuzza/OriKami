using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;


public class ObjectUpdate : MonoBehaviour
{

    #region Edit Mode Methods
    /*
    public void OnValidate()
    {
        var tempList = GameObject.FindGameObjectWithTag("InObjRepo").GetComponent<InObjRepo>().ObjInactive;

        if (!this.gameObject.activeSelf && !tempList.Contains(this.gameObject.GetComponent<ObjectUpdate>()))
            GameObject.FindGameObjectWithTag("InObjRepo").GetComponent<InObjRepo>().ObjInactive.Add(this.gameObject.GetComponent<ObjectUpdate>());
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

    private void SavingCurrentState()
    {
        var currentSceneData =
            GameObject.FindGameObjectWithTag("GameController")
                .GetComponent<SuperDataManager>()
                .EnvSensData.Find(x => x.GpSceneName == SceneManager.GetActiveScene().name);

        Transform thisTrans = this.gameObject.transform;

        ObjectsData objToUpdate = currentSceneData.ObjState.Find(x => x.ObjName == this.gameObject.name);


        if (objToUpdate != null)
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
