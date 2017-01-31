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
    int questemp;
    private StoryLineInstance slTempLink;

    private void Awake()
    {
        GameController gcTempLink = this.GetComponent<GameController>();
        gcTempLink.gpInitializer.AddListener(this.GameplayInitialization);
        var storyLineCheck = GameObject.FindGameObjectWithTag("StoryLine");


        if (storyLineCheck != null)
        {
            this.slTempLink = storyLineCheck.GetComponent<StoryLineInstance>();

            if (this.slTempLink != null)
                this.questemp = (this.slTempLink.CurrentStoryLine.Stories.Count * 2) + 1;

        }
    }

    private void Update()
    {
        if (this.slTempLink == null) return;

        if (Input.GetKeyDown(KeyCode.K))
        {
            SceneManager.LoadScene("Ricky Testing");
        }

        if (Input.GetKeyDown(KeyCode.J))
        {

            this.slTempLink.CurrentStoryLine.Completed = true;

        }
        if (Input.GetKeyDown(KeyCode.G))
        {

            foreach (var item in this.slTempLink.CurrentStoryLine.Stories)
            {
                item.ActiveS = true;
                item.CompletedS = true;
            }
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
        FileStream file = File.Create(Application.persistentDataPath + "/PlayerDataDef.dat");
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

        questData.StoryLine_Completed_Save = storyLineTempLink.CurrentStoryLine.Completed;
        questemp = storyLineTempLink.CurrentStoryLine.Stories.Count * 2 + 1;



        bool[] tempSave = new bool[questemp];
        tempSave[0] = storyLineTempLink.CurrentStoryLine.Completed;
        int j = 0;
        for (int i = 1; i < storyLineTempLink.CurrentStoryLine.Stories.Count; i += 2)
        {
            tempSave[i] = storyLineTempLink.CurrentStoryLine.Stories[j].ActiveS;
            tempSave[i + 1] = storyLineTempLink.CurrentStoryLine.Stories[j].CompletedS;
            j++;
        }

        #endregion

        bf.Serialize(file, data);
        bf.Serialize(file, questData);
        file.Close();

    }

    public void LoadState()
    {
        if (File.Exists(Application.persistentDataPath + "/PlayerDataDef.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/PlayerDataDef.dat", FileMode.Open);
            SensibleData data = (SensibleData)bf.Deserialize(file);
            QuestData questData = (QuestData)bf.Deserialize(file);
            FSMChecker fsmTempLink = PlayerTempLink.gameObject.GetComponent<FSMChecker>();
            StoryLineInstance storyLineTempLink = GameObject.FindGameObjectWithTag("StoryLine").GetComponent<StoryLineInstance>();
            
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
            bool[] loadTemp = new bool[questemp];
            loadTemp = questData.infoQuest;
            storyLineTempLink.CurrentStoryLine.Completed = questData.StoryLine_Completed_Save;
            
            
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
    public bool[] infoQuest;  
}



    




