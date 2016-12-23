using System.Collections.Generic;

using UnityEngine;


#region Enum Types

public enum EnvActionable
{
    Trigger,

    EndOfPreviousStoryEvent,

    EndOfPreviousStory,

    none
}

public enum PlayerActionable
{
    Input,

    Auto,

    none
}

public enum Stories
{
    Story1,

    Story2,

    Story3,

    none
}

public enum Events
{
    Event1,

    Event2,

    Event3,

    none
}

public enum Storylines
{
    StoryLine1,

    StoryLine2,

    StoryLine3,

    none
}

public enum Movies
{
    movie1,

    movie2,

    movie3,

    none
}
#endregion

#region Player Effect Classes

[System.Serializable]
public class PlayerEffect
{
    public controlStates PlayerControlEffect;

    public PlayerReposition PlayerRepositionEffect;

    public PlayerMove PlayerMoveEffect;
}

[System.Serializable]
public class PlayerReposition
{
    public GameObject GbRef;
}

[System.Serializable]
public class PlayerMove
{
    public GameObject GbRef;

    public float LerpSpeed;
}

#endregion Player Effect Classes

#region Camera Effect Classes

[System.Serializable]
public class CameraEffect
{
    public CameraMove CameraMoveEffect;

}

[System.Serializable]
public class CameraMove
{
    public GameObject GbRef;
    public float LerpSpeed;
}



#endregion Player Effect Classes

#region Environment Npc Effects

[System.Serializable]
public class EnvEffect
{
    public ObjectActivation ObjActiEffect;
    public ObjectMoving ObjMovEffect;
}

[System.Serializable]
public class ObjectActivation
{
    public GameObject GbRef;
    public bool Dialogue;
    public string DialogueText;
}

[System.Serializable]
public class ObjectMoving
{
    public GameObject GbRef;
    public float LerpSpeed;
}

#endregion

#region Ui Effect
[System.Serializable]
public class UiEffect
{
    public UiObjectActivation ObjActiEffect;
    public UiObjectMoving ObjMovEffect;
}

[System.Serializable]
public class UiObjectActivation
{
    public GameObject GbRef;

    public bool Dialogue;

    public string DialogueText;
}

[System.Serializable]
public class UiObjectMoving
{
    public GameObject GbRef;
    public float LerpSpeed;
}

#endregion

#region Sound Effect
[System.Serializable]
public class SoundEffect
{
    public PersistentSound PerSoundEffect;
    public NormalSound NormSoundEffect;
}

[System.Serializable]
public class PersistentSound
{
    public GameObject GbRef;
    public int SoundCategory;
    public int SoundIndex;
}

[System.Serializable]
public class NormalSound
{
    public GameObject GbRef;
}
#endregion

#region Animation Effect
[System.Serializable]
public class AnimationEffect
{
    public GameObject GbRef;
    public int AnimationIndex;
}

#endregion

#region Movie Effect

[System.Serializable]
public class MovieEffect
{
    public Movies MovieName;
}
#endregion

#region Story Element

[System.Serializable]
public class GenTriggerConditions
{
    public EnvActionable HowEnvActionable;
    public BoxCollider TriggerRef;
    public PlayerActionable HowPlaActionable;
    public buttonsJoy PlayerInputJoy;
    public buttonsPc PlayerInputPc;
}

[System.Serializable]
public class SlTriggerConditions
{
    public Storylines StoryLineDep;
    public bool StoryLineCompleted;
    public Stories StoryDep;
    public bool SingleStoryCompleted;
    public Events EventDep;
    public bool SingleStoryEventCompleted;
}

[System.Serializable]
public class StoryEvent
{
    public string EventName;
    public Events EventEnumName;
    public bool Completed;
    public bool Repeatable;
    public GenTriggerConditions GenTriggerCond;
    public PlayerEffect PlayerEffects;
    public CameraEffect CameraEffects;
    public List<EnvEffect> EnvEffects;
    public List<UiEffect> UiEffects;
    public List<SoundEffect> SoundEffects;
    public List<AnimationEffect> AnimationEffects;
    public MovieEffect VideoToPlay;
}

[System.Serializable]
public class SingleStory
{
    public string StoryName;
    public Stories StoryEnumName;
    public bool Completed;
    public bool Repeatable;
    public GenTriggerConditions GenTriggerCond;
    public StoryEvent[] Events;
}

[System.Serializable]
public class StoryLine
{
    public string StoryLineName;
    public Storylines StoryEnumName;
    public bool Completed;
    public bool Repeatable;
    public GenTriggerConditions GenTriggerCond;
    public SlTriggerConditions[] StoryTriggerCond;
    public SingleStory[] Stories;
}

#endregion


public class StoryLineInstance : MonoBehaviour
{

    public StoryLine CurrentStoryLine;

    #region Validate Algorithm

    /* 
       public void OnValidate()
       {
           foreach (var t in this.Stories)
           {
               if (!t.IsAQuest)
               {
                   t.QuestName = quests.none;
                   t.QuestPhase = questPhase.none;
               }
    
               if (!t.InputActionable)
               {
                   t.PlayerInputJoy = buttonsJoy.none;
                   t.PlayerInputPc = buttonsPc.none;
               }
    
    
               foreach (var storyEvent in t.Events)
               {
                   if (storyEvent.PlayerEffects.PlayerControlEffect == controlStates.totalControl)
                   {
                       storyEvent.PlayerEffects.PlayerRepositionEffect.GbRef = this.gameObject;
                       storyEvent.PlayerEffects.PlayerMoveEffect.LerpSpeed = float.NaN;
                   }
                   else
                   {
                       storyEvent.PlayerEffects.PlayerRepositionEffect.GbRef = null;
                   }
               }
           }
       }
       */

    #endregion


    public void Initialization()
    {
        Debug.Log("Here");
    }

}