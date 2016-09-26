using UnityEngine;
using System.Collections;

public class DesignerT : MonoBehaviour
{
    [System.Serializable]
    public struct standardM
    {
        public float moveSpeed;
        public float jumpStrength;
        
    }

    [System.Serializable]
    public struct Movement {

        public standardM StandMove; 
    }

    public Movement GestioneMovimento;

    #region SingleTone
    [HideInInspector]
    public static DesignerT instance = null;

    void Awake()
    {


        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);





    }
    #endregion
}

