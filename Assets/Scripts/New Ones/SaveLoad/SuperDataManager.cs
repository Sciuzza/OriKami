using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;
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
    public bool Saved;

    public int Collectible1;
    public int Collectible2;
    public int Collectible3;
    public int Collectible4;

    public bool FrogUnlocked;
    public bool ArmaUnlocked;
    public bool CraneUnlocked;
    public bool DolphinUnlocked;

    public bool Legend1Unlocked;
    public bool Legend2Unlocked;
    public bool Legend3Unlocked;
    public bool Legend4Unlocked;
    public bool Legend5Unlocked;

    public string SceneToLoad;
}

[System.Serializable]
public class TweakableSettings
{
    [Range(0, 1)]
    public int DifficultyIndex;
    [Range(0.5f, 1.5f)]
    public float CurCamValue;
    [Range(0, 1)]
    public int QualityIndex;

    [Range(0.5f, 1.5f)]
    public float MasterValue;
    [Range(0.5f, 1.5f)]
    public float MusicValue;
    [Range(0.5f, 1.5f)]
    public float EffectsValue;

    [Range(0, 3)]
    public int Form1Index;
    [Range(0, 3)]
    public int Form2Index;
    [Range(0, 3)]
    public int Form3Index;
    [Range(0, 3)]
    public int Form4Index;
    [Range(0, 3)]
    public int StdFormIndex;
    [Range(0, 3)]
    public int JdIndex;
    [Range(0, 3)]
    public int PtIndex;
}
#endregion

public class SuperDataManager : MonoBehaviour
{
    #region Public Variables
    [SerializeField]
    public List<EnvDatas> EnvSensData;

    [SerializeField]
    public PlayerNsData PlNsData;

    [SerializeField]
    public TweakableSettings TwkSettings;
    #endregion

    #region Private Variables

    #endregion

    #region Events
    public UnityEvent RequestLocalUpdateToRepo, RequestLocalUpdateByRepo, DisableContinueRequest, menuInitRequest;
    public event_string SwitchSceneRequest;
    #endregion

    #region Taking References and Linking Events and ReWriting Default Data
    private void Awake()
    {
        //this.ErasingData();

        this.InitializeOriginalData();

        var mmTempLink = this.gameObject.GetComponent<MenuManager>();

        mmTempLink.newDataRequest.AddListener(this.NewGameHandler);
        mmTempLink.loadDataRequest.AddListener(this.ContinueHandler);
        mmTempLink.SaveDataRequest.AddListener(this.SaveHandler);

        var gcTempLink = this.gameObject.GetComponent<GameController>();

        gcTempLink.gpInitializer.AddListener(this.InitializingGameplayScene);
        gcTempLink.ngpInitializer.AddListener(this.InitializingNgpScene);
    }

    private void InitializeOriginalData()
    {
        var bf = new BinaryFormatter();

        if (!File.Exists(Application.persistentDataPath + "/EnvSensOriData.dat"))
        {
            var envSensFile = File.Create(Application.persistentDataPath + "/EnvSensOriData.dat");
            bf.Serialize(envSensFile, this.EnvSensData);
            envSensFile.Close();
        }

        if (!File.Exists(Application.persistentDataPath + "/PlNsOriData.dat"))
        {
            var plSensFile = File.Create(Application.persistentDataPath + "/PlNsOriData.dat");
            bf.Serialize(plSensFile, this.PlNsData);
            plSensFile.Close();
        }

        if (!File.Exists(Application.persistentDataPath + "/TweaksOriData.dat"))
        {
            var tweaksFile = File.Create(Application.persistentDataPath + "/TweaksOriData.dat");
            bf.Serialize(tweaksFile, this.TwkSettings);
            tweaksFile.Close();
        }
    }
    #endregion

