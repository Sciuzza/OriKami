using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    private string currentScene;

    #region Public Variables
    public event_int switchSceneRequestByInt;
    public event_string switchSceneRequestByName;

    public event_int_int soundRequest;
    #endregion

    #region Private Variables
    private MainMenuRepo mmRepo;
    private Button newGame, continueB, legends, options, exit;

    private GameObject levelSelPanel;
    private Button route1, frogsV, route2, armaV, route3, dolphinsV, route4, back;

    private Image progressBar;

    private EventSystem esLink;


    private HudRefRepo gpUiRef;
    #endregion

    #region Public variables
    public UnityEvent loadingSceneRequest;
    public event_string changingSceneRequest;
    public UnityEvent movieEndNotification;
    public UnityEvent newDataRequest, loadDataRequest;
    #endregion

    #region Taking References and linking Events
    void Awake()
    {
        var gcTempLink = this.GetComponent<GameController>();

        gcTempLink.ngpInitializer.AddListener(this.InitializingNgpScene);
        gcTempLink.gpInitializer.AddListener(this.InitializingGpScene);


        var scTempLink = this.GetComponent<SceneController>();

        scTempLink.ProgressUpdateRequest.AddListener(this.UpdatingProgressBar);

        var sdManTempLink = this.gameObject.GetComponent<SuperDataManager>();

        sdManTempLink.DisableContinueRequest.AddListener(this.DisablingContinue);
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
            case 11:
                this.InitializingLoadingScreen();
                break;
        }
    }
    #endregion

    #region Main Menu Handler
    public void InitializingMainMenuPanel()
    {
        this.mmRepo = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<MainMenuRepo>();

        this.mmRepo.NewGameB.onClick.AddListener(this.PlayNewGameSound);
        this.mmRepo.NewGameB.onClick.AddListener(this.SendingNewDataRequestEvent);

        this.mmRepo.ContinueB.onClick.AddListener(this.PlayNewGameSound);
        this.mmRepo.ContinueB.onClick.AddListener(this.SendingLoadDataRequestEvent);

        //this.mmRepo.LegendsB.onClick.AddListener(this.PlayNewGameSound);
        //this.mmRepo.LegendsB.onClick.AddListener(this.PlayNewGameSound);

        //this.mmRepo.OptionsB.onClick.AddListener(this.PlayNewGameSound);
        //this.mmRepo.OptionsB.onClick.AddListener(this.PlayNewGameSound);

        this.mmRepo.ExitB.onClick.AddListener(this.PlayNewGameSound);
        this.mmRepo.ExitB.onClick.AddListener(this.QuitGame);

    }

    private void DisablingContinue()
    {
        this.continueB.interactable = false;
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    public void PlayNewGameSound()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<SoundManager>().PlaySound(1, 0);
    }

    public void PlayBackSound()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<SoundManager>().PlaySound(1, 2);
    }

    public void SendingNewDataRequestEvent()
    {
        this.newDataRequest.Invoke();
    }

    public void SendingLoadDataRequestEvent()
    {
        this.loadDataRequest.Invoke();
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
        route1.onClick.AddListener(PlayNewGameSound);
        frogsV.onClick.AddListener(InvokingFrogsV);
        frogsV.onClick.AddListener(PlayNewGameSound);
        route2.onClick.AddListener(InvokingRoute2);
        route2.onClick.AddListener(PlayNewGameSound);
        armaV.onClick.AddListener(InvokingArmaV);
        armaV.onClick.AddListener(PlayNewGameSound);
        route3.onClick.AddListener(InvokingRoute3);
        route3.onClick.AddListener(PlayNewGameSound);
        dolphinsV.onClick.AddListener(InvokingDolphinsV);
        dolphinsV.onClick.AddListener(PlayNewGameSound);
        route4.onClick.AddListener(InvokingRoute4);
        route4.onClick.AddListener(PlayNewGameSound);
        back.onClick.AddListener(InvokingMainMenu);
        back.onClick.AddListener(PlayBackSound);
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
        this.gpUiRef = GameObject.FindGameObjectWithTag("Gameplay Ui").GetComponent<HudRefRepo>();

        var storyLineCheck = GameObject.FindGameObjectWithTag("StoryLine");

        if (storyLineCheck == null) return;
        var slTempLink = GameObject.FindGameObjectWithTag("StoryLine").GetComponent<StoryLineInstance>();
        slTempLink.UiDialogueRequest.AddListener(this.PoppingOutDialogue);
        slTempLink.UiDialogueWILRequest.AddListener(this.PoppingOutDialogueWIL);
        slTempLink.DialogueEnded.AddListener(this.ResettingDialogue);
        slTempLink.MovieRequest.AddListener(this.PoppingOutMovie);
    }

    private void PoppingOutDialogue(string name, string label, string sentence, string spritename)
    {
        this.gpUiRef.Dialogue.SetActive(true);

        if (label == "Right")
        {
            this.gpUiRef.RightLabel.SetActive(true);
            this.gpUiRef.LeftLabel.SetActive(false);
            this.gpUiRef.leftImageGb.SetActive(false);
            this.gpUiRef.RightLabelT.text = name;
            this.gpUiRef.rightImageGb.SetActive(true);

            if (this.gpUiRef.AvatarNames.Contains(spritename))
                this.gpUiRef.rightImage.sprite = this.gpUiRef.Avatars[this.gpUiRef.AvatarNames.IndexOf(spritename)];
        }
        else
        {
            this.gpUiRef.LeftLabel.SetActive(true);
            this.gpUiRef.RightLabel.SetActive(false);
            this.gpUiRef.rightImageGb.SetActive(false);
            this.gpUiRef.LeftLabelT.text = name;
            this.gpUiRef.leftImageGb.SetActive(true);

            if (this.gpUiRef.AvatarNames.Contains(spritename))
                this.gpUiRef.leftImage.sprite = this.gpUiRef.Avatars[this.gpUiRef.AvatarNames.IndexOf(spritename)];
        }

        this.gpUiRef.DialogueT.text = sentence;
    }

    private void PoppingOutDialogueWIL(string sentence)
    {
        this.gpUiRef.Dialogue.SetActive(true);

        this.gpUiRef.RightLabel.SetActive(false);
        this.gpUiRef.LeftLabel.SetActive(false);
        this.gpUiRef.leftImageGb.SetActive(false);
        this.gpUiRef.rightImageGb.SetActive(false);

        this.gpUiRef.DialogueT.text = sentence;
    }

    private void ResettingDialogue()
    {
        this.gpUiRef.RightLabel.SetActive(false);
        this.gpUiRef.LeftLabel.SetActive(false);
        this.gpUiRef.Dialogue.SetActive(false);
        this.gpUiRef.leftImageGb.SetActive(false);
        this.gpUiRef.rightImageGb.SetActive(false);
    }

    private void PoppingOutMovie(int movieIndex, float smoothingAlpha)
    {
        if (movieIndex < this.gpUiRef.MovieRef.Movie.Length)
        {
            this.gpUiRef.MovieRef.ImLink.texture = this.gpUiRef.MovieRef.Movie[movieIndex] as MovieTexture;
            this.gpUiRef.MovieRef.AudioLink.clip = this.gpUiRef.MovieRef.Audio[movieIndex];

            this.gpUiRef.MovieRef.Movie[movieIndex].Play();
            this.gpUiRef.MovieRef.AudioLink.Play();
            this.StartCoroutine(this.MovieCheck(movieIndex, smoothingAlpha));
        }
        else
        {
            Debug.Log("Movie can't be played");
            this.movieEndNotification.Invoke();
        }
    }

    private IEnumerator MovieCheck(int movieIndex, float smoothingAlpha)
    {
        var colorLink = this.gpUiRef.MovieRef.ImLink.color;

        var alphaDelta = 1 / smoothingAlpha;

        while (colorLink.a < 1)
        {
            colorLink.a += alphaDelta * Time.deltaTime;
            this.gpUiRef.MovieRef.ImLink.color = colorLink;
            yield return null;
        }

        colorLink.a = 1;
        this.gpUiRef.MovieRef.ImLink.color = colorLink;

        while (this.gpUiRef.MovieRef.Movie[movieIndex].isPlaying)
        {
            yield return null;
        }

        while (colorLink.a > 0)
        {
            colorLink.a -= alphaDelta * Time.deltaTime;
            this.gpUiRef.MovieRef.ImLink.color = colorLink;
            yield return null;
        }

        colorLink.a = 0;
        this.gpUiRef.MovieRef.ImLink.color = colorLink;

        this.movieEndNotification.Invoke();

    }
    #endregion

    #region Loading Screen Handler
    private void InitializingLoadingScreen()
    {
        this.progressBar = GameObject.FindGameObjectWithTag("ProgressBar").GetComponent<Image>();
        this.loadingSceneRequest.Invoke();
    }

    private void UpdatingProgressBar(float value)
    {
        this.progressBar.fillAmount = value;
    }
    #endregion
}
