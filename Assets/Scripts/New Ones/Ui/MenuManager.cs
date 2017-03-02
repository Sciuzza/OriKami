using System;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

#if UNITY_STANDALONE_WIN
using WindowsInput;
#endif


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

    private GameObject levelSelPanel;
    private Button route1, frogsV, route2, armaV, route3, dolphinsV, route4, back;

    private Image progressBar;

    private EventSystem esLink;

    private SoundManager audioSourceRef;

    private HudRefRepo gpUiRef;

    private string sectionTempRef;
    #endregion

    #region Public variables
    public UnityEvent loadingSceneRequest;
    public event_string changingSceneRequest;
    public UnityEvent movieEndNotification;
    public UnityEvent newDataRequest, loadDataRequest, SaveDataRequest;



    public float MinCamValue;
    public float MaxCamValue;

    public string[] Difficulty;
    public string[] Quality;
    public string[] PossibleFormKeys;
    public string[] PossibleGenAbiKeys;

    public TweakableSettings LocalGbSettings;


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
        sdManTempLink.menuInitRequest.AddListener(this.InitializingMainMenuPanel);
    }
    #endregion

    #region Menu Handling Methods
    private void InitializingNgpScene()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 10:
                this.InitializingLoadingScreen();
                break;
        }
    }
    #endregion

    #region Main Menu Handler
    public void InitializingMainMenuPanel()
    {
        this.mmRepo = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<MainMenuRepo>();
        this.sectionTempRef = "MainMenu";
        this.LocalGbSettings = this.gameObject.GetComponent<SuperDataManager>().TwkSettings;

        this.InitializingMainMenu();
        this.InitializingJournal();
        this.InitializingOptions();
        this.InitializingValues();

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
    }

    private void InitializingMainMenu()
    {
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
    }

    private void InitializingJournal()
    {
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
    }

    private void InitializingOptions()
    {
        this.mmRepo.GameplayMenuB.onClick.AddListener(() => this.ActivatingOptionsPanel(this.mmRepo.GameplayG, this.mmRepo.GameplayOptionsB.gameObject, "OGameplay"));
        this.mmRepo.VideoMenuB.onClick.AddListener(() => this.ActivatingOptionsPanel(this.mmRepo.VideoG, this.mmRepo.GraphicSettingsB.gameObject, "OGraphics"));
        this.mmRepo.AudioMenuB.onClick.AddListener(() => this.ActivatingOptionsPanel(this.mmRepo.AudioG, this.mmRepo.MasterB.gameObject, "OAudio"));
        this.mmRepo.KeyBindingsMenuB.onClick.AddListener(() => this.ActivatingOptionsPanel(this.mmRepo.KeyBindingsG, this.mmRepo.Form1B.gameObject, "OKeys"));


        this.mmRepo.GameplayOptionsB.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.GameplayOptionsB.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);
        this.mmRepo.GameplayOptionsB.onClick.AddListener(() => this.ActivatingOptionSetting(this.mmRepo.LeftArrowGpoB.gameObject, this.mmRepo.RightArrowGpoB.gameObject, "OGpFirst"));
        this.mmRepo.LeftArrowGpoB.onClick.AddListener(() => this.SettingsHandler(-1));
        this.mmRepo.RightArrowGpoB.onClick.AddListener(() => this.SettingsHandler(1));

        this.mmRepo.CameraSpeedB.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.CameraSpeedB.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);
        this.mmRepo.CameraSpeedB.onClick.AddListener(() => this.ActivatingOptionSetting(this.mmRepo.LeftArrowCsB.gameObject, this.mmRepo.RightArrowCsB.gameObject, "OGpSecond"));
        this.mmRepo.LeftArrowCsB.onClick.AddListener(() => this.SettingsHandler(-1));
        this.mmRepo.RightArrowCsB.onClick.AddListener(() => this.SettingsHandler(1));


        this.mmRepo.GraphicSettingsB.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.GraphicSettingsB.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);
        this.mmRepo.GraphicSettingsB.onClick.AddListener(() => this.ActivatingOptionSetting(this.mmRepo.LeftArrowGsB.gameObject, this.mmRepo.RightArrowGsB.gameObject, "OGrFirst"));
        this.mmRepo.LeftArrowGsB.onClick.AddListener(() => this.SettingsHandler(-1));
        this.mmRepo.RightArrowGsB.onClick.AddListener(() => this.SettingsHandler(1));


        this.mmRepo.MasterB.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.MasterB.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);
        this.mmRepo.MasterB.onClick.AddListener(() => this.ActivatingOptionSetting(this.mmRepo.LeftArrowMsB.gameObject, this.mmRepo.RightArrowMsB.gameObject, "OAdFirst"));
        this.mmRepo.LeftArrowMsB.onClick.AddListener(() => this.SettingsHandler(-1));
        this.mmRepo.RightArrowMsB.onClick.AddListener(() => this.SettingsHandler(1));

        this.mmRepo.MusicB.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.MusicB.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);
        this.mmRepo.MusicB.onClick.AddListener(() => this.ActivatingOptionSetting(this.mmRepo.LeftArrowMuB.gameObject, this.mmRepo.RightArrowMuB.gameObject, "OAdSecond"));
        this.mmRepo.LeftArrowMuB.onClick.AddListener(() => this.SettingsHandler(-1));
        this.mmRepo.RightArrowMuB.onClick.AddListener(() => this.SettingsHandler(1));

        this.mmRepo.EffectsB.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.EffectsB.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);
        this.mmRepo.EffectsB.onClick.AddListener(() => this.ActivatingOptionSetting(this.mmRepo.LeftArrowEfB.gameObject, this.mmRepo.RightArrowEfB.gameObject, "OAdThird"));
        this.mmRepo.LeftArrowEfB.onClick.AddListener(() => this.SettingsHandler(-1));
        this.mmRepo.RightArrowEfB.onClick.AddListener(() => this.SettingsHandler(1));


        this.mmRepo.Form1B.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.Form1B.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);
        this.mmRepo.Form1B.onClick.AddListener(() => this.ActivatingOptionSetting(this.mmRepo.LeftArrowF1B.gameObject, this.mmRepo.RightArrowF1B.gameObject, "OKeFirst"));
        this.mmRepo.LeftArrowF1B.onClick.AddListener(() => this.SettingsHandler(-1));
        this.mmRepo.RightArrowF1B.onClick.AddListener(() => this.SettingsHandler(1));

        this.mmRepo.Form2B.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.Form2B.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);
        this.mmRepo.Form2B.onClick.AddListener(() => this.ActivatingOptionSetting(this.mmRepo.LeftArrowF2B.gameObject, this.mmRepo.RightArrowF2B.gameObject, "OKeSecond"));
        this.mmRepo.LeftArrowF2B.onClick.AddListener(() => this.SettingsHandler(-1));
        this.mmRepo.RightArrowF2B.onClick.AddListener(() => this.SettingsHandler(1));

        this.mmRepo.Form3B.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.Form3B.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);
        this.mmRepo.Form3B.onClick.AddListener(() => this.ActivatingOptionSetting(this.mmRepo.LeftArrowF3B.gameObject, this.mmRepo.RightArrowF3B.gameObject, "OKeThird"));
        this.mmRepo.LeftArrowF3B.onClick.AddListener(() => this.SettingsHandler(-1));
        this.mmRepo.RightArrowF3B.onClick.AddListener(() => this.SettingsHandler(1));

        this.mmRepo.Form4B.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.Form4B.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);
        this.mmRepo.Form4B.onClick.AddListener(() => this.ActivatingOptionSetting(this.mmRepo.LeftArrowF4B.gameObject, this.mmRepo.RightArrowF4B.gameObject, "OKeFourth"));
        this.mmRepo.LeftArrowF4B.onClick.AddListener(() => this.SettingsHandler(-1));
        this.mmRepo.RightArrowF4B.onClick.AddListener(() => this.SettingsHandler(1));

        this.mmRepo.StdFormB.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.StdFormB.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);
        this.mmRepo.StdFormB.onClick.AddListener(() => this.ActivatingOptionSetting(this.mmRepo.LeftArrowStB.gameObject, this.mmRepo.RightArrowStB.gameObject, "OKeFifth"));
        this.mmRepo.LeftArrowStB.onClick.AddListener(() => this.SettingsHandler(-1));
        this.mmRepo.RightArrowStB.onClick.AddListener(() => this.SettingsHandler(1));

        this.mmRepo.JumpDashB.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.JumpDashB.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);
        this.mmRepo.JumpDashB.onClick.AddListener(() => this.ActivatingOptionSetting(this.mmRepo.LeftArrowJdB.gameObject, this.mmRepo.RightArrowJdB.gameObject, "OKeSixth"));
        this.mmRepo.LeftArrowJdB.onClick.AddListener(() => this.SettingsHandler(-1));
        this.mmRepo.RightArrowJdB.onClick.AddListener(() => this.SettingsHandler(1));

        this.mmRepo.PasstB.GetComponent<PointerHandler>().ActivationRequest.AddListener(this.ActivatingGb);
        this.mmRepo.PasstB.GetComponent<PointerHandler>().DeActivationRequest.AddListener(this.DeActivatingGb);
        this.mmRepo.PasstB.onClick.AddListener(() => this.ActivatingOptionSetting(this.mmRepo.LeftArrowPtB.gameObject, this.mmRepo.RightArrowPtB.gameObject, "OKeSeventh"));
        this.mmRepo.LeftArrowPtB.onClick.AddListener(() => this.SettingsHandler(-1));
        this.mmRepo.RightArrowPtB.onClick.AddListener(() => this.SettingsHandler(1));
    }

    private void InitializingValues()
    {
        this.mmRepo.Difficulty.text = this.Difficulty[this.LocalGbSettings.DifficultyIndex];
        this.mmRepo.CameraBar.fillAmount = Mathf.InverseLerp(0.5f, 1.5f, this.LocalGbSettings.CurCamValue);

        this.mmRepo.Quality.text = this.Quality[this.LocalGbSettings.QualityIndex];

        this.mmRepo.MasterBar.fillAmount = this.LocalGbSettings.MasterValue;
        this.mmRepo.MusicBar.fillAmount = this.LocalGbSettings.MusicValue;
        this.mmRepo.EffectsBar.fillAmount = this.LocalGbSettings.EffectsValue;

        this.mmRepo.Form1T.text = this.PossibleFormKeys[this.LocalGbSettings.Form1Index];
        this.mmRepo.Form2T.text = this.PossibleFormKeys[this.LocalGbSettings.Form2Index];
        this.mmRepo.Form3T.text = this.PossibleFormKeys[this.LocalGbSettings.Form3Index];
        this.mmRepo.Form4T.text = this.PossibleFormKeys[this.LocalGbSettings.Form4Index];
        this.mmRepo.StdT.text = this.PossibleGenAbiKeys[this.LocalGbSettings.StdFormIndex];
        this.mmRepo.JumpDashT.text = this.PossibleGenAbiKeys[this.LocalGbSettings.JdIndex];
        this.mmRepo.PasstT.text = this.PossibleGenAbiKeys[this.LocalGbSettings.PtIndex];

    }

    private void DisablingContinue()
    {
        this.mmRepo.ContinueB.interactable = false;
    }

    private void OpeningLegendsJournal()
    {
        this.sectionTempRef = "Journal";
        this.mmRepo.JournalG.SetActive(true);
        EventSystem.current.SetSelectedGameObject(this.mmRepo.Legend1B.gameObject);
        this.mmRepo.MainPageG.SetActive(false);
    }

    private void OpeningOptions()
    {
        this.sectionTempRef = "OptionsMenu";
        this.mmRepo.OptionsG.SetActive(true);
        EventSystem.current.SetSelectedGameObject(this.mmRepo.GameplayMenuB.gameObject);
        this.mmRepo.GameplayG.SetActive(false);
        this.mmRepo.MainPageG.SetActive(false);
    }

    private void ActivatingGb(GameObject gbToActivate)
    {
        gbToActivate.SetActive(true);
    }

    private void ActivatingOptionsPanel(GameObject gbToActivate, GameObject activeButton, string section)
    {
        this.sectionTempRef = section;
        gbToActivate.SetActive(true);
        EventSystem.current.SetSelectedGameObject(activeButton);

        switch (this.sectionTempRef)
        {
            case "OGameplay":
                this.mmRepo.CsArrow.SetActive(false);
                break;
            case "OGraphics":
                break;
            case "OAudio":
                this.mmRepo.MuArrow.SetActive(false);
                this.mmRepo.EfArrow.SetActive(false);
                break;
            case "OKeys":
                this.mmRepo.F2Arrow.SetActive(false);
                this.mmRepo.F3Arrow.SetActive(false);
                this.mmRepo.F4Arrow.SetActive(false);
                this.mmRepo.StdArrow.SetActive(false);
                this.mmRepo.JdArrow.SetActive(false);
                this.mmRepo.PtArrow.SetActive(false);
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

        switch (this.sectionTempRef)
        {
            case "OGpFirst":
                break;
            case "OGpSecond":
                break;
            case "OGrFirst":
                break;
            case "OAdFirst":
                break;
            case "OAdSecond":
                break;
            case "OAdThird":
                break;
            case "OKeFirst":
                break;
            case "OKeSecond":
                break;
            case "OKeThird":
                break;
            case "OKeFourth":
                break;
            case "OKeFifth":
                break;
            case "OKeSixth":
                break;
            case "OKeSeventh":
                break;
            case "OKeEighth":
                break;
        }
    }

    private void SettingsHandler(int direction)
    {
        switch (this.sectionTempRef)
        {
            case "OGpFirst":
                if (this.LocalGbSettings.DifficultyIndex + direction >= 0 && this.LocalGbSettings.DifficultyIndex + direction < this.Difficulty.Length)
                {
                    this.LocalGbSettings.DifficultyIndex += direction;
                }
                else
                {
                    if (direction < 0)
                    {
                        this.LocalGbSettings.DifficultyIndex = this.Difficulty.Length - 1;
                    }
                    else
                    {
                        this.LocalGbSettings.DifficultyIndex = 0;
                    }
                }

                this.mmRepo.Difficulty.text = this.Difficulty[this.LocalGbSettings.DifficultyIndex];
                break;
            case "OGpSecond":
                if (direction > 0)
                {
                    this.LocalGbSettings.CurCamValue += 0.1f;
                }
                else
                {
                    this.LocalGbSettings.CurCamValue -= 0.1f;
                }

                this.LocalGbSettings.CurCamValue = (float)Math.Round(this.LocalGbSettings.CurCamValue, 1);
                this.LocalGbSettings.CurCamValue = Mathf.Clamp(this.LocalGbSettings.CurCamValue, 0.5f, 1.5f);

                this.mmRepo.CameraBar.fillAmount = Mathf.InverseLerp(0.5f, 1.5f, this.LocalGbSettings.CurCamValue);
                break;
            case "OGrFirst":
                if (this.LocalGbSettings.QualityIndex + direction >= 0 && this.LocalGbSettings.QualityIndex + direction < this.Quality.Length)
                {
                    this.LocalGbSettings.QualityIndex += direction;
                }
                else
                {
                    if (direction < 0)
                    {
                        this.LocalGbSettings.QualityIndex = this.Quality.Length - 1;
                    }
                    else
                    {
                        this.LocalGbSettings.QualityIndex = 0;
                    }
                }

                this.mmRepo.Quality.text = this.Quality[this.LocalGbSettings.QualityIndex];

#if UNITY_STANDALONE
                if (this.LocalGbSettings.QualityIndex == 1)
                {
                    
                    Debug.Log("Sweetfx");
                    InputSimulator.SimulateKeyDown(VirtualKeyCode.SCROLL);
                }
#endif
                break;
            case "OAdFirst":
                if (direction > 0)
                {
                    this.LocalGbSettings.MasterValue += 0.1f;
                }
                else
                {
                    this.LocalGbSettings.MasterValue -= 0.1f;
                }

                this.LocalGbSettings.MasterValue = (float)Math.Round(this.LocalGbSettings.MasterValue, 1);
                this.LocalGbSettings.MasterValue = Mathf.Clamp(this.LocalGbSettings.MasterValue, 0, 1);

                this.mmRepo.MasterBar.fillAmount = this.LocalGbSettings.MasterValue;
                break;
            case "OAdSecond":
                if (direction > 0)
                {
                    this.LocalGbSettings.MusicValue += 0.1f;
                }
                else
                {
                    this.LocalGbSettings.MusicValue -= 0.1f;
                }

                this.LocalGbSettings.MusicValue = (float)Math.Round(this.LocalGbSettings.MusicValue, 1);
                this.LocalGbSettings.MusicValue = Mathf.Clamp(this.LocalGbSettings.MusicValue, 0, 1);

                this.mmRepo.MusicBar.fillAmount = this.LocalGbSettings.MusicValue;
                break;
            case "OAdThird":
                if (direction > 0)
                {
                    this.LocalGbSettings.EffectsValue += 0.1f;
                }
                else
                {
                    this.LocalGbSettings.EffectsValue -= 0.1f;
                }

                this.LocalGbSettings.EffectsValue = (float)Math.Round(this.LocalGbSettings.EffectsValue, 1);
                this.LocalGbSettings.EffectsValue = Mathf.Clamp(this.LocalGbSettings.EffectsValue, 0, 1);

                this.mmRepo.EffectsBar.fillAmount = this.LocalGbSettings.EffectsValue;
                break;
            case "OKeFirst":

                var form1TempIndex = this.LocalGbSettings.Form1Index;

                if (this.LocalGbSettings.Form1Index + direction >= 0 && this.LocalGbSettings.Form1Index + direction < this.PossibleFormKeys.Length)
                {
                    this.LocalGbSettings.Form1Index += direction;
                }
                else
                {
                    if (direction < 0)
                    {
                        this.LocalGbSettings.Form1Index = this.PossibleFormKeys.Length - 1;
                    }
                    else
                    {
                        this.LocalGbSettings.Form1Index = 0;
                    }
                }

                if (this.LocalGbSettings.Form2Index == this.LocalGbSettings.Form1Index)
                {
                    this.LocalGbSettings.Form2Index = form1TempIndex;
                    this.mmRepo.Form2T.text = this.PossibleFormKeys[this.LocalGbSettings.Form2Index];
                }
                else if (this.LocalGbSettings.Form3Index == this.LocalGbSettings.Form1Index)
                {
                    this.LocalGbSettings.Form3Index = form1TempIndex;
                    this.mmRepo.Form3T.text = this.PossibleFormKeys[this.LocalGbSettings.Form3Index];
                }
                if (this.LocalGbSettings.Form4Index == this.LocalGbSettings.Form1Index)
                {
                    this.LocalGbSettings.Form4Index = form1TempIndex;
                    this.mmRepo.Form4T.text = this.PossibleFormKeys[this.LocalGbSettings.Form4Index];
                }

                this.mmRepo.Form1T.text = this.PossibleFormKeys[this.LocalGbSettings.Form1Index];
                break;
            case "OKeSecond":
                var form2TempIndex = this.LocalGbSettings.Form2Index;

                if (this.LocalGbSettings.Form2Index + direction >= 0 && this.LocalGbSettings.Form2Index + direction < this.PossibleFormKeys.Length)
                {
                    this.LocalGbSettings.Form2Index += direction;
                }
                else
                {
                    if (direction < 0)
                    {
                        this.LocalGbSettings.Form2Index = this.PossibleFormKeys.Length - 1;
                    }
                    else
                    {
                        this.LocalGbSettings.Form2Index = 0;
                    }
                }

                if (this.LocalGbSettings.Form1Index == this.LocalGbSettings.Form2Index)
                {
                    this.LocalGbSettings.Form1Index = form2TempIndex;
                    this.mmRepo.Form1T.text = this.PossibleFormKeys[this.LocalGbSettings.Form1Index];
                }
                else if (this.LocalGbSettings.Form3Index == this.LocalGbSettings.Form2Index)
                {
                    this.LocalGbSettings.Form3Index = form2TempIndex;
                    this.mmRepo.Form3T.text = this.PossibleFormKeys[this.LocalGbSettings.Form3Index];
                }
                if (this.LocalGbSettings.Form4Index == this.LocalGbSettings.Form2Index)
                {
                    this.LocalGbSettings.Form4Index = form2TempIndex;
                    this.mmRepo.Form4T.text = this.PossibleFormKeys[this.LocalGbSettings.Form4Index];
                }

                this.mmRepo.Form2T.text = this.PossibleFormKeys[this.LocalGbSettings.Form2Index];
                break;
            case "OKeThird":
                var form3TempIndex = this.LocalGbSettings.Form3Index;

                if (this.LocalGbSettings.Form3Index + direction >= 0 && this.LocalGbSettings.Form3Index + direction < this.PossibleFormKeys.Length)
                {
                    this.LocalGbSettings.Form3Index += direction;
                }
                else
                {
                    if (direction < 0)
                    {
                        this.LocalGbSettings.Form3Index = this.PossibleFormKeys.Length - 1;
                    }
                    else
                    {
                        this.LocalGbSettings.Form3Index = 0;
                    }
                }

                if (this.LocalGbSettings.Form1Index == this.LocalGbSettings.Form3Index)
                {
                    this.LocalGbSettings.Form1Index = form3TempIndex;
                    this.mmRepo.Form1T.text = this.PossibleFormKeys[this.LocalGbSettings.Form1Index];
                }
                else if (this.LocalGbSettings.Form2Index == this.LocalGbSettings.Form3Index)
                {
                    this.LocalGbSettings.Form2Index = form3TempIndex;
                    this.mmRepo.Form2T.text = this.PossibleFormKeys[this.LocalGbSettings.Form2Index];
                }
                if (this.LocalGbSettings.Form4Index == this.LocalGbSettings.Form3Index)
                {
                    this.LocalGbSettings.Form4Index = form3TempIndex;
                    this.mmRepo.Form4T.text = this.PossibleFormKeys[this.LocalGbSettings.Form4Index];
                }

                this.mmRepo.Form3T.text = this.PossibleFormKeys[this.LocalGbSettings.Form3Index];
                break;
            case "OKeFourth":
                var form4TempIndex = this.LocalGbSettings.Form4Index;

                if (this.LocalGbSettings.Form4Index + direction >= 0 && this.LocalGbSettings.Form4Index + direction < this.PossibleFormKeys.Length)
                {
                    this.LocalGbSettings.Form4Index += direction;
                }
                else
                {
                    if (direction < 0)
                    {
                        this.LocalGbSettings.Form4Index = this.PossibleFormKeys.Length - 1;
                    }
                    else
                    {
                        this.LocalGbSettings.Form4Index = 0;
                    }
                }

                if (this.LocalGbSettings.Form1Index == this.LocalGbSettings.Form4Index)
                {
                    this.LocalGbSettings.Form1Index = form4TempIndex;
                    this.mmRepo.Form1T.text = this.PossibleFormKeys[this.LocalGbSettings.Form1Index];
                }
                else if (this.LocalGbSettings.Form2Index == this.LocalGbSettings.Form4Index)
                {
                    this.LocalGbSettings.Form2Index = form4TempIndex;
                    this.mmRepo.Form2T.text = this.PossibleFormKeys[this.LocalGbSettings.Form2Index];
                }
                if (this.LocalGbSettings.Form3Index == this.LocalGbSettings.Form4Index)
                {
                    this.LocalGbSettings.Form3Index = form4TempIndex;
                    this.mmRepo.Form3T.text = this.PossibleFormKeys[this.LocalGbSettings.Form3Index];
                }

                this.mmRepo.Form4T.text = this.PossibleFormKeys[this.LocalGbSettings.Form4Index];
                break;
            case "OKeFifth":
                var stdTempIndex = this.LocalGbSettings.StdFormIndex;

                if (this.LocalGbSettings.StdFormIndex + direction >= 0 && this.LocalGbSettings.StdFormIndex + direction < this.PossibleGenAbiKeys.Length)
                {
                    this.LocalGbSettings.StdFormIndex += direction;
                }
                else
                {
                    if (direction < 0)
                    {
                        this.LocalGbSettings.StdFormIndex = this.PossibleGenAbiKeys.Length - 1;
                    }
                    else
                    {
                        this.LocalGbSettings.StdFormIndex = 0;
                    }
                }

                if (this.LocalGbSettings.JdIndex == this.LocalGbSettings.StdFormIndex)
                {
                    this.LocalGbSettings.JdIndex = stdTempIndex;
                    this.mmRepo.JumpDashT.text = this.PossibleGenAbiKeys[this.LocalGbSettings.JdIndex];
                }
                else if (this.LocalGbSettings.PtIndex == this.LocalGbSettings.StdFormIndex)
                {
                    this.LocalGbSettings.PtIndex = stdTempIndex;
                    this.mmRepo.PasstT.text = this.PossibleGenAbiKeys[this.LocalGbSettings.PtIndex];
                }
                

                this.mmRepo.StdT.text = this.PossibleGenAbiKeys[this.LocalGbSettings.StdFormIndex];
                break;
            case "OKeSixth":
                var jdTempIndex = this.LocalGbSettings.JdIndex;

                if (this.LocalGbSettings.JdIndex + direction >= 0 && this.LocalGbSettings.JdIndex + direction < this.PossibleGenAbiKeys.Length)
                {
                    this.LocalGbSettings.JdIndex += direction;
                }
                else
                {
                    if (direction < 0)
                    {
                        this.LocalGbSettings.JdIndex = this.PossibleGenAbiKeys.Length - 1;
                    }
                    else
                    {
                        this.LocalGbSettings.JdIndex = 0;
                    }
                }

                if (this.LocalGbSettings.StdFormIndex == this.LocalGbSettings.JdIndex)
                {
                    this.LocalGbSettings.StdFormIndex = jdTempIndex;
                    this.mmRepo.StdT.text = this.PossibleGenAbiKeys[this.LocalGbSettings.StdFormIndex];
                }
                else if (this.LocalGbSettings.PtIndex == this.LocalGbSettings.JdIndex)
                {
                    this.LocalGbSettings.PtIndex = jdTempIndex;
                    this.mmRepo.PasstT.text = this.PossibleGenAbiKeys[this.LocalGbSettings.PtIndex];
                }


                this.mmRepo.JumpDashT.text = this.PossibleGenAbiKeys[this.LocalGbSettings.JdIndex];
                break;
            case "OKeSeventh":
                var ptTempIndex = this.LocalGbSettings.PtIndex;

                if (this.LocalGbSettings.PtIndex + direction >= 0 && this.LocalGbSettings.PtIndex + direction < this.PossibleGenAbiKeys.Length)
                {
                    this.LocalGbSettings.PtIndex += direction;
                }
                else
                {
                    if (direction < 0)
                    {
                        this.LocalGbSettings.StdFormIndex = this.PossibleGenAbiKeys.Length - 1;
                    }
                    else
                    {
                        this.LocalGbSettings.StdFormIndex = 0;
                    }
                }

                if (this.LocalGbSettings.JdIndex == this.LocalGbSettings.PtIndex)
                {
                    this.LocalGbSettings.JdIndex = ptTempIndex;
                    this.mmRepo.JumpDashT.text = this.PossibleGenAbiKeys[this.LocalGbSettings.JdIndex];
                }
                else if (this.LocalGbSettings.StdFormIndex == this.LocalGbSettings.PtIndex)
                {
                    this.LocalGbSettings.PtIndex = ptTempIndex;
                    this.mmRepo.PasstT.text = this.PossibleGenAbiKeys[this.LocalGbSettings.PtIndex];
                }


                this.mmRepo.PasstT.text = this.PossibleGenAbiKeys[this.LocalGbSettings.PtIndex];
                break;
        }
    }

    private void DeActivatingGb(GameObject gbToDeActivate)
    {
        gbToDeActivate.SetActive(false);
    }

    private void QuitGame()
    {
        this.SaveDataRequest.Invoke();
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
        /*
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
        */
        if (Input.GetButtonDown("B"))
        {
            switch (this.sectionTempRef)
            {
                case "MainMenu":
                    this.QuitGame();
                    break;
                case "Journal":
                    this.mmRepo.MainPageG.SetActive(true);
                    this.mmRepo.JournalG.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(this.mmRepo.LegendsB.gameObject);
                    this.sectionTempRef = "MainMenu";
                    break;
                case "OptionsMenu":
                    this.mmRepo.MainPageG.SetActive(true);
                    this.mmRepo.OptionsG.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(this.mmRepo.OptionsB.gameObject);
                    this.sectionTempRef = "MainMenu";
                    break;
                case "OGameplay":
                    this.mmRepo.GameplayG.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(this.mmRepo.GameplayMenuB.gameObject);
                    this.sectionTempRef = "OptionsMenu";
                    break;
                case "OGraphics":
                    this.mmRepo.VideoG.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(this.mmRepo.VideoMenuB.gameObject);
                    this.sectionTempRef = "OptionsMenu";
                    break;
                case "OAudio":
                    this.mmRepo.AudioG.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(this.mmRepo.AudioMenuB.gameObject);
                    this.sectionTempRef = "OptionsMenu";
                    break;
                case "OKeys":
                    this.mmRepo.KeyBindingsG.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(this.mmRepo.KeyBindingsMenuB.gameObject);
                    this.sectionTempRef = "OptionsMenu";
                    break;
                case "OGpFirst":
                    this.mmRepo.LeftArrowGpoB.gameObject.SetActive(false);
                    this.mmRepo.RightArrowGpoB.gameObject.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(this.mmRepo.GameplayOptionsB.gameObject);
                    this.sectionTempRef = "OGameplay";
                    break;
                case "OGpSecond":
                    this.mmRepo.LeftArrowCsB.gameObject.SetActive(false);
                    this.mmRepo.RightArrowCsB.gameObject.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(this.mmRepo.CameraSpeedB.gameObject);
                    this.sectionTempRef = "OGameplay";
                    break;
                case "OGrFirst":
                    this.mmRepo.LeftArrowGsB.gameObject.SetActive(false);
                    this.mmRepo.RightArrowGsB.gameObject.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(this.mmRepo.GraphicSettingsB.gameObject);
                    this.sectionTempRef = "OGraphics";
                    break;
                case "OAdFirst":
                    this.mmRepo.LeftArrowMsB.gameObject.SetActive(false);
                    this.mmRepo.RightArrowMsB.gameObject.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(this.mmRepo.MasterB.gameObject);
                    this.sectionTempRef = "OAudio";
                    break;
                case "OAdSecond":
                    this.mmRepo.LeftArrowMuB.gameObject.SetActive(false);
                    this.mmRepo.RightArrowMuB.gameObject.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(this.mmRepo.MusicB.gameObject);
                    this.sectionTempRef = "OAudio";
                    break;
                case "OAdThird":
                    this.mmRepo.LeftArrowEfB.gameObject.SetActive(false);
                    this.mmRepo.RightArrowEfB.gameObject.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(this.mmRepo.EffectsB.gameObject);
                    this.sectionTempRef = "OAudio";
                    break;
                case "OKeFirst":
                    this.mmRepo.LeftArrowF1B.gameObject.SetActive(false);
                    this.mmRepo.RightArrowF1B.gameObject.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(this.mmRepo.Form1B.gameObject);
                    this.sectionTempRef = "OKeys";
                    break;
                case "OKeSecond":
                    this.mmRepo.LeftArrowF2B.gameObject.SetActive(false);
                    this.mmRepo.RightArrowF2B.gameObject.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(this.mmRepo.Form2B.gameObject);
                    this.sectionTempRef = "OKeys";
                    break;
                case "OKeThird":
                    this.mmRepo.LeftArrowF3B.gameObject.SetActive(false);
                    this.mmRepo.RightArrowF3B.gameObject.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(this.mmRepo.Form3B.gameObject);
                    this.sectionTempRef = "OKeys";
                    break;
                case "OKeFourth":
                    this.mmRepo.LeftArrowF4B.gameObject.SetActive(false);
                    this.mmRepo.RightArrowF4B.gameObject.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(this.mmRepo.Form4B.gameObject);
                    this.sectionTempRef = "OKeys";
                    break;
                case "OKeFifth":
                    this.mmRepo.LeftArrowStB.gameObject.SetActive(false);
                    this.mmRepo.RightArrowStB.gameObject.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(this.mmRepo.StdFormB.gameObject);
                    this.sectionTempRef = "OKeys";
                    break;
                case "OKeSixth":
                    this.mmRepo.LeftArrowJdB.gameObject.SetActive(false);
                    this.mmRepo.RightArrowJdB.gameObject.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(this.mmRepo.JumpDashB.gameObject);
                    this.sectionTempRef = "OKeys";
                    break;
                case "OKeSeventh":
                    this.mmRepo.LeftArrowPtB.gameObject.SetActive(false);
                    this.mmRepo.RightArrowPtB.gameObject.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(this.mmRepo.PasstB.gameObject);
                    this.sectionTempRef = "OKeys";
                    break;
            }

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
        audioSourceRef = GameObject.FindGameObjectWithTag("Player").GetComponent<SoundManager>();
        this.gpUiRef.Dialogue.SetActive(true);
        audioSourceRef.PersistendAudio[1].AudioSourceRef.Stop();
        

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
        audioSourceRef.PersistendAudio[1].AudioSourceRef.Stop();

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
