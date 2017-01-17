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

        bf.Serialize(file, data);

        file.Close();

    }

    public void LoadState()
    {
        if (File.Exists(Application.persistentDataPath + "/PlayerData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/PlayerData.dat", FileMode.Open);
            SensibleData data = (SensibleData)bf.Deserialize(file);     

            file.Close();

            PlayerTempLink.transform.position = new Vector3(data.posx, data.posy, data.posz);
            PlayerTempLink.transform.rotation = Quaternion.Euler(data.rotx, data.roty, data.rotz);

            FSMChecker fsmTempLink = PlayerTempLink.gameObject.GetComponent<FSMChecker>();

            //fsmTempLink.abiUnlocked.frogUnlocked = data.frogUnlocked;
            //fsmTempLink.abiUnlocked.armaUnlocked = data.armaUnlocked;
            //fsmTempLink.abiUnlocked.craneUnlocked = data.craneUnlocked;
            //fsmTempLink.abiUnlocked.dolphinUnlocked = data.dolphinUnlocked;

            fsmTempLink.abiUnlocked.frogUnlocked = data.formsUnlocked[0];
            fsmTempLink.abiUnlocked.armaUnlocked = data.formsUnlocked[1];
            fsmTempLink.abiUnlocked.craneUnlocked = data.formsUnlocked[2];
            fsmTempLink.abiUnlocked.dolphinUnlocked = data.formsUnlocked[3];

            //player.GetComponent<FSMChecker>().abiUnlocked.frogUnlocked = data.frogUnLocked;
            //player.GetComponent<FSMChecker>().abiUnlocked.armaUnlocked = data.armaUnLocked;
            //player.GetComponent<FSMChecker>().abiUnlocked.craneUnlocked = data.cranUnLocked;
            //player.GetComponent<FSMChecker>().abiUnlocked.dolphinUnlocked = data.dolpingUnLocked;
            PlayerTempLink.GetComponent<FSMChecker>().UpdatingAbilityList();

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

    //public bool frogUnlocked;
    //public bool armaUnlocked;
    //public bool craneUnlocked;
    //public bool dolphinUnlocked;

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




