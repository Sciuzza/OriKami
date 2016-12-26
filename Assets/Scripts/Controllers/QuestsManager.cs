using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestsManager : MonoBehaviour
{


    public List<StoryLine> StoryLineRepo;

    void Awake()
    {
        GameController gcLink = this.GetComponent<GameController>();

        gcLink.gpInitializer.AddListener(this.InitializingStoryLines);
       
    }


    private void InitializingStoryLines(GameObject player)
    {
        var storyLineTempRepo = GameObject.FindGameObjectsWithTag("StoryLine");

        foreach (GameObject t in storyLineTempRepo)
        {
            if (this.StoryLineRepo.Find(x => x.StoryEnumName == t.GetComponent<StoryLineInstance>().CurrentStoryLine.StoryEnumName) == null)
                this.StoryLineRepo.Add(t.GetComponent<StoryLineInstance>().CurrentStoryLine);
        }
    }

   
}
