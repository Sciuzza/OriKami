using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class MainMenuRepo : MonoBehaviour
{

    // Main Menu Section
    public Image FrogIm, ArmaIm, CraneIm, DolphIm;

    public Button NewGameB, ContinueB, LegendsB, OptionsB, ExitB;

    public GameObject MainPageG, JournalG, OptionsG;


    // Journal Section
    public Button Legend1B, Legend2B, Legend3B, Legend4B, Legend5B, Legend6B, Legend7B, Legend8B;

    public GameObject Legend1G, Legend2G, Legend3G, Legend4G, Legend5G, Legend6G, Legend7G, Legend8G;


    // Options Section

    public Button GameplayMenuB, VideoMenuB, AudioMenuB, KeyBindingsMenuB;

    public GameObject GameplayG, VideoG, AudioG, KeyBindingsG;


    public Button GameplayOptionsB, CameraSpeedB;

    public GameObject GbArrow, CsArrow;

    public Button LeftArrowGpoB, RightArrowGpoB, LeftArrowCsB, RightArrowCsB;

    public Text Difficulty;

    public Image CameraBar;


    public Button GraphicSettingsB;

    public GameObject GsArrow;

    public Button LeftArrowGsB, RightArrowGsB;

    public Text Quality;


    public Button MasterB, MusicB, EffectsB;

    public GameObject MsArrow, MuArrow, EfArrow;

    public Button LeftArrowMsB, RightArrowMsB, LeftArrowMuB, RightArrowMuB, LeftArrowEfB, RightArrowEfB;

    public Image MasterBar, MusicBar, EffectsBar;


    public Button Form1B, Form2B, Form3B, Form4B, StdFormB, JumpDashB, PasstB, EpicViewB;

    public GameObject F1Arrow, F2Arrow, F3Arrow, F4Arrow, StdArrow, JdArrow, PtArrow, EvArrow;

    public Button LeftArrowF1B, RightArrowF1B, LeftArrowF2B, RightArrowF2B, LeftArrowF3B, RightArrowF3B, LeftArrowF4B, RightArrowF4B, LeftArrowStB, RightArrowStB
                 ,LeftArrowJdB, RightArrowJdB, LeftArrowPtB, RightArrowPtB, LeftArrowEwB, RightArrowEwB;

    public Text Form1T, Form2T, Form3T, Form4T, StdT, JumpDashT, PasstT, EpicViewT;

}
