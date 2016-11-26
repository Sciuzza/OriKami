using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class MenuManager : MonoBehaviour
{
    private GameObject mainMenuPanel;
    private Button newGame, loadGame, options, credits, quitGame;

    [System.Serializable]
    public class ngEvent : UnityEvent<string>
    {
    }

    public ngEvent newGameRequest;

    void Awake()
    {
        GameController gcTempLink = this.GetComponent<GameController>();

        gcTempLink.ngpInitializer.AddListener(InitializingNgpScene);
        gcTempLink.gpInitializer.AddListener(InitializingGpScene);
    }

    private void InitializingNgpScene()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Main Menu":
                InitializingMainMenuPanel();
                break;
        }
    }

    private void InitializingMainMenuPanel()
    {
        mainMenuPanel = GameObject.FindGameObjectWithTag("Menu Panel");

        Button[] mmButtonRef = new Button[5];

        mmButtonRef = mainMenuPanel.GetComponentsInChildren<Button>();

        newGame = mmButtonRef[0];
        loadGame = mmButtonRef[1];
        options = mmButtonRef[2];
        credits = mmButtonRef[3];
        quitGame = mmButtonRef[4];


        newGame.onClick.AddListener(InvokingNewGame);

    }

    private void InvokingNewGame()
    {
        newGameRequest.Invoke("Cri Testing 2");
    }

    private void InitializingGpScene(GameObject player)
    {

    }
}
