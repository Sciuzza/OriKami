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


        
    }
    void Start()
    {
        GameController gcTempLink = gbTempLink.GetComponent<GameController>();
        Debug.Log("Game Loader once");
        gcTempLink.InitializingScene();
    }

}
    