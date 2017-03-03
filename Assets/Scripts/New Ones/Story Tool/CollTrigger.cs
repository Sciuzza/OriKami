using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class CollTrigger : MonoBehaviour
{

    public bool TriggerStory;
    public string StoryName;

    public Image UiObjectRef;

    public event_string CheckStoryAccessRequest;
    public event_image UnlockingUiObject;

    public void CheckingStoryCondition()
    {
        this.CheckStoryAccessRequest.Invoke(this.StoryName);

        if (this.UnlockingUiObject != null)
        this.UnlockingUiObject.Invoke(this.UiObjectRef);
    }
}
