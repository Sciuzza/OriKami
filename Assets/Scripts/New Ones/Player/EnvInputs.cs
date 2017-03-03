using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class EnvInputs : MonoBehaviour
{
    #region Private Variables
    private CharacterController ccLink;
    private bool onWaterFlag = false, onAir = false, onWater = false, onGround = false;
    private CollTrigger ctTempLink;
    private SoundManager soundRef;
    #endregion

    #region Events
    public event_vfscript_string vFissureRequestOn;
    public event_ps psChanged;
    public UnityEvent vFissureRequestOff, playerIsDead, cameraOnRequest;
    public event_Gb cameraOffRequest;

    public event_collider storyActivationRequest, storyZoneExit, storyZoneEnter;
    public UnityEvent SaveRequestByCheck;
    public event_Gb wallDCheckRequest;
    public event_int_gb IncrementNCollRequest;
    #endregion

    #region Taking References
    void Awake()
    {
        ccLink = this.GetComponent<CharacterController>();
        soundRef = GameObject.FindGameObjectWithTag("GameController").GetComponent<SoundManager>();
    }
    #endregion

    #region Physic State Inputs Handler
    void Update()
    {
        PhysicStateInput();
    }

    private void PhysicStateInput()
    {
        if ((ccLink.collisionFlags & CollisionFlags.Below) != 0)
        {
            if (onAir)
            {

                if (!onWaterFlag && !onWater)
                {
                    //Debug.Log("Terra");
                    psChanged.Invoke(physicStates.onGround);
                    onWater = false;
                    onGround = true;
                }
                else if (onWaterFlag && !onWater)
                {
                    //Debug.Log("Acqua");
                    psChanged.Invoke(physicStates.onWater);
                    onWater = true;
                    onGround = false;
                }

                onAir = false;
            }
            else
            {
                if (!onWaterFlag && onWater && !onGround)
                {
                    //Debug.Log("Terra");
                    psChanged.Invoke(physicStates.onGround);
                    onWater = false;
                    onGround = true;

                }
                else if (onWaterFlag && !onWater && onGround)
                {
                    // Debug.Log("Acqua");
                    psChanged.Invoke(physicStates.onWater);
                    onWater = true;
                    onGround = false;
                }
            }

        }
        else if ((ccLink.collisionFlags & CollisionFlags.Below) == 0 && !onAir)
        {
            //Debug.LogWarning("Aria");
            psChanged.Invoke(physicStates.onAir);

            onWater = false;
            onGround = false;
            onAir = true;
        }
    }
    #endregion

    #region Triggers Handler
    void OnTriggerEnter(Collider envTrigger)
    {
        switch (envTrigger.gameObject.tag)
        {
            case "vAbilityta":
            case "vAbilitytb":
                vFissureRequestOn.Invoke(envTrigger.gameObject.GetComponentInParent<VFissure>(), envTrigger.gameObject.tag);
                break;
            case "hAbilityta":
            case "hAbilitytb":
                vFissureRequestOn.Invoke(envTrigger.gameObject.GetComponentInParent<VFissure>(), envTrigger.gameObject.tag);
                break;
            case "dAbilityta":
            case "dAbilitytb":
                vFissureRequestOn.Invoke(envTrigger.gameObject.GetComponentInParent<VFissure>(), envTrigger.gameObject.tag);
                break;
            case "Water":
                onWaterFlag = true;
                break;
            case "Death":
                playerIsDead.Invoke();
                break;
            case "Camera Control":
                this.cameraOffRequest.Invoke(envTrigger.gameObject.GetComponentInChildren<CameraDirRef>().CameraDirRefObj);
                break;
            case "Story Triggers":
                if (!FSMChecker.storyMode) this.storyActivationRequest.Invoke(envTrigger);
                else
                {
                    this.storyZoneEnter.Invoke(envTrigger);
                }
                break;
            case "CheckPoint":
                envTrigger.gameObject.SetActive(false);
                this.SaveRequestByCheck.Invoke();
                soundRef.PlaySound(1, 9);
                Debug.Log("Saved");
                break;
            case "GoldenCrane":                
                this.IncrementNCollRequest.Invoke(0, envTrigger.gameObject);
                soundRef.PlaySound(1, 8);

                this.ctTempLink = envTrigger.gameObject.GetComponent<CollTrigger>();
                if (this.ctTempLink != null && this.ctTempLink.TriggerStory) this.ctTempLink.CheckingStoryCondition();

                break;
            case "BlackSmith":
                this.IncrementNCollRequest.Invoke(2, envTrigger.gameObject);

                this.ctTempLink = envTrigger.gameObject.GetComponent<CollTrigger>();
                if (this.ctTempLink != null && this.ctTempLink.TriggerStory) this.ctTempLink.CheckingStoryCondition();

                break;
            case "V3":
                this.IncrementNCollRequest.Invoke(3, envTrigger.gameObject);

                this.ctTempLink = envTrigger.gameObject.GetComponent<CollTrigger>();
                if (this.ctTempLink != null && this.ctTempLink.TriggerStory) this.ctTempLink.CheckingStoryCondition();

                break;
            case "DRocks":
                this.wallDCheckRequest.Invoke(envTrigger.gameObject);

                this.ctTempLink = envTrigger.gameObject.GetComponent<CollTrigger>();
                if (this.ctTempLink != null && this.ctTempLink.TriggerStory) this.ctTempLink.CheckingStoryCondition();

                break;
        }

    }

    void OnTriggerExit(Collider envTrigger)
    {

        switch (envTrigger.gameObject.tag)
        {
            case "vAbilityta":
            case "vAbilitytb":
                vFissureRequestOff.Invoke();
                break;
            case "hAbilityta":
            case "hAbilitytb":
                vFissureRequestOff.Invoke();
                break;
            case "dAbilityta":
            case "dAbilitytb":
                vFissureRequestOff.Invoke();
                break;
            case "Water":
                onWaterFlag = false;
                break;
            case "Camera Control":
                this.cameraOnRequest.Invoke();
                break;
            case "Story Triggers":
                this.storyZoneExit.Invoke(envTrigger);
                break;
        }
    }

    /*
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch (hit.collider.gameObject.tag)
        {
            case "DRocks":
                this.wallDCheckRequest.Invoke(hit.collider.gameObject);

                this.ctTempLink = hit.gameObject.GetComponent<CollTrigger>();
                if (this.ctTempLink != null && this.ctTempLink.TriggerStory) this.ctTempLink.CheckingStoryCondition();

                break;
        }
    }
    */
    #endregion
}
