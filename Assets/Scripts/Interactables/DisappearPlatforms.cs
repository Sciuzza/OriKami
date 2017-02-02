using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Orikami
{

    public class DisappearPlatforms : MonoBehaviour
    {

        public BoxCollider platformCollider;
        public MeshRenderer PlatformMesh;
        public MeshRenderer[] dandelionDeactivation;
        public float smoothing = 1f;
        public float disappearingTime = 0f;
        public bool destroyPlatforms;
        public bool timedPlatforms;
        public bool groupPlatforms;

     

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

        IEnumerator MyGroupDeactivation()
        {
            yield return new WaitForSeconds(disappearingTime);
            foreach (var item in dandelionDeactivation)
            {
                item.enabled = false;
            }

            platformCollider.isTrigger = true;
        }

        IEnumerator MyGroupDeactivationExit()
        {
            yield return new WaitForSeconds(disappearingTime);
            foreach (var item in dandelionDeactivation)
            {
                item.enabled = true;
            }

            platformCollider.isTrigger = false;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player" && timedPlatforms)
            {
                StartCoroutine(MyCoroutine());
            }

            if (other.gameObject.tag == "Player" && destroyPlatforms)
            {
                StartCoroutine(MyCoroutine());
            }
            if (other.gameObject.tag == "Player" && groupPlatforms)
            {
                StartCoroutine(MyGroupDeactivation());
            }
        }
        void OnTriggerExit(Collider other)
        {

            if (other.gameObject.tag == "Player" && timedPlatforms)
            {
                StartCoroutine(MyCoroutineExit());
            }

            if (other.gameObject.tag =="Player" &&groupPlatforms)
            {
                StartCoroutine(MyGroupDeactivationExit());

            }

        }

    }
}