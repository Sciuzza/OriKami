using UnityEngine;
using System.Collections;
using UnityEngine.Events;




public class CameraManager : MonoBehaviour
{


    #region Player Camera Variables
    private Transform lookAt;
    private Transform camTransform;


    private float currentx = 0.0f;
    private float currenty = 0.0f;

    public CameraPlayer currentPlCameraSettings;



    #endregion

    public bool initPlayerDone = false;

    #region Linking References
    void Awake()
    {
        this.GetComponent<GameController>().initializer.AddListener(SettingPlayerCamera);

        //SettingDefaultCameraPlValues();
    } 
    #endregion



    void Update()
    {


        #region Camera Player Zoom
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



        currentx += Input.GetAxis("RJHor") * currentPlCameraSettings.sensitivityX;
        currenty += Input.GetAxis("RJVer") * currentPlCameraSettings.sensitivityY;

        if (Input.GetButton("CamZjoy"))
            currentPlCameraSettings.currentDistance += Input.GetAxis("RJVer") * currentPlCameraSettings.sensitivityZoom;

        currentPlCameraSettings.currentDistance = Mathf.Clamp(currentPlCameraSettings.currentDistance, currentPlCameraSettings.distanceMin, currentPlCameraSettings.distanceMax);
        currenty = Mathf.Clamp(currenty, currentPlCameraSettings.yAngleMin, currentPlCameraSettings.yAngleMax);

    }



    #region Old

    void LateUpdate()
    {

        #region Camera Player Follow
        InitializingCamera();
        #endregion

    }

    private void SettingDefaultCameraPlValues()
    {
        CameraPlayer defaultValues;

        defaultValues.currentDistance = 15;
        defaultValues.distanceMin = 5;
        defaultValues.distanceMax = 30;
        defaultValues.sensitivityX = 4;
        defaultValues.sensitivityY = 1;
        defaultValues.sensitivityZoom = 30;
        defaultValues.yAngleMin = 0.5f;
        defaultValues.yAngleMax = 50;

        currentPlCameraSettings = defaultValues;
    } 
    #endregion

    #region Initializing Camera in Gameplay Scenes
    public void InitializingCamera()
    {
        if (initPlayerDone)
        {
            Vector3 dir = new Vector3(0, 0, -currentPlCameraSettings.currentDistance);
            Quaternion rotation = Quaternion.Euler(currenty, currentx, 0);
            camTransform.position = lookAt.position + rotation * dir;

            camTransform.LookAt(lookAt.position);
        }
    }

    public void SettingPlayerCamera(GameObject player)
    {
        camTransform = Camera.main.transform;
        lookAt = player.transform;
        initPlayerDone = true;
    } 
    #endregion
}
