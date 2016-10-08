using UnityEngine;
using System.Collections;




    public enum playMode { KMInput, JoyInput };

    #region Structs
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
    public struct dolphin
    {
        public float swimSpeed;
        public float jumpStrength;

    }

    [System.Serializable]
    public struct generalTweaks
    {
        public float globalGravity;
        public float jumpGravity;
        public float glideGravity;
        [Range(0.5f, 5)]
        public float rotateSpeed;
        public playMode currentInput;

    }

    [System.Serializable]
    public struct Movement
    {

        public standardM standMove;
        public frogM frogMove;
        public craneM craneMove;
        public armaM armaMove;
        public dolphin dolphinMove;
    }

    [System.Serializable]
    public struct CameraPlayer
    {
        [Range(5, 50)]
        public float distanceMin;
        [Range(10, 150)]
        public float distanceMax;
        [Range(5, 20)]
        public float currentDistance;
        [Range(0.5f, 10)]
        public float sensitivityX;
        [Range(0.5f, 10)]
        public float sensitivityY;
        [Range(0.1f, 50)]
        public float sensitivityZoom;
        [Range(0, 50)]
        public float yAngleMin;
        [Range(40, 90)]
        public float yAngleMax;
    }

    #endregion



    public class DesignerT : MonoBehaviour
    {
        public generalTweaks GeneralTweaks;
        public Movement GestioneMovimento;
        public CameraPlayer GestioneCamera;

        void Awake()
        {
            GameController gcLink = this.GetComponent<GameController>();

            gcLink.initializer.AddListener(ApplyingDesignTweaks);
            gcLink.designRunningTweaks.AddListener(ApplyingDesignTweaks);
        }


        public void ApplyingDesignTweaks(GameObject player)
        {
            PlCore plCoreTempLink = player.GetComponent<PlCore>();

            plCoreTempLink.CurrentMoveValues = GestioneMovimento;
            plCoreTempLink.GeneralValues = GeneralTweaks;

            this.GetComponent<CameraManager>().currentPlCameraSettings = GestioneCamera;
        }

    }



