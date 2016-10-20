using UnityEngine;
using System.Collections;

namespace Orikami
{

    public class DisappearPlatforms : MonoBehaviour
    {

        public BoxCollider platformCollider;
        public MeshRenderer PlatformMesh;
        public float smoothing = 1f;
        public GameObject provacazzo;

        IEnumerator MyCoroutine()
        {


            print("On platform");
            yield return new WaitForSeconds(2f);
            PlatformMesh.enabled = false;
            platformCollider.isTrigger = true;
            provacazzo.SetActive(false);


        }

        IEnumerator MyCoroutineExit()
        {
            yield return new WaitForSeconds(2f);
            PlatformMesh.enabled = true;
            platformCollider.isTrigger = false;

        }

        void OnTriggerEnter(Collider other)
        {
            StartCoroutine(MyCoroutine());
        }
        void OnTriggerExit(Collider other)
        {
            StartCoroutine(MyCoroutineExit());
        }

    }
}