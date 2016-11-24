using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GameLoader : MonoBehaviour
{

    

    private GameObject gbTempLink;

    // Use this for initialization
    void Awake()
    {

        gbTempLink = GameObject.FindGameObjectWithTag("GameController");

        GameController gcTempLink = gbTempLink.GetComponent<GameController>();
        CameraManager cmTempLink = gbTempLink.GetComponent<CameraManager>();

       
        gcTempLink.InitializingScene();
        cmTempLink.InitializingCamera();
        
    }


}
    