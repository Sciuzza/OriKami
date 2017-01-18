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
    private bool cameraPlayerControl = true;
    private RaycastHit hitInfo;
    #endregion Private Variables

    #region Initializing Camera in Gameplay Scenes
    public void SettingPlayerCamera(GameObject player)
    {
        this.cameraTransform = Camera.main.transform;
        this.playerTransform = player.transform;
        Debug.Log("Camera Initialization Done");

        var fsmLink = player.GetComponent<FSMChecker>();

        fsmLink.switchingCameraControlToOFF.AddListener(this.SwitchingCameraControlToOff);
        fsmLink.switchingCameraControlToOn.AddListener(this.SwitchingCameraControlToOn);
        fsmLink.switchingCameraToStoryRequest.AddListener(this.SwitchingCameraToStoryCamera);
        fsmLink.switchingCameraToPlayer.AddListener(this.SwitchingCameraToPlayer);
    }

    #endregion Initializing Camera in Gameplay Scenes

    #region Taking Game Controller Reference and Link the Initializer Event
    private void Awake()
    {
        var gcLink = this.GetComponent<GameController>();

        gcLink.gpInitializer.AddListener(this.SettingPlayerCamera);
    }
    #endregion Taking Game Controller Reference and Link the Initializer Event

    #region Camera Player Mouse Zoom
    private void Update()
    {
        if (!this.cameraPlayerControl) return;

        // Pc COde
        if (Input.GetMouseButton(0))
        {
            Cursor.visible = false;
            this.currentX += Input.GetAxis("Mouse X") * this.CurrentPlCameraSettings.sensitivityX;
            this.currentY -= Input.GetAxis("Mouse Y") * this.CurrentPlCameraSettings.sensitivityY;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Cursor.visible = true;
        }

        this.CurrentPlCameraSettings.currentDistance -= Input.GetAxis("Mouse ScrollWheel")
                                                   * this.CurrentPlCameraSettings.sensitivityZoom;

        this.CurrentPlCameraSettings.currentDistance = Mathf.Clamp(
            this.CurrentPlCameraSettings.currentDistance,
            this.CurrentPlCameraSettings.distanceMin,
            this.CurrentPlCameraSettings.distanceMax);
        this.currentY = Mathf.Clamp(this.currentY, this.CurrentPlCameraSettings.yAngleMin, this.CurrentPlCameraSettings.yAngleMax);

        // Joy Code
        this.currentX += Input.GetAxis("RJHor") * this.CurrentPlCameraSettings.sensitivityX;
        this.currentY += Input.GetAxis("RJVer") * this.CurrentPlCameraSettings.sensitivityY;

        if (Input.GetButton("CamZjoy"))
            this.CurrentPlCameraSettings.currentDistance += Input.GetAxis("RJVer")
                                                       * this.CurrentPlCameraSettings.sensitivityZoom;

        this.CurrentPlCameraSettings.currentDistance = Mathf.Clamp(
            this.CurrentPlCameraSettings.currentDistance,
            this.CurrentPlCameraSettings.distanceMin,
            this.CurrentPlCameraSettings.distanceMax);
        this.currentY = Mathf.Clamp(this.currentY, this.CurrentPlCameraSettings.yAngleMin, this.CurrentPlCameraSettings.yAngleMax);
    }
    #endregion Camera Player Mouse Zoom

    #region Camera Follow
    private void LateUpdate()
    {
        if (!this.cameraPlayerControl) return;

        // ReSharper disable once InvertIf
        if (this.playerTransform != null)
        {
            var dir = new Vector3(0, 0, -this.CurrentPlCameraSettings.currentDistance);
            var rotation = Quaternion.Euler(this.currentY, this.currentX, 0);

            this.CameraTargetTransform.position = this.playerTransform.position + (rotation * dir);
            this.CameraTargetTransform.LookAt(this.playerTransform.position);

        

            var playerToCameraRay = new Ray(this.playerTransform.position, -this.CameraTargetTransform.forward);

            // Debug.DrawRay(this.CameraTargetTransform.position, this.CameraTargetTransform.forward * (this.CurrentPlCameraSettings.currentDistance - this.CurrentPlCameraSettings.distanceMin));
            this.cameraTransform.position = Vector3.Lerp(this.cameraTransform.position, !Physics.Raycast(playerToCameraRay, out this.hitInfo, this.CurrentPlCameraSettings.currentDistance, this.Env.value) ? this.CameraTargetTransform.position : this.hitInfo.point, 6f * Time.deltaTime);

            this.cameraTransform.rotation = Quaternion.Slerp(this.cameraTransform.rotation, this.CameraTargetTransform.rotation, 6f * Time.deltaTime);
        }

       

       
    }
    #endregion Camera Follow

    #region General Methods

    private void SwitchingCameraControlToOff(GameObject cameraDir)
    {
        this.cameraPlayerControl = false;

        this.StartCoroutine(this.LerpOnDesignerCamera(cameraDir));
    }

    private IEnumerator LerpOnDesignerCamera(GameObject cameraDir)
    {
        var targetRotation = cameraDir.transform.rotation;
        var targetPosition = cameraDir.transform.position;

        var posReached = false;

        while (!this.cameraPlayerControl && !posReached)
        {
            this.cameraTransform.position = Vector3.Lerp(this.cameraTransform.position, targetPosition, 2f * Time.deltaTime);
            this.cameraTransform.rotation = Quaternion.Slerp(this.cameraTransform.rotation, targetRotation, 2f * Time.deltaTime);

            if (Quaternion.Angle(this.cameraTransform.rotation, targetRotation) < 0.1f
                && ((this.cameraTransform.position - targetPosition).sqrMagnitude < 0.1f)) posReached = true;

            yield return null;
        }
    }

    private void SwitchingCameraControlToOn()
    {
        // Inserire Comportamento di rientro telecamera sul giocatore in modo da prevedere cambi di rotazione e posizione troppo elevati
        this.cameraPlayerControl = true;

        this.StopCoroutine("LerpOnDesignerCamera");
    }

    private void SwitchingCameraToStoryCamera()
    {
        this.cameraPlayerControl = false;
    }

    private void SwitchingCameraToPlayer()
    {
        this.cameraPlayerControl = true;
    }
    #endregion General Methods
}