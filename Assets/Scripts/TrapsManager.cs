using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TrapsManager : MonoBehaviour
{


    public float sphereSpeed;
    float sphereRotation = 5f;
    Transform targetTr;
    public Transform A;
    public Transform B;
    Transform rotationSphere;
    bool isDirectionRight = true;
    Rigidbody rbody;


    #region SingleTone
    [HideInInspector]
    public static TrapsManager instance = null;

    void Start()
    {
        rbody = GetComponent<Rigidbody>();
    }

    void Awake()
    {

       /* if (instance == null)
            instance = this;
        else if (instance != this)   // da controllare non fa funzionare il prefab delle trappole in movimento
            Destroy(gameObject);
        */      

        #endregion
        
        targetTr = A;
    }

    void Update()
    {

        Vector3 distance = targetTr.position - this.transform.position;
        Vector3 direction = (targetTr.position - this.transform.position).normalized;
        transform.position = transform.position + direction * sphereSpeed * Time.deltaTime;

        Vector3 newRotation = transform.localEulerAngles;

        if (isDirectionRight == true)
            newRotation.z -= sphereRotation * Time.deltaTime * sphereSpeed;

        else if (isDirectionRight == false)
            newRotation.z += sphereRotation * Time.deltaTime * sphereSpeed;

        transform.localEulerAngles = newRotation;

        if (distance.magnitude < 5f)
        {

            if (targetTr == A)
            {
                targetTr = B;
                isDirectionRight = false;
            }

            else
            {
                targetTr = A;
                isDirectionRight = true;

            }
        }
        

    }



   /* void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.tag == "Player")
        {

            SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
        }
    }*/


}
