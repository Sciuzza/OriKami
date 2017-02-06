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
    public bool IsDisabled;
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
    }

    #endregion

    #region Requesting New Values by Local Variables and Objects to Update Temp Repo and Saving to SuperSaveLoadManager
    public void RequestingSave()
    {
        this.RequestUpdateToSave.Invoke();
        this.SaveRequest.Invoke(this.EnvSensData, this.PlNsData);
    } 
    #endregion

    #region Loading Update Temp Repo by SuperSaveLoadManager and request Involved Local Variables and Objects to update themselves
    private void UpdatingTempRepo(List<EnvDatas> newEnvSensData, PlayerNsData newPlSensData)
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
    }

    public void UpdatingButState(Puzzles button)
    {
        var sceneData = this.CheckingSceneBelonging();

        if (sceneData == null) return;

        if (sceneData.ButState.Find(x => x.ButtonName == button.gameObject.name) == null)
        {
            var newButton = new ButtonsData { ButtonName = button.gameObject.name, IsDisabled = button.keyHit };

            sceneData.ButState.Add(newButton);
        }
        else
        {
            ButtonsData buttonToUpdate = sceneData.ButState.Find(x => x.ButtonName == button.gameObject.name);
            buttonToUpdate.IsDisabled = button.keyHit;
        }
    }

    private EnvDatas CheckingSceneBelonging()
    {
        var currentScene = SceneManager.GetActiveScene().name;

        return this.EnvSensData.Find(x => x.GpSceneName == currentScene);
    }
    #endregion
}

/*
    [SerializeField]
    public List<QuestsData> questSensData;

    private void Awake()
    {
        MenuManager mmTempLink = this.gameObject.GetComponent<MenuManager>();

        mmTempLink.newDataRequest.AddListener(this.InitializingNewData);
    }

    private void InitializingNewData()
    {
        this.NewGameQuestDataInitializer();
    }

    private void NewGameQuestDataInitializer()
    {
        var qmTempLink = this.gameObject.GetComponent<QuestsManager>();

        this.questSensData = new List<QuestsData>();

        for (var slIndex = 0; slIndex < qmTempLink.StoryLineRepo.Count; slIndex++)
        {
            this.questSensData.Add(new QuestsData());
            this.questSensData[slIndex].IsCompleted = qmTempLink.StoryLineRepo[slIndex].Completed;
            this.questSensData[slIndex].Story = new List<StoryData>();

            for (var ssIndex = 0; ssIndex < qmTempLink.StoryLineRepo[slIndex].Stories.Count; ssIndex++)
            {
                this.questSensData[slIndex].Story.Add(new StoryData());
                this.questSensData[slIndex].Story[ssIndex].IsDisabled = qmTempLink.StoryLineRepo[slIndex].Stories[ssIndex].Active;
                this.questSensData[slIndex].Story[ssIndex].IsCompleted = qmTempLink.StoryLineRepo[slIndex].Stories[ssIndex].Completed;
            }
        }   
    }
*/
