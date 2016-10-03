using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class CameraManager : MonoBehaviour {


    #region Player Camera Variables
    private Transform lookAt;
    private Transform camTransform;

    
    private float currentx = 0.0f;
    private float currenty = 0.0f;

    public CameraPlayer currentPlCameraSettings;

   

    #endregion

    private bool initPlayerDone = false;

    void Awake()
    {
        this.GetComponent<GameController>().initializer.AddListener(SettingPlayerCamera);

        SettingDefaultCameraPlValues();
    }



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


        currentPlCameraSettings.startingDistance -= Input.GetAxis("Mouse ScrollWheel") * currentPlCameraSettings.sensitivityZoom;

        currentPlCameraSettings.startingDistance = Mathf.Clamp(currentPlCameraSettings.startingDistance, currentPlCameraSettings.distanceMin, currentPlCameraSettings.distanceMax);
        currenty = Mathf.Clamp(currenty, currentPlCameraSettings.yAngleMin, currentPlCameraSettings.yAngleMax);  
        #endregion

    }

    void LateUpdate()
    {

        #region Camera Player Follow
        if (initPlayerDone)
        {
            Vector3 dir = new Vector3(0, 0, -currentPlCameraSettings.startingDistance);
            Quaternion rotation = Quaternion.Euler(currenty, currentx, 0);
            camTransform.position = lookAt.position + rotation * dir;

            camTransform.LookAt(lookAt.position);
        } 
        #endregion
    }




    public void SettingPlayerCamera(GameObject player)
    {


        camTransform = Camera.main.transform;
        lookAt = player.transform;
        initPlayerDone = true;
    }


    private void SettingDefaultCameraPlValues()
    {
        CameraPlayer defaultValues;

        defaultValues.startingDistance = 15;
        defaultValues.distanceMin = 5;
        defaultValues.distanceMax = 30;
        defaultValues.sensitivityX = 4;
        defaultValues.sensitivityY = 1;
        defaultValues.sensitivityZoom = 30;
        defaultValues.yAngleMin = 0.5f;
        defaultValues.yAngleMax = 50;

        currentPlCameraSettings = defaultValues;
    }
}
