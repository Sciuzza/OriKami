﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

public class HudRefRepo : MonoBehaviour
{

    // to handle Collectible value and image
    public Text CollectibleValue;
    public Image CollectibleImage;

    // to handle image sources
    public Image rightImage, leftImage;

    // To handle enabling and disabling
    public GameObject Dialogue, Pause, BlackScreen, LeftLabel, RightLabel, rightImageGb, leftImageGb;

    //to hadle buttons
    public Button ContinueB, MainMenuB, ExitB;

    // to handle dialogue texts
    public Text DialogueT, RightLabelT, LeftLabelT;

    public MovieLogic MovieRef;

    public Image Spider;

    public List<Sprite> Avatars;
    public List<string> AvatarNames;
}
