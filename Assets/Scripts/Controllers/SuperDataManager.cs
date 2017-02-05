using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.SceneManagement;

#region Env Data Classes 
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
public class PlayerPosData
{
    public int Collectible1;
    public int Collectible2;
    public int Collectible3;
    public int Collectible4;

    public float PlayerPosX;
    public float PlayerPosY;
    public float PlayerPosZ;

    public float PlayerRotX;
    public float PlayerRotY;
    public float PlayerRotZ;

    public bool FrogUnlocked;
    public bool ArmaUnlocked;
    public bool CraneUnlocked;
    public bool DolphinUnlocked;
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
    public bool IsActive;
}

[System.Serializable]
public class EnvDatas
{
    public string GpSceneName;
    public PlayerPosData PlState;
    public List<ObjectsData> ObjState;
    public List<ButtonsData> ButState;
    public QuestsData SlState;
} 
#endregion

public class SuperDataManager : MonoBehaviour
{
    [SerializeField]
    public List<EnvDatas> EnvSensData;

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

        sceneData.PlState.PlayerPosX = player.transform.position.x;
        sceneData.PlState.PlayerPosY = player.transform.position.y;
        sceneData.PlState.PlayerPosZ = player.transform.position.z;

        sceneData.PlState.PlayerRotX = player.transform.eulerAngles.x;
        sceneData.PlState.PlayerRotY = player.transform.eulerAngles.y;
        sceneData.PlState.PlayerRotZ = player.transform.eulerAngles.z;
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
                this.questSensData[slIndex].Story[ssIndex].IsActive = qmTempLink.StoryLineRepo[slIndex].Stories[ssIndex].Active;
                this.questSensData[slIndex].Story[ssIndex].IsCompleted = qmTempLink.StoryLineRepo[slIndex].Stories[ssIndex].Completed;
            }
        }   
    }
*/
