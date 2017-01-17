using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnvManager : MonoBehaviour {

    public Transform[] specialObjects;


    void Awake()
    {
        GameController gcTempLink = this.GetComponent<GameController>();
        gcTempLink.gpInitializer.AddListener(GameplayInitialization);
    }

    private void GameplayInitialization(GameObject player)
    {

        specialObjects = GameObject.FindGameObjectWithTag("...").GetComponentsInChildren<Transform>();

        

    }
}
