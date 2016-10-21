using UnityEngine;
using System.Collections;

namespace Orikami
{

    public class DisappearPlatforms : MonoBehaviour
    {

        public BoxCollider platformCollider;
        public MeshRenderer PlatformMesh;
        public float smoothing = 1f;
        

        IEnumerator MyCoroutine()
        {


            print("On platform");
            yield return new WaitForSeconds(1f);
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
            if (other.gameObject.tag == "Player")
            {
                StartCoroutine(MyCoroutine());
            }

        }
        void OnTriggerExit(Collider other)
        {

            if (other.gameObject.tag == "Player")
            {
                StartCoroutine(MyCoroutineExit());
            }

        }

    }
}