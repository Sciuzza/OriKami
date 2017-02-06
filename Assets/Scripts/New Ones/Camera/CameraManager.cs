using System.Collections;

using UnityEngine;

public class CameraManager : MonoBehaviour
{
    #region Public Variables

    [HideInInspector]
    public CameraPlayer CurrentPlCameraSettings;
    public Transform CameraTargetTransform;
    public LayerMask Env;
    #endregion Public Variables

    #region Private Variables

    // Variable needed in all the three styles
    private enum CameraStyle
    {
        player,
        designer,
        story
    }

    private Transform cameraTransform;
    private RaycastHit hitInfo;
    private CameraStyle currentCamera = CameraStyle.player;

    // Variables needed for Player Style Camera
    private Transform playerTransform;
    private float currentX;
    private float currentY;
    private float currentDistance;
    private Coroutine plCamMoving;

    // Variables needed for Designer Style Camera
    private Coroutine deCamMoving;
    private GameObject lastDesignCamera;
    private float camSpeed = 6;
    #endregion Private Variables

    #region Events
    public event_float_float_float reAdjustingCamValues; 
    #endregion

    #region Initializing Camera in Gameplay Scenes
    public void SettingPlayerCamera(GameObject player)
    {
        this.cameraTransform = Camera.main.transform;
        this.playerTransform = player.transform;
        Debug.Log("Camera Initialization Done");

        var fsmLink = player.GetComponent<FSMChecker>();

        fsmLink.switchingCameraControlToOFF.AddListener(this.SwitchByPlCameraToDeCamera);
        fsmLink.switchingCameraControlToOn.AddListener(this.SwitchByDeCameraToPlCamera);
        fsmLink.switchingCameraToStoryRequest.AddListener(this.SwitchByPlCameraToStoryCamera);
        fsmLink.switchingCameraToPlayer.AddListener(this.SwitchByStoryCameraToPlCamera);
        fsmLink.plCameraMoveUsed.AddListener(this.PlayerCameraDirectInput);


        this.currentDistance = this.CurrentPlCameraSettings.currentDistance;
        this.currentX = 0;
        this.currentY = 0;
        this.plCamMoving = this.StartCoroutine(this.PlayerCameraMoving());
    }

    #endregion Initializing Camera in Gameplay Scenes

    #region Taking Game Controller Reference and Link the Initializer Event
    private void Awake()
    {
        var gcLink = this.GetComponent<GameController>();

        gcLink.gpInitializer.AddListener(this.SettingPlayerCamera);

        var scLink = this.GetComponent<SceneController>();

        scLink.stoppingLogicRequest.AddListener(this.StoppingCoro);
    }
    #endregion Taking Game Controller Reference and Link the Initializer Event

    #region Player Camera Handler
    private void PlayerCameraDirectInput(float xInput, float yInput, float distInput)
    {
        this.currentX = xInput;
        this.currentY = yInput;
        this.currentDistance = distInput;

        if (this.plCamMoving == null)
        {
            this.currentCamera = CameraStyle.player;
            this.plCamMoving = this.StartCoroutine(this.PlayerCameraMoving());
        }
    }

    private IEnumerator PlayerCameraMoving()
    {

        this.reAdjustingCamValues.Invoke(this.currentX, this.currentY, this.currentDistance);

        while (this.currentCamera == CameraStyle.player)
        {
            var dir = new Vector3(0, 0, -this.currentDistance);
            var rotation = Quaternion.Euler(this.currentY, this.currentX, 0);

            this.CameraTargetTransform.position = this.playerTransform.position + (rotation * dir);
            this.CameraTargetTransform.LookAt(this.playerTransform.position);

            /*
            if ((this.cameraTransform.position - this.CameraTargetTransform.position).sqrMagnitude < 0.1f
                && Quaternion.Angle(this.cameraTransform.rotation, this.CameraTargetTransform.rotation) < 0.1f) yield return null;
            */
            var playerToCameraRay = new Ray(this.playerTransform.position, -this.CameraTargetTransform.forward);

            // Debug.DrawRay(this.CameraTargetTransform.position, this.CameraTargetTransform.forward * (this.CurrentPlCameraSettings.currentDistance - this.CurrentPlCameraSettings.distanceMin));
            this.cameraTransform.position = Vector3.Lerp(this.cameraTransform.position, !Physics.Raycast(playerToCameraRay, out this.hitInfo, this.currentDistance, this.Env.value) ? this.CameraTargetTransform.position : this.hitInfo.point, this.camSpeed * Time.deltaTime);

            this.cameraTransform.rotation = Quaternion.Slerp(this.cameraTransform.rotation, this.CameraTargetTransform.rotation, this.camSpeed * Time.deltaTime);

            yield return null;
        }

        GameController.Debugging("Coroutine Player Camera Ended");
        this.plCamMoving = null;
    }
    #endregion Camera Follow

