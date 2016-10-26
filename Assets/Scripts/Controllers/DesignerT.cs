using UnityEngine;
using System.Collections;







#region Structs

[System.Serializable]
public struct standardInputs
{
    public string Jump;
    public string VerticalFissure;

}

[System.Serializable]
public struct standardM
{
    public float moveSpeed;
    public float jumpStrength;

}

[System.Serializable]
public struct standardI
{
    public standardInputs joyInputs;
    public standardInputs keyInputs;
}

[System.Serializable]
public struct frogInputs
{
    public string Jump;
    public string HorizontalFissure;

}

[System.Serializable]
public struct frogM
{
    public float moveSpeed;
    public float jumpStrength;

}

[System.Serializable]
public struct frogI
{

    public frogInputs joyInputs;
    public frogInputs keyInputs;

}

[System.Serializable]
public struct armaInputs
{
    public string roll;
    public string rockMoving;

}

[System.Serializable]
public struct armaM
{
    public float moveSpeed;
    public float rollingStrength;
    public float rollingTime;

}

[System.Serializable]
public struct armaI
{
    
    public armaInputs joyInputs;
    public armaInputs keyInputs;

}

[System.Serializable]
public struct craneInputs
{
    public string abi1;
    public string abi2;

}

[System.Serializable]
public struct craneM
{
    public float glideSpeed;

}

[System.Serializable]
public struct craneI
{
    public craneInputs joyInputs;
    public craneInputs keyInputs;

}

[System.Serializable]
public struct dolphinInputs
{
    public string abi1;
    public string abi2;

}

[System.Serializable]
public struct dolphinM
{
    public float swimSpeed;
    public float jumpStrength;

}

[System.Serializable]
public struct dolphinI
{
    public dolphinInputs joyInputs;
    public dolphinInputs keyInputs;

}

[System.Serializable]
public struct generalTweaks
{
    public float globalGravity;
    public float jumpGravity;
    public float glideGravity;
    [Range(0.5f, 5)]
    public float rotateSpeed;
   

}

[System.Serializable]
public struct inputSettings
{
    public standardI standardInputs;
    public frogI frogInputs;
    public armaI armaInputs;
    public craneI craneInputs;
    public dolphinI dolphinInputs;

}

[System.Serializable]
public struct moveValues
{

    public standardM standMove;
    public frogM frogMove;
    public craneM craneMove;
    public armaM armaMove;
    public dolphinM dolphinMove;
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
   
    public moveValues GestioneMovimento;
    public inputSettings GestioneInputs;
    public CameraPlayer GestioneCamera;

    void Awake()
    {
        GameController gcLink = this.GetComponent<GameController>();

        gcLink.initializer.AddListener(ApplyingDesignTweaks);
        gcLink.designRunningTweaks.AddListener(ApplyingDesignTweaks);

        Physics.gravity = GeneralTweaks.globalGravity * Vector3.down;
    }


    public void ApplyingDesignTweaks(GameObject player)
    {
        FSMExecutor fsmExecutorTempLink = player.GetComponent<FSMExecutor>();

        fsmExecutorTempLink.currentMoveValues = GestioneMovimento;
        fsmExecutorTempLink.generalValues = GeneralTweaks;

        this.GetComponent<CameraManager>().currentPlCameraSettings = GestioneCamera;

       


        PlayerInputs playerInputsTempLink = player.GetComponent<PlayerInputs>();

        playerInputsTempLink.currentInputs = GestioneInputs;

        Physics.gravity = GeneralTweaks.globalGravity * Vector3.down;
    }

   

}



