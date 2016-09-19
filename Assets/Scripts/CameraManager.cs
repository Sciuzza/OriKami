using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {


    #region Player Camera Variables
    private Transform lookAt;
    private Transform camTransform;

    private float distance = 200.0f;
    private float currentx = 0.0f;
    private float currenty = 0.0f;
    private float sensitivityX = 4.0f;
    private float sensitivityY = 1.0f;
    private float sensitivityZoom = 30.0f;

    private const float Y_ANGLE_MIN = 0.5f;
    private const float Y_ANGLE_MAX = 50.0f;

    

    #endregion



    #region SingleTone
    [HideInInspector]
    public static CameraManager instance = null;

    void Awake()
    {


        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);



       

    }
    #endregion

	

    void Update()
    {

        if (Input.GetMouseButton(0))
        {      
            Cursor.visible = false;
            currentx += Input.GetAxis("Mouse X") * sensitivityX;
            currenty -= Input.GetAxis("Mouse Y") * sensitivityY;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Cursor.visible = true;  
        }


        distance -= Input.GetAxis("Mouse ScrollWheel") * sensitivityZoom;

        distance = Mathf.Clamp(distance, 50, 300);
        currenty = Mathf.Clamp(currenty, Y_ANGLE_MIN, Y_ANGLE_MAX);
    }

    void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currenty, currentx, 0);
        camTransform.position = lookAt.position + rotation * dir;

        camTransform.LookAt(lookAt.position);
    }


    public void SettingPlayerCamera()
    {
        camTransform = Camera.main.transform;
        lookAt = GameObject.FindGameObjectWithTag("Player").transform;

    }


}
