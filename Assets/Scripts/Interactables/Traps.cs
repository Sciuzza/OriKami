using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Traps : MonoBehaviour
{
    bool isDirectionRight = true;
    public bool movingPlatforms;

    public float sphereSpeed;
    float sphereRotation = 0f;

    public Transform A;
    public Transform B;
    Transform targetTr;
    Transform rotationSphere;

    void Start()
    {
        targetTr = A;
    }


    void Update()
    {
        //Transform trap system
        Vector3 distance = targetTr.position - this.transform.position;
        Vector3 direction = (targetTr.position - this.transform.position).normalized;
        transform.position = transform.position + direction * sphereSpeed * Time.deltaTime;

        Vector3 newRotation = transform.localEulerAngles;

        if (isDirectionRight == true)
            newRotation.z -= sphereRotation * Time.deltaTime * sphereSpeed;

        else if (isDirectionRight == false)
            newRotation.z += sphereRotation * Time.deltaTime * sphereSpeed;

        transform.localEulerAngles = newRotation;

        if (distance.magnitude < 0.1f)
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

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.SetParent(this.gameObject.transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.parent = null;
        }
    }



    /*void OnCollisionEnter (Collision coll)
    {
        if (coll.collider.tag == "Player")
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }*/

}
