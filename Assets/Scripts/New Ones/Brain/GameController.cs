using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Camera cameraRef;
    public GameObject dLight;
  
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
        
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(cameraRef.gameObject);
        DontDestroyOnLoad(this.dLight);
        
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

            this.SettingDLight();

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

    private void SettingDLight()
    {
        var dLightInScene = GameObject.FindGameObjectWithTag("DLight");

        this.dLight.transform.rotation = dLightInScene.transform.rotation;

        var dLightComp = this.dLight.GetComponent<Light>();
        var dLightInSceneComp = dLightInScene.GetComponent<Light>();

        dLightComp.type = dLightInSceneComp.type;
        dLightComp.bakedIndex = dLightInSceneComp.bakedIndex;
        dLightComp.color = dLightInSceneComp.color;
        dLightComp.intensity = dLightInSceneComp.intensity;
        dLightComp.bounceIntensity = dLightInSceneComp.bounceIntensity;
        dLightComp.shadows = dLightInSceneComp.shadows;
        dLightComp.shadowStrength = dLightInSceneComp.shadowStrength;
        dLightComp.shadowBias = dLightInSceneComp.shadowBias;
        dLightComp.shadowNormalBias = dLightInSceneComp.shadowNormalBias;
        dLightComp.shadowNearPlane = dLightInSceneComp.shadowNearPlane;

        dLightInScene.SetActive(false);
    }
    #endregion

    #region Debugging Static Methods
    public static void Debugging(string whatToSay)
    {
        Debug.Log(whatToSay);
    }

    public static void Debugging(string WhatToSay, int integerNum)
    {
        Debug.Log(WhatToSay + " :" + integerNum);
    }

    public static void Debugging(string WhatToSay, float floatNum)
    {
        Debug.Log(WhatToSay + " :" + floatNum);
    }
    #endregion
}
