using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    #region Taking References and linking Events
    void Awake()
    {
        MenuManager mmTempLink = this.GetComponent<MenuManager>();

        mmTempLink.switchSceneRequestByInt.AddListener(LoadingScenebyIndex);
        mmTempLink.switchSceneRequestByName.AddListener(LoadingScenebyName);

        GameController gcTempLink = this.GetComponent<GameController>();

        gcTempLink.gpInitializer.AddListener(GamePlayInitialization);

    }

    private void GamePlayInitialization(GameObject player)
    {
        FSMChecker fsmTempLink = player.GetComponent<FSMChecker>();
        fsmTempLink.deathRequest.AddListener(ResettingCurrentScene);

        MoveHandler mhTempLink = player.GetComponent<MoveHandler>();

        mhTempLink.deathRequest.AddListener(ResettingCurrentScene);

        PlayerInputs plTempLink = player.GetComponent<PlayerInputs>();

        plTempLink.mainMenuRequest.AddListener(LoadingScenebyIndex);
        plTempLink.nextSceneRequest.AddListener(LoadingNextScene);
        plTempLink.previousSceneRequest.AddListener(LoadingPreviousScene);
        plTempLink.resettingSceneRequest.AddListener(ResettingCurrentScene);

    }
    #endregion

    #region Game Starter Una Tantum Initialization
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            SceneManager.LoadScene(1);
    }
    #endregion

    #region Scene Switch Handler Methods
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
    #endregion
}
