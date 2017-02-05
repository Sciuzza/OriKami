using System.Collections.Generic;

using UnityEngine;


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

        /*
        foreach (var t in storyLineTempRepo)
        {
            if (
                this.StoryLineRepo.Find(
                    x => x.StoryEnumName == t.GetComponent<StoryLineInstance>().CurrentStoryLine.StoryEnumName)
                == null) this.StoryLineRepo.Add(t.GetComponent<StoryLineInstance>().CurrentStoryLine);
        }
        */
    }

    public void AddToRepository(StoryLine currentSlTemp)
    {


        if (this.StoryLineRepo.Find(x => x.StoryLineName == currentSlTemp.StoryLineName) == null)
            this.StoryLineRepo.Add(currentSlTemp);
        else
        {
            int i = this.StoryLineRepo.FindIndex(x => x.StoryLineName == currentSlTemp.StoryLineName);
            this.StoryLineRepo.RemoveAt(i);
            this.StoryLineRepo.Add(currentSlTemp);
        }
    }


}
