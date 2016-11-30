using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class CameraManager : MonoBehaviour
{
    #region Public Variables
    [HideInInspector]
    public CameraPlayer currentPlCameraSettings; 
    #endregion

    #region Private Variables
    private Transform lookAt;
    private Transform camTransform;

    private float currentx = 0.0f;
    private float currenty = 0.0f;
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
        lookAt = player.transform;
        Debug.Log("Camera Initialization Done");
    }
    #endregion

    #region Camera Player Mouse Zoom
    void Update()
    {
        #region Pc Zoom
        if (Input.GetMouseButton(0))
        {
            Cursor.visible = false;
            currentx += Input.GetAxis("Mouse X") * currentPlCameraSettings.sensitivityX;
            currenty -= Input.GetAxis("Mouse Y") * currentPlCameraSettings.sensitivityY;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Cursor.visible = true;
        }

        currentPlCameraSettings.currentDistance -= Input.GetAxis("Mouse ScrollWheel") * currentPlCameraSettings.sensitivityZoom;


        currentPlCameraSettings.currentDistance = Mathf.Clamp(currentPlCameraSettings.currentDistance, currentPlCameraSettings.distanceMin, currentPlCameraSettings.distanceMax);
        currenty = Mathf.Clamp(currenty, currentPlCameraSettings.yAngleMin, currentPlCameraSettings.yAngleMax);
        #endregion

        #region Joy Zoom

        currentx += Input.GetAxis("RJHor") * currentPlCameraSettings.sensitivityX;
        currenty += Input.GetAxis("RJVer") * currentPlCameraSettings.sensitivityY;

        if (Input.GetButton("CamZjoy"))
            currentPlCameraSettings.currentDistance += Input.GetAxis("RJVer") * currentPlCameraSettings.sensitivityZoom;

        currentPlCameraSettings.currentDistance = Mathf.Clamp(currentPlCameraSettings.currentDistance, currentPlCameraSettings.distanceMin, currentPlCameraSettings.distanceMax);
        currenty = Mathf.Clamp(currenty, currentPlCameraSettings.yAngleMin, currentPlCameraSettings.yAngleMax); 
        #endregion
    }
    #endregion

    #region Camera Follow 
    void LateUpdate()
    {

        if (lookAt != null)
        {
            Vector3 dir = new Vector3(0, 0, -currentPlCameraSettings.currentDistance);
            Quaternion rotation = Quaternion.Euler(currenty, currentx, 0);
            camTransform.position = lookAt.position + rotation * dir;

            camTransform.LookAt(lookAt.position);
        }

    } 
    #endregion
}
