using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveSystem : MonoBehaviour {

    public void SaveState()
    {

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/PlayerData.dat");

        PlayerData data = new PlayerData();
        data.posx = transform.position.x;
        data.posy = transform.position.y;
        data.posz = transform.position.z;

        data.rotx = transform.eulerAngles.x;
        data.roty = transform.eulerAngles.y;
        data.rotz = transform.eulerAngles.z;

        bf.Serialize(file, data); 

        //PlayerPrefs.SetFloat("PosX", transform.position.x);
        //PlayerPrefs.SetFloat("PosY", transform.position.y);
        //PlayerPrefs.SetFloat("PosZ", transform.position.z);

        //PlayerPrefs.SetFloat("RotX", transform.eulerAngles.x);
        //PlayerPrefs.SetFloat("RotY", transform.eulerAngles.y);
        //PlayerPrefs.SetFloat("RotZ", transform.eulerAngles.z);
    }

    public void LoadState()
    {
        //float x = PlayerPrefs.GetFloat("PosX");
        //float y = PlayerPrefs.GetFloat("PosY");
        //float z = PlayerPrefs.GetFloat("PosZ");

        //float rx = PlayerPrefs.GetFloat("RotX");
        //float ry = PlayerPrefs.GetFloat("RotY");
        //float rz = PlayerPrefs.GetFloat("RotZ");

        //transform.position = new Vector3(x, y, z);
        //transform.rotation = Quaternion.Euler(rx, ry, rz);
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
}





