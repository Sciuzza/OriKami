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
    }
    #endregion

    #region Sending the Scene Initialization Order
    void Start()
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
    