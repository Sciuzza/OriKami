using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;

public class ButtonUpdateState : MonoBehaviour {

    #region Edit Mode Methods

    public void OnValidate()
    {
        //GameObject.FindGameObjectWithTag("GameController").GetComponent<SuperDataManager>().UpdatingButState(this.gameObject.GetComponent<Puzzles>());
    }
    #endregion

    private void Awake()
    {
        var sdmTempLink = GameObject.FindGameObjectWithTag("GameController").GetComponent<SuperDataManager>();

        sdmTempLink.RequestUpdateToSave.AddListener(this.SavingCurrentState);
        sdmTempLink.RequestUpdateByLoad.AddListener(this.LoadingCurrentState);
    }

    private void SavingCurrentState()
    {
        var currentSceneData =
            GameObject.FindGameObjectWithTag("GameController")
                .GetComponent<SuperDataManager>()
                .EnvSensData.Find(x => x.GpSceneName == SceneManager.GetActiveScene().name);

        var butToUpdate = currentSceneData.ButState.Find(x => x.ButtonName == this.gameObject.name);

        if (butToUpdate != null)
        butToUpdate.IsDisabled = this.gameObject.GetComponent<Puzzles>().keyHit;
        else
        {
            Debug.Log(this.gameObject.name + " not present in Temp Repo");
        }
    }

    private void LoadingCurrentState()
    {
        var currentSceneData =
    GameObject.FindGameObjectWithTag("GameController")
        .GetComponent<SuperDataManager>()
        .EnvSensData.Find(x => x.GpSceneName == SceneManager.GetActiveScene().name);

        var butToUpdate = currentSceneData.ButState.Find(x => x.ButtonName == this.gameObject.name);

        if (butToUpdate != null)
        this.gameObject.GetComponent<Puzzles>().keyHit = butToUpdate.IsDisabled;
        else
        {
            Debug.Log(this.gameObject.name + " not present in Temp Repo");
        }
    }
}
