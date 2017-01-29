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
    private Transform playerTransform;
    private Transform cameraTransform;
    private float currentX;
    private float currentY;
    private float currentDistance;
    private Coroutine plCamMoving;

    private bool CameraByDesignToPlayer = true;
    private RaycastHit hitInfo;

    private bool corStoryStopped;
    private bool cameraByStoryToPlayer = true;
    private GameObject lastDesignCamera;

    private CameraStyle currentCamera = CameraStyle.player;
    private enum CameraStyle
    {
        player,
        designer,
        story
    }
    #endregion Private Variables

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
        this.plCamMoving = this.StartCoroutine(this.PlayerCameraMoving());
    }

    #endregion Initializing Camera in Gameplay Scenes

    #region Taking Game Controller Reference and Link the Initializer Event
    private void Awake()
    {
        var gcLink = this.GetComponent<GameController>();

        gcLink.gpInitializer.AddListener(this.SettingPlayerCamera);
    }

    #endregion Taking Game Controller Reference and Link the Initializer Event

    #region Camera Follow
    private void Update()
    {
        if (this.PlayerCamUpdateCheck())
            this.plCamMoving = this.StartCoroutine(this.PlayerCameraMoving());
    }

    private bool PlayerCamUpdateCheck()
    {
        if (this.currentCamera != CameraStyle.player) return false;
        if (this.plCamMoving != null) return false;
        if (Mathf.Abs((this.currentDistance * this.currentDistance) - (this.cameraTransform.position - this.playerTransform.position).sqrMagnitude) < 0.1f) return false;
        if (Quaternion.Angle(this.cameraTransform.rotation, this.CameraTargetTransform.rotation) < 0.1f) return false;
        return true;
    }

    private void PlayerCameraDirectInput(float xInput, float yInput, float distInput)
    {
        this.currentX = xInput;
        this.currentY = yInput;
        this.currentDistance = distInput;

        if (this.plCamMoving == null) this.plCamMoving = this.StartCoroutine(this.PlayerCameraMoving());
    }

    private IEnumerator PlayerCameraMoving()
    {
        var posReached = false;

        while (!posReached)
        {
            var dir = new Vector3(0, 0, -this.currentDistance);
            var rotation = Quaternion.Euler(this.currentY, this.currentX, 0);

            this.CameraTargetTransform.position = this.playerTransform.position + (rotation * dir);
            this.CameraTargetTransform.LookAt(this.playerTransform.position);



            var playerToCameraRay = new Ray(this.playerTransform.position, -this.CameraTargetTransform.forward);

            // Debug.DrawRay(this.CameraTargetTransform.position, this.CameraTargetTransform.forward * (this.CurrentPlCameraSettings.currentDistance - this.CurrentPlCameraSettings.distanceMin));
            this.cameraTransform.position = Vector3.Lerp(this.cameraTransform.position, !Physics.Raycast(playerToCameraRay, out this.hitInfo, this.currentDistance, this.Env.value) ? this.CameraTargetTransform.position : this.hitInfo.point, 6f * Time.deltaTime);

            this.cameraTransform.rotation = Quaternion.Slerp(this.cameraTransform.rotation, this.CameraTargetTransform.rotation, 6f * Time.deltaTime);

            if ((this.cameraTransform.position - this.CameraTargetTransform.position).sqrMagnitude < 0.1f
                && Quaternion.Angle(this.cameraTransform.rotation, this.CameraTargetTransform.rotation) < 0.1f) posReached = true;

            yield return null;
        }

        GameController.Debugging("Ended");
        this.plCamMoving = null;
    }
    #endregion Camera Follow

    #region General Methods

    private void SwitchByPlCameraToDeCamera(GameObject cameraDir)
    {
        this.CameraByDesignToPlayer = false;
        this.lastDesignCamera = cameraDir;

        if (this.cameraByStoryToPlayer)
        {
            this.StartCoroutine(this.LerpOnDesignerCamera(cameraDir));
            Debug.Log("Camera 2 on");
        }
    }

    private void SwitchByDeCameraToPlCamera()
    {
        // Inserire Comportamento di rientro telecamera sul giocatore in modo da prevedere cambi di rotazione e posizione troppo elevati
        this.CameraByDesignToPlayer = true;
        this.lastDesignCamera = null;
        Debug.Log("Camera 1 on");
        this.StopAllCoroutines();
    }

    private IEnumerator LerpOnDesignerCamera(GameObject cameraDir)
    {
        var targetRotation = cameraDir.transform.rotation;
        var targetPosition = cameraDir.transform.position;

        var posReached = false;
        Debug.Log("Coroutine Camera 2");

        while (!posReached)
        {
            this.cameraTransform.position = Vector3.Lerp(this.cameraTransform.position, targetPosition, 2f * Time.deltaTime);
            this.cameraTransform.rotation = Quaternion.Slerp(this.cameraTransform.rotation, targetRotation, 2f * Time.deltaTime);

            if (Quaternion.Angle(this.cameraTransform.rotation, targetRotation) < 0.1f
                && ((this.cameraTransform.position - targetPosition).sqrMagnitude < 0.1f)) posReached = true;


            yield return null;
        }
        Debug.Log("Coroutine Camera 2 finished");
    }

    private void SwitchByPlCameraToStoryCamera()
    {
        this.cameraByStoryToPlayer = false;
        Debug.Log("Camera 3 on");
        this.StopAllCoroutines();

    }

    private void SwitchByStoryCameraToPlCamera()
    {
        this.cameraByStoryToPlayer = true;
        if (this.lastDesignCamera != null && this.CameraByDesignToPlayer == false)
        {
            Debug.Log("Camera 2 on");
            this.StartCoroutine(this.LerpOnDesignerCamera(this.lastDesignCamera));
        }
        else
        {
            Debug.Log("Camera 1 on");


        }
    }
    #endregion General Methods
}