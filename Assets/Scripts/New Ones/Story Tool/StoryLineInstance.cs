using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Random = UnityEngine.Random;

#region Enum Types
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
    public float TimeTaken;
}

[Serializable]
public class PlayerSee
{
    public bool End;
    public GameObject GbRef;
    public float TimeTaken;
}

[Serializable]
public class PlayerPushBack
{
    public bool End;
    public GameObject GbRef;
    public float PushingBackPower;
    public float TimeTaken;
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
    public float TimeTaken;
}

[Serializable]
public class CameraShake
{
    public bool End;
    public float TimeTaken;
    public float ShakingPower;
    public float ShakingRoughness;
}

[Serializable]
public class CameraEventRevert
{
    public bool End;
    public bool Activated;
    public float TimeTaken;
}

[Serializable]
public class CameraStoryRevert
{
    public bool End;
    public bool Activated;
    public float TimeTaken;
}
#endregion Camera Effect Classes

#region Environment Npc Effects

[Serializable]
public class EnvironmentEffect
{
    public ObjectMoving ObjMovEffect;
    public ObjectActivation ObjActiEffect;
    public ObjectDeActivation ObjDeActiEffect;
    public ObjectRotateTo ObjRotateEffect;
    public Baloon BaloonEffect;
}

[Serializable]
public class ObjectMoving
{
    public bool End;
    public GameObject GbToMove;
    public GameObject GbTarget;
    public float TimeTaken;
}

[Serializable]
public class ObjectActivation
{
    public bool End;
    public GameObject GbRef;
    public float FadingInTime;
    public float Time;
    public float FadingOutTime;
}

[Serializable]
public class ObjectDeActivation
{
    public bool End;
    public GameObject GbRef;
    public float Time;
    public float FadingOutTime;
}

[Serializable]
public class ObjectRotateTo
{
    public bool End;
    public GameObject ObjectToRotate;
    public GameObject Target;
    public float TimeTaken;
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
    public bool End;
    public GameObject GbToMove;
    public GameObject GbTarget;
    public float TimeTaken;
}

[Serializable]
public class UiObjectActivation
{
    public bool End;
    public GameObject GbRef;
    public float FadingInTime;
    public float Time;
    public float FadingOutTime;
}

[Serializable]
public class UiObjectDeActivation
{
    public bool End;
    public GameObject GbRef;
    public float Time;
    public float FadingOutTime;
}

[Serializable]
public class UiDialogue
{
    public bool End;
    public TextAsset DialogueRef;
    public List<string> Name;
    public List<string> Label;
    public List<string> Sentence;
    public List<string> Sprite;
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
    public bool End;
    public int MovieIndex;
    public float SmoothInTime;
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
    public buttonsJoy PlayerInputJoy;
    public buttonsPc PlayerInputPc;
    public float EventEndDelay;
    public EvEffects Effects;
}

[Serializable]
public class SingleStory
{
    public string StoryName;
    public bool Active;
    public bool AutoComplete;
    public bool Completed;

    [Tooltip("Flag only if the story contains one event with 1 baloon effect")]
    public bool BaloonType;

    public GenTriggerConditions GenAccessCond;
    public List<ItemDependencies> ItemAccessCondition;

    public List<string> StoryActiveOnActivation;
    public List<string> StoryActiveOnCompletion;
    public List<string> StoryCompleteOnActivation;
    public List<string> StoryCompleteOnCompletion;
    public List<string> StoryLineCompleteOnActivation;
    public List<string> StoryLineCompleteOnCompletion;

    public controlStates PlayerControlEffect;
    public controlStates EndControlEffect;

    public List<StoryEvent> Events;
}

[Serializable]
public class StoryLine
{
    public string StoryLineName;
    public bool Completed;

    public List<string> StoryActiveOnCompletion;
    public List<string> StoryCompleteOnCompletion;
    public List<string> StoryLineCompleteOnCompletion;

    public List<SingleStory> Stories;
}
#endregion Story Element

public class StoryLineInstance : MonoBehaviour
{
    #region Public Variables
    public StoryLine CurrentStoryLine;
    #endregion

    #region Events
    public event_joy_pc_story ActivateStoryInputRequest;
    public event_string FormUnlockRequest;
    public event_cs ChangeCsEnterRequest, ChangeCsExitRequest;
    public event_string_string_string_string UiDialogueRequest;
    public UnityEvent DialogueEnded;
    public event_bool IsStoryMode;
    public event_int_float MovieRequest;
    public UnityEvent eraseInputMemoryRequest;
    public UnityEvent UpdateTempMemoryRequest;
    #endregion

    #region Private Variables
    private GameObject player;

    private SingleStory storySelected;



    private List<SingleStory> baloonStory = new List<SingleStory>();

    private int eventIndex;
    private int effectCounter;
    private int totalEventEffects;

    private List<Vector3> camLastPos = new List<Vector3>();
    private List<Quaternion> camLastRot = new List<Quaternion>();
    private int cameraChangeCounter;

    private QuestsManager questRepo;
    private MenuManager mmLink;
    #endregion

    #region Taking References and Linking Events
    private void Awake()
    {
        this.player = GameObject.FindGameObjectWithTag("Player");

        var envTempLink = this.player.GetComponent<EnvInputs>();
        envTempLink.storyActivationRequest.AddListener(this.CheckingAccessAndExistenceConditions);
        envTempLink.storyZoneExit.AddListener(this.ErasingInputMemory);
        envTempLink.storyZoneEnter.AddListener(this.LoadingInputMemory);

        var plTempLink = this.player.GetComponent<PlayerInputs>();
        plTempLink.storyLivingRequest.AddListener(this.TriggeringStoryByInput);

        this.questRepo = GameObject.FindGameObjectWithTag("GameController").GetComponent<QuestsManager>();

        this.mmLink = GameObject.FindGameObjectWithTag("GameController").GetComponent<MenuManager>();

        this.mmLink.movieEndNotification.AddListener(this.MovieEnd);

        var sdmTempLink = GameObject.FindGameObjectWithTag("GameController").GetComponent<SuperDataManager>();

        sdmTempLink.RequestUpdateByLoad.AddListener(this.LoadingCurrentState);
    }
    #endregion

