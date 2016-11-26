using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{


    void Awake()
    {
        MenuManager mmTempLink = this.GetComponent<MenuManager>();

        mmTempLink.newGameRequest.AddListener(LoadingScenebyName);

        GameController gcTempLink = this.GetComponent<GameController>();

        gcTempLink.gpInitializer.AddListener(GamePlayInitialization);
     
    }

    void Start()
    {
        SceneManager.LoadScene(1);
    }

    void Update()
    {
        SkipLevel();
    }

    private void GamePlayInitialization(GameObject player)
    {
        FSMChecker fsmTempLink = player.GetComponent<FSMChecker>();
        fsmTempLink.deathRequest.AddListener(ResettingCurrentScene);

        MoveHandler mhTempLink = player.GetComponent<MoveHandler>();

        mhTempLink.deathRequest.AddListener(ResettingCurrentScene);

    }

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

    private void SkipLevel()
    {
        if (Input.GetKeyDown(KeyCode.PageUp))
            LoadingNextScene();
        else if (Input.GetKeyDown(KeyCode.PageDown))
            LoadingPreviousScene();
        else if (Input.GetKeyDown(KeyCode.End))
            ResettingCurrentScene();
    }

    /*
#region Old
private void Initialization(GameObject player)
{
    EnvInputs envInputsTempLink = player.GetComponent<EnvInputs>();
    envInputsTempLink.playerIsDead.AddListener(ResettingScene);
}






private void GettingSceneIndex()
{
    sceneIndex = SceneManager.GetActiveScene().buildIndex;
}

public void SkipLevel()
{
   
    else if (SceneManager.GetActiveScene().buildIndex == 0)
    {
        GameStarter();
    }
    else if (Input.GetKeyDown(KeyCode.End))
    {
        SceneManager.LoadScene("Proto Level Selection");

    }

}




*/
}
