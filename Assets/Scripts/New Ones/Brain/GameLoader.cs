using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GameLoader : MonoBehaviour
{
    //Game Controller Reference
    private GameObject gbTempLink;

    #region Taking Brain Reference
    void Awake()
    {
        gbTempLink = GameObject.FindGameObjectWithTag("GameController");
        Application.targetFrameRate = 60;
        //gbTempLink.GetComponent<GameController>().cameraRef.
    }
    #endregion

    #region Sending the Scene Initialization Order
    private void Start()
    {
        if (this.gbTempLink != null)
        {
            GameController gcTempLink = gbTempLink.GetComponent<GameController>();
            Debug.Log("Game Loader");
            gcTempLink.InitializingScene();
        }
        else
        {
            Debug.Log("Game Brain Missing");
        }
    } 
    #endregion
}