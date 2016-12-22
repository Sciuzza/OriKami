using UnityEngine;
using System.Collections;

namespace Orikami
{
    
    public class DescendingPlatform : MonoBehaviour
    {
        public Vector3 startPos;
        public Transform TargetA, TargetB;
        public bool resetPosition = false, moveToPosition = false;
        public float moveTimer = 0f;

        void Update()
        {
            if (resetPosition)
            {
                moveTimer += Time.deltaTime;
                transform.position = Vector3.Lerp(startPos, TargetB.position, moveTimer/2);
            }
            else if (moveToPosition)
            {
                moveTimer += Time.deltaTime;
                transform.position = Vector3.Lerp(startPos, TargetA.position, moveTimer/2);
            }
        }

        void OnTriggerEnter(Collider objectHit)
        {
            if (objectHit.tag == "Player")
            {
                startPos = transform.position;
                moveTimer = 0;
                moveToPosition = true;
                resetPosition = false;
            }
        }

        void OnTriggerExit(Collider objectHit)
        {
            if (objectHit.tag == "Player")
            {
                startPos = transform.position;
                moveTimer = 0;
                resetPosition = true;
                moveToPosition = false;
            }
        }
    }
}
