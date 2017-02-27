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

    public Button LeftArrowGpoB, RightArrowGpoB, LeftArrowCsB, RightArrowCsB;

}
