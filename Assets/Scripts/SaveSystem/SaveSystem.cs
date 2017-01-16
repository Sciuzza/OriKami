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
    public Transform Player;

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
    }


    public void SaveState()
    {

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/PlayerData.dat");

        PlayerData data = new PlayerData();
        data.posx = Player.transform.position.x;
        data.posy = Player.transform.position.y;
        data.posz = Player.transform.position.z;

        data.rotx = Player.transform.eulerAngles.x;
        data.roty = Player.transform.eulerAngles.y;
        data.rotz = Player.transform.eulerAngles.z;

        data.saveAbilities = new List<abilties>();
        data.saveAbilities.AddRange(Player.GetComponent<FSMChecker>().cPlayerState.currentAbilities);

        bf.Serialize(file, data);
        file.Close();

    }

    public void LoadState()
    {
        if (File.Exists(Application.persistentDataPath + "/PlayerData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/PlayerData.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            Player.transform.position = new Vector3(data.posx, data.posy, data.posz);
            Player.transform.rotation = Quaternion.Euler(data.rotx, data.roty, data.rotz);

            Player.GetComponent<FSMChecker>().cPlayerState.currentAbilities.AddRange(data.saveAbilities);
        }

    }
}

[Serializable]
class PlayerData
{
    public float posx;
    public float posy;
    public float posz;

    public float rotx;
    public float roty;
    public float rotz;

    public List<abilties> saveAbilities;

}





