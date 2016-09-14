using UnityEngine;
using System.Collections;

public class TrapsManager : MonoBehaviour
{

    public float sphereSpeed;
    Transform targetTr;
    public Transform A;
    public Transform B;

    void Awake()
    {
        targetTr = A;
    }

    void Update()
    {

        Vector3 distance = targetTr.position - this.transform.position;
        Vector3 direction = (targetTr.position - this.transform.position).normalized;

        transform.position = transform.position + direction * sphereSpeed * Time.deltaTime;

        if (distance.magnitude < 5f)
        {
                    
            if (targetTr == A)
            {
                targetTr = B;
            }

            else 
            {
                targetTr = A;
            }
        }

    }
 
}
