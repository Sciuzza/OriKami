using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class CameraManager : MonoBehaviour
{
    #region Public Variables
    [HideInInspector]
    public CameraPlayer currentPlCameraSettings;
    public Transform tempRotLookAt;
    #endregion

    #region Private Variables
    private Transform playerTransform;
    private Transform camTransform;

    private float currentx = 0.0f;
    private float currenty = 0.0f;

    private bool CameraPlayerControl = true;
    #endregion

    #region Taking Game Controller Reference and Link the Initializer Event
    void Awake()
    {
        GameController gcLink = this.GetComponent<GameController>();

        gcLink.gpInitializer.AddListener(SettingPlayerCamera);

    }
    #endregion

    #region Initializing Camera in Gameplay Scenes
    public void SettingPlayerCamera(GameObject player)
    {
        camTransform = Camera.main.transform;
        this.playerTransform = player.transform;
        Debug.Log("Camera Initialization Done");


        FSMChecker fsmLink = player.GetComponent<FSMChecker>();

        fsmLink.switchingCameraControlToOFF.AddListener(this.SwitchingCameraControlToOff);
        fsmLink.switchingCameraControlToOn.AddListener(this.SwitchingCameraControlToON);

    }
    #endregion

    #region Camera Player Mouse Zoom
    void Update()
    {
        if (!this.CameraPlayerControl) return;

        #region Pc Zoom and Moving

        if (Input.GetMouseButton(0))
        {
            Cursor.visible = false;
            this.currentx += Input.GetAxis("Mouse X") * this.currentPlCameraSettings.sensitivityX;
            this.currenty -= Input.GetAxis("Mouse Y") * this.currentPlCameraSettings.sensitivityY;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Cursor.visible = true;
        }

        this.currentPlCameraSettings.currentDistance -= Input.GetAxis("Mouse ScrollWheel")
                                                   * this.currentPlCameraSettings.sensitivityZoom;


        this.currentPlCameraSettings.currentDistance = Mathf.Clamp(
            this.currentPlCameraSettings.currentDistance,
            this.currentPlCameraSettings.distanceMin,
            this.currentPlCameraSettings.distanceMax);
        this.currenty = Mathf.Clamp(this.currenty, this.currentPlCameraSettings.yAngleMin, this.currentPlCameraSettings.yAngleMax);

        #endregion

        #region Joy Zoom and Moving

        this.currentx += Input.GetAxis("RJHor") * this.currentPlCameraSettings.sensitivityX;
        this.currenty += Input.GetAxis("RJVer") * this.currentPlCameraSettings.sensitivityY;

        if (Input.GetButton("CamZjoy"))
            this.currentPlCameraSettings.currentDistance += Input.GetAxis("RJVer")
                                                       * this.currentPlCameraSettings.sensitivityZoom;

        this.currentPlCameraSettings.currentDistance = Mathf.Clamp(
            this.currentPlCameraSettings.currentDistance,
            this.currentPlCameraSettings.distanceMin,
            this.currentPlCameraSettings.distanceMax);
        this.currenty = Mathf.Clamp(this.currenty, this.currentPlCameraSettings.yAngleMin, this.currentPlCameraSettings.yAngleMax);

        #endregion
    }
    #endregion

    #region Camera Follow 
    void LateUpdate()
    {

        if (!this.CameraPlayerControl) return;

        if (this.playerTransform != null)
        {
            Vector3 dir = new Vector3(0, 0, -currentPlCameraSettings.currentDistance);
            Quaternion rotation = Quaternion.Euler(currenty, currentx, 0);
            //camTransform.position = playerTransform.position + rotation * dir;

            this.tempRotLookAt.position = playerTransform.position + rotation * dir;
            this.tempRotLookAt.LookAt(this.playerTransform.position);


            this.camTransform.position = Vector3.Slerp(this.camTransform.position, this.playerTransform.position + rotation * dir, 2f * Time.deltaTime);

            

            this.camTransform.rotation = Quaternion.Slerp(
                this.camTransform.rotation,
                this.tempRotLookAt.rotation,
                4f * Time.deltaTime);


            //this.camTransform.LookAt(this.playerTransform.position);


        }

    }
    #endregion

    #region General Methods

    private void SwitchingCameraControlToOff(GameObject CameraDir)
    {
        this.CameraPlayerControl = false;

        StartCoroutine(LerpOnDesignerCamera(CameraDir));
    }

    IEnumerator LerpOnDesignerCamera(GameObject CameraDir)
    {
        Quaternion targetRotation = CameraDir.transform.rotation;
        Vector3 targetPosition = CameraDir.transform.position;

        bool posReached = false;

        while (!this.CameraPlayerControl && !posReached)
        {
            this.camTransform.rotation = Quaternion.Slerp(this.camTransform.rotation, targetRotation, 4 * Time.deltaTime);
            this.camTransform.position = Vector3.Slerp(this.camTransform.position, targetPosition, 2 * Time.deltaTime);

            if (Quaternion.Angle(this.camTransform.rotation, targetRotation) < 0.1f
                && ((this.camTransform.position - targetPosition).sqrMagnitude < 0.1f)) posReached = true;

            yield return null;
        }

    }

    private void SwitchingCameraControlToON()
    {
        this.CameraPlayerControl = true;

        this.StopCoroutine("LerpOnDesignerCamera");
    }

}
    #endregion

