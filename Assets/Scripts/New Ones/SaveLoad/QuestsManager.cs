using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

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
    /*
    public void OnValidate()
    {
        for (int index0 = 0; index0 < this.StoryLineRepo.Count; index0++)
        {
            for (int index1 = 0; index1 < this.StoryLineRepo[index0].Stories.Count; index1++)
            {
                for (int index2 = 0; index2 < this.StoryLineRepo.Count; index2++)
                {

                    for (int index3 = 0; index3 < this.StoryLineRepo[index2].Stories.Count; index3++)
                    {
                        if (this.StoryLineRepo[index0].Stories[index1].StoryName
                            == this.StoryLineRepo[index2].Stories[index3].StoryName && (index0 != index2)
                            && (index1 != index3) && !this.StoryLineRepo[index0].Stories[index1].StoryName.Contains("Map"))
                        {
                            Debug.Log(this.StoryLineRepo[index0].Stories[index1].StoryName + " with index " + index0 + " " + index1);
                            Debug.Log("has a copy");
                            Debug.Log(this.StoryLineRepo[index2].Stories[index3].StoryName + " with index " + index2 + " " + index3);
                        }
                    }
                }
            }
        }
    }
    */
}
