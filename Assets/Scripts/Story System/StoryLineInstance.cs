using System.Collections.Generic;

using UnityEditor;

using UnityEditorInternal;

using UnityEngine;
using System;


using System.Collections;

using UnityEngine.Events;

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

    public PlayerSee PlayerSeeEffect;

    public float PushingBackPower;
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
public class PlayerSee
{
    [Tooltip("Standard Form, Frog Form, Armadillo Form, Dragon Form, Dolphin Form")]
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
    public CameraShake CameraShakeEffect;
    public float BtpLerpSpeed;
    public float BtsLerpSpeed;
}

[System.Serializable]
public class CameraMove
{
    public GameObject GbRef;
    public float LerpSpeed;
}

[System.Serializable]
public class CameraShake
{
    public float ShakingPower;
    public float ShakingDuration;
}
#endregion Player Effect Classes

#region Environment Npc Effects

[System.Serializable]
public class EnvironmentEffect
{
    public ObjectActivation ObjActiEffect;
    public Baloon BaloonEffect;
    public ObjectDeActivation ObjDeActiEffect;
    public ObjectMoving ObjMovEffect;
}

[System.Serializable]
public class ObjectActivation
{
    public GameObject GbRef;
    public bool Timed;
    public float Time;
}

[System.Serializable]
public class Baloon
{
    public GameObject GbRefRikiLogic;
    public float BaloonSpeed;
    public int StartLine;
    public int EndLine;
}

[System.Serializable]
public class ObjectDeActivation
{
    public GameObject GbRef;
    public bool Timed;
    public float Time;
}

[System.Serializable]
public class ObjectMoving
{
    public GameObject GbTarget;
    public GameObject GbToMove;
    public float LerpSpeed;
}

#endregion

#region Ui Effect
[System.Serializable]
public class UiEffect
{
    public UiObjectActivation ObjActiEffect;
    public UiDialogue UiDialogueEffect;
    public UiObjectDeActivation ObjDeActiEffect;
    public UiObjectMoving ObjMovEffect;
}

[System.Serializable]
public class UiObjectActivation
{
    public GameObject GbRef;
    public bool Timed;
    public float Time;
    public float FadingTime;

}

[System.Serializable]
public class UiDialogue
{
    public TextAsset DialogueRef;
    public List<DialogueStructDebug> DialogueDebug;
}

[System.Serializable]
public class DialogueStructDebug
{
    public string WhoIsTalking;
    public string LabelPos;
    public string WhatIsSaying;
}
[System.Serializable]
public class UiObjectDeActivation
{
    public GameObject GbRef;
    public bool Timed;
    public float Time;
}

[System.Serializable]
public class UiObjectMoving
{
    public GameObject GbTarget;
    public GameObject GbToMove;
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
    public SphereCollider STriggerRef;
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
    public buttonsJoy PlayerInputJoy;
    public buttonsPc PlayerInputPc;

    public float EventEndDelay;

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

    public List<StoryEvent> Events;

    //public static List<StoryEvent> Events1;

    //[System.Serializable]
    // public ReorderableList reorderableList = new ReorderableList(Events1, (typeof(StoryEvent)),true, true, true, true);


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





    // public SingleStory[] Stories;

    public List<SingleStory> Stories;
}

#endregion


public class StoryLineInstance : MonoBehaviour
{

    #region Public Variables
    public StoryLine CurrentStoryLine;
    #endregion

    #region Events
    public event_joy_pc activateStoryInputRequest;
    public event_string formUnlockRequest;
    #endregion

    #region Private Variables
    private GameObject player;
    private SingleStory storySelected;
    private int eventIndex = 0;
    #endregion

    #region Taking References and Linking Events
    private void Awake()
    {

        player = GameObject.FindGameObjectWithTag("Player");

        EnvInputs envTempLink = player.GetComponent<EnvInputs>();

        envTempLink.storyActivationRequest.AddListener(InitializationByTrigger);

        PlayerInputs plTempLink = player.GetComponent<PlayerInputs>();

        plTempLink.storyLivingRequest.AddListener(StartingStoryEventByInput);

    }
    #endregion

    #region Story Check Access Condition Methods
    // Check Completed , Active and Relation Conditions
    private void InitializationByTrigger(Collider trigger)
    {
        Debug.Log(trigger.name);

        if (storySelected != null)
            storySelected = null;

        if (CurrentStoryLine.Completed)
        {
            Debug.Log("Storyline already Completed");
            return;
        }

        foreach (var story in CurrentStoryLine.Stories)
        {
            if ((story.GenAccessCond.STriggerRef == trigger || story.GenAccessCond.TriggerRef == trigger) && story.Active)
            {
                storySelected = story;
                break;
            }
        }
        if (storySelected != null)
        {
            Debug.Log(storySelected.StoryName);
            CheckStoryLivingConditions();

        }
        else
            Debug.Log("No Story is accessible through this Trigger " + trigger.name);
    }

    // Check all the other Access conditions
    private void CheckStoryLivingConditions()
    {
        if (storySelected.Completed) return;
        

        foreach (var item in storySelected.ItemAccessCondition)
        {
            // control logic achievement system based
        }

        if (IsNeededInput(storySelected.GenAccessCond.PlayerInputJoy, storySelected.GenAccessCond.PlayerInputPc))
            activateStoryInputRequest.Invoke(storySelected.GenAccessCond.PlayerInputJoy, storySelected.GenAccessCond.PlayerInputPc);

        else if (IsNeededInput(storySelected.Events[0].PlayerInputJoy, storySelected.Events[0].PlayerInputPc))
            activateStoryInputRequest.Invoke(storySelected.Events[0].PlayerInputJoy, storySelected.Events[0].PlayerInputPc);

        else
            LivingStoryEvent(eventIndex);
    }


    #endregion

    private void StartingStoryEventByInput()
    {
        LivingStoryEvent(eventIndex);
    }

    //Start the Story
    private void LivingStoryEvent(int eventIndex)
    {
        Debug.Log("Living Story Started for the story " + storySelected.StoryName);

        PlayerEffectsHandler();

      
       
    }

    private bool IsNeededInput(buttonsJoy joyInput, buttonsPc pcInput)
    {
        if (joyInput != buttonsJoy.none && pcInput != buttonsPc.none)
            return true;
        else
            return false;
    }

    private void PlayerEffectsHandler()
    {
        PlayerEffect plaEffectsToEvaluate = storySelected.Events[eventIndex].Effects.PlaEffect;

        if (plaEffectsToEvaluate.PlayerRepositionEffect.GbRef != null)
        {
            PlayPlayerRepoEffect(plaEffectsToEvaluate.PlayerRepositionEffect);

            if (plaEffectsToEvaluate.PlayerReward.FormName != " ")
                PlayPlayerRewardEffect(plaEffectsToEvaluate.PlayerReward);
        }
        else if (plaEffectsToEvaluate.PlayerMoveEffect.GbRef != null)
        {
            PlayPlayerMoveEffect(plaEffectsToEvaluate.PlayerMoveEffect);

            if (plaEffectsToEvaluate.PlayerReward.FormName != " ")
                PlayPlayerRewardEffect(plaEffectsToEvaluate.PlayerReward);
        }
        else if (plaEffectsToEvaluate.PlayerSeeEffect.GbRef != null)
        {
            PlayPlayerSeeEffect(plaEffectsToEvaluate.PlayerSeeEffect);

            if (plaEffectsToEvaluate.PlayerReward.FormName != " ")
                PlayPlayerRewardEffect(plaEffectsToEvaluate.PlayerReward);
        }
        else if (plaEffectsToEvaluate.PushingBackPower != 0)
        {
            PlayPlayerPushingBackEffect(plaEffectsToEvaluate.PushingBackPower);

            if (plaEffectsToEvaluate.PlayerReward.FormName != " ")
                PlayPlayerRewardEffect(plaEffectsToEvaluate.PlayerReward);
        }
        else
        {
            if (plaEffectsToEvaluate.PlayerReward.FormName != " ")
                PlayPlayerRewardEffect(plaEffectsToEvaluate.PlayerReward);
        }
    }

    private void PlayPlayerRepoEffect(PlayerReposition effectToPlay)
    {
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = effectToPlay.GbRef.transform.position;
        player.GetComponent<CharacterController>().enabled = true;
    }

    private void PlayPlayerMoveEffect(PlayerMove effectToPlay)
    {
        player.GetComponent<CharacterController>().enabled = false;
        StartCoroutine(MovingObject(player, effectToPlay.GbRef, effectToPlay.LerpSpeed));
    }


    private void PlayPlayerRewardEffect(PlayerReward effectToPlay)
    {
        formUnlockRequest.Invoke(effectToPlay.FormName);
    }

    private void PlayPlayerSeeEffect(PlayerSee effectToPlay)
    {

    }

    private void PlayPlayerPushingBackEffect(float pushingBackPower)
    {

    }

    private IEnumerator MovingObject(GameObject objToMove, GameObject whereToMove, float lerpSpeed)
    {
        var targetRotation = whereToMove.transform.rotation;
        var targetPosition = whereToMove.transform.position;

        var posReached = false;

        while (!posReached)
        {
            objToMove.transform.position = Vector3.Lerp(objToMove.transform.position, targetPosition, lerpSpeed * Time.deltaTime);
            objToMove.transform.rotation = Quaternion.Slerp(objToMove.transform.rotation, targetRotation, lerpSpeed * Time.deltaTime);

            if (Quaternion.Angle(objToMove.transform.rotation, targetRotation) < 0.1f
                && ((objToMove.transform.position - targetPosition).sqrMagnitude < 0.1f)) posReached = true;

            yield return null;
        }

        if(objToMove.CompareTag("Player"))
            player.GetComponent<CharacterController>().enabled = true;
    } 


    /*
   public void OnValidate()
   {
       GameObject.FindGameObjectWithTag("GameController").GetComponent<QuestsManager>().AddToRepository(this.CurrentStoryLine);
   }
   */
}
