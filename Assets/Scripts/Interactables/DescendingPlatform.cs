using UnityEngine;
using System.Collections;

namespace Orikami
{


    public class DescendingPlatform : MonoBehaviour
    {

        float platformSpeed = 1.5f;
        Transform targetTr;
        public Transform A;
        public Transform B;
        bool isGoingDown = true;
        public Vector3 distance, direction;
        bool movimento = false;
        
        void Start()
        {
            targetTr = B;
        }

        public void PaddleGoing()
        {

            if (targetTr == B)
                targetTr = A;
            else
                targetTr = B;

            movimento = true;
        }

        void Update()
        {

            if (movimento == true)
            {

                distance = targetTr.position - this.transform.position;
                direction = (targetTr.position - this.transform.position).normalized;
                transform.position = transform.position + direction * platformSpeed * Time.deltaTime;
            }

            if (distance.magnitude < 0.1f)
            {
                movimento = false;
            }
            
            if (Input.GetKeyDown(KeyCode.O))
            {
                PaddleGoing();
                Debug.Log("ciao");
            }
        }

    }
}
