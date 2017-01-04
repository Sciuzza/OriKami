using UnityEngine;
using System.Collections;

namespace Orikami
{

    public class DisappearPlatforms : MonoBehaviour
    {
        public bool DisablePlatforms;
        public bool TimedPlatforms;
        public BoxCollider platformCollider;
        public MeshRenderer PlatformMesh;
        public float smoothing = 1f;
        public float disappearingTime = 0f;

        IEnumerator MyCoroutine()
        {
            
            print("On platform");
            yield return new WaitForSeconds(disappearingTime);
            PlatformMesh.enabled = false;
            platformCollider.isTrigger = true;
            
        }

        IEnumerator MyCoroutineExit()
        {
            yield return new WaitForSeconds(1.5f);
            PlatformMesh.enabled = true;
            platformCollider.isTrigger = false;

        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player" && TimedPlatforms)
            {
                StartCoroutine(MyCoroutine());
            }

            if (other.gameObject.tag == "Player" && DisablePlatforms)
            {
                StartCoroutine(MyCoroutine());
            }

        }
        void OnTriggerExit(Collider other)
        {

            if (other.gameObject.tag == "Player" && TimedPlatforms)
            {
                StartCoroutine(MyCoroutineExit());
            }

        }    

    }
}