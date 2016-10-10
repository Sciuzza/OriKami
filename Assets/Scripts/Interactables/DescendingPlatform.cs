using UnityEngine;
using System.Collections;

namespace Orikami
{


    public class DescendingPlatform : MonoBehaviour
    {

        public Transform TargetA, TargetB;
        public bool resetPosition = false;

        void Update()
        {
            if (resetPosition)
            {
                transform.position = Vector3.Slerp(transform.position, TargetB.position, 0.01f);
            }
        }
        void Start()
        {

        }
        void OnTriggerStay(Collider objectHit)
        {
            resetPosition = false;
            if (objectHit.tag == "Player")
            {
                transform.position = Vector3.Slerp(transform.position, TargetA.position, 0.01f);
            }

        }

        void OnTriggerExit(Collider objectHit)
        {

            if (objectHit.tag == "Player")
            {
                resetPosition = true;
            }
        }

    }
}