    #region Check Initial Conditions Methods
    // Check Completed , Active and Relation Conditions
    private void CheckingAccessAndExistenceConditions(Collider trigger)
    {
        Debug.Log(trigger.name);

        if (this.CurrentStoryLine.Completed)
        {
            Debug.Log("Storyline already Completed");
            return;
        }

        bool storyFound = false;
        bool baloonFound = false;

        foreach (var story in this.CurrentStoryLine.Stories)
        {
            if ((story.GenAccessCond.STriggerRef == trigger || story.GenAccessCond.TriggerRef == trigger)
                && story.Active && !story.Completed)
            {
                if (!story.BaloonType)
                {
                    this.storySelected = story;
                    Debug.Log(this.storySelected.StoryName);
                    storyFound = true;
                    break;

                }
                else
                {
                    if (!this.baloonStory.Contains(story))
                    {

                        this.baloonStory.Add(story);
                        Debug.Log(this.baloonStory[this.baloonStory.Count - 1].StoryName);
                        baloonFound = true;
                        break;
                    }
                }

            }
            else
            {
                continue;
            }
        }

        if (storyFound && this.CheckItemConditions(this.storySelected))
        {
            this.InitializingStory();
        }
        else if (baloonFound)
        {
            this.LivingBaloonStory();
        }
        else Debug.Log("No Story is accessible through this Trigger " + trigger.name);
    }

