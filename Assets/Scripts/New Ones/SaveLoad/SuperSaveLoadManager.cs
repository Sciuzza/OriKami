using System;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine.Events;

public class SuperSaveLoadManager : MonoBehaviour {




    public UnityEvent TempDataUpdateRequest;

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


        bf.Serialize(envSensFile, newEnvDatas);
        bf.Serialize(plSensFile, newPlNsData);

        plSensFile.Close();
        envSensFile.Close();

    }

    private void LoadingOnDiskData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/EnvSensData.dat"))
        {
            var envSensFile = File.Open(Application.persistentDataPath + "/EnvSensData.dat", FileMode.Open);

            List<EnvDatas> diskEnvDatas = new List<EnvDatas>();
              diskEnvDatas.AddRange(this.gameObject.GetComponent<SuperDataManager>().EnvSensData);  

            diskEnvDatas = (List<EnvDatas>)bf.Deserialize(envSensFile);

            this.gameObject.GetComponent<SuperDataManager>().EnvSensData = diskEnvDatas;
            envSensFile.Close();
        }

        if (File.Exists(Application.persistentDataPath + "/PlNsData.dat"))
        {
            var plSensFile = File.Open(Application.persistentDataPath + "/PlNsData.dat", FileMode.Open);

            var diskPlNsData = this.gameObject.GetComponent<SuperDataManager>().PlNsData;

            diskPlNsData = (PlayerNsData)bf.Deserialize(plSensFile);

            this.gameObject.GetComponent<SuperDataManager>().PlNsData = diskPlNsData;
            plSensFile.Close();
        }

        this.TempDataUpdateRequest.Invoke();
    }

    private void InitializingGameplayScene(GameObject player)
    {
        //this.LoadingOnDiskData();
        player.GetComponent<FSMChecker>().deathRequest.AddListener(this.LoadingOnDiskData);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) this.LoadingOnDiskData();
    }

}
