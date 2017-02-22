using UnityEngine;
using System.Collections;

public class CollTrigger : MonoBehaviour
{



    public bool TriggerStory;
    public string StoryName;

    public event_string CheckStoryAccessRequest;

    public void CheckingStoryCondition()
    {
        this.CheckStoryAccessRequest.Invoke(this.StoryName);
    }


}
