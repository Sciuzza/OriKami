using UnityEngine;
using System.Collections;



#region Structs

#region Input Structs
public enum buttonsJoy {A, B, Y, X, LT, RT, LB, RB };
public enum buttonsPc {Q, E, R, F, Z, X, C, Space, N1, N2, N3, N4 };

[System.Serializable]
public struct standardInputsJ
{
    public buttonsJoy Jump;
    public buttonsJoy VerticalFissure;
    public buttonsJoy toFrog;
    public buttonsJoy toArma;
    public buttonsJoy toCrane;
    public buttonsJoy toDolphin;
}

[System.Serializable]
public struct standardInputsPc
{
    public buttonsPc Jump;
    public buttonsPc VerticalFissure;
    public buttonsPc toFrog;
    public buttonsPc toArma;
    public buttonsPc toCrane;
    public buttonsPc toDolphin;

}

[System.Serializable]
public struct standardI
{
    public standardInputsJ joyInputs;
    public standardInputsPc keyInputs;
}

[System.Serializable]
public struct frogInputsJ
{
    public buttonsJoy Jump;
    public buttonsJoy HorizontalFissure;
    public buttonsJoy toStd;
    public buttonsJoy toArma;
    public buttonsJoy toCrane;
    public buttonsJoy toDolphin;

}

[System.Serializable]
public struct frogInputsPc
{
    public buttonsPc Jump;
    public buttonsPc HorizontalFissure;
    public buttonsPc toStd;
    public buttonsPc toArma;
    public buttonsPc toCrane;
    public buttonsPc toDolphin;
}

[System.Serializable]
public struct frogI
{

    public frogInputsJ joyInputs;
    public frogInputsPc keyInputs;

}

[System.Serializable]
public struct armaInputsJ
{
    public buttonsJoy roll;
    public buttonsJoy rockMoving;
    public buttonsJoy toStd;
    public buttonsJoy toFrog;
    public buttonsJoy toCrane;
    public buttonsJoy toDolphin;
}

[System.Serializable]
public struct armaInputsPc
{
    public buttonsPc roll;
    public buttonsPc rockMoving;
    public buttonsPc toStd;
    public buttonsPc toFrog;
    public buttonsPc toCrane;
    public buttonsPc toDolphin;
}

[System.Serializable]
public struct armaI
{

    public armaInputsJ joyInputs;
    public armaInputsPc keyInputs;

}

[System.Serializable]
public struct craneInputsJ
{
    public buttonsJoy toStd;
    public buttonsJoy toFrog;
    public buttonsJoy toArma;
    public buttonsJoy toDolphin;
}

[System.Serializable]
public struct craneInputsPc
{
    public buttonsPc toStd;
    public buttonsPc toFrog;
    public buttonsPc toArma;
    public buttonsPc toDolphin;

}

[System.Serializable]
public struct craneI
{
    public craneInputsJ joyInputs;
    public craneInputsPc keyInputs;

}

[System.Serializable]
public struct dolphinInputsJ
{
    public buttonsJoy jump;
    public buttonsJoy moveBelow;
    public buttonsJoy toStd;
    public buttonsJoy toFrog;
    public buttonsJoy toArma;
    public buttonsJoy toCrane;
}

[System.Serializable]
public struct dolphinInputsPc
{
    public buttonsPc jump;
    public buttonsPc moveBelow;
    public buttonsPc toStd;
    public buttonsPc toFrog;
    public buttonsPc toArma;
    public buttonsPc toCrane;

}

[System.Serializable]
public struct dolphinI
{
    public dolphinInputsJ joyInputs;
    public dolphinInputsPc keyInputs;

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
#endregion

#region Form Tweaks Structs

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
public struct dolphinM
{
    public float swimSpeed;
    public float jumpStrength;

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
#endregion

[System.Serializable]
public struct generalTweaks
{
    public float globalGravity;
    [Range(0.5f, 5)]
    public float rotateSpeed;
   

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

        Physics.gravity = GeneralTweaks.globalGravity * Vector3.up;
    }


    public void ApplyingDesignTweaks(GameObject player)
    {
        FSMExecutor fsmExecutorTempLink = player.GetComponent<FSMExecutor>();

        fsmExecutorTempLink.currentMoveValues = GestioneMovimento;
        fsmExecutorTempLink.generalValues = GeneralTweaks;

        this.GetComponent<CameraManager>().currentPlCameraSettings = GestioneCamera;

       


        PlayerInputs playerInputsTempLink = player.GetComponent<PlayerInputs>();

        playerInputsTempLink.currentInputs = GestioneInputs;

        Physics.gravity = GeneralTweaks.globalGravity * Vector3.up;
    }

   

}



