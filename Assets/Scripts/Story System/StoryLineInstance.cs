using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    public float ShakingRoughness;
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
    public ObjectMoving ObjMovEffect;
    public ObjectActivation ObjActiEffect;
    public ObjectDeActivation ObjDeActiEffect;
    public Baloon BaloonEffect;
}

[Serializable]
public class ObjectMoving
{
    public bool End;
    public GameObject GbToMove;
    public GameObject GbTarget;
    public float LerpSpeed;
}

[Serializable]
public class ObjectActivation
{
    public bool End;
    public GameObject GbRef;
    public float Time;
}

[Serializable]
public class ObjectDeActivation
{
    public bool End;
    public GameObject GbRef;
    public float Time;
}

[Serializable]
public class Baloon
{
    public bool End;
    public GameObject NpcRef;
    public float BaloonSpeed;
    public int StartLine;
    public int EndLine;
}
#endregion Environment Npc Effects

#region Ui Effect
[Serializable]
public class UiEffect
{
    public UiObjectMoving ObjMovEffect;
    public UiObjectActivation ObjActiEffect;
    public UiObjectDeActivation ObjDeActiEffect;
    public UiDialogue UiDialogueEffect;
}

[Serializable]
public class UiObjectMoving
{
    public GameObject GbToMove;
    public GameObject GbTarget;
    public float LerpSpeed;
}

[Serializable]
public class UiObjectActivation
{
    public GameObject GbRef;
    public float Time;
    public float FadingTime;
}

[Serializable]
public class UiObjectDeActivation
{
    public GameObject GbRef;
    public float Time;
    public float FadingTime;
}

[Serializable]
public class UiDialogue
{
    public bool End;
    public TextAsset DialogueRef;
    public List<string> Name;
    public List<string> Label;
    public List<string> Sentence;
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
    public controlStates EndControlEffect;

    public List<StoryEvent> Events;
}

[Serializable]
public class StoryLine 
{
    public string StoryLineName;
    public Storylines StoryEnumName;
    public bool Completed;

    public List<Stories> StoryActiveOnCompletion;
    public List<Stories> StoryCompleteOnCompletion;
    public List<Storylines> StoryLineCompleteOnCompletion;

    public List<SingleStory> Stories;
}
#endregion Story Element

public class StoryLineInstance : MonoBehaviour
{
    #region Public Variables
    public StoryLine CurrentStoryLine;
    #endregion

    #region Events
    public event_joy_pc ActivateStoryInputRequest;
    public event_string FormUnlockRequest;
    public event_cs ChangeCsRequest;
    public event_string_string_string UiDialogueRequest;
    public UnityEvent DialogueEnded;
    public event_bool IsStoryMode;
    #endregion

    #region Private Variables
    private GameObject player;

    public SingleStory storySelected;

    private int eventIndex;
    private int effectCounter;
    private int totalEventEffects;
   
    private List<Vector3> camLastPos = new List<Vector3>();
    private List<Quaternion> camLastRot = new List<Quaternion>();
    private int cameraChangeCounter;
    #endregion

    #region Taking References and Linking Events
    private void Awake()
    {
        this.player = GameObject.FindGameObjectWithTag("Player");

        var envTempLink = this.player.GetComponent<EnvInputs>();
        envTempLink.storyActivationRequest.AddListener(this.CheckingAccessAndExistenceConditions);

        var plTempLink = this.player.GetComponent<PlayerInputs>();
        plTempLink.storyLivingRequest.AddListener(this.LivingStoryEvent);
    }
    #endregion

    #region Check Initial Conditions Methods
    // Check Completed , Active and Relation Conditions
    private void CheckingAccessAndExistenceConditions(Collider trigger)
    {
        GameController.Debugging(trigger.name);

        if (this.CurrentStoryLine.Completed)
        {
            GameController.Debugging("Storyline already Completed");
            return;
        }

        foreach (var story in this.CurrentStoryLine.Stories)
        {
            if ((story.GenAccessCond.STriggerRef != trigger && story.GenAccessCond.TriggerRef != trigger)
                || !story.Active || story.Completed)
            {
                continue;
            }

            this.storySelected = story;
            GameController.Debugging(this.storySelected.StoryName);
            break;
        }

        if (this.storySelected != null && this.CheckItemConditions())
            this.InitializingStory();
        else
            GameController.Debugging("No Story is accessible through this Trigger " + trigger.name);
    }

