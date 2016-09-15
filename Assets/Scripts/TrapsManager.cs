using UnityEngine;
using System.Collections;

public class TrapsManager : MonoBehaviour
{


    public float sphereSpeed;
    float sphereRotation = 5f;
    Transform targetTr;
    public Transform A;
    public Transform B;
    Transform rotationSphere;
    bool isDirectionRight = true;

    void Awake()
    {
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

}
