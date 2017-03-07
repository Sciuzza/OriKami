using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

using VolumetricFogAndMist;

public class GameController : MonoBehaviour
{
    public Camera cameraRef;
    public GameObject dLight;
    private AudioClip audioRef;
    private SoundManager soundRef;
    private AudioSource audioSourceRef;
    public string currentScene;
    private AudioListener audioListenerRef;


    #region Private Variables
    private GameObject player; 
    #endregion

    #region Event Variables
    public event_Gb gpInitializer, gameSettingsChanged;

    public UnityEvent ngpInitializer;
    public UnityEvent requestErase;
    #endregion

    #region Do not Destroy Behaviour
    void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
        //DontDestroyOnLoad(cameraRef.gameObject);
        //DontDestroyOnLoad(this.dLight);

        soundRef = GameObject.FindGameObjectWithTag("GameController").GetComponent<SoundManager>();
        audioListenerRef = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioListener>();
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "Game Starter")
            this.requestErase.Invoke();
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

            //this.SettingDLight();

            gpInitializer.Invoke(player);

            #region Background Music

            Scene scene = SceneManager.GetActiveScene();
            StopAllCoroutines();
         
            if (scene.name == "Game Starter" || scene.name == "Main Menu")
            {
                audioListenerRef.enabled = true;
                SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main Menu"));
                
            }

            else {
                audioListenerRef.enabled = false;
            }           

            if (scene.name=="Frogs' Village")
            {
                soundRef.PersistendAudio[0].AudioSourceRef.Stop();
                soundRef.PersistendAudio[1].AudioSourceRef.Stop();

                StartCoroutine(FrogVillageSoundTrack());
            }

            else if (scene.name == "Dolphins and Swallows' Village")
            {
                soundRef.PersistendAudio[0].AudioSourceRef.Stop();
                soundRef.PersistendAudio[1].AudioSourceRef.Stop();
                StartCoroutine(DolphinVillageSoundTrackCO());
            }

            else if (scene.name == "Armadillos' Village")
            {
                soundRef.PersistendAudio[0].AudioSourceRef.Stop();
                soundRef.PersistendAudio[1].AudioSourceRef.Stop();
                StartCoroutine(ArmadilloVillageSoundTrackCO());
            }

            else if (scene.name == "Main Menu")
            {
                Debug.Log("DIO DIO DIO DIO ");
                soundRef.PlaySound(0, 6);
            }

            else if (scene.name == "Route 1" || scene.name == "Route 2" || scene.name == "route 3" || scene.name == "Route 4")
            {
                soundRef.PersistendAudio[0].AudioSourceRef.Stop();
                soundRef.PersistendAudio[1].AudioSourceRef.Stop();
                StartCoroutine(RouteSoundTrackCO());
            }

            else if (scene.name == "Dragon's Spring Temple")
            {
                soundRef.PersistendAudio[0].AudioSourceRef.Stop();
                soundRef.PersistendAudio[1].AudioSourceRef.Stop();
                StartCoroutine(DragonTempleSoundTrackCO());
            }

            #endregion

            StartCoroutine(this.player.GetComponent<MoveHandler>().MoveHandlerUpdate());
            this.SettingCameraImageEffect();
        }
        else
        {
            Debug.Log("Not on Gameplay Scene");
            this.cameraRef.GetComponent<AudioListener>().enabled = true;
            ngpInitializer.Invoke();
        }
    }

    #region Coroutines for SoundTracks

    IEnumerator FrogVillageSoundTrack()
    {
        soundRef.PlaySound(1, 6);
        yield return new WaitForSeconds(soundRef.PersistendAudio[1].AudioSourceRef.clip.length);
        soundRef.PlaySound(0, 1);
    }
    IEnumerator DolphinVillageSoundTrackCO()
    {
        soundRef.PlaySound(1, 6);
        yield return new WaitForSeconds(soundRef.PersistendAudio[1].AudioSourceRef.clip.length);
        soundRef.PlaySound(0, 2);
    }
    IEnumerator ArmadilloVillageSoundTrackCO()
    {
        soundRef.PlaySound(1, 6);
        yield return new WaitForSeconds(soundRef.PersistendAudio[1].AudioSourceRef.clip.length);
        soundRef.PlaySound(0, 3);
    }
    IEnumerator RouteSoundTrackCO()
    {
        soundRef.PlaySound(1, 5);
        yield return new WaitForSeconds(soundRef.PersistendAudio[1].AudioSourceRef.clip.length);

        soundRef.PlaySound(0, 4);
    }
    IEnumerator DragonTempleSoundTrackCO()
    {
        soundRef.PlaySound(1, 7);
        yield return new WaitForSeconds(soundRef.PersistendAudio[1].AudioSourceRef.clip.length);
        soundRef.PlaySound(0, 5);
    }

    IEnumerator StopOverlapingCO()
    {
        yield return new WaitForSeconds(soundRef.PersistendAudio[1].AudioSourceRef.clip.length);
    }
    

    #endregion

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

    private void SettingCameraImageEffect()
    {
        VolumetricFog vfTempLink = Camera.main.GetComponent<VolumetricFog>();

        switch (SceneManager.GetActiveScene().name)
        {
            case "Route 1":
                break;
            case "Route 2":
                break;
            case "route 3":
                break;
            case "Route 4":
                break;
            case "Armadillos' Village":
                break;
            case "Dolphins and Swallows' Village":
                break;
            case "Dragon's Spring Temple":
                break;
            case "Frogs' Village":
                break;
        }
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