    private bool CheckItemConditions()
    {
        /*
        foreach (var item in this.storySelected.ItemAccessCondition)
        {
            // control logic achievement system based
        }
        */

        return true;
    }

    // Check all the other Access conditions
    private void InitializingStory()
    {
        if (this.IsNeededInput(
             this.storySelected.GenAccessCond.PlayerInputJoy,
             this.storySelected.GenAccessCond.PlayerInputPc))
        {
            this.ActivateStoryInputRequest.Invoke(
             this.storySelected.GenAccessCond.PlayerInputJoy,
             this.storySelected.GenAccessCond.PlayerInputPc);
        }
        else if (this.IsNeededInput(this.storySelected.Events[0].PlayerInputJoy, this.storySelected.Events[0].PlayerInputPc))
        {
            this.ActivateStoryInputRequest.Invoke(
             this.storySelected.Events[0].PlayerInputJoy,
             this.storySelected.Events[0].PlayerInputPc);
        }
        else
        {
            this.LivingStoryEvent();
        }
    }

    private bool IsNeededInput(buttonsJoy joyInput, buttonsPc pcInput)
    {
        return joyInput != buttonsJoy.none && pcInput != buttonsPc.none;
    }
    #endregion

    #region Living Story Core
    // Start the Story
    private void LivingStoryEvent()
    {
        GameController.Debugging("Living Story Started for the story " + this.storySelected.StoryName);

        //this.camLastPos.Add(Camera.main.transform.position);
        //this.camLastRot.Add(Camera.main.transform.rotation);

        //this.IsStoryMode.Invoke(true);
        //this.ChangeCsRequest.Invoke(this.storySelected.PlayerControlEffect);

        //this.StartCoroutine(this.LivingStory());
    }

    private IEnumerator LivingStory()
    {
        while (this.eventIndex < this.storySelected.Events.Count)
        {
            GameController.Debugging("event", this.eventIndex);

            if (this.eventIndex == 0)
            {
                this.PlayerEffectsHandler();
                this.CameraEffectsHandler();
                this.EnvEffectsHandler();
                this.UiEffectsHandler();

                GameController.Debugging("Total Events", this.totalEventEffects);
                GameController.Debugging("Effect Counter", this.effectCounter);

                if (this.totalEventEffects == 0)
                {
                    GameController.Debugging("Event index 0 , 0 effects found");
                    this.eventIndex++;
                    continue;
                }
            }
            else
            {
                if (this.storySelected.Events[this.eventIndex].PlayerInputJoy != buttonsJoy.none
                    && this.storySelected.Events[this.eventIndex].PlayerInputPc != buttonsPc.none)
                {
                    while (!Input.GetButtonDown(this.storySelected.Events[this.eventIndex].PlayerInputJoy.ToString())
                        && !Input.GetButtonDown(this.storySelected.Events[this.eventIndex].PlayerInputPc.ToString()))
                    {
                        yield return null;
                    }

                    this.PlayerEffectsHandler();
                    this.CameraEffectsHandler();
                    this.EnvEffectsHandler();
                    this.UiEffectsHandler();

                    GameController.Debugging("Total Events", this.totalEventEffects);
                    GameController.Debugging("Effect Counter", this.effectCounter);

                    if (this.totalEventEffects == 0)
                    {
                        GameController.Debugging("Event index 0 , 0 effects found");
                        this.eventIndex++;
                        continue;
                    }
                }
                else
                {
                    this.PlayerEffectsHandler();
                    this.CameraEffectsHandler();
                    this.EnvEffectsHandler();
                    this.UiEffectsHandler();

                    GameController.Debugging("Total Events", this.totalEventEffects);
                    GameController.Debugging("Effect Counter", this.effectCounter);

                    if (this.totalEventEffects == 0)
                    {
                        GameController.Debugging("Event index 0 , 0 effects found");
                        this.eventIndex++;
                        continue;
                    }
                }
            }

            while (this.effectCounter < this.totalEventEffects)
            {
                yield return null;
            }

            var timer = 0.0f;

            while (timer < this.storySelected.Events[this.eventIndex].EventEndDelay)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            this.eventIndex++;
            this.ResettingEventCommonVariables();
            yield return null;
        }

        this.ApplyingLivingEffects();
        this.IsStoryMode.Invoke(false);
        this.ResettingStoryCommonVariables();
    }

