using System;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.Events;
using UnityEngine.SceneManagement;

#region Scene Based Data
[System.Serializable]
public class QuestsData
{
    public string SlName;
    public bool IsCompleted;
    public List<StoryData> Story;
}

[System.Serializable]
public class StoryData
{
    public bool IsActive;
    public bool IsCompleted;
}

[System.Serializable]
public class PlayerSrData
{
    public float PlayerPosX;
    public float PlayerPosY;
    public float PlayerPosZ;

    public float PlayerRotX;
    public float PlayerRotY;
    public float PlayerRotZ;
}

[System.Serializable]
public class ObjectsData
{
    public string ObjName;

    public bool IsActive;

    public float ObjPosX;
    public float ObjPosY;
    public float ObjPosZ;

    public float ObjRotX;
    public float ObjRotY;
    public float ObjRotZ;
}

[System.Serializable]
public class ButtonsData
{
    public string ButtonName;
    public List<bool> IsDisabled;
}

[System.Serializable]
public class EnvDatas
{
    public string GpSceneName;
    public PlayerSrData PlState;
    public List<ObjectsData> ObjState;
    public List<ButtonsData> ButState;
    public QuestsData SlState;
}
#endregion

#region Global Data
[System.Serializable]
public class PlayerNsData
{
    public int Collectible1;
    public int Collectible2;
    public int Collectible3;
    public int Collectible4;

    public bool FrogUnlocked;
    public bool ArmaUnlocked;
    public bool CraneUnlocked;
    public bool DolphinUnlocked;
}
#endregion

public class SuperDataManager : MonoBehaviour
{
    #region Public Variables
    [SerializeField]
    public List<EnvDatas> EnvSensData;

    [SerializeField]
    public PlayerNsData PlNsData;
    #endregion

    #region Events
    public event_listEnvSens_plNsSens SaveRequest;
    public UnityEvent RequestUpdateToSave, RequestUpdateByLoad;
    #endregion

    #region Taking References and Linking Events
    private void Awake()
    {
        MenuManager mmTempLink = this.gameObject.GetComponent<MenuManager>();

        mmTempLink.newDataRequest.AddListener(this.RequestingSave);

        SuperSaveLoadManager sslmTempLinkl = this.gameObject.GetComponent<SuperSaveLoadManager>();

        sslmTempLinkl.TempDataUpdateRequest.AddListener(this.UpdatingTempRepo);

        GameController gcTempLink = this.gameObject.GetComponent<GameController>();

        gcTempLink.gpInitializer.AddListener(this.Initialization);
    }

    private void Initialization(GameObject player)
    {
        StoryLineInstance currentSlInScene =
            GameObject.FindGameObjectWithTag("StoryLine").GetComponent<StoryLineInstance>();

        currentSlInScene.UpdateTempMemoryRequest.AddListener(this.UpdatingQuestData);
    }
    #endregion

    #region Requesting New Values by Local Variables and Objects to Update Temp Repo and Saving to SuperSaveLoadManager
    public void RequestingSave()
    {
        this.RequestUpdateToSave.Invoke();
        this.SaveRequest.Invoke(this.EnvSensData, this.PlNsData);
    }

    private void UpdatingQuestData()
    {
        var slRepoToUpdate = new List<StoryLine>();

        slRepoToUpdate.AddRange(this.gameObject.GetComponent<QuestsManager>().StoryLineRepo);

        for (var slInRepo = 0; slInRepo < slRepoToUpdate.Count; slInRepo++)
        {
            var scenedata = this.EnvSensData.Find(x => x.SlState.SlName == slRepoToUpdate[slInRepo].StoryLineName);

            if (scenedata == null) continue;

            scenedata.SlState.IsCompleted = slRepoToUpdate[slInRepo].Completed;

            for (var storyIndex = 0; storyIndex < scenedata.SlState.Story.Count; storyIndex++)
            {
                scenedata.SlState.Story[storyIndex].IsActive = slRepoToUpdate[slInRepo].Stories[storyIndex].Active;
                scenedata.SlState.Story[storyIndex].IsCompleted = slRepoToUpdate[slInRepo].Stories[storyIndex].Completed;
            }
        }


    }
    #endregion

    #region Loading Update Temp Repo by SuperSaveLoadManager and request Involved Local Variables and Objects to update themselves
    private void UpdatingTempRepo()
    {
        /*
        this.EnvSensData.Clear();
        this.EnvSensData.TrimExcess();
        this.EnvSensData = new List<EnvDatas>();
        this.EnvSensData = newEnvSensData;

        this.PlNsData = null;
        this.PlNsData = newPlSensData;
        */
        this.RequestUpdateByLoad.Invoke();
    }
    #endregion

