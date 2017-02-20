using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;

public class KoiSpecialUpdate : MonoBehaviour
{


    private Transform KoiTransf;

    #region Edit Mode Methods

    public void OnValidate()
    {
        //GameObject.FindGameObjectWithTag("GameController").GetComponent<SuperDataManager>().UpdatingObjState(this.gameObject);
    }
    #endregion

    private void Awake()
    {
        var sdmTempLink = GameObject.FindGameObjectWithTag("GameController").GetComponent<SuperDataManager>();

        sdmTempLink.RequestLocalUpdateToRepo.AddListener(this.SavingCurrentState);
        sdmTempLink.RequestLocalUpdateByRepo.AddListener(this.LoadingCurrentState);

        var slTempLink = GameObject.FindGameObjectWithTag("StoryLine").GetComponent<StoryLineInstance>();

        slTempLink.KoiNewTransfRequest.AddListener(this.UpdatingLocalTransform);
    }

    private void UpdatingLocalTransform(Transform newTransf)
    {
        this.KoiTransf = newTransf;
    }

    private void SavingCurrentState()
    {
        var currentSceneData =
            GameObject.FindGameObjectWithTag("GameController")
                .GetComponent<SuperDataManager>()
                .EnvSensData.Find(x => x.GpSceneName == SceneManager.GetActiveScene().name);

        Transform thisTrans = this.gameObject.transform;

        ObjectsData objToUpdate = currentSceneData.ObjState.Find(x => x.ObjName == this.gameObject.name);


        if (objToUpdate != null && this.KoiTransf != null)
        {
            objToUpdate.ObjPosX = this.KoiTransf.position.x;
            objToUpdate.ObjPosY = this.KoiTransf.position.y;
            objToUpdate.ObjPosZ = this.KoiTransf.position.z;

            objToUpdate.ObjRotX = this.KoiTransf.eulerAngles.x;
            objToUpdate.ObjRotY = this.KoiTransf.eulerAngles.y;
            objToUpdate.ObjRotZ = this.KoiTransf.eulerAngles.z;

            objToUpdate.IsActive = this.gameObject.activeInHierarchy;

            this.KoiTransf = null;
        }
        else if (objToUpdate != null && this.KoiTransf == null)
        {
            objToUpdate.ObjPosX = thisTrans.position.x;
            objToUpdate.ObjPosY = thisTrans.position.y;
            objToUpdate.ObjPosZ = thisTrans.position.z;

            objToUpdate.ObjRotX = thisTrans.eulerAngles.x;
            objToUpdate.ObjRotY = thisTrans.eulerAngles.y;
            objToUpdate.ObjRotZ = thisTrans.eulerAngles.z;

            objToUpdate.IsActive = this.gameObject.activeInHierarchy;
        }
        else
        {
            Debug.Log(this.gameObject.name + " not present in Repo");
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
            Debug.Log(this.gameObject.name + " not present in Repo");
        }
    }
}
