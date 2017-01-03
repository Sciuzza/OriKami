using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;

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

        if (storyLineTempRepo == null) return;

        foreach (var t in storyLineTempRepo)
        {
            if (
                this.StoryLineRepo.Find(
                    x => x.StoryEnumName == t.GetComponent<StoryLineInstance>().CurrentStoryLine.StoryEnumName)
                == null) this.StoryLineRepo.Add(t.GetComponent<StoryLineInstance>().CurrentStoryLine);
        }
    }

    public void AddToRepository(StoryLine currentSlTemp)
    {


        if (this.StoryLineRepo.Find(x => x == currentSlTemp) == null && 
            this.StoryLineRepo.Find(x => x.StoryLineName == currentSlTemp.StoryLineName) == null &&
            currentSlTemp.StoryLineName == currentSlTemp.StoryEnumName.ToString())
            this.StoryLineRepo.Add(currentSlTemp);


        


    }


}