    #region Designer Camera Handler
    private void SwitchByPlCameraToDeCamera(GameObject cameraDir)
    {
        this.currentCamera = CameraStyle.designer;
        this.lastDesignCamera = cameraDir;

        this.deCamMoving = this.StartCoroutine(this.LerpOnDesignerCamera(cameraDir));
        GameController.Debugging("Designer Camera On");

    }

    private IEnumerator LerpOnDesignerCamera(GameObject cameraDir)
    {
        var targetRotation = cameraDir.transform.rotation;
        var targetPosition = cameraDir.transform.position;

        var posReached = false;
        
        while (!posReached && this.currentCamera == CameraStyle.designer)
        {
            this.cameraTransform.position = Vector3.Lerp(this.cameraTransform.position, targetPosition, 2f * Time.deltaTime);
            this.cameraTransform.rotation = Quaternion.Slerp(this.cameraTransform.rotation, targetRotation, 2f * Time.deltaTime);

            if (Quaternion.Angle(this.cameraTransform.rotation, targetRotation) < 0.1f
                && ((this.cameraTransform.position - targetPosition).sqrMagnitude < 0.1f)) posReached = true;

            yield return null;
        }
        GameController.Debugging("Coroutine Designer Camera Ended");
        this.deCamMoving = null;
    }

    private void SwitchByDeCameraToPlCamera()
    {
        GameController.Debugging("Player Camera On");
        this.ReadjustingCameraTarget();
        this.currentCamera = CameraStyle.player;
        this.plCamMoving = this.StartCoroutine(this.PlayerCameraMoving());
    }

    private void ReadjustingCameraTarget()
    {
        var calPos = new GameObject();

        calPos.transform.position = this.lastDesignCamera.transform.position;
        calPos.transform.LookAt(this.playerTransform);

        this.currentX = calPos.transform.eulerAngles.y;
        this.currentY = calPos.transform.eulerAngles.x;
        this.currentDistance = (this.lastDesignCamera.transform.position - this.playerTransform.position).magnitude / 2;

        Destroy(calPos);
       
        //this.reAdjustingCamValues.Invoke(this.currentX, this.currentY, this.currentDistance);
        this.StartCoroutine(this.SmoothingSlerp());
    }

    private IEnumerator SmoothingSlerp()
    {
        var smoothTime = 1;

        var smoothOnDelta = 4 / smoothTime;

        this.camSpeed = 2;

        while (this.camSpeed < 6)
        {
            this.camSpeed += smoothOnDelta * Time.deltaTime;
            yield return null;
        }

        this.camSpeed = 6;
    }
    #endregion General Methods

    #region Story Camera Handler
    private void SwitchByPlCameraToStoryCamera()
    {
        Debug.Log("Camera 3 on");
        this.currentCamera = CameraStyle.story;
    }

    private void SwitchByStoryCameraToPlCamera()
    {
        Debug.Log("Player Camera On");
        this.currentCamera = CameraStyle.player;
        this.plCamMoving = this.StartCoroutine(this.PlayerCameraMoving());

    }
    #endregion

    #region General Methods
    private void StoppingCoro()
    {
        this.StopAllCoroutines();
    } 
    #endregion
}