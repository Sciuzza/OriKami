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
    public struct frogM
    {
        public float moveSpeed;
        public float jumpStrength;

    }

    [System.Serializable]
    public struct armaM
    {
        public float moveSpeed;
        public float rollingStrength;
        public float rollingTime;

    }

    [System.Serializable]
    public struct craneM
    {
        public float glideSpeed;

    }

    [System.Serializable]
    public struct generalTweaks
    {
        public float globalGravity;
        public float jumpGravity;
        public float glideGravity;
        [Range(0.5f,5)]
        public float rotateSpeed;
     
    }

   
    public generalTweaks GeneralTweaks;

    [System.Serializable]
    public struct Movement {

        public standardM standMove;
        public frogM frogMove;
        public craneM craneMove;
        public armaM armaMove;
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