    #region Testing Save (to be Removed)
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            this.RequestingSave();
        }
    }
    #endregion

    #region Edit Mode Called Methods
    public void AddingStoryLineEditMode(StoryLine slToAdd)
    {
        var sceneData = this.CheckingSceneBelonging();

        if (sceneData == null) return;

        sceneData.SlState.SlName = slToAdd.StoryLineName;
        sceneData.SlState.IsCompleted = slToAdd.Completed;
        sceneData.SlState.Story = new List<StoryData>();

        for (var ssIndex = 0; ssIndex < slToAdd.Stories.Count; ssIndex++)
        {
            sceneData.SlState.Story.Add(new StoryData());
            sceneData.SlState.Story[ssIndex].IsActive = slToAdd.Stories[ssIndex].Active;
            sceneData.SlState.Story[ssIndex].IsCompleted = slToAdd.Stories[ssIndex].Completed;
        }
    }

    public void UpdatingPlState(GameObject player)
    {
        var sceneData = this.CheckingSceneBelonging();

        if (sceneData == null) return;

        Transform plTrans = player.transform;

        sceneData.PlState.PlayerPosX = plTrans.position.x;
        sceneData.PlState.PlayerPosY = plTrans.position.y;
        sceneData.PlState.PlayerPosZ = plTrans.position.z;

        sceneData.PlState.PlayerRotX = plTrans.eulerAngles.x;
        sceneData.PlState.PlayerRotY = plTrans.eulerAngles.y;
        sceneData.PlState.PlayerRotZ = plTrans.eulerAngles.z;
    }

    public void UpdatingPlNsState(GameObject player)
    {
        FSMChecker fsmTempLink = player.GetComponent<FSMChecker>();

        this.PlNsData.FrogUnlocked = fsmTempLink.abiUnlocked.frogUnlocked;
        this.PlNsData.ArmaUnlocked = fsmTempLink.abiUnlocked.armaUnlocked;
        this.PlNsData.CraneUnlocked = fsmTempLink.abiUnlocked.craneUnlocked;
        this.PlNsData.DolphinUnlocked = fsmTempLink.abiUnlocked.dolphinUnlocked;

        Collectibles clTempLink = player.GetComponent<Collectibles>();

        this.PlNsData.Collectible1 = clTempLink.GoldenCollectible;
        this.PlNsData.Collectible2 = clTempLink.Collectible2;
        this.PlNsData.Collectible3 = clTempLink.Collectible3;
        this.PlNsData.Collectible4 = clTempLink.Collectible4;
    }

    public void UpdatingObjState(GameObject obj)
    {
        var sceneData = this.CheckingSceneBelonging();

        if (sceneData == null) return;

        Transform thisTrans = obj.transform;

        ObjectsData objToUpdate = sceneData.ObjState.Find(x => x.ObjName == obj.name);


        if (objToUpdate != null)
        {
            sceneData.ObjState.Remove(objToUpdate);

            objToUpdate = new ObjectsData();

            objToUpdate.ObjPosX = thisTrans.position.x;
            objToUpdate.ObjPosY = thisTrans.position.y;
            objToUpdate.ObjPosZ = thisTrans.position.z;

            objToUpdate.ObjRotX = thisTrans.eulerAngles.x;
            objToUpdate.ObjRotY = thisTrans.eulerAngles.y;
            objToUpdate.ObjRotZ = thisTrans.eulerAngles.z;

            objToUpdate.IsActive = this.gameObject.activeInHierarchy;

            sceneData.ObjState.Add(objToUpdate);
        }
        else
        {
            objToUpdate = new ObjectsData();

            objToUpdate.ObjName = obj.name;

            objToUpdate.ObjPosX = thisTrans.position.x;
            objToUpdate.ObjPosY = thisTrans.position.y;
            objToUpdate.ObjPosZ = thisTrans.position.z;

            objToUpdate.ObjRotX = thisTrans.eulerAngles.x;
            objToUpdate.ObjRotY = thisTrans.eulerAngles.y;
            objToUpdate.ObjRotZ = thisTrans.eulerAngles.z;

            objToUpdate.IsActive = this.gameObject.activeInHierarchy;

            sceneData.ObjState.Add(objToUpdate);
        }
    }

    public void UpdatingButState(GameObject puzzleType)
    {
        var sceneData = this.CheckingSceneBelonging();

        if (sceneData == null) return;

        var newButton = sceneData.ButState.Find(x => x.ButtonName == puzzleType.name);

        if (newButton == null)
        {
            newButton = new ButtonsData();

            newButton.ButtonName = puzzleType.name;

            var puzzleScripts = new List<Puzzles>();

            puzzleScripts.AddRange(puzzleType.GetComponents<Puzzles>());

            newButton.IsDisabled = new List<bool>();

            foreach (var puzzle in puzzleScripts)
            {
                newButton.IsDisabled.Add(puzzle.keyHit);
            }

            sceneData.ButState.Add(newButton);
        }
        else
        {
            sceneData.ButState.Remove(newButton);

            newButton = new ButtonsData();

            newButton.ButtonName = puzzleType.name;

            var puzzleScripts = new List<Puzzles>();

            puzzleScripts.AddRange(puzzleType.GetComponents<Puzzles>());

            newButton.IsDisabled = new List<bool>();

            foreach (var puzzle in puzzleScripts)
            {
                newButton.IsDisabled.Add(puzzle.keyHit);
            }

            sceneData.ButState.Add(newButton);
        }
    }

    private EnvDatas CheckingSceneBelonging()
    {
        var currentScene = SceneManager.GetActiveScene().name;

        return this.EnvSensData.Find(x => x.GpSceneName == currentScene);
    }
    #endregion


}