    private void ResettingEventCommonVariables()
    {
        this.effectCounter = 0;
        this.totalEventEffects = 0;
    }

    private void ResettingStoryCommonVariables()
    {
        this.effectCounter = 0;
        this.totalEventEffects = 0;

        this.eventIndex = 0;
        this.storySelected = null;
        this.camLastPos.Clear();
        this.camLastPos.TrimExcess();
        this.camLastRot.Clear();
        this.camLastRot.TrimExcess();
        this.cameraChangeCounter = 0;
    }

    private void ApplyingLivingEffects()
    {
        this.ChangeCsRequest.Invoke(this.storySelected.EndControlEffect);
    }
    #endregion

    #region Player Effects Methods
    private void PlayerEffectsHandler()
    {
        var plaEffectsToEvaluate = this.storySelected.Events[this.eventIndex].Effects.PlaEffect;

        if (plaEffectsToEvaluate.PlayerRepositionEffect.GbRef != null)
        {
            this.totalEventEffects++;
            GameController.Debugging("Player Repo");
            this.PlayPlayerRepoEffect(plaEffectsToEvaluate.PlayerRepositionEffect);

            if (plaEffectsToEvaluate.PlayerReward.FormName != " ")
            {
                this.totalEventEffects++;
                GameController.Debugging("Player Reward");
                this.PlayPlayerRewardEffect(plaEffectsToEvaluate.PlayerReward);
            }
        }
        else if (plaEffectsToEvaluate.PlayerMoveEffect.GbRef != null)
        {
            this.totalEventEffects++;
            GameController.Debugging("Player Move");
            this.PlayPlayerMoveEffect(plaEffectsToEvaluate.PlayerMoveEffect);

            if (plaEffectsToEvaluate.PlayerReward.FormName != " ")
            {
                this.totalEventEffects++;
                GameController.Debugging("Player Reward");
                this.PlayPlayerRewardEffect(plaEffectsToEvaluate.PlayerReward);
            }
        }
        else if (plaEffectsToEvaluate.PlayerSeeEffect.GbRef != null)
        {
            this.totalEventEffects++;
            GameController.Debugging("Player See");
            this.PlayPlayerSeeEffect(plaEffectsToEvaluate.PlayerSeeEffect);

            if (plaEffectsToEvaluate.PlayerReward.FormName != " ")
            {
                this.totalEventEffects++;
                GameController.Debugging("Player Reward");
                this.PlayPlayerRewardEffect(plaEffectsToEvaluate.PlayerReward);
            }
        }
        else if (plaEffectsToEvaluate.PushingBackEffect.PushingBackPower > 0)
        {
            this.totalEventEffects++;
            GameController.Debugging("Player Pushing Back");
            this.PlayPlayerPushingBackEffect(plaEffectsToEvaluate.PushingBackEffect);

            if (plaEffectsToEvaluate.PlayerReward.FormName != " ")
            {
                this.totalEventEffects++;
                GameController.Debugging("Player Reward");
                this.PlayPlayerRewardEffect(plaEffectsToEvaluate.PlayerReward);
            }
        }
        else
        {
            if (plaEffectsToEvaluate.PlayerReward.FormName != " ")
            {
                this.totalEventEffects++;
                GameController.Debugging("Player Reward");
                this.PlayPlayerRewardEffect(plaEffectsToEvaluate.PlayerReward);
            }
        }
    }

    private void PlayPlayerRepoEffect(PlayerReposition effectToPlay)
    {
        this.player.transform.position = effectToPlay.GbRef.transform.position;
        effectToPlay.End = true;
        this.effectCounter++;
    }

    private void PlayPlayerMoveEffect(PlayerMove effectToPlay)
    {
        this.StartCoroutine(this.MovingPlayer(effectToPlay));
    }

    private void PlayPlayerRewardEffect(PlayerReward effectToPlay)
    {
        this.FormUnlockRequest.Invoke(effectToPlay.FormName);
        effectToPlay.End = true;
        this.effectCounter++;
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
                && ((objToMove.transform.position - targetPosYFixed).sqrMagnitude < 0.1f))
            {
                posReached = true;
            }

