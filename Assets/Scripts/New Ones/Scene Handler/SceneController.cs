using UnityEngine;
using System.Collections;

using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{

    public event_float ProgressUpdateRequest;
     
    public GameObject GameLoader;

    public float standardLoadingTime;

    private string sceneToLoad;

    public Scene[] scenes;

    private AsyncOperation loadingStatus;

    public UnityEvent stoppingLogicRequest;

    #region Taking References and linking Events
    void Awake()
    {
        MenuManager mmTempLink = this.GetComponent<MenuManager>();

        mmTempLink.switchSceneRequestByInt.AddListener(LoadingScenebyIndex);
        mmTempLink.switchSceneRequestByName.AddListener(LoadingScenebyName);
        mmTempLink.loadingSceneRequest.AddListener(this.NgpInitializer);
        mmTempLink.changingSceneRequest.AddListener(ChangingScenehandler);

        GameController gcTempLink = this.GetComponent<GameController>();

        gcTempLink.gpInitializer.AddListener(GamePlayInitialization);
        //gcTempLink.ngpInitializer.AddListener(NgpInitializer);

        //this.SavingSceneRef();

    }


    private void SavingSceneRef()
    {

        if (SceneManager.sceneCount > 0)
        {
            Debug.Log(SceneManager.sceneCount);
            this.scenes = new Scene[SceneManager.sceneCount];

            for (int n = 0; n < SceneManager.sceneCount; ++n)
            {
                this.scenes[n] = SceneManager.GetSceneAt(n);
                Debug.Log(this.scenes[n]);
                Debug.Log("Ciao");
            }
        }
    }

    #endregion

    #region Game Starter Una Tantum Initialization
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            SceneManager.LoadScene(1);
    }
    #endregion

    #region Gameplay Intialization
    private void GamePlayInitialization(GameObject player)
    {
        FSMChecker fsmTempLink = player.GetComponent<FSMChecker>();

      //  fsmTempLink.deathRequest.AddListener(ResettingCurrentScene);

        MoveHandler mhTempLink = player.GetComponent<MoveHandler>();

     //   mhTempLink.deathRequest.AddListener(ResettingCurrentScene);


        PlayerInputs plTempLink = player.GetComponent<PlayerInputs>();

        plTempLink.mainMenuRequest.AddListener(LoadingScenebyIndex);
        plTempLink.nextSceneRequest.AddListener(LoadingNextScene);
        plTempLink.previousSceneRequest.AddListener(LoadingPreviousScene);
        plTempLink.resettingSceneRequest.AddListener(ResettingCurrentScene);

        plTempLink.switchSceneRequest.AddListener(this.ChangingScenehandler);

        var changeLevTempLink = GameObject.FindGameObjectsWithTag("ChangeScene");

        foreach (var t in changeLevTempLink)
        {
            t.GetComponent<MoveToNextLevel>().SceneChangeRequest.AddListener(this.ChangingScenehandler);
        }



    }
    #endregion

    #region No Gameplay Initialization

    private void NgpInitializer()
    {
        if (SceneManager.GetActiveScene().buildIndex == 11) StartCoroutine(LoadingNewScene(this.sceneToLoad));
    }

    private IEnumerator LoadingNewScene(string sceneToLoad)
    {
      
        this.loadingStatus = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
      
        this.loadingStatus.allowSceneActivation = false;

        var ciccioRef = SceneManager.GetSceneByName(sceneToLoad);
        var loadingRef = SceneManager.GetSceneByName("LoadingScreen");

        float timer = 0;



        while (timer <= this.standardLoadingTime || this.loadingStatus.progress < 0.9f)
        {
            timer += Time.deltaTime;
            this.ProgressUpdateRequest.Invoke(Mathf.InverseLerp(0, this.standardLoadingTime, timer));

            yield return null;
        }

        this.loadingStatus.allowSceneActivation = true;

        while (!this.loadingStatus.isDone)
        {
            //this.ProgressUpdateRequest.Invoke(this.loadingStatus.progress);
            yield return null;
        }
            

        Debug.Log("GamePlay Scene Loaded");

        //loadingStatus.allowSceneActivation = true;
        SceneManager.SetActiveScene(ciccioRef);
        SceneManager.UnloadScene(loadingRef);

        Instantiate(this.GameLoader);

        //SceneManager.LoadScene(sceneToLoad);

    }
    #endregion

    #region Scene Switch GeneraL Methods
    private void LoadingScenebyName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void LoadingScenebyIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    private void LoadingNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void LoadingPreviousScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    private void ResettingCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ChangingScenehandler(string sceneName)
    {
        var sceneIndex = SceneManager.GetSceneByName(sceneName).buildIndex;


        if (sceneName == "Main Menu")
        {
            this.stoppingLogicRequest.Invoke();
            this.LoadingScenebyName(sceneName);
        }
        else if (sceneName == "LoadingScreen")
        {
            Debug.LogWarning(
                "You Cannot load Loading Screen directly, please change the scene Name with the a gameplay or menu scene");
        }
        else
        {
            this.stoppingLogicRequest.Invoke();
            this.sceneToLoad = sceneName;
            SceneManager.LoadScene("LoadingScreen");
        }
    }


    #endregion

}