    #region No Gameplay Scenes Handler
    private void InitializingNgpScene()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Main Menu":
                if (!this.LoadByFile("/TweaksData.dat", 2)) this.LoadByFile("/TweaksOriData.dat", 2);
                this.menuInitRequest.Invoke();
                if (!this.LoadByFile("/EnvSensData.dat", 0) || !this.LoadByFile("/PlNsData.dat", 1) || !this.PlNsData.Saved)
                    this.DisableContinueRequest.Invoke();
                break;
        }
    }

    private void ContinueHandler()
    {
        this.SwitchSceneRequest.Invoke(this.PlNsData.SceneToLoad);
    }

    private void NewGameHandler()
    {
        this.LoadByFile("/EnvSensOriData.dat", 0);
        this.LoadByFile("/PlNsOriData.dat", 1);

        this.SaveToFile("/EnvSensData.dat", 0);
        this.SaveToFile("/PlNsData.dat", 1);
        this.SaveToFile("/TweaksData.dat", 2);
        this.SwitchSceneRequest.Invoke("Route 1");
    }
    #endregion

    #region Gameplay Scenes Handler
    private void InitializingGameplayScene(GameObject player)
    {

        StoryLineInstance currentSlInScene =
          GameObject.FindGameObjectWithTag("StoryLine").GetComponent<StoryLineInstance>();

        currentSlInScene.RequestRepoUpdateQuests.AddListener(this.UpdatingQuestData);
        currentSlInScene.SaveRequest.AddListener(this.SaveHandler);

        player.GetComponent<FSMChecker>().deathRequest.AddListener(this.LoadingHandler);
        player.GetComponent<EnvInputs>().SaveRequestByCheck.AddListener(this.SaveHandler);

        //this.LoadingHandler();
    }
    #endregion

    #region Save Handler
    private void SaveHandler()
    {
        Debug.Log("Saving");
        this.RequestLocalUpdateToRepo.Invoke();
        this.SaveToFile("/EnvSensData.dat", 0);
        this.SaveToFile("/PlNsData.dat", 1);
        this.SaveToFile("/TweaksData.dat", 2);
    }

    private void SaveToFile(string fileName, int fileType)
    {
        var bf = new BinaryFormatter();

        var fileRef = File.Create(Application.persistentDataPath + fileName);

        switch (fileType)
        {
            case 0:
                bf.Serialize(fileRef, this.EnvSensData);
                break;
            case 1:
                bf.Serialize(fileRef, this.PlNsData);
                break;
            case 2:
                bf.Serialize(fileRef, this.TwkSettings);
                break;
        }

        fileRef.Close();
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

    #region Loader Handler

    private void LoadingHandler()
    {
        Debug.Log("Loading");
        if (this.LoadByFile("/EnvSensData.dat", 0) && this.LoadByFile("/PlNsData.dat", 1) && this.LoadByFile("/TweaksData.dat", 2))
        {
            this.RequestLocalUpdateByRepo.Invoke();
            Debug.Log("Loading Successfull");
        }
        else
        {
            Debug.Log("There is a problem on Loading");
        }
    }

    private bool LoadByFile(string fileName, int fileType)
    {
        var bf = new BinaryFormatter();

        if (File.Exists(Application.persistentDataPath + fileName))
        {
            var fileRef = File.Open(Application.persistentDataPath + fileName, FileMode.Open);

            switch (fileType)
            {
                case 0:
                    this.EnvSensData.Clear();
                    this.EnvSensData.TrimExcess();
                    this.EnvSensData = (List<EnvDatas>)bf.Deserialize(fileRef);
                    break;
                case 1:
                    this.PlNsData = null;
                    this.PlNsData = (PlayerNsData)bf.Deserialize(fileRef);
                    break;
                case 2:
                    this.TwkSettings = null;
                    this.TwkSettings = (TweakableSettings)bf.Deserialize(fileRef);
                    break;
            }

            fileRef.Close();
            return true;
        }
        else
        {
            Debug.Log("EnvSensData not Found");
            return false;
        }
    }
    #endregion

    #region Testing Save (to be Removed)
    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            this.SaveHandler();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            this.LoadingOnDiskData();
        }
    }
    */
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

        this.PlNsData.Legend1Unlocked = fsmTempLink.legUnlocked.Legend1;
        this.PlNsData.Legend2Unlocked = fsmTempLink.legUnlocked.Legend2;
        this.PlNsData.Legend3Unlocked = fsmTempLink.legUnlocked.Legend3;
        this.PlNsData.Legend4Unlocked = fsmTempLink.legUnlocked.Legend4;
        this.PlNsData.Legend5Unlocked = fsmTempLink.legUnlocked.Legend5;

        this.PlNsData.Collectible1 = fsmTempLink.GoldenCrane;
        this.PlNsData.Collectible2 = fsmTempLink.DRocks;
        this.PlNsData.Collectible3 = fsmTempLink.BlackSmith;
        this.PlNsData.Collectible4 = fsmTempLink.V3;
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

            objToUpdate.ObjName = obj.name;

            objToUpdate.ObjPosX = thisTrans.position.x;
            objToUpdate.ObjPosY = thisTrans.position.y;
            objToUpdate.ObjPosZ = thisTrans.position.z;

            objToUpdate.ObjRotX = thisTrans.eulerAngles.x;
            objToUpdate.ObjRotY = thisTrans.eulerAngles.y;
            objToUpdate.ObjRotZ = thisTrans.eulerAngles.z;

            objToUpdate.IsActive = obj.activeSelf;


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

            objToUpdate.IsActive = obj.activeSelf;

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

    #region General Utilities Methods
    private void ErasingData()
    {
        if (File.Exists(Application.persistentDataPath + "/EnvSensData.dat"))
        {
            File.Delete(Application.persistentDataPath + "/EnvSensData.dat");
        }

        if (File.Exists(Application.persistentDataPath + "/PlNsData.dat"))
        {
            File.Delete(Application.persistentDataPath + "/PlNsData.dat");
        }

        if (File.Exists(Application.persistentDataPath + "/TweaksData.dat"))
        {
            File.Delete(Application.persistentDataPath + "/TweaksData.dat");
        }

        if (File.Exists(Application.persistentDataPath + "/EnvSensOriData.dat"))
        {
            File.Delete(Application.persistentDataPath + "/EnvSensOriData.dat");
        }

        if (File.Exists(Application.persistentDataPath + "/PlNsOriData.dat"))
        {
            File.Delete(Application.persistentDataPath + "/PlNsOriData.dat");
        }

        if (File.Exists(Application.persistentDataPath + "/TweaksOriData.dat"))
        {
            File.Delete(Application.persistentDataPath + "/TweaksOriData.dat");
        }
    }
    #endregion
}