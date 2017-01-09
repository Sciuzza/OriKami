﻿using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
    

{
    public Camera cameraRef;
    #region Private Variables
    private GameObject player; 
    #endregion

    #region Event Variables
    public event_Gb gpInitializer, gameSettingsChanged;

    public UnityEvent ngpInitializer;
    #endregion

    #region Do not Destroy Behaviour
    void Awake()
    {
        cameraRef = Camera.main;
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(cameraRef.gameObject);
    }
    #endregion

    #region In game Design Tweaks
    void Update()
    {
        if (Input.GetKeyDown("k"))
            gameSettingsChanged.Invoke(player);
    }
    #endregion

    #region Initialization Methods
    public void InitializingScene()
    {
        if (FindingPlayer())
        {
            Debug.Log("Initializer Invoked Once");
            gpInitializer.Invoke(player);
            StartCoroutine(this.player.GetComponent<MoveHandler>().MoveHandlerUpdate());

        }
        else
        {
            Debug.Log("Not on Gameplay Scene");
            ngpInitializer.Invoke();
        }
    }

    public bool FindingPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Missing Player in Scene");
            return false;
        }
        else
            return true;
    } 
    #endregion
}
