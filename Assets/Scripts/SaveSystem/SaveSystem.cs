using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    private Transform PlayerTempLink;
    private StoryLine storyLineTempLink;

    void Awake()
    {
        GameController gcTempLink = this.GetComponent<GameController>();
        gcTempLink.gpInitializer.AddListener(GameplayInitialization);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SceneManager.LoadScene("Ricky Testing");
        }
    }


    private void GameplayInitialization(GameObject player)
    {
        FSMChecker fsmCheckerTempLink = player.GetComponent<FSMChecker>();
        fsmCheckerTempLink.deathRequest.AddListener(LoadState);
        PlayerTempLink = player.transform;
        

    }

    public void SaveState()
    {
        #region Binary Formatter & File Creation

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/PlayerData.dat");
        #endregion

        #region Position & Rotation
        SensibleData data = new SensibleData();
        

        data.posx = PlayerTempLink.transform.position.x;
        data.posy = PlayerTempLink.transform.position.y;
        data.posz = PlayerTempLink.transform.position.z;

        data.rotx = PlayerTempLink.transform.eulerAngles.x;
        data.roty = PlayerTempLink.transform.eulerAngles.y;
        data.rotz = PlayerTempLink.transform.eulerAngles.z;
        #endregion

        #region FormSettings

        data.formsUnlocked = new bool[4];
        FSMChecker fsmTempLink = PlayerTempLink.gameObject.GetComponent<FSMChecker>();

        data.formsUnlocked[0] = fsmTempLink.abiUnlocked.frogUnlocked;
        data.formsUnlocked[1] = fsmTempLink.abiUnlocked.armaUnlocked;
        data.formsUnlocked[2] = fsmTempLink.abiUnlocked.craneUnlocked;
        data.formsUnlocked[3] = fsmTempLink.abiUnlocked.dolphinUnlocked;
        #endregion

        #region QuestSave
        QuestData questData = new QuestData();
        StoryLine storyLineTempLink = GameObject.FindGameObjectWithTag("StoryLine").GetComponent<StoryLine>();

        questData.StoryLineName_Save = storyLineTempLink.StoryLineName;
        questData.StoryEnumName_Save = storyLineTempLink.StoryEnumName;
        questData.Completed_Save = storyLineTempLink.Completed;

        questData.StoryActiveOnCompletition_Save = storyLineTempLink.StoryActiveOnCompletion;
        questData.StoryCompleteOnCompletition_Save = storyLineTempLink.StoryCompleteOnCompletion;
        questData.StoryLineCompleteOnCompletion_Save = storyLineTempLink.StoryLineCompleteOnCompletion;

        questData.Stories_Save = storyLineTempLink.Stories;
        #endregion





        bf.Serialize(file, data);
        bf.Serialize(file, questData);
        file.Close();

    }

    public void LoadState()
    {
        if (File.Exists(Application.persistentDataPath + "/PlayerData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/PlayerData.dat", FileMode.Open);
            SensibleData data = (SensibleData)bf.Deserialize(file);
            QuestData questData = (QuestData)bf.Deserialize(file);
            FSMChecker fsmTempLink = PlayerTempLink.gameObject.GetComponent<FSMChecker>();
            StoryLine storyLineTempLink = GameObject.FindGameObjectWithTag("StoryLine").GetComponent<StoryLine>();
            file.Close();

            #region LoadPosition&Rotation
            PlayerTempLink.transform.position = new Vector3(data.posx, data.posy, data.posz);
            PlayerTempLink.transform.rotation = Quaternion.Euler(data.rotx, data.roty, data.rotz);
            #endregion

            #region LoadUnLockedForms
            fsmTempLink.abiUnlocked.frogUnlocked = data.formsUnlocked[0];
            fsmTempLink.abiUnlocked.armaUnlocked = data.formsUnlocked[1];
            fsmTempLink.abiUnlocked.craneUnlocked = data.formsUnlocked[2];
            fsmTempLink.abiUnlocked.dolphinUnlocked = data.formsUnlocked[3];
            PlayerTempLink.GetComponent<FSMChecker>().UpdatingAbilityList();
            #endregion

            #region LoadQuestData

            storyLineTempLink.StoryLineName = questData.StoryLineName_Save;
            storyLineTempLink.StoryEnumName = questData.StoryEnumName_Save;
            storyLineTempLink.Completed = questData.Completed_Save;

            storyLineTempLink.StoryActiveOnCompletion = questData.StoryActiveOnCompletition_Save;
            storyLineTempLink.StoryCompleteOnCompletion = questData.StoryCompleteOnCompletition_Save;
            storyLineTempLink.StoryLineCompleteOnCompletion = questData.StoryLineCompleteOnCompletion_Save;

            storyLineTempLink.Stories = questData.Stories_Save;
            #endregion

            // player.GetComponent<FSMChecker>().cPlayerState.currentAbilities.AddRange(data.saveAbilities);
        }

    }
}

[System.Serializable]
public class SensibleData
{
    #region Position and Rotation
    public float posx;
    public float posy;
    public float posz;

    public float rotx;
    public float roty;
    public float rotz;
    #endregion



    public bool[] formsUnlocked;
    // public List<abilties> saveAbilities;

    public EnvData[] envObjects;

}


[System.Serializable]
public class EnvData
{
    public float[] position;
    public float[] rotation;
    public bool isActive;
}

[System.Serializable]
public class QuestData
{
    public string StoryLineName_Save;
    public Storylines StoryEnumName_Save;
    public bool Completed_Save;

    public List<Stories> StoryActiveOnCompletition_Save;
    public List<Stories> StoryCompleteOnCompletition_Save;
    public List<Storylines> StoryLineCompleteOnCompletion_Save;

    public List<SingleStory> Stories_Save;

}