            yield return null;
        }

        movingPlayerEffect.End = true;
        this.effectCounter++;
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

            if ((objToMove.transform.position - targetPosition).sqrMagnitude < 0.1f)
            {
                posReached = true;
            }

            yield return null;
        }

        pushBackEffect.End = true;
        this.effectCounter++;
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

            if (Quaternion.Angle(objToMove.transform.rotation, targetRotation) < 0.1f)
            {
                posReached = true;
            }

            yield return null;
        }

        rotateEffect.End = true;
        this.effectCounter++;
        DestroyObject(tempObj);
    }
    #endregion

    #region Camera Effects Methods

    private void CameraEffectsHandler()
    {
        var camEffectsToEvaluate = this.storySelected.Events[this.eventIndex].Effects.CamEffect;

        if (camEffectsToEvaluate.CameraMoveEffect.GbRef != null)
        {
            this.totalEventEffects++;
            GameController.Debugging("Camera Move");
            this.PlayCameraMoveEffect(camEffectsToEvaluate.CameraMoveEffect);
        }
        else if (camEffectsToEvaluate.CameraShakeEffect.ShakingPower > 0)
        {
            this.totalEventEffects++;
            GameController.Debugging("Camera Shake");
            this.PlayCameraShakeEffect(camEffectsToEvaluate.CameraShakeEffect);
        }
        else if (camEffectsToEvaluate.CameraErEffect.ErLerpSpeed > 0)
        {
            this.totalEventEffects++;
            GameController.Debugging("Camera ER");
            this.PlayCameraErEffect(camEffectsToEvaluate.CameraErEffect);
        }
        else if (camEffectsToEvaluate.CameraSrEffect.SrLerpSpeed > 0)
        {
            this.totalEventEffects++;
            GameController.Debugging("Camera SR");
            this.PlayCameraSrEffect(camEffectsToEvaluate.CameraSrEffect);
        }
    }

    private void PlayCameraMoveEffect(CameraMove effectToPlay)
    {
        this.cameraChangeCounter++;
        if (this.cameraChangeCounter > 1)
        {
            this.camLastPos.Add(Camera.main.transform.position);
            this.camLastRot.Add(Camera.main.transform.rotation);
        }

        this.StartCoroutine(this.MovingStoryCamera(effectToPlay));
    }

    private void PlayCameraShakeEffect(CameraShake effectToPlay)
    {
        this.StartCoroutine(this.ShakingStoryCamera(effectToPlay));
    }

    private void PlayCameraErEffect(CameraEventRevert effectToPlay)
    {
        this.StartCoroutine(this.MovingStoryCamera(effectToPlay, this.cameraChangeCounter - 1));
    }

    private void PlayCameraSrEffect(CameraStoryRevert effectToPlay)
    {
        this.StartCoroutine(this.MovingStoryCamera(effectToPlay));
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

            if (Quaternion.Angle(objToMove.transform.rotation, targetRotation) < 0.1f
                && ((objToMove.transform.position - targetPosition).sqrMagnitude < 0.1f))
            {
                posReached = true;
            }

            yield return null;
        }

        movingCameraEffect.End = true;
        this.effectCounter++;
    }

    private IEnumerator MovingStoryCamera(CameraEventRevert erCameraEffect, int listIndex)
    {
        var objToMove = Camera.main;
        var lerpSpeed = erCameraEffect.ErLerpSpeed;

        var targetRotation = Quaternion.identity;
        var targetPosition = new Vector3();

        if (listIndex >= 0)
        {
            targetRotation = this.camLastRot[listIndex];
            targetPosition = this.camLastPos[listIndex];
        }

        var posReached = false;

        while (!posReached && listIndex >= 0)
        {
            objToMove.transform.position = Vector3.Lerp(
                objToMove.transform.position,
                targetPosition,
                lerpSpeed * Time.deltaTime);
            objToMove.transform.rotation = Quaternion.Slerp(
                objToMove.transform.rotation,
                targetRotation,
                lerpSpeed * Time.deltaTime);

            if (Quaternion.Angle(objToMove.transform.rotation, targetRotation) < 0.1f
                && ((objToMove.transform.position - targetPosition).sqrMagnitude < 0.1f))
            {
                posReached = true;
            }

            yield return null;
        }

        if (listIndex < 0)
        {
            Debug.Log("Event Revert Effect bad applied");
        }

        erCameraEffect.End = true;
        this.effectCounter++;
    }

    private IEnumerator MovingStoryCamera(CameraStoryRevert srCameraEffect)
    {
        var objToMove = Camera.main;
        var lerpSpeed = srCameraEffect.SrLerpSpeed;

        var targetRotation = this.camLastRot[0];
        var targetPosition = this.camLastPos[0];

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

            if (Quaternion.Angle(objToMove.transform.rotation, targetRotation) < 0.1f
                && ((objToMove.transform.position - targetPosition).sqrMagnitude < 0.1f))
            {
                posReached = true;
            }

            yield return null;
        }

        srCameraEffect.End = true;
        this.effectCounter++;
    }

    private IEnumerator ShakingStoryCamera(CameraShake shakingCameraEffect)
    {
        var timer = 0.0f;
        var originalCameraRot = Camera.main.transform.rotation;

        while (shakingCameraEffect.ShakingDuration > timer)
        {
            var rotationAmount = Random.insideUnitSphere * shakingCameraEffect.ShakingPower;
            rotationAmount.z = 0;

            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, Camera.main.transform.rotation * Quaternion.Euler(rotationAmount), Time.deltaTime * shakingCameraEffect.ShakingRoughness);

            timer += Time.deltaTime;

            yield return null;
        }

        while (Quaternion.Angle(Camera.main.transform.rotation, originalCameraRot) > 0.1f)
        {
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, originalCameraRot, Time.deltaTime * shakingCameraEffect.ShakingRoughness);

            yield return null;
        }

        shakingCameraEffect.End = true;
        this.effectCounter++;
    }
    #endregion

    #region Env Effects Methods
    private void EnvEffectsHandler()
    {
        var envEffectToEvaluate = new List<EnvironmentEffect>();
        envEffectToEvaluate.AddRange(this.storySelected.Events[this.eventIndex].Effects.EnvEffect);

        var gbCheckTempRepo = new List<GameObject>();

        if (envEffectToEvaluate.Count == 0)
        {
        }
        else
        {
            foreach (var envEffect in envEffectToEvaluate)
            {
                if (envEffect.ObjMovEffect.GbToMove != null && envEffect.ObjMovEffect.GbTarget != null)
                {
                    if (!this.IsGameobjectRefUnique(gbCheckTempRepo, envEffect.ObjMovEffect.GbToMove))
                    {
                        continue;
                    }

                    this.totalEventEffects++;
                    GameController.Debugging("Object Moving");
                    this.PlayObjMovingEffect(envEffect.ObjMovEffect);
                    gbCheckTempRepo.Add(envEffect.ObjMovEffect.GbToMove);
                }

                if (envEffect.ObjActiEffect.GbRef != null)
                {
                    if (!this.IsGameobjectRefUnique(gbCheckTempRepo, envEffect.ObjActiEffect.GbRef))
                    {
                        continue;
                    }

                    this.totalEventEffects++;
                    GameController.Debugging("Object Activation");
                    this.PlayObjActiEffect(envEffect.ObjActiEffect);
                    gbCheckTempRepo.Add(envEffect.ObjActiEffect.GbRef);
                }

                if (envEffect.ObjDeActiEffect.GbRef != null)
                {
                    if (!this.IsGameobjectRefUnique(gbCheckTempRepo, envEffect.ObjDeActiEffect.GbRef))
                    {
                        continue;
                    }

                    this.totalEventEffects++;
                    GameController.Debugging("Object DeActivation");
                    this.PlayObjDeActiEffect(envEffect.ObjDeActiEffect);
                    gbCheckTempRepo.Add(envEffect.ObjDeActiEffect.GbRef);
                }

                if (envEffect.BaloonEffect.NpcRef != null)
                {
                    if (!this.IsGameobjectRefUnique(gbCheckTempRepo, envEffect.BaloonEffect.NpcRef))
                    {
                        continue;
                    }

                    this.totalEventEffects++;
                    GameController.Debugging("Baloon");
                    this.PlayBaloonEffect(envEffect.BaloonEffect);
                    gbCheckTempRepo.Add(envEffect.BaloonEffect.NpcRef);
                }
            }
        }
    }

    private bool IsGameobjectRefUnique(List<GameObject> gbRepo, GameObject gbToCheck)
    {
        foreach (var el in gbRepo)
        {
            if (gbToCheck == el)
            {
                return false;
            }
        }

        return true;
    }

    private void PlayObjMovingEffect(ObjectMoving effectToPlay)
    {
        this.StartCoroutine(this.MovingObject(effectToPlay));
    }

    private void PlayObjActiEffect(ObjectActivation effectToPlay)
    {
        if (effectToPlay.Time == 0)
        {
            effectToPlay.GbRef.SetActive(true);
            effectToPlay.End = true;
            this.effectCounter++;
        }
        else
        {
            this.StartCoroutine(this.TimedActivation(effectToPlay));
        }
    }

    private void PlayObjDeActiEffect(ObjectDeActivation effectToPlay)
    {
        if (effectToPlay.Time == 0)
        {
            effectToPlay.GbRef.SetActive(false);
            effectToPlay.End = true;
            this.effectCounter++;
        }
        else
        {
            this.StartCoroutine(this.TimedDeActivation(effectToPlay));
        }
    }

    private void PlayBaloonEffect(Baloon effectToPlay)
    {
        this.StartCoroutine(this.BubbleAdjustRot(effectToPlay));
        this.StartCoroutine(this.BaloonDialogue(effectToPlay));
    }

    private IEnumerator MovingObject(ObjectMoving movingObjEffect)
    {
        var whereToMove = movingObjEffect.GbTarget;
        var objToMove = movingObjEffect.GbToMove;
        var lerpSpeed = movingObjEffect.LerpSpeed;

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
                && ((objToMove.transform.position - targetPosYFixed).sqrMagnitude < 0.1f))
            {
                posReached = true;
            }

            yield return null;
        }

        movingObjEffect.End = true;
        this.effectCounter++;
    }

    private IEnumerator TimedActivation(ObjectActivation actiObjEffect)
    {
        actiObjEffect.GbRef.SetActive(true);

        var timer = 0.0f;

        while (timer < actiObjEffect.Time)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        actiObjEffect.GbRef.SetActive(false);
        actiObjEffect.End = true;
        this.effectCounter++;
    }

    private IEnumerator TimedDeActivation(ObjectDeActivation deActiObjEffect)
    {
        deActiObjEffect.GbRef.SetActive(false);

        var timer = 0.0f;

        while (timer < deActiObjEffect.Time)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        deActiObjEffect.GbRef.SetActive(true);
        deActiObjEffect.End = true;
        this.effectCounter++;
    }

    private IEnumerator BaloonDialogue(Baloon baloonEffect)
    {
        var baloonTempLink = baloonEffect.NpcRef.GetComponentInChildren<BaloonBeha>(true);

        var sentences = baloonTempLink.TextAssetRef.text.Split('\n');
        baloonTempLink.gameObject.SetActive(true);

        for (var i = baloonEffect.StartLine; i <= baloonEffect.EndLine; i++)
        {
            baloonTempLink.TextRef.text = sentences[i];
            yield return new WaitForSeconds(baloonEffect.BaloonSpeed);
        }

        baloonTempLink.CanvasRef.gameObject.SetActive(false);
        baloonEffect.End = true;
        this.effectCounter++;
    }

    private IEnumerator BubbleAdjustRot(Baloon baloonEffect)
    {
        var npcTempRef = baloonEffect.NpcRef;

        var gbTargetTemp = new GameObject();
        gbTargetTemp.transform.position = npcTempRef.transform.position;
        gbTargetTemp.transform.rotation = npcTempRef.transform.rotation;

        while (!baloonEffect.End)
        {
            gbTargetTemp.transform.LookAt(Camera.main.transform);
            npcTempRef.transform.rotation = Quaternion.Slerp(npcTempRef.transform.rotation, gbTargetTemp.transform.rotation, Time.deltaTime);
            yield return null;
        }

        DestroyObject(gbTargetTemp);
    }
    #endregion

    #region Ui Effects Methods
    private void UiEffectsHandler()
    {
        var uiEffectToEvaluate = new List<UiEffect>();
        uiEffectToEvaluate.AddRange(this.storySelected.Events[this.eventIndex].Effects.UiEffect);

        var gbCheckTempRepo = new List<GameObject>();
        var justOneDialogue = false;

        if (uiEffectToEvaluate.Count == 0)
        {
        }
        else
        {
            foreach (var uiEffect in uiEffectToEvaluate)
            {
                if (uiEffect.ObjMovEffect.GbToMove != null && uiEffect.ObjMovEffect.GbTarget != null)
                {
                    if (!this.IsGameobjectRefUnique(gbCheckTempRepo, uiEffect.ObjMovEffect.GbToMove))
                    {
                        continue;
                    }

                    this.totalEventEffects++;
                    GameController.Debugging("Ui Obj Move");
                    this.PlayUiObjMovingEffect(uiEffect.ObjMovEffect);
                    gbCheckTempRepo.Add(uiEffect.ObjMovEffect.GbToMove);
                }

                if (uiEffect.ObjActiEffect.GbRef != null)
                {
                    if (!this.IsGameobjectRefUnique(gbCheckTempRepo, uiEffect.ObjActiEffect.GbRef))
                    {
                        continue;
                    }

                    this.totalEventEffects++;
                    GameController.Debugging("Ui Obj Acti");
                    this.PlayUiObjActiEffect(uiEffect.ObjActiEffect);
                    gbCheckTempRepo.Add(uiEffect.ObjActiEffect.GbRef);
                }

                if (uiEffect.ObjDeActiEffect.GbRef != null)
                {
                    if (!this.IsGameobjectRefUnique(gbCheckTempRepo, uiEffect.ObjDeActiEffect.GbRef))
                    {
                        continue;
                    }

                    this.totalEventEffects++;
                    GameController.Debugging("Ui Obj DeActi");
                    this.PlayUiObjDeActiEffect(uiEffect.ObjDeActiEffect);
                    gbCheckTempRepo.Add(uiEffect.ObjDeActiEffect.GbRef);
                }

                if (uiEffect.UiDialogueEffect.DialogueRef != null && !justOneDialogue)
                {
                    this.totalEventEffects++;
                    GameController.Debugging("Ui Dialogue");
                    this.PlayDialogueEffect(uiEffect.UiDialogueEffect);
                    justOneDialogue = true;
                }
            }
        }
    }


    private void PlayUiObjMovingEffect(UiObjectMoving effectToPlay)
    {

    }

    private void PlayUiObjActiEffect(UiObjectActivation effectToPlay)
    {


    }

    private void PlayUiObjDeActiEffect(UiObjectDeActivation effectToPlay)
    {

    }

    private void PlayDialogueEffect(UiDialogue effectToPlay)
    {
        var delimiters = new char[2] { '/', '\n' };
        var initialSplit = effectToPlay.DialogueRef.text.Split(delimiters, StringSplitOptions.None);

        if (initialSplit.Length % 3 != 0)
        {
            GameController.Debugging("Txt File lines are not a multiple of 3");
            return;
        }

        for (var i = 0; i < initialSplit.Length; i += 3)
        {
            effectToPlay.Name.Add(initialSplit[i]);
            effectToPlay.Label.Add(initialSplit[i + 1]);
            effectToPlay.Sentence.Add(initialSplit[i + 2]);
        }

        for (var index = 0; index < effectToPlay.Sentence.Count; index++)
        {
            var tempSubdivision = effectToPlay.Sentence[index].Split('*');

            effectToPlay.Sentence[index] = null;

            for (int i = 0; i < tempSubdivision.Length; i++)
            {
                if (tempSubdivision[i].StartsWith(" "))
                {
                    tempSubdivision[i] = tempSubdivision[i].Remove(0, 1);
                }

                effectToPlay.Sentence[index] += tempSubdivision[i];

                if (tempSubdivision.Length - i >= 1)
                {
                    effectToPlay.Sentence[index] += "\n";
                }
            }
        }

        this.StartCoroutine(this.LivingDialogue(effectToPlay));
    }

    private IEnumerator LivingDialogue(UiDialogue dialogueEffect)
    {
        this.UiDialogueRequest.Invoke(dialogueEffect.Name[0], dialogueEffect.Label[0], dialogueEffect.Sentence[0]);
        var counter = 1;

        while (counter < dialogueEffect.Name.Count)
        {
            if (Input.GetButtonDown(buttonsJoy.X.ToString()) || Input.GetButtonDown(buttonsPc.E.ToString()))
            {
                this.UiDialogueRequest.Invoke(dialogueEffect.Name[counter], dialogueEffect.Label[counter], dialogueEffect.Sentence[counter]);
                counter++;
            }
            yield return null;
        }

        while (!Input.GetButtonDown(buttonsJoy.X.ToString()) && !Input.GetButtonDown(buttonsPc.E.ToString()))
        {
            yield return null;
        }

        this.DialogueEnded.Invoke();
        dialogueEffect.End = true;
        this.effectCounter++;
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