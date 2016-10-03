using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {


    private int sceneIndex;


    void Awake()
    {
        //this.GetComponent<GameController>().initializer.AddListener(GettingSceneIndex);
    }



    public void SceneLoader(string sceneName)
    {
        //SceneManager.LoadScene(sceneName);
              
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void GettingSceneIndex()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

}
