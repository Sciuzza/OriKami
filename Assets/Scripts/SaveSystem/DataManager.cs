using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class SensiblePlayerData
{

}

[Serializable]
public class Asd
{
    public bool[] asdone = new bool[4];
}

[Serializable]
public class SensibleGeneralData
{
    public float PlayerX;
    public float PlayerY;
    public float PlayerZ;

    public float PlayerRotX;
    public float PlayerRotY;
    public float PlayerRotZ;
    public bool[] forms = new bool[4];

    public List<Asd> playerForms = new List<Asd>();
    public List<List<Vector3>> gameTraps = new List<List<Vector3>>();
}


public class DataManager : MonoBehaviour {

    public SensiblePlayerData spData;
    public SensibleGeneralData sgData;
    

	public void UpdateData(float x, float y, float z, float rotX, float rotY, float rotZ)
    {
        sgData.PlayerX = x;
        sgData.PlayerY = y;
        sgData.PlayerRotZ = z;

        sgData.PlayerRotX = rotX;
        sgData.PlayerRotY = rotY;
        sgData.PlayerRotZ = rotZ;


    }

    private void UpdateData(bool[] forms)
    {
        sgData.playerForms.Clear();
        sgData.playerForms.TrimExcess();

        sgData.playerForms.AddRange(forms);
    }

    public void UpdateDataObjects(List<List<T>>trapPositions)
    {
        sgData.gameTraps.Clear();
        sgData.gameTraps.TrimExcess();

        sgData.gameTraps.Add(trapPositions);

    }




}
