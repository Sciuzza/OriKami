using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.SceneManagement;

public class ButtonUpdateState : MonoBehaviour {

    #region Edit Mode Methods

    public void OnValidate()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<SuperDataManager>().UpdatingButState(this.gameObject);
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
        {

            var puzzleScripts = new List<Puzzles>();

            butToUpdate.IsDisabled.Clear();
            butToUpdate.IsDisabled.TrimExcess();

            puzzleScripts.AddRange(this.gameObject.GetComponents<Puzzles>());

            butToUpdate.IsDisabled = new List<bool>();

            foreach (var puzzle in puzzleScripts)
            {
                butToUpdate.IsDisabled.Add(puzzle.keyHit);
            }
        }
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
        {
            var puzzleScripts = this.gameObject.GetComponents<Puzzles>();

            for (var index = 0; index < butToUpdate.IsDisabled.Count; index++)
            {
                puzzleScripts[index].keyHit = butToUpdate.IsDisabled[index];
            }
        }
       
        else
        {
            Debug.Log(this.gameObject.name + " not present in Temp Repo");
        }
    }
}
