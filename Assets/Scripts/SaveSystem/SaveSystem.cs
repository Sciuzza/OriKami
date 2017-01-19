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


    void Awake()
    {
        GameController gcTempLink = this.GetComponent<GameController>();
        gcTempLink.gpInitializer.AddListener(GameplayInitialization);

    }

    void Update()
    {
        StoryLineInstance storyLineTempLink = GameObject.FindGameObjectWithTag("StoryLine").GetComponent<StoryLineInstance>();
        StoryLineInstance singleStoryTempLink = GameObject.FindGameObjectWithTag("StoryLine").GetComponent<StoryLineInstance>();
        if (Input.GetKeyDown(KeyCode.K))
        {
            SceneManager.LoadScene("Ricky Testing");
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
           
            storyLineTempLink.CurrentStoryLine.Completed = true;
          
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
      
            singleStoryTempLink.storySelected.Active = true;
            Debug.Log(singleStoryTempLink.storySelected.Active);
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
        StoryLineInstance storyLineTempLink = GameObject.FindGameObjectWithTag("StoryLine").GetComponent<StoryLineInstance>();
        StoryLineInstance singleStoryTempLink = GameObject.FindGameObjectWithTag("StoryLine").GetComponent<StoryLineInstance>();


        questData.StoryLine_Completed_Save = storyLineTempLink.CurrentStoryLine.Completed;
        questData.SingleStory_Active_Save = singleStoryTempLink.storySelected.Active;
        questData.SingleStory_Completed_Save = singleStoryTempLink.storySelected.Completed;



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
            StoryLineInstance storyLineTempLink = GameObject.FindGameObjectWithTag("StoryLine").GetComponent<StoryLineInstance>();
            StoryLineInstance singleStoryTempLink = GameObject.FindGameObjectWithTag("StoryLine").GetComponent<StoryLineInstance>();
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
            storyLineTempLink.CurrentStoryLine.Completed = questData.StoryLine_Completed_Save;
            singleStoryTempLink.storySelected.Active = questData.SingleStory_Active_Save;
            singleStoryTempLink.storySelected.Completed = questData.SingleStory_Completed_Save;
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

    //Array for Forms
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
    public bool StoryLine_Completed_Save;
    public bool SingleStory_Completed_Save;
    public bool SingleStory_Active_Save;
    //public List<SingleStory> Stories_Save;

}