    private bool CheckItemConditions(SingleStory storyToCheck)
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
             this.storySelected.GenAccessCond.PlayerInputPc,
             this.storySelected);
        }
        else if (this.IsNeededInput(this.storySelected.Events[0].PlayerInputJoy, this.storySelected.Events[0].PlayerInputPc))
        {
            this.ActivateStoryInputRequest.Invoke(
             this.storySelected.Events[0].PlayerInputJoy,
             this.storySelected.Events[0].PlayerInputPc,
             this.storySelected);
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

    private void TriggeringStoryByInput(SingleStory storyToTrigger)
    {
        if (this.storySelected == storyToTrigger)
        {
            this.LivingStoryEvent();
        }
        else
        {
            this.storySelected = storyToTrigger;
            this.LivingStoryEvent();
        }
    }

    private void RemovingBaloonStories()
    {
        this.StopAllCoroutines();

        foreach (var bstory in this.baloonStory)
        {
            bstory.Events[0].Effects.EnvEffect[0].BaloonEffect.NpcRef.GetComponentInChildren<BaloonBeha>().gameObject.SetActive(false);
        }

        this.baloonStory.Clear();
        this.baloonStory.TrimExcess();
    }

    private void ErasingInputMemory(Collider trigger)
    {
        if (this.storySelected.GenAccessCond.STriggerRef == trigger || this.storySelected.GenAccessCond.TriggerRef == trigger)
            this.eraseInputMemoryRequest.Invoke();
    }

    private void LoadingInputMemory(Collider trigger)
    {
        Debug.Log(trigger.name);

        if (this.CurrentStoryLine.Completed)
        {
            return;
        }

        bool storyFound = false;
        SingleStory storyRef = null;

        foreach (var story in this.CurrentStoryLine.Stories)
        {
            if ((story.GenAccessCond.STriggerRef == trigger || story.GenAccessCond.TriggerRef == trigger)
                && story.Active && !story.Completed)
            {
                if (!story.BaloonType)
                {
                    storyRef = story;
                    storyFound = true;
                    break;

                }
            }
        }

        if (storyFound && this.CheckItemConditions(storyRef))
        {
            if (this.IsNeededInput(
            storyRef.GenAccessCond.PlayerInputJoy,
            storyRef.GenAccessCond.PlayerInputPc))
            {
                this.ActivateStoryInputRequest.Invoke(
                 storyRef.GenAccessCond.PlayerInputJoy,
                 storyRef.GenAccessCond.PlayerInputPc,
                 storyRef);
            }
            else if (this.IsNeededInput(storyRef.Events[0].PlayerInputJoy, storyRef.Events[0].PlayerInputPc))
            {
                this.ActivateStoryInputRequest.Invoke(
                 storyRef.Events[0].PlayerInputJoy,
                 storyRef.Events[0].PlayerInputPc,
                 storyRef);
            }
        }
        
    }
    #endregion

    #region Living Story Core
    // Start the Story
    private void LivingStoryEvent()
    {
        GameController.Debugging("Living Story Started for the story " + this.storySelected.StoryName);

        this.RemovingBaloonStories();

        this.ActivationEffects();
        this.camLastPos.Add(Camera.main.transform.position);
        this.camLastRot.Add(Camera.main.transform.rotation);

        this.IsStoryMode.Invoke(true);
        this.ChangeCsEnterRequest.Invoke(this.storySelected.PlayerControlEffect);

        this.StartCoroutine(this.LivingStory());
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
                this.MovieEffectsHandler();

                GameController.Debugging("Total Effects", this.totalEventEffects);
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
                    this.MovieEffectsHandler();

                    GameController.Debugging("Total Effects", this.totalEventEffects);
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
                    this.MovieEffectsHandler();

                    GameController.Debugging("Total Effects", this.totalEventEffects);
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

        this.IsStoryMode.Invoke(false);
        this.ApplyingLivingEffects();
        this.ResettingStoryCommonVariables();
    }

    private void LivingBaloonStory()
    {
        GameController.Debugging("Living Baloon Story Started for the story " + this.baloonStory[this.baloonStory.Count - 1].StoryName);

        this.PlayBaloonStory(this.baloonStory[this.baloonStory.Count - 1].Events[0].Effects.EnvEffect[0].BaloonEffect, this.baloonStory[this.baloonStory.Count - 1]);
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
        if (this.storySelected.AutoComplete) this.storySelected.Completed = true;

        this.CompletionEffects();

        this.UpdateTempMemoryRequest.Invoke();
        this.ChangeCsExitRequest.Invoke(this.storySelected.EndControlEffect);
    }

    private void ActivationEffects()
    {
        foreach (var storyName in this.storySelected.StoryActiveOnActivation)
        {
            SingleStory storyToFind = this.SearchingStoryInside(storyName);

            if (storyToFind != null) storyToFind.Active = true;
            else
            {
                Debug.Log(storyName + " Not Found inside this Storyline");
            }

            SingleStory storyToFindInRepo = this.SearchingStoryInRepo(storyName);

            if (storyToFindInRepo != null) storyToFindInRepo.Active = true;
            else
            {
                Debug.Log(storyName + " Not Found inside Repo");
            }
        }

        foreach (var storyName in this.storySelected.StoryCompleteOnActivation)
        {
            SingleStory storyToFind = this.SearchingStoryInside(storyName);

            if (storyToFind != null) storyToFind.Completed = true;
            else
            {
                Debug.Log(storyName + " Not Found inside this Storyline");
            }

            SingleStory storyToFindInRepo = this.SearchingStoryInRepo(storyName);

            if (storyToFindInRepo != null) storyToFindInRepo.Completed = true;
            else
            {
                Debug.Log(storyName + " Not Found inside Repo");
            }
        }

        foreach (var storyLine in this.storySelected.StoryLineCompleteOnActivation)
        {
            StoryLine storyLineToFind = this.SearchingStoryLineInRepo(storyLine);

            if (storyLineToFind != null) storyLineToFind.Completed = true;
            else
            {
                Debug.Log(storyLine + " Not Found inside Repo");
            }
        }
    }

    private void CompletionEffects()
    {
        foreach (var storyName in this.storySelected.StoryActiveOnCompletion)
        {
            SingleStory storyToFind = this.SearchingStoryInside(storyName);

            if (storyToFind != null) storyToFind.Active = true;
            else
            {
                Debug.Log(storyName + " Not Found inside this Storyline");
            }

            SingleStory storyToFindInRepo = this.SearchingStoryInRepo(storyName);

            if (storyToFindInRepo != null) storyToFindInRepo.Active = true;
            else
            {
                Debug.Log(storyName + " Not Found inside Repo");
            }
        }

        foreach (var storyName in this.storySelected.StoryCompleteOnCompletion)
        {
            SingleStory storyToFind = this.SearchingStoryInside(storyName);

            if (storyToFind != null) storyToFind.Completed = true;
            else
            {
                Debug.Log(storyName + " Not Found inside this Storyline");
            }

            SingleStory storyToFindInRepo = this.SearchingStoryInRepo(storyName);

            if (storyToFindInRepo != null) storyToFindInRepo.Completed = true;
            else
            {
                Debug.Log(storyName + " Not Found inside Repo");
            }
        }

        foreach (var storyLine in this.storySelected.StoryLineCompleteOnCompletion)
        {
            StoryLine storyLineToFind = this.SearchingStoryLineInRepo(storyLine);

            if (storyLineToFind != null) storyLineToFind.Completed = true;
            else
            {
                Debug.Log(storyLine + " Not Found inside Repo");
            }
        }

        if (this.storySelected.AutoComplete)
        {
            SingleStory storyToFindInRepo = this.SearchingStoryInRepo(this.storySelected.StoryName);

            if (storyToFindInRepo != null) storyToFindInRepo.Completed = true;
            else
            {
                Debug.Log(this.storySelected.StoryName + " Not Found inside Repo");
            }
        }
    }

    private SingleStory SearchingStoryInside(string storyName)
    {
        return this.CurrentStoryLine.Stories.Find(x => x.StoryName == storyName);
    }

    private SingleStory SearchingStoryInRepo(string storyName)
    {
        StoryLine storyline = this.questRepo.StoryLineRepo.Find(x => x.Stories.Find(y => y.StoryName == storyName) != null);

        if (storyline != null) return storyline.Stories.Find(x => x.StoryName == storyName);
        else
        {
            return null;
        }
    }

    private StoryLine SearchingStoryLineInRepo(string storyLineName)
    {
        return this.questRepo.StoryLineRepo.Find(x => x.StoryLineName == storyLineName);
    }

    private void LoadingCurrentState()
    {
        EnvDatas sceneData = GameObject.FindGameObjectWithTag("GameController").GetComponent<SuperDataManager>().EnvSensData.Find(x => x.GpSceneName == SceneManager.GetActiveScene().name);

        this.CurrentStoryLine.Completed = sceneData.SlState.IsCompleted;

        if (this.CurrentStoryLine.Stories.Count != sceneData.SlState.Story.Count)
        {
            Debug.Log("Quest Syncro not possible");
            return;
        }
        else
        {
            for (int storyIndex = 0; storyIndex < this.CurrentStoryLine.Stories.Count; storyIndex++)
            {
                this.CurrentStoryLine.Stories[storyIndex].Active = sceneData.SlState.Story[storyIndex].IsActive;
                this.CurrentStoryLine.Stories[storyIndex].Completed = sceneData.SlState.Story[storyIndex].IsCompleted;
            }
        }

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
            if (plaEffectsToEvaluate.PushingBackEffect.PushingBackPower > 0
                && plaEffectsToEvaluate.PushingBackEffect.TimeTaken > 0 && plaEffectsToEvaluate.PushingBackEffect.GbRef != null)
            {
                this.totalEventEffects++;
                GameController.Debugging("Player Pushing Back");
                this.PlayPlayerPushingBackEffect(plaEffectsToEvaluate.PushingBackEffect);
            }

        }
        else if (plaEffectsToEvaluate.PushingBackEffect.PushingBackPower > 0 && plaEffectsToEvaluate.PushingBackEffect.TimeTaken > 0 && plaEffectsToEvaluate.PushingBackEffect.GbRef != null)
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
        this.player.transform.rotation = effectToPlay.GbRef.transform.rotation;
        effectToPlay.End = true;
        this.effectCounter++;
    }

    private void PlayPlayerMoveEffect(PlayerMove effectToPlay)
    {
        if (effectToPlay.TimeTaken == 0)
        {
            this.player.transform.position = effectToPlay.GbRef.transform.position;
            this.player.transform.rotation = effectToPlay.GbRef.transform.rotation;
            effectToPlay.End = true;
            this.effectCounter++;
        }
        else
        {
            this.StartCoroutine(this.MovingPlayer(effectToPlay));

        }

    }

    private void PlayPlayerRewardEffect(PlayerReward effectToPlay)
    {
        this.FormUnlockRequest.Invoke(effectToPlay.FormName);
        effectToPlay.End = true;
        this.effectCounter++;
    }

    private void PlayPlayerSeeEffect(PlayerSee effectToPlay)
    {
        if (effectToPlay.TimeTaken == 0)
        {
            this.player.transform.LookAt(effectToPlay.GbRef.transform);
            effectToPlay.End = true;
            this.effectCounter++;
        }
        else
        {
            this.StartCoroutine(this.RotatePlayer(effectToPlay));
        }
    }

    private void PlayPlayerPushingBackEffect(PlayerPushBack effectToPlay)
    {
        this.StartCoroutine(this.PushBackPlayer(effectToPlay));
    }

    private IEnumerator MovingPlayer(PlayerMove movingPlayerEffect)
    {
        var whereToMove = movingPlayerEffect.GbRef;
        var objToMove = this.player;

        var timeTaken = movingPlayerEffect.TimeTaken;

        var oriPos = this.player.transform.position;
        var oriRot = this.player.transform.rotation;
        var targetRotation = whereToMove.transform.rotation;
        var targetPosition = whereToMove.transform.position;

        var posReached = false;

        var timePassed = 0f;

        while (!posReached)
        {
            timePassed += Time.deltaTime / timeTaken;

            objToMove.transform.position = Vector3.Lerp(
                oriPos,
                targetPosition,
                timePassed);
            objToMove.transform.rotation = Quaternion.Slerp(
                oriRot,
                targetRotation,
                timePassed);

            if (timePassed >= 1)
            {
                posReached = true;
            }

            yield return null;
        }

        movingPlayerEffect.End = true;
        this.effectCounter++;
        GameController.Debugging("Effect Counter", this.effectCounter);
    }

    private IEnumerator PushBackPlayer(PlayerPushBack pushBackEffect)
    {
        var targetPosition = this.player.transform.position - (Vector3.ProjectOnPlane(pushBackEffect.GbRef.transform.forward, this.player.transform.up).normalized * pushBackEffect.PushingBackPower);
        var objToMove = this.player;

        var timeTaken = pushBackEffect.TimeTaken;

        var oriPos = this.player.transform.position;

        var posReached = false;

        var timePassed = 0f;

        while (!posReached)
        {
            timePassed += Time.deltaTime / timeTaken;

            objToMove.transform.position = Vector3.Lerp(
                oriPos,
                targetPosition,
                timePassed);

            if (timePassed >= 1)
            {
                posReached = true;
            }

            yield return null;
        }

        pushBackEffect.End = true;
        this.effectCounter++;
        GameController.Debugging("Effect Counter", this.effectCounter);
    }

    private IEnumerator RotatePlayer(PlayerSee rotateEffect)
    {
        var objToMove = this.player;
        var timeTaken = rotateEffect.TimeTaken;

        var tempObj = new GameObject("temp");
        tempObj.transform.position = this.player.transform.position;
        tempObj.transform.rotation = this.player.transform.rotation;

        tempObj.transform.LookAt(rotateEffect.GbRef.transform);

        var oriRot = this.player.transform.rotation;
        var targetRotation = tempObj.transform.rotation;

        var posReached = false;

        var timePassed = 0f;

        while (!posReached)
        {
            timePassed += Time.deltaTime / timeTaken;

            objToMove.transform.rotation = Quaternion.Slerp(
                objToMove.transform.rotation,
                targetRotation,
                timePassed);

            if (timePassed >= 1)
            {
                posReached = true;
            }

            yield return null;
        }

        rotateEffect.End = true;
        this.effectCounter++;
        GameController.Debugging("Effect Counter", this.effectCounter);
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
        else if (camEffectsToEvaluate.CameraShakeEffect.ShakingPower > 0 &&
                 camEffectsToEvaluate.CameraShakeEffect.ShakingRoughness > 0 &&
                 camEffectsToEvaluate.CameraShakeEffect.TimeTaken > 0)
        {
            this.totalEventEffects++;
            GameController.Debugging("Camera Shake");
            this.PlayCameraShakeEffect(camEffectsToEvaluate.CameraShakeEffect);
        }
        else if (camEffectsToEvaluate.CameraErEffect.Activated)
        {
            this.totalEventEffects++;
            GameController.Debugging("Camera ER");
            this.PlayCameraErEffect(camEffectsToEvaluate.CameraErEffect);
        }
        else if (camEffectsToEvaluate.CameraSrEffect.Activated)
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

        if (effectToPlay.TimeTaken == 0)
        {
            Camera.main.transform.position = effectToPlay.GbRef.transform.position;
            Camera.main.transform.rotation = effectToPlay.GbRef.transform.rotation;
            effectToPlay.End = true;
            this.effectCounter++;
        }
        else
        {
            this.StartCoroutine(this.MovingStoryCamera(effectToPlay));

        }
    }

    private void PlayCameraShakeEffect(CameraShake effectToPlay)
    {
        this.StartCoroutine(this.ShakingStoryCamera(effectToPlay));
    }

    private void PlayCameraErEffect(CameraEventRevert effectToPlay)
    {
        if (this.cameraChangeCounter - 1 < 0)
        {
            Debug.Log("Event Revert Effect bad applied");
            effectToPlay.End = true;
            this.effectCounter++;
        }
        else if (effectToPlay.TimeTaken == 0)
        {
            Camera.main.transform.position = this.camLastPos[this.cameraChangeCounter - 1];
            Camera.main.transform.rotation = this.camLastRot[this.cameraChangeCounter - 1];
            effectToPlay.End = true;
            this.effectCounter++;
        }
        else
        {
            this.StartCoroutine(this.MovingStoryCamera(effectToPlay, this.cameraChangeCounter - 1));
        }
    }

    private void PlayCameraSrEffect(CameraStoryRevert effectToPlay)
    {
        if (effectToPlay.TimeTaken == 0)
        {
            Camera.main.transform.position = this.camLastPos[0];
            Camera.main.transform.rotation = this.camLastRot[0];
            effectToPlay.End = true;
            this.effectCounter++;
        }
        else
        {
            this.StartCoroutine(this.MovingStoryCamera(effectToPlay));
        }
    }

    private IEnumerator MovingStoryCamera(CameraMove movingCameraEffect)
    {
        var whereToMove = movingCameraEffect.GbRef;
        var objToMove = Camera.main;
        var timeTaken = movingCameraEffect.TimeTaken;

        var oriPos = objToMove.transform.position;
        var oriRot = objToMove.transform.rotation;
        var targetRotation = whereToMove.transform.rotation;
        var targetPosition = whereToMove.transform.position;

        var posReached = false;

        var timePassed = 0f;

        while (!posReached)
        {
            timePassed += Time.deltaTime / timeTaken;

            objToMove.transform.position = Vector3.Lerp(
                oriPos,
                targetPosition,
                timePassed);
            objToMove.transform.rotation = Quaternion.Slerp(
                oriRot,
                targetRotation,
                timePassed);

            if (timePassed >= 1)
            {
                posReached = true;
            }

            yield return null;
        }

        movingCameraEffect.End = true;
        this.effectCounter++;
        GameController.Debugging("Effect Counter", this.effectCounter);
    }

    private IEnumerator MovingStoryCamera(CameraEventRevert erCameraEffect, int listIndex)
    {
        var objToMove = Camera.main;

        var timeTaken = erCameraEffect.TimeTaken;

        var oriPos = objToMove.transform.position;
        var oriRot = objToMove.transform.rotation;
        var targetRotation = this.camLastRot[listIndex];
        var targetPosition = this.camLastPos[listIndex];

        var posReached = false;

        var timePassed = 0f;

        while (!posReached)
        {
            timePassed += Time.deltaTime / timeTaken;

            objToMove.transform.position = Vector3.Lerp(
                oriPos,
                targetPosition,
                timePassed);
            objToMove.transform.rotation = Quaternion.Slerp(
                oriRot,
                targetRotation,
                timePassed);

            if (timePassed >= 1)
            {
                posReached = true;
            }

            yield return null;
        }

        erCameraEffect.End = true;
        this.effectCounter++;
        GameController.Debugging("Effect Counter", this.effectCounter);
    }

    private IEnumerator MovingStoryCamera(CameraStoryRevert srCameraEffect)
    {
        var objToMove = Camera.main;

        var timeTaken = srCameraEffect.TimeTaken;

        var oriPos = objToMove.transform.position;
        var oriRot = objToMove.transform.rotation;
        var targetRotation = this.camLastRot[0];
        var targetPosition = this.camLastPos[0];

        var posReached = false;

        var timePassed = 0f;

        while (!posReached)
        {
            timePassed += Time.deltaTime / timeTaken;

            objToMove.transform.position = Vector3.Lerp(
                oriPos,
                targetPosition,
                timePassed);
            objToMove.transform.rotation = Quaternion.Slerp(
                oriRot,
                targetRotation,
                timePassed);

            if (timePassed >= 1)
            {
                posReached = true;
            }

            yield return null;
        }

        srCameraEffect.End = true;
        this.effectCounter++;
        GameController.Debugging("Effect Counter", this.effectCounter);
    }

    private IEnumerator ShakingStoryCamera(CameraShake shakingCameraEffect)
    {
        var timer = 0.0f;
        var originalCameraRot = Camera.main.transform.rotation;

        while (shakingCameraEffect.TimeTaken > timer)
        {
            var rotationAmount = Random.insideUnitSphere * shakingCameraEffect.ShakingPower;
            rotationAmount.z = 0;

            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, Camera.main.transform.rotation * Quaternion.Euler(rotationAmount), Time.deltaTime * shakingCameraEffect.ShakingRoughness);

            timer += Time.deltaTime;

            yield return null;
        }


        var oriRot = Camera.main.transform.rotation;
        var backToPos = 0.5f;
        var timePassed = 0f;

        while (timePassed <= 1)
        {
            timePassed += Time.deltaTime / backToPos;

            Camera.main.transform.rotation = Quaternion.Slerp(oriRot, originalCameraRot, timePassed);

            yield return null;
        }

        shakingCameraEffect.End = true;
        this.effectCounter++;
        GameController.Debugging("Effect Counter", this.effectCounter);
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

                if (envEffect.ObjRotateEffect.ObjectToRotate != null && envEffect.ObjRotateEffect.Target != null)
                {
                    if (!this.IsGameobjectRefUnique(gbCheckTempRepo, envEffect.ObjRotateEffect.ObjectToRotate))
                    {
                        continue;
                    }

                    this.totalEventEffects++;
                    GameController.Debugging("Object Rotate To");
                    this.PlayObjRotateEffect(envEffect.ObjRotateEffect);
                    gbCheckTempRepo.Add(envEffect.ObjRotateEffect.ObjectToRotate);
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
        if (effectToPlay.TimeTaken == 0)
        {
            effectToPlay.GbToMove.transform.position = effectToPlay.GbTarget.transform.position;
            effectToPlay.GbToMove.transform.rotation = effectToPlay.GbTarget.transform.rotation;
            effectToPlay.End = true;
            this.effectCounter++;
        }
        else
        {
            this.StartCoroutine(this.MovingObject(effectToPlay));
        }
    }

    private void PlayObjActiEffect(ObjectActivation effectToPlay)
    {
        if (effectToPlay.Time == 0 && effectToPlay.FadingInTime == 0 && effectToPlay.FadingOutTime == 0)
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
        if (effectToPlay.Time == 0 && effectToPlay.FadingOutTime == 0)
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

    private void PlayObjRotateEffect(ObjectRotateTo effectToPlay)
    {
        if (effectToPlay.TimeTaken == 0)
        {
            effectToPlay.ObjectToRotate.transform.LookAt(effectToPlay.Target.transform);
            effectToPlay.End = true;
            this.effectCounter++;
        }
        else
        {
            this.StartCoroutine(this.ObjectRotation(effectToPlay));
        }
    }

    private void PlayBaloonEffect(Baloon effectToPlay)
    {
        this.StartCoroutine(this.BubbleAdjustRot(effectToPlay));
        this.StartCoroutine(this.BaloonDialogue(effectToPlay));
    }

    private void PlayBaloonStory(Baloon effectToPlay, SingleStory baloonStorySelected)
    {
        this.StartCoroutine(this.BubbleAdjustRotStory(effectToPlay));
        this.StartCoroutine(this.BaloonDialogueStory(effectToPlay, baloonStorySelected));
    }

    private IEnumerator MovingObject(ObjectMoving movingObjEffect)
    {
        var whereToMove = movingObjEffect.GbTarget;
        var objToMove = movingObjEffect.GbToMove;
        var timeTaken = movingObjEffect.TimeTaken;

        var oriPos = objToMove.transform.position;
        var oriRot = objToMove.transform.rotation;
        var targetRotation = whereToMove.transform.rotation;
        var targetPosition = whereToMove.transform.position;

        var posReached = false;

        var timePassed = 0f;

        while (!posReached)
        {
            timePassed += Time.deltaTime / timeTaken;

            objToMove.transform.position = Vector3.Lerp(
                oriPos,
                targetPosition,
                timePassed);
            objToMove.transform.rotation = Quaternion.Slerp(
                oriRot,
                targetRotation,
                timePassed);

            if (timePassed >= 1)
            {
                posReached = true;
            }

            yield return null;
        }

        movingObjEffect.End = true;
        this.effectCounter++;
        GameController.Debugging("Effect Counter", this.effectCounter);
    }

    private IEnumerator TimedActivation(ObjectActivation actiObjEffect)
    {
        var imTempLink = actiObjEffect.GbRef.GetComponent<MeshRenderer>();
        var coOriginal = imTempLink.material.color;
        coOriginal.a = 0;
        imTempLink.material.color = coOriginal;
        var coTempCopy = imTempLink.material.color;

        actiObjEffect.GbRef.SetActive(true);

        if (actiObjEffect.FadingInTime == 0)
        {
            coTempCopy.a = 1;
            imTempLink.material.color = coTempCopy;
        }
        else
        {
            var alphaDelta = 1f / actiObjEffect.FadingInTime;
            while (imTempLink.material.color.a < 1)
            {
                coTempCopy.a += alphaDelta * Time.deltaTime;
                imTempLink.material.color = coTempCopy;
                yield return null;
            }
        }

        coTempCopy.a = 1;
        imTempLink.material.color = coTempCopy;

        var timer = 0.0f;

        while (timer < actiObjEffect.Time)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (actiObjEffect.FadingOutTime == 0)
        {
            coTempCopy.a = 0;
            imTempLink.material.color = coTempCopy;
        }
        else
        {
            var alphaDelta = 1f / actiObjEffect.FadingOutTime;

            while (imTempLink.material.color.a > 0)
            {
                coTempCopy.a -= alphaDelta * Time.deltaTime;
                imTempLink.material.color = coTempCopy;
                yield return null;
            }
        }

        if (actiObjEffect.Time != 0 && actiObjEffect.FadingOutTime != 0)
        {
            actiObjEffect.GbRef.SetActive(false);
        }

        coOriginal.a = 1;
        imTempLink.material.color = coOriginal;
        actiObjEffect.End = true;
        this.effectCounter++;
        GameController.Debugging("Effect Counter", this.effectCounter);
    }

    private IEnumerator TimedDeActivation(ObjectDeActivation deActiObjEffect)
    {
        var imTempLink = deActiObjEffect.GbRef.GetComponent<MeshRenderer>();
        var coOriginal = imTempLink.material.color;
        var coTempCopy = imTempLink.material.color;

        var timer = 0.0f;

        while (timer < deActiObjEffect.Time)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (deActiObjEffect.FadingOutTime == 0)
        {
            coTempCopy.a = 0;
            imTempLink.material.color = coTempCopy;
        }
        else
        {
            var alphaDelta = 1f / deActiObjEffect.FadingOutTime;

            while (imTempLink.material.color.a > 0)
            {
                coTempCopy.a -= alphaDelta * Time.deltaTime;
                imTempLink.material.color = coTempCopy;
                yield return null;
            }
        }

        deActiObjEffect.GbRef.SetActive(false);
        imTempLink.material.color = coOriginal;
        deActiObjEffect.End = true;
        this.effectCounter++;
        GameController.Debugging("Effect Counter", this.effectCounter);
    }

    private IEnumerator ObjectRotation(ObjectRotateTo objRotEffect)
    {
        var objToMove = objRotEffect.ObjectToRotate;
        var timeTaken = objRotEffect.TimeTaken;

        var oriRot = objToMove.transform.rotation;

        var target = new GameObject();

        target.transform.position = objToMove.transform.position;
        target.transform.rotation = objToMove.transform.rotation;

        target.transform.LookAt(objRotEffect.Target.transform);

        var posReached = false;

        var timePassed = 0.0f;

        while (!posReached)
        {
            timePassed += Time.deltaTime / timeTaken;

            objToMove.transform.rotation = Quaternion.Slerp(
                oriRot,
                target.transform.rotation,
                timePassed);

            if (timePassed >= 1)
            {
                posReached = true;
            }

            yield return null;
        }

        objRotEffect.End = true;
        this.effectCounter++;
        GameController.Debugging("Effect Counter", this.effectCounter);
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

        baloonTempLink.gameObject.SetActive(false);
        baloonEffect.End = true;
        this.effectCounter++;
        GameController.Debugging("Effect Counter", this.effectCounter);
    }

    private IEnumerator BubbleAdjustRot(Baloon baloonEffect)
    {
        var npcTempRef = baloonEffect.NpcRef;

        var gbTargetTemp = new GameObject();

        while (!baloonEffect.End)
        {
            gbTargetTemp.transform.position = npcTempRef.transform.position;
            gbTargetTemp.transform.rotation = npcTempRef.transform.rotation;
            gbTargetTemp.transform.LookAt(Camera.main.transform);
            npcTempRef.transform.rotation = Quaternion.Slerp(npcTempRef.transform.rotation, gbTargetTemp.transform.rotation, 5 * Time.deltaTime);
            yield return null;
        }

        baloonEffect.End = false;
        DestroyObject(gbTargetTemp);
    }

    private IEnumerator BaloonDialogueStory(Baloon baloonEffect, SingleStory baloonStorySelected)
    {
        var baloonTempLink = baloonEffect.NpcRef.GetComponentInChildren<BaloonBeha>(true);

        var sentences = baloonTempLink.TextAssetRef.text.Split('\n');
        baloonTempLink.gameObject.SetActive(true);

        for (var i = baloonEffect.StartLine; i <= baloonEffect.EndLine; i++)
        {
            baloonTempLink.TextRef.text = sentences[i];
            yield return new WaitForSeconds(baloonEffect.BaloonSpeed);
        }

        baloonTempLink.gameObject.SetActive(false);
        baloonEffect.End = true;
        this.baloonStory.Remove(baloonStorySelected);

        Debug.Log("Baloon Story Ended");
    }

    private IEnumerator BubbleAdjustRotStory(Baloon baloonEffect)
    {
        var npcTempRef = baloonEffect.NpcRef;

        var gbTargetTemp = new GameObject();

        while (!baloonEffect.End)
        {
            gbTargetTemp.transform.position = npcTempRef.transform.position;
            gbTargetTemp.transform.rotation = npcTempRef.transform.rotation;
            gbTargetTemp.transform.LookAt(Camera.main.transform);
            npcTempRef.transform.rotation = Quaternion.Slerp(npcTempRef.transform.rotation, gbTargetTemp.transform.rotation, 5 * Time.deltaTime);
            yield return null;
        }

        baloonEffect.End = false;
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
        if (effectToPlay.TimeTaken == 0)
        {
            effectToPlay.GbToMove.GetComponent<RectTransform>().position = effectToPlay.GbTarget.GetComponent<RectTransform>().position;
            effectToPlay.End = true;
            this.effectCounter++;
        }
        else
        {
            this.StartCoroutine(this.UiObjMoving(effectToPlay));
        }
    }

    private void PlayUiObjActiEffect(UiObjectActivation effectToPlay)
    {
        if (effectToPlay.Time == 0 && effectToPlay.FadingInTime == 0 && effectToPlay.FadingOutTime == 0)
        {
            effectToPlay.GbRef.SetActive(true);
            effectToPlay.End = true;
            this.effectCounter++;
        }
        else
        {
            this.StartCoroutine(this.UiObjTimedActi(effectToPlay));
        }
    }

    private void PlayUiObjDeActiEffect(UiObjectDeActivation effectToPlay)
    {
        if (effectToPlay.Time == 0 && effectToPlay.FadingOutTime == 0)
        {
            effectToPlay.GbRef.SetActive(false);
            effectToPlay.End = true;
            this.effectCounter++;
        }
        else
        {
            this.StartCoroutine(this.UiObjTimedDeActi(effectToPlay));
        }
    }

    private void PlayDialogueEffect(UiDialogue effectToPlay)
    {
        var delimiters = new char[] { '/', '\n' };
        var initialSplit = effectToPlay.DialogueRef.text.Split(delimiters, StringSplitOptions.None);

        if (initialSplit.Length % 4 != 0)
        {
            GameController.Debugging("Txt File lines are not a multiple of 4");
            effectToPlay.End = true;
            this.effectCounter++;
            return;
        }

        for (var i = 0; i < initialSplit.Length; i += 4)
        {
            effectToPlay.Name.Add(initialSplit[i]);
            effectToPlay.Label.Add(initialSplit[i + 1]);
            effectToPlay.Sprite.Add(initialSplit[i + 2]);
            effectToPlay.Sentence.Add(initialSplit[i + 3]);
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
        this.UiDialogueRequest.Invoke(dialogueEffect.Name[0], dialogueEffect.Label[0], dialogueEffect.Sentence[0], dialogueEffect.Sprite[0]);
        var counter = 1;
        var dialogueSkip = false;

        while (counter < dialogueEffect.Name.Count)
        {
            if (Input.GetButtonDown(buttonsJoy.Y.ToString()))
            {
                dialogueSkip = true;
                break;
            }

            if (Input.GetButtonDown(buttonsJoy.X.ToString()) || Input.GetButtonDown(buttonsPc.E.ToString()))
            {
                this.UiDialogueRequest.Invoke(dialogueEffect.Name[counter], dialogueEffect.Label[counter], dialogueEffect.Sentence[counter], dialogueEffect.Sprite[counter]);
                counter++;
            }

            yield return null;
        }

        while (!Input.GetButtonDown(buttonsJoy.X.ToString()) && !Input.GetButtonDown(buttonsPc.E.ToString()) && !dialogueSkip)
        {
            yield return null;
        }

        this.DialogueEnded.Invoke();
        dialogueEffect.End = true;
        this.effectCounter++;
        GameController.Debugging("Effect Counter", this.effectCounter);
    }

    private IEnumerator UiObjTimedActi(UiObjectActivation uiObjActiEffect)
    {
        var imTempLink = uiObjActiEffect.GbRef.GetComponent<Image>();
        var coOriginal = imTempLink.color;
        coOriginal.a = 0;
        imTempLink.color = coOriginal;
        var coTempCopy = imTempLink.color;

        uiObjActiEffect.GbRef.SetActive(true);

        if (uiObjActiEffect.FadingInTime == 0)
        {
            coTempCopy.a = 1;
            imTempLink.color = coTempCopy;
        }
        else
        {
            var alphaDelta = 1f / uiObjActiEffect.FadingInTime;

            while (imTempLink.color.a < 1)
            {
                coTempCopy.a += alphaDelta * Time.deltaTime;
                imTempLink.color = coTempCopy;
                yield return null;
            }
        }

        coTempCopy.a = 1;
        imTempLink.color = coTempCopy;

        var timer = 0.0f;

        while (timer < uiObjActiEffect.Time)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (uiObjActiEffect.FadingOutTime == 0)
        {
            coTempCopy.a = 0;
            imTempLink.color = coTempCopy;
        }
        else
        {
            var alphaDelta = 1f / uiObjActiEffect.FadingOutTime;

            while (imTempLink.color.a > 0)
            {
                coTempCopy.a -= alphaDelta * Time.deltaTime;
                imTempLink.color = coTempCopy;
                yield return null;
            }
        }

        if (uiObjActiEffect.Time != 0 && uiObjActiEffect.FadingOutTime != 0)
        {
            uiObjActiEffect.GbRef.SetActive(false);
        }
        coOriginal.a = 1;
        imTempLink.color = coOriginal;
        uiObjActiEffect.End = true;
        this.effectCounter++;
        GameController.Debugging("Effect Counter", this.effectCounter);
    }

    private IEnumerator UiObjTimedDeActi(UiObjectDeActivation uiObjDeActiEffect)
    {

        var imTempLink = uiObjDeActiEffect.GbRef.GetComponent<Image>();
        var coOriginal = imTempLink.color;
        var coTempCopy = imTempLink.color;

        var timer = 0.0f;

        while (timer < uiObjDeActiEffect.Time)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (uiObjDeActiEffect.FadingOutTime == 0)
        {
            coTempCopy.a = 0;
            imTempLink.color = coTempCopy;
        }
        else
        {
            var alphaDelta = 1f / uiObjDeActiEffect.FadingOutTime;

            while (imTempLink.color.a > 0)
            {
                coTempCopy.a -= alphaDelta * Time.deltaTime;
                imTempLink.color = coTempCopy;
                yield return null;
            }
        }


        uiObjDeActiEffect.GbRef.SetActive(false);
        imTempLink.color = coOriginal;
        uiObjDeActiEffect.End = true;
        this.effectCounter++;
        GameController.Debugging("Effect Counter", this.effectCounter);
    }

    private IEnumerator UiObjMoving(UiObjectMoving uiObjMovEffect)
    {
        var whereToMove = uiObjMovEffect.GbTarget.GetComponent<RectTransform>();
        var objToMove = uiObjMovEffect.GbToMove.GetComponent<RectTransform>();

        var timeTaken = uiObjMovEffect.TimeTaken;

        var oriPos = whereToMove.position;

        var posReached = false;

        var timePassed = 0.0f;

        while (!posReached)
        {
            timePassed += Time.deltaTime / timeTaken;

            objToMove.position = Vector3.Lerp(
                oriPos,
                whereToMove.position,
                timePassed);

            if (timePassed >= 1)
            {
                posReached = true;
            }

            yield return null;
        }

        uiObjMovEffect.End = true;
        this.effectCounter++;
        GameController.Debugging("Effect Counter", this.effectCounter);
    }
    #endregion

    #region Movie Effects Methods
    private void MovieEffectsHandler()
    {
        var movieEffectToEvaluate = this.storySelected.Events[this.eventIndex].Effects.MovieEffect;

        if (movieEffectToEvaluate.MovieIndex > 0)
        {
            this.totalEventEffects++;
            Debug.Log("Movie");
            this.PlayMovieEffect(movieEffectToEvaluate);
        }
    }

    private void PlayMovieEffect(MovieEffect movEffect)
    {
        this.MovieRequest.Invoke(movEffect.MovieIndex - 1, movEffect.SmoothInTime);
    }

    private void MovieEnd()
    {
        this.effectCounter++;
    }
    #endregion

    #region Edit Mode Methods
    
    public void OnValidate()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<SuperDataManager>().AddingStoryLineEditMode(this.CurrentStoryLine);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<QuestsManager>().AddToRepository(this.CurrentStoryLine);
    }
    
    #endregion
}