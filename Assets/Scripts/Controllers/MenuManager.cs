using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{


    private int sceneIndex;
    private int sceneToLoad = 0;

    GameController gcTempLink;

    void Awake()
    {
        //this.GetComponent<GameController>().initializer.AddListener(GettingSceneIndex);

        //gcTempLink = this.GetComponent<GameController>();

        //gcTempLink.initializer.AddListener(Initialization);
    }

    void Update()
    {
        SkipLevel();
    }

    #region Old
    private void Initialization(GameObject player)
    {
        EnvInputs envInputsTempLink = player.GetComponent<EnvInputs>();
        envInputsTempLink.playerIsDead.AddListener(ResettingScene);
    }

    public void SceneLoader(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

    }

    private void ResettingScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void GettingSceneIndex()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void SkipLevel()
    {
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        else if (Input.GetKeyDown(KeyCode.PageDown))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

        }
        else if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            GameStarter();
        }
        else if (Input.GetKeyDown(KeyCode.End))
        {
            SceneManager.LoadScene("Proto Level Selection");

        }

    }

    public void GameStarter()
    {
       // SceneManager.LoadScene("Proto Main Menu");


    } 
    #endregion

    #region Main Menu Methods

    #endregion


}
