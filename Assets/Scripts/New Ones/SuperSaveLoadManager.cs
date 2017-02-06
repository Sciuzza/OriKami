using System;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SuperSaveLoadManager : MonoBehaviour {


    [SerializeField]
    public List<EnvDatas> EnvSensDataLocalCopy;

    [SerializeField]
    public PlayerNsData PlNsDataLocalCopy;

    public event_listEnvSens_plNsSens TempDataUpdateRequest;

    private void Awake()
    {
        var sdmTempLink = this.gameObject.GetComponent<SuperDataManager>();

        sdmTempLink.SaveRequest.AddListener(this.SavingOnDiskData);

        GameController gcTempLink = this.gameObject.GetComponent<GameController>();

        gcTempLink.gpInitializer.AddListener(this.InitializingGameplayScene);
    }

    private void SavingOnDiskData(List<EnvDatas> newEnvDatas, PlayerNsData newPlNsData)
    {
        var bf = new BinaryFormatter();

        if (File.Exists(Application.persistentDataPath + "/EnvSensData.dat"))
        {
            File.Delete(Application.persistentDataPath + "/EnvSensData.dat");
        }

        if (File.Exists(Application.persistentDataPath + "/PlNsData.dat"))
        {
            File.Delete(Application.persistentDataPath + "/PlNsData.dat");
        }

        var envSensFile = File.Create(Application.persistentDataPath + "/EnvSensData.dat");
        var plSensFile = File.Create(Application.persistentDataPath + "/PlNsData.dat");


        this.EnvSensDataLocalCopy.Clear();
        this.EnvSensDataLocalCopy.TrimExcess();
        this.EnvSensDataLocalCopy.AddRange(newEnvDatas);
  

        this.PlNsDataLocalCopy = null;
        this.PlNsDataLocalCopy = new PlayerNsData();
        this.PlNsDataLocalCopy = newPlNsData;

        bf.Serialize(envSensFile, this.EnvSensDataLocalCopy);
        bf.Serialize(plSensFile, this.PlNsDataLocalCopy);

        plSensFile.Close();
        envSensFile.Close();

    }

    private void LoadingOnDiskData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/EnvSensData.dat"))
        {
            var envSensFile = File.Open(Application.persistentDataPath + "/EnvSensData.dat", FileMode.Open);
            this.EnvSensDataLocalCopy.Clear();
            this.EnvSensDataLocalCopy.TrimExcess();
            this.EnvSensDataLocalCopy = (List<EnvDatas>)bf.Deserialize(envSensFile);
            envSensFile.Close();
        }

        if (File.Exists(Application.persistentDataPath + "/PlNsData.dat"))
        {
            var plSensFile = File.Open(Application.persistentDataPath + "/PlNsData.dat", FileMode.Open);
            this.PlNsDataLocalCopy = null;
            this.PlNsDataLocalCopy = (PlayerNsData)bf.Deserialize(plSensFile);
            plSensFile.Close();
        }
        this.TempDataUpdateRequest.Invoke(this.EnvSensDataLocalCopy, this.PlNsDataLocalCopy);
    }

    private void InitializingGameplayScene(GameObject player)
    {
        player.GetComponent<FSMChecker>().deathRequest.AddListener(this.LoadingOnDiskData);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) this.LoadingOnDiskData();
    }


    /*
           foreach (var scene in newEnvDatas)
        {
            EnvDatas tempData = new EnvDatas();

            tempData.PlState.PlayerPosX = scene.PlState.PlayerPosX;
            tempData.PlState.PlayerPosY = scene.PlState.PlayerPosY;
            tempData.PlState.PlayerPosY = scene.PlState.PlayerPosZ;

            tempData.PlState.PlayerRotX = scene.PlState.PlayerRotX;
            tempData.PlState.PlayerRotY = scene.PlState.PlayerRotY;
            tempData.PlState.PlayerRotZ = scene.PlState.PlayerRotY;

            tempData.GpSceneName = scene.GpSceneName;

            foreach (var button in scene.ButState)
            {
                ButtonsData tempButData = new ButtonsData();

                tempButData.ButtonName = button.ButtonName;
                tempButData.IsDisabled = button.IsDisabled;


                tempData.ButState
            }

            
        }
        
     */ 
}
