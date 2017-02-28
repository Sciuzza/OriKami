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
    private SoundManager playerRef;

    #region Public Variables
    public event_int switchSceneRequestByInt;
    public event_string switchSceneRequestByName;

    public event_int_int soundRequest;
    #endregion

    #region Private Variables
    private MainMenuRepo mmRepo;

    private GameObject levelSelPanel;
    private Button route1, frogsV, route2, armaV, route3, dolphinsV, route4, back;

    private Image progressBar;

    private EventSystem esLink;


    private HudRefRepo gpUiRef;

    private string sectionTempRef;
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
        //playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<SoundManager>();
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

        this.mmRepo.LegendsB.onClick.AddListener(this.PlayNewGameSound);
        this.mmRepo.LegendsB.onClick.AddListener(this.OpeningLegendsJournal);

        this.mmRepo.OptionsB.onClick.AddListener(this.PlayNewGameSound);
        this.mmRepo.OptionsB.onClick.AddListener(this.OpeningOptions);

        this.mmRepo.ExitB.onClick.AddListener(this.PlayNewGameSound);
        this.mmRepo.ExitB.onClick.AddListener(this.QuitGame);


        this.mmRepo.Legend1B.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.Legend1B.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);

        this.mmRepo.Legend2B.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.Legend2B.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);

        this.mmRepo.Legend3B.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.Legend3B.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);

        this.mmRepo.Legend4B.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.Legend4B.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);

        this.mmRepo.Legend5B.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.Legend5B.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);

        this.mmRepo.Legend6B.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.Legend6B.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);

        this.mmRepo.Legend7B.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.Legend7B.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);

        this.mmRepo.Legend8B.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.Legend8B.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);


        // Options Listeners on Select

        /*
        this.mmRepo.GameplayMenuB.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.GameplayMenuB.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);

        this.mmRepo.VideoMenuB.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.VideoMenuB.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);

        this.mmRepo.AudioMenuB.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.AudioMenuB.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);

        this.mmRepo.KeyBindingsMenuB.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.KeyBindingsMenuB.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);
        */

        this.mmRepo.GameplayMenuB.onClick.AddListener(() => this.ActivatingOptionsPanel(this.mmRepo.GameplayG, this.mmRepo.GameplayOptionsB.gameObject, "Gameplay"));
        this.mmRepo.VideoMenuB.onClick.AddListener(() => this.ActivatingOptionsPanel(this.mmRepo.VideoG, this.mmRepo.GraphicSettingsB.gameObject, "Graphics"));
        this.mmRepo.AudioMenuB.onClick.AddListener(() => this.ActivatingOptionsPanel(this.mmRepo.AudioG, this.mmRepo.MasterB.gameObject, "Audio"));
        this.mmRepo.KeyBindingsMenuB.onClick.AddListener(() => this.ActivatingOptionsPanel(this.mmRepo.KeyBindingsG, this.mmRepo.Form1B.gameObject, "Keys"));


        this.mmRepo.GameplayOptionsB.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.GameplayOptionsB.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);
        this.mmRepo.GameplayOptionsB.onClick.AddListener(() => this.ActivatingOptionSetting(this.mmRepo.LeftArrowGpoB.gameObject, this.mmRepo.RightArrowGpoB.gameObject, "Gp1"));

        this.mmRepo.CameraSpeedB.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.CameraSpeedB.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);
        this.mmRepo.CameraSpeedB.onClick.AddListener(() => this.ActivatingOptionSetting(this.mmRepo.LeftArrowCsB.gameObject, this.mmRepo.RightArrowCsB.gameObject, "Gp2"));


        this.mmRepo.GraphicSettingsB.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.GraphicSettingsB.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);
        this.mmRepo.GraphicSettingsB.onClick.AddListener(() => this.ActivatingOptionSetting(this.mmRepo.LeftArrowGsB.gameObject, this.mmRepo.RightArrowGsB.gameObject, "Gs1"));


        this.mmRepo.MasterB.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.MasterB.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);
        this.mmRepo.MasterB.onClick.AddListener(() => this.ActivatingOptionSetting(this.mmRepo.LeftArrowMsB.gameObject, this.mmRepo.RightArrowMsB.gameObject, "Audio1"));

        this.mmRepo.MusicB.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.MusicB.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);
        this.mmRepo.MusicB.onClick.AddListener(() => this.ActivatingOptionSetting(this.mmRepo.LeftArrowMuB.gameObject, this.mmRepo.RightArrowMuB.gameObject, "Audio2"));

        this.mmRepo.EffectsB.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.EffectsB.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);
        this.mmRepo.EffectsB.onClick.AddListener(() => this.ActivatingOptionSetting(this.mmRepo.LeftArrowEfB.gameObject, this.mmRepo.RightArrowEfB.gameObject, "Audio3"));


        this.mmRepo.Form1B.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.Form1B.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);
        this.mmRepo.Form1B.onClick.AddListener(() => this.ActivatingOptionSetting(this.mmRepo.LeftArrowF1B.gameObject, this.mmRepo.RightArrowF2B.gameObject, "Keys1"));

        this.mmRepo.Form2B.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.Form2B.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);
        this.mmRepo.Form2B.onClick.AddListener(() => this.ActivatingOptionSetting(this.mmRepo.LeftArrowF2B.gameObject, this.mmRepo.RightArrowF2B.gameObject, "Keys2"));

        this.mmRepo.Form3B.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.Form3B.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);
        this.mmRepo.Form3B.onClick.AddListener(() => this.ActivatingOptionSetting(this.mmRepo.LeftArrowF3B.gameObject, this.mmRepo.RightArrowF3B.gameObject, "Keys3"));

        this.mmRepo.Form4B.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.Form4B.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);
        this.mmRepo.Form4B.onClick.AddListener(() => this.ActivatingOptionSetting(this.mmRepo.LeftArrowF4B.gameObject, this.mmRepo.RightArrowF4B.gameObject, "Keys4"));

        this.mmRepo.StdFormB.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.StdFormB.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);
        this.mmRepo.StdFormB.onClick.AddListener(() => this.ActivatingOptionSetting(this.mmRepo.LeftArrowStB.gameObject, this.mmRepo.RightArrowStB.gameObject, "Keys5"));

        this.mmRepo.JumpDashB.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.JumpDashB.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);
        this.mmRepo.JumpDashB.onClick.AddListener(() => this.ActivatingOptionSetting(this.mmRepo.LeftArrowJdB.gameObject, this.mmRepo.RightArrowJdB.gameObject, "Keys6"));

        this.mmRepo.PasstB.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.PasstB.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);
        this.mmRepo.PasstB.onClick.AddListener(() => this.ActivatingOptionSetting(this.mmRepo.LeftArrowPtB.gameObject, this.mmRepo.RightArrowPtB.gameObject, "Keys7"));

        this.mmRepo.EpicViewB.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.EpicViewB.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);
        this.mmRepo.EpicViewB.onClick.AddListener(() => this.ActivatingOptionSetting(this.mmRepo.LeftArrowEwB.gameObject, this.mmRepo.RightArrowEwB.gameObject, "Keys8"));
    }


    private void DisablingContinue()
    {
        this.mmRepo.ContinueB.interactable = false;
    }

    private void OpeningLegendsJournal()
    {
        this.mmRepo.JournalG.SetActive(true);
        EventSystem.current.SetSelectedGameObject(this.mmRepo.Legend1B.gameObject);
        this.mmRepo.MainPageG.SetActive(false);
    }

    private void OpeningOptions()
    {
        this.mmRepo.OptionsG.SetActive(true);
        EventSystem.current.SetSelectedGameObject(this.mmRepo.GameplayMenuB.gameObject);
        this.mmRepo.GameplayG.SetActive(false);
        this.mmRepo.MainPageG.SetActive(false);
        this.sectionTempRef = "Out";
    }

    private void ActivatingGb(GameObject gbToActivate)
    {
        gbToActivate.SetActive(true);
    }

    private void ActivatingOptionsPanel(GameObject gbToActivate, GameObject activeButton, string section)
    {
        gbToActivate.SetActive(true);
        EventSystem.current.SetSelectedGameObject(activeButton);

        switch (section)
        {
            case "Gameplay":
                this.mmRepo.CsArrow.SetActive(false);
                break;
            case "Graphics":
                break;
            case "Audio":
                this.mmRepo.MuArrow.SetActive(false);
                this.mmRepo.EfArrow.SetActive(false);
                break;
            case "Keys":
                this.mmRepo.F2Arrow.SetActive(false);
                this.mmRepo.F3Arrow.SetActive(false);
                this.mmRepo.F4Arrow.SetActive(false);
                this.mmRepo.StdArrow.SetActive(false);
                this.mmRepo.JdArrow.SetActive(false);
                this.mmRepo.PtArrow.SetActive(false);
                this.mmRepo.EvArrow.SetActive(false);
                break;
        }

        this.sectionTempRef = section;
    }

    private void ActivatingOptionSetting(GameObject leftAr, GameObject rightAr, string innerSection)
    {
        leftAr.SetActive(true);
        rightAr.SetActive(true);
        EventSystem.current.SetSelectedGameObject(leftAr);
        this.sectionTempRef = innerSection;
    }

    private void DeActivatingGb(GameObject gbToDeActivate)
    {
        gbToDeActivate.SetActive(false);
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

    private void Update()
    {
        if (Input.GetButtonDown("B") && this.mmRepo.JournalG.activeInHierarchy)
        {
            this.mmRepo.MainPageG.SetActive(true);
            this.mmRepo.JournalG.SetActive(false);
            EventSystem.current.SetSelectedGameObject(this.mmRepo.LegendsB.gameObject);
        }
        else if (Input.GetButtonDown("B") && this.mmRepo.GameplayG.activeInHierarchy)
        {
            this.mmRepo.GameplayG.SetActive(false);
            EventSystem.current.SetSelectedGameObject(this.mmRepo.GameplayMenuB.gameObject);
            this.sectionTempRef = "Out";
        }
        else if (Input.GetButtonDown("B") && this.mmRepo.VideoG.activeInHierarchy)
        {
            this.mmRepo.VideoG.SetActive(false);
            EventSystem.current.SetSelectedGameObject(this.mmRepo.VideoMenuB.gameObject);
            this.sectionTempRef = "Out";
        }
        else if (Input.GetButtonDown("B") && this.mmRepo.AudioG.activeInHierarchy)
        {
            this.mmRepo.AudioG.SetActive(false);
            EventSystem.current.SetSelectedGameObject(this.mmRepo.AudioMenuB.gameObject);
            this.sectionTempRef = "Out";
        }
        else if (Input.GetButtonDown("B") && this.mmRepo.KeyBindingsG.activeInHierarchy)
        {
            this.mmRepo.KeyBindingsG.SetActive(false);
            EventSystem.current.SetSelectedGameObject(this.mmRepo.KeyBindingsMenuB.gameObject);
            this.sectionTempRef = "Out";
        }
        else if (Input.GetButtonDown("B") && this.mmRepo.OptionsG.activeInHierarchy)
        {
            this.mmRepo.MainPageG.SetActive(true);
            this.mmRepo.OptionsG.SetActive(false);
            EventSystem.current.SetSelectedGameObject(this.mmRepo.OptionsB.gameObject);
        }

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
        this.playerRef = player.GetComponent<SoundManager>();

        this.gpUiRef = GameObject.FindGameObjectWithTag("Gameplay Ui").GetComponent<HudRefRepo>();

        var storyLineCheck = GameObject.FindGameObjectWithTag("StoryLine");

        if (storyLineCheck == null) return;
        var slTempLink = GameObject.FindGameObjectWithTag("StoryLine").GetComponent<StoryLineInstance>();
        slTempLink.UiDialogueRequest.AddListener(this.PoppingOutDialogue);
        slTempLink.UiDialogueWILRequest.AddListener(this.PoppingOutDialogueWIL);
        slTempLink.DialogueEnded.AddListener(this.ResettingDialogue);
        slTempLink.MovieRequest.AddListener(this.PoppingOutMovie);

        var fsmTempLink = player.GetComponent<FSMChecker>();

        fsmTempLink.updateUiCollRequest.AddListener(this.UpdatingCollectibleNumber);
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

    private void UpdatingCollectibleNumber(int goldenCraneNumber)
    {
        this.gpUiRef.CollectibleValue.text = goldenCraneNumber.ToString();
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
