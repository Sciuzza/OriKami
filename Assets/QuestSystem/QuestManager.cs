using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{

    private Dictionary<string, Sprite> iconDict;

    public Sprite[] icons;

    public GameObject createdDialog;

    void Start()
    {

        createdDialog.SetActive(false);                 //hide the dialog ui
        iconDict = new Dictionary<string, Sprite>();
        foreach (Sprite sprite in icons)
            iconDict.Add(sprite.name, sprite);          //Loads icons into dictionary to allow quick lookup
    }


    /**
     * Triggers the start of a quest
     * questName: supply file path to the xml file
     **/
    public void StartQuest(string questName)
    {
        createdDialog.SetActive(true);
        createdDialog.GetComponent<QuestDisplay>().Initialize(Quest.LoadQuest(questName));      //Uses the Dialog UI and initializes the quest onto the display
    }

    /**
     * iconName: Name of the icon
     * Return: Icon
     **/
    public Sprite GetIcon(string iconName)
    {
        if (iconDict[iconName] != null)
            return iconDict[iconName];
        else
            return null;
    }

}