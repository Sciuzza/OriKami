using System.Collections.Generic;

using UnityEngine;


#region Enum Types



public enum Stories
{
    Story1,

    Story2,

    Story3,

    None
}

public enum Events
{
    Event1,

    Event2,

    Event3,

    None
}

public enum Storylines
{
    StoryLine1,

    StoryLine2,

    StoryLine3,

    None
}

public enum Movies
{
    Movie1,

    Movie2,

    Movie3,

    None
}

public enum ItemType
{
    Item1,

    Item2,

    Item3
}

#endregion

#region Effect Classes

#region Player Effect Classes

[System.Serializable]
public class PlayerEffect
{

    public PlayerReposition PlayerRepositionEffect;

    public PlayerMove PlayerMoveEffect;

    public PlayerReward PlayerReward;
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

[System.Serializable]
public class PlayerReward
{
    [Tooltip("Standard Form, Frog Form, Armadillo Form, Dragon Form, Dolphin Form")]
    public string FormName;

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
public class EnvironmentEffect
{
    public ObjectActivation ObjActiEffect;
    public ObjectMoving ObjMovEffect;
}

[System.Serializable]
public class ObjectActivation
{
    public GameObject GbRef;
    public bool Timed;
    public float Time;
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
    public bool Timed;
    public float Time;
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
    public bool Active;
    public int SoundCategory;
    public int SoundIndex;
}

[System.Serializable]
public class NormalSound
{
    public GameObject GbRef;
    public int SoundIndex;
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


[System.Serializable]
public class EvEffects
{
    public PlayerEffect PlaEffect;
    public CameraEffect CamEffect;
    public List<EnvironmentEffect> EnvEffect;
    public List<UiEffect> UiEffect;
    public List<SoundEffect> SoundEffect;
    public List<AnimationEffect> AniEffect;
    public MovieEffect MovieEffect;
}
#endregion


#region Sub Story and Storyline Classes

[System.Serializable]
public class GenTriggerConditions
{
    public BoxCollider TriggerRef;
    public buttonsJoy PlayerInputJoy;
    public buttonsPc PlayerInputPc;
}

[System.Serializable]
public class ItemDependencies
{
    public ItemType ItemDep;
    public int ItemValue;
}




#endregion



#region Story Element

[System.Serializable]
public class StoryEvent
{
    public string EventName;
    public Events EventEnumName;
  

    public EvEffects Effects;
}

[System.Serializable]
public class SingleStory
{
    public string StoryName;
    public Stories StoryEnumName;
    public bool Completed;
    public bool Active;
    public bool AutoComplete;


    public GenTriggerConditions GenAccessCond;
    public List<ItemDependencies> ItemAccessCondition;



    public controlStates PlayerControlEffect;


    public List<Storylines> StoryLineCompleteOnActivation;
    public List<Stories> StoryCompleteOnActivation;
    public List<Stories> StoryActiveOnActivation;

    public List<Storylines> StoryLineCompleteOnCompletion;
    public List<Stories> StoryCompleteOnCompletion;
    public List<Stories> StoryActiveOnCompletion;

 

    public StoryEvent[] Events;
}

[System.Serializable]
public class StoryLine
{
    public string StoryLineName;
    public Storylines StoryEnumName;
    public bool Completed;


    public List<Storylines> StoryLineCompleteOnCompletion;
    public List<Stories> StoryCompleteOnCompletion;
    public List<Stories> StoryActiveOnCompletion;

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
        this.CheckSlGenTriggerConditions();
    }


    private void CheckSlGenTriggerConditions()
    {
        
    }

}