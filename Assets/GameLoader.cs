using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GameLoader : MonoBehaviour
{

    private GameController gameControllerLinker;
    private CameraManager cameraManagerLinker;


    // Use this for initialization
    void Awake()
    {
        gameControllerLinker = FindObjectOfType<GameController>();
        cameraManagerLinker = FindObjectOfType<CameraManager>();

        if (cameraManagerLinker.tag == "GameController")
        {
            cameraManagerLinker.initPlayerDone = false;
        }
        gameControllerLinker.InitializingScene();
        cameraManagerLinker.InitializingCamera();
        
    }

    // Update is called once per frame
    void Update()
    {

    }

}
    