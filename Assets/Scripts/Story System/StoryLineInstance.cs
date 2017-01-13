using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

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

#endregion Enum Types

#region Effect Classes

#region Player Effect Classes

[Serializable]
public class PlayerEffect
{
    public PlayerReposition PlayerRepositionEffect;
    public PlayerMove PlayerMoveEffect;
    public PlayerSee PlayerSeeEffect;
    public PlayerPushBack PushingBackEffect;
    public PlayerReward PlayerReward;
}

[Serializable]
public class PlayerReposition
{
    public bool End;
    public GameObject GbRef;
}

[Serializable]
public class PlayerMove
{
    public bool End;
    public GameObject GbRef;
    public float LerpSpeed;
}

[Serializable]
public class PlayerSee
{
    public bool End;
    [Tooltip("Standard Form, Frog Form, Armadillo Form, Dragon Form, Dolphin Form")]
    public GameObject GbRef;
    public float LerpSpeed;
}

[Serializable]
public class PlayerPushBack
{
    public bool End;
    public float PushingBackPower;
    public float LerpSpeed;
}

[Serializable]
public class PlayerReward
{
    public bool End;
    [Tooltip("Standard Form, Frog Form, Armadillo Form, Dragon Form, Dolphin Form")]
    public string FormName;
}


#endregion Player Effect Classes

#region Camera Effect Classes

[Serializable]
public class CameraEffect
{
    public CameraMove CameraMoveEffect;
    public CameraShake CameraShakeEffect;
    public CameraEventRevert CameraErEffect;
    public CameraStoryRevert CameraSrEffect;
}

[Serializable]
public class CameraMove
{
    public bool End;
    public GameObject GbRef;
    public float LerpSpeed;
}

[Serializable]
public class CameraShake
{
    public bool End;
    public float ShakingDuration;
    public float ShakingPower;
}

[Serializable]
public class CameraEventRevert
{
    public bool End;
    public float ErLerpSpeed;
}

[Serializable]
public class CameraStoryRevert
{
    public bool End;
    public float SrLerpSpeed;

}
#endregion Camera Effect Classes

#region Environment Npc Effects

[Serializable]
public class EnvironmentEffect
{
    public Baloon BaloonEffect;

    public ObjectActivation ObjActiEffect;

    public ObjectDeActivation ObjDeActiEffect;

    public ObjectMoving ObjMovEffect;
}

[Serializable]
public class ObjectActivation
{
    public GameObject GbRef;

    public float Time;

    public bool Timed;
}

[Serializable]
public class Baloon
{
    public float BaloonSpeed;

    public int EndLine;

    public GameObject GbRefRikiLogic;

    public int StartLine;
}

[Serializable]
public class ObjectDeActivation
{
    public GameObject GbRef;

    public float Time;

    public bool Timed;
}

[Serializable]
public class ObjectMoving
{
    public GameObject GbTarget;

    public GameObject GbToMove;

    public float LerpSpeed;
}

#endregion Environment Npc Effects

#region Ui Effect

[Serializable]
public class UiEffect
{
    public UiObjectActivation ObjActiEffect;

    public UiObjectDeActivation ObjDeActiEffect;

    public UiObjectMoving ObjMovEffect;

    public UiDialogue UiDialogueEffect;
}

[Serializable]
public class UiObjectActivation
{
    public float FadingTime;

    public GameObject GbRef;

    public float Time;

    public bool Timed;
}

[Serializable]
public class UiDialogue
{
    public List<DialogueStructDebug> DialogueDebug;

    public TextAsset DialogueRef;
}

[Serializable]
public class DialogueStructDebug
{
    public string LabelPos;

    public string WhatIsSaying;

    public string WhoIsTalking;
}

[Serializable]
public class UiObjectDeActivation
{
    public GameObject GbRef;

    public float Time;

    public bool Timed;
}

[Serializable]
public class UiObjectMoving
{
    public GameObject GbTarget;

    public GameObject GbToMove;

    public float LerpSpeed;
}

#endregion Ui Effect

#region Sound Effect

[Serializable]
public class SoundEffect
{
    public NormalSound NormSoundEffect;

    public PersistentSound PerSoundEffect;
}

[Serializable]
public class PersistentSound
{
    public bool Active;

    public int SoundCategory;

    public int SoundIndex;
}

[Serializable]
public class NormalSound
{
    public GameObject GbRef;

    public int SoundIndex;
}

#endregion Sound Effect

#region Animation Effect

[Serializable]
public class AnimationEffect
{
    public int AnimationIndex;

    public GameObject GbRef;
}

#endregion Animation Effect

#region Movie Effect

[Serializable]
public class MovieEffect
{
    public Movies MovieName;
}

#endregion Movie Effect

[Serializable]
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

#endregion Effect Classes

#region Sub Story and Storyline Classes

[Serializable]
public class GenTriggerConditions
{
    public buttonsJoy PlayerInputJoy;

    public buttonsPc PlayerInputPc;

    public SphereCollider STriggerRef;

    public BoxCollider TriggerRef;
}

[Serializable]
public class ItemDependencies
{
    public ItemType ItemDep;

    public int ItemValue;
}

#endregion Sub Story and Storyline Classes

#region Story Element

[Serializable]
public class StoryEvent
{
    public string EventName;

    public Events EventEnumName;

    public buttonsJoy PlayerInputJoy;

    public buttonsPc PlayerInputPc;

    public float EventEndDelay;

    public EvEffects Effects;

}

[Serializable]
public class SingleStory
{
    public string StoryName;

    public Stories StoryEnumName;

    public bool Active;

    public bool AutoComplete;

    public bool Completed;

    public GenTriggerConditions GenAccessCond;

    public List<ItemDependencies> ItemAccessCondition;

    public List<Stories> StoryActiveOnActivation;

    public List<Stories> StoryActiveOnCompletion;

    public List<Stories> StoryCompleteOnActivation;

    public List<Stories> StoryCompleteOnCompletion;

    public List<Storylines> StoryLineCompleteOnActivation;

    public List<Storylines> StoryLineCompleteOnCompletion;

    public controlStates PlayerControlEffect;

    public List<StoryEvent> Events;

    // public ReorderableList reorderableList = new ReorderableList(Events1, (typeof(StoryEvent)),true, true, true, true);

    // [System.Serializable]

    // public static List<StoryEvent> Events1;
}

[Serializable]
public class StoryLine
{
    public string StoryLineName;

    public Storylines StoryEnumName;

    public bool Completed;

    // public SingleStory[] Stories;

    public List<Stories> StoryActiveOnCompletion;

    public List<Stories> StoryCompleteOnCompletion;

    public List<Storylines> StoryLineCompleteOnCompletion;

    public List<SingleStory> Stories;

}

#endregion Story Element

public class StoryLineInstance : MonoBehaviour
{

    public StoryLine CurrentStoryLine;

    #region Events
    public event_joy_pc ActivateStoryInputRequest;

    public event_string FormUnlockRequest;

    public event_cs ChangeControlStateRequest;
    #endregion

    #region Private Variables
    private int eventIndex = 0;
    private GameObject player;
    private SingleStory storySelected;

    private Vector3 camLastPos = new Vector3(0, 0, 0);
    private Quaternion camLastRot = new Quaternion(0, 0, 0, 0);
    #endregion

    #region Taking References and Linking Events
    private void Awake()
    {
        this.player = GameObject.FindGameObjectWithTag("Player");

        var envTempLink = this.player.GetComponent<EnvInputs>();

        envTempLink.storyActivationRequest.AddListener(this.InitializationByTrigger);

        var plTempLink = this.player.GetComponent<PlayerInputs>();

        plTempLink.storyLivingRequest.AddListener(this.LivingStoryEvent);
    }
    #endregion

    #region Check Initial Conditions Methods
    // Check all the other Access conditions
    private void CheckStoryLivingConditions()
    {
        if (this.storySelected.Completed) return;

        foreach (var item in this.storySelected.ItemAccessCondition)
        {
            // control logic achievement system based
        }

        if (this.IsNeededInput(
            this.storySelected.GenAccessCond.PlayerInputJoy,
            this.storySelected.GenAccessCond.PlayerInputPc))
            this.ActivateStoryInputRequest.Invoke(
                this.storySelected.GenAccessCond.PlayerInputJoy,
                this.storySelected.GenAccessCond.PlayerInputPc);
        else if (this.IsNeededInput(this.storySelected.Events[0].PlayerInputJoy, this.storySelected.Events[0].PlayerInputPc))
            this.ActivateStoryInputRequest.Invoke(
                this.storySelected.Events[0].PlayerInputJoy,
                this.storySelected.Events[0].PlayerInputPc);
        else this.LivingStoryEvent();
    }

    // Check Completed , Active and Relation Conditions
    private void InitializationByTrigger(Collider trigger)
    {
        Debug.Log(trigger.name);

        if (this.CurrentStoryLine.Completed)
        {
            Debug.Log("Storyline already Completed");
            return;
        }

        foreach (var story in this.CurrentStoryLine.Stories)
        {
            if ((story.GenAccessCond.STriggerRef != trigger && story.GenAccessCond.TriggerRef != trigger)
                || !story.Active) continue;
            this.storySelected = story;
            break;
        }

        if (this.storySelected != null)
        {
            Debug.Log(this.storySelected.StoryName);
            this.CheckStoryLivingConditions();
        }
        else Debug.Log("No Story is accessible through this Trigger " + trigger.name);
    }

    private bool IsNeededInput(buttonsJoy joyInput, buttonsPc pcInput)
    {
        return joyInput != buttonsJoy.none && pcInput != buttonsPc.none;
    }

    // Start the Story
    private void LivingStoryEvent()
    {
        Debug.Log("Living Story Started for the story " + this.storySelected.StoryName);

        this.camLastPos = Camera.main.transform.position;
        this.camLastRot = Camera.main.transform.rotation;

        this.ChangeControlStateRequest.Invoke(this.storySelected.PlayerControlEffect);
        this.PlayerEffectsHandler();
        this.CameraEffectsHandler();
    }
    #endregion

    #region Player Effects Methods
    private void PlayerEffectsHandler()
    {
        var plaEffectsToEvaluate = this.storySelected.Events[this.eventIndex].Effects.PlaEffect;

        if (plaEffectsToEvaluate.PlayerRepositionEffect.GbRef != null)
        {
            this.PlayPlayerRepoEffect(plaEffectsToEvaluate.PlayerRepositionEffect);

            if (plaEffectsToEvaluate.PlayerReward.FormName != " ") this.PlayPlayerRewardEffect(plaEffectsToEvaluate.PlayerReward);
        }
        else if (plaEffectsToEvaluate.PlayerMoveEffect.GbRef != null)
        {
            this.PlayPlayerMoveEffect(plaEffectsToEvaluate.PlayerMoveEffect);

            if (plaEffectsToEvaluate.PlayerReward.FormName != " ") this.PlayPlayerRewardEffect(plaEffectsToEvaluate.PlayerReward);
        }
        else if (plaEffectsToEvaluate.PlayerSeeEffect.GbRef != null)
        {
            this.PlayPlayerSeeEffect(plaEffectsToEvaluate.PlayerSeeEffect);

            if (plaEffectsToEvaluate.PlayerReward.FormName != " ") this.PlayPlayerRewardEffect(plaEffectsToEvaluate.PlayerReward);
        }
        else if (plaEffectsToEvaluate.PushingBackEffect.PushingBackPower > 0)
        {
            this.PlayPlayerPushingBackEffect(plaEffectsToEvaluate.PushingBackEffect);

            if (plaEffectsToEvaluate.PlayerReward.FormName != " ") this.PlayPlayerRewardEffect(plaEffectsToEvaluate.PlayerReward);
        }
        else
        {
            if (plaEffectsToEvaluate.PlayerReward.FormName != " ") this.PlayPlayerRewardEffect(plaEffectsToEvaluate.PlayerReward);
        }
    }

    private void PlayPlayerRepoEffect(PlayerReposition effectToPlay)
    {
        this.player.transform.position = effectToPlay.GbRef.transform.position;
        effectToPlay.End = true;
    }

    private void PlayPlayerMoveEffect(PlayerMove effectToPlay)
    {
        this.StartCoroutine(this.MovingPlayer(effectToPlay));
    }

    private void PlayPlayerRewardEffect(PlayerReward effectToPlay)
    {
        this.FormUnlockRequest.Invoke(effectToPlay.FormName);
        effectToPlay.End = true;
    }

    private void PlayPlayerSeeEffect(PlayerSee effectToPlay)
    {
        this.StartCoroutine(this.RotatePlayer(effectToPlay));
    }

    private void PlayPlayerPushingBackEffect(PlayerPushBack effectToPlay)
    {
        this.StartCoroutine(this.PushBackPlayer(effectToPlay));
    }

    private IEnumerator MovingPlayer(PlayerMove movingPlayerEffect)
    {
        var whereToMove = movingPlayerEffect.GbRef;
        var objToMove = this.player;
        var lerpSpeed = movingPlayerEffect.LerpSpeed;

        var targetRotation = whereToMove.transform.rotation;
        var targetPosition = whereToMove.transform.position;

        var posReached = false;

        while (!posReached)
        {
            objToMove.transform.position = Vector3.Lerp(
                objToMove.transform.position,
                targetPosition,
                lerpSpeed * Time.deltaTime);
            objToMove.transform.rotation = Quaternion.Slerp(
                objToMove.transform.rotation,
                targetRotation,
                lerpSpeed * Time.deltaTime);


            var targetPosYFixed = targetPosition;


            targetPosYFixed.y = this.player.transform.position.y;

            if (Quaternion.Angle(objToMove.transform.rotation, targetRotation) < 0.1f
                && ((objToMove.transform.position - targetPosYFixed).sqrMagnitude < 0.1f)) posReached = true;

            yield return null;
        }

        movingPlayerEffect.End = true;
    }

    private IEnumerator PushBackPlayer(PlayerPushBack pushBackEffect)
    {
        var targetPosition = this.player.transform.position - (this.player.transform.forward * pushBackEffect.PushingBackPower);
        var objToMove = this.player;

        var posReached = false;

        while (!posReached)
        {
            objToMove.transform.position = Vector3.Lerp(
                objToMove.transform.position,
                targetPosition,
                pushBackEffect.LerpSpeed * Time.deltaTime);


            if ((objToMove.transform.position - targetPosition).sqrMagnitude < 0.1f) posReached = true;

            yield return null;
        }

        pushBackEffect.End = true;

    }

    private IEnumerator RotatePlayer(PlayerSee rotateEffect)
    {
        var objToMove = this.player;
        var lerpSpeed = rotateEffect.LerpSpeed;

        var tempObj = new GameObject("temp");
        tempObj.transform.position = this.player.transform.position;
        tempObj.transform.rotation = this.player.transform.rotation;

        tempObj.transform.LookAt(rotateEffect.GbRef.transform);

        var targetRotation = tempObj.transform.rotation;


        var posReached = false;

        while (!posReached)
        {

            objToMove.transform.rotation = Quaternion.Slerp(
                objToMove.transform.rotation,
                targetRotation,
                lerpSpeed * Time.deltaTime);

            if (Quaternion.Angle(objToMove.transform.rotation, targetRotation) < 0.1f) posReached = true;

            yield return null;
        }

        rotateEffect.End = true;
        DestroyObject(tempObj);
    }
    #endregion

    #region Camera Effects Methods

    private void CameraEffectsHandler()
    {
        var camEffectsToEvaluate = this.storySelected.Events[this.eventIndex].Effects.CamEffect;

        if (camEffectsToEvaluate.CameraMoveEffect.GbRef != null)
            this.PlayCameraMoveEffect(camEffectsToEvaluate.CameraMoveEffect);
        else if (camEffectsToEvaluate.CameraShakeEffect.ShakingPower > 0)
            this.PlayCameraShakeEffect(camEffectsToEvaluate.CameraShakeEffect);
        else if (camEffectsToEvaluate.CameraErEffect.ErLerpSpeed > 0)
            this.PlayCameraErEffect(camEffectsToEvaluate.CameraErEffect);
        else if (camEffectsToEvaluate.CameraSrEffect.SrLerpSpeed > 0)
            this.PlayCameraSrEffect(camEffectsToEvaluate.CameraSrEffect);


    }

    private void PlayCameraMoveEffect(CameraMove effectToPlay)
    {
        this.StartCoroutine(this.MovingStoryCamera(effectToPlay));
    }

    private void PlayCameraShakeEffect(CameraShake effectToPlay)
    {
        this.StartCoroutine(this.ShakingStoryCamera(effectToPlay));
    }

    private void PlayCameraErEffect(CameraEventRevert effectToPlay)
    {

    }

    private void PlayCameraSrEffect(CameraStoryRevert effectToPlay)
    {

    }

    private IEnumerator MovingStoryCamera(CameraMove movingCameraEffect)
    {
        var whereToMove = movingCameraEffect.GbRef;
        var objToMove = Camera.main;
        var lerpSpeed = movingCameraEffect.LerpSpeed;

        var targetRotation = whereToMove.transform.rotation;
        var targetPosition = whereToMove.transform.position;

        var posReached = false;

        while (!posReached)
        {
            objToMove.transform.position = Vector3.Lerp(
                objToMove.transform.position,
                targetPosition,
                lerpSpeed * Time.deltaTime);
            objToMove.transform.rotation = Quaternion.Slerp(
                objToMove.transform.rotation,
                targetRotation,
                lerpSpeed * Time.deltaTime);


            var targetPosYFixed = targetPosition;


            targetPosYFixed.y = this.player.transform.position.y;

            if (Quaternion.Angle(objToMove.transform.rotation, targetRotation) < 0.1f
                && ((objToMove.transform.position - targetPosYFixed).sqrMagnitude < 0.1f)) posReached = true;

            yield return null;
        }

        movingCameraEffect.End = true;
    }

    private IEnumerator ShakingStoryCamera(CameraShake shakingCameraEffect)
    {

        var timer = 0.0f;

        while (shakingCameraEffect.ShakingDuration > timer)
        {
            var rotationAmount = Random.insideUnitSphere * shakingCameraEffect.ShakingPower;
            rotationAmount.z = 0;


            timer += Time.deltaTime;

            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, Camera.main.transform.rotation * Quaternion.Euler(rotationAmount), Time.deltaTime * 5);


            yield return null;
        }

        shakingCameraEffect.End = true;
    }
    #endregion

    #region Edit Mode Methods
    /*
public void OnValidate()
{
GameObject.FindGameObjectWithTag("GameController").GetComponent<QuestsManager>().AddToRepository(this.CurrentStoryLine);
}
*/
    #endregion
}