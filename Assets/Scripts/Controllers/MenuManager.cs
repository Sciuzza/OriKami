using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class MenuManager : MonoBehaviour
{


    #region Public Variables
    public event_int switchSceneRequestByInt;
    public event_string switchSceneRequestByName; 
    #endregion

    #region Private Variables
    private GameObject mainMenuPanel;
    private Button newGame, levelSel, quitGame;

    private GameObject levelSelPanel;
    private Button route1, frogsV, route2, armaV, route3, dolphinsV, route4, back; 
    #endregion

    #region Taking References and linking Events
    void Awake()
    {
        GameController gcTempLink = this.GetComponent<GameController>();

        gcTempLink.ngpInitializer.AddListener(InitializingNgpScene);
        gcTempLink.gpInitializer.AddListener(InitializingGpScene);
    }
    #endregion

    #region Menu Handling Methods
    private void InitializingNgpScene()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 1:
                InitializingMainMenuPanel();
                break;
            case 2:
                InitializingLevelSelection();
                break;
        }
    }

    #region Main Menu Handler
    public void InitializingMainMenuPanel()
    {
        mainMenuPanel = GameObject.FindGameObjectWithTag("Menu Panel");

        Button[] mmButtonRef = new Button[3];

        mmButtonRef = mainMenuPanel.GetComponentsInChildren<Button>();

        newGame = mmButtonRef[0];
        levelSel = mmButtonRef[1];
        quitGame = mmButtonRef[2];

        newGame.onClick.AddListener(InvokingNewGame);
        levelSel.onClick.AddListener(InvokingLevelSel);
        quitGame.onClick.AddListener(QuitGame);
    }

    private void InvokingNewGame()
    {
        switchSceneRequestByInt.Invoke(3);
    }

    private void InvokingLevelSel()
    {
        switchSceneRequestByInt.Invoke(2);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    public void PlayNewGameSound()
    {
        GameObject.FindGameObjectWithTag("Game Controller").GetComponent<SoundManager>().PlaySound(1,1);
    }
    #endregion

    #region Level Selection Handler
    private void InitializingLevelSelection()
    {
        levelSelPanel = GameObject.FindGameObjectWithTag("Proto Sel");

        Button[] mmButtonRef = new Button[8];

        mmButtonRef = levelSelPanel.GetComponentsInChildren<Button>();

        route1 = mmButtonRef[0];
        frogsV = mmButtonRef[1];
        route2 = mmButtonRef[2];
        armaV = mmButtonRef[3];
        route3 = mmButtonRef[4];
        dolphinsV = mmButtonRef[5];
        route4 = mmButtonRef[6];
        back = mmButtonRef[7];


        route1.onClick.AddListener(InvokingRoute1);
        frogsV.onClick.AddListener(InvokingFrogsV);
        route2.onClick.AddListener(InvokingRoute2);
        armaV.onClick.AddListener(InvokingArmaV);
        route3.onClick.AddListener(InvokingRoute3);
        dolphinsV.onClick.AddListener(InvokingDolphinsV);
        route4.onClick.AddListener(InvokingRoute4);
        back.onClick.AddListener(InvokingMainMenu);
    }

    private void InvokingRoute1()
    {
        switchSceneRequestByName.Invoke("Route 1");
    }

    private void InvokingFrogsV()
    {
        switchSceneRequestByName.Invoke("Frogs' Village");
    }

    private void InvokingRoute2()
    {
        switchSceneRequestByName.Invoke("Route 2");
    }

    private void InvokingArmaV()
    {
        switchSceneRequestByName.Invoke("Armadillos' Village");
    }

    private void InvokingRoute3()
    {
        switchSceneRequestByName.Invoke("Route 3");
    }

    private void InvokingDolphinsV()
    {
        switchSceneRequestByName.Invoke("Dolphins and Swallows' Village");
    }

    private void InvokingRoute4()
    {
        switchSceneRequestByName.Invoke("Route 4");
    }

    private void InvokingMainMenu()
    {
        switchSceneRequestByName.Invoke("Proto Main Menu");
    }

    #endregion

    #region Menu In Game Handler
    private void InitializingGpScene(GameObject player)
    {

    }
    #endregion 
    #endregion
}
