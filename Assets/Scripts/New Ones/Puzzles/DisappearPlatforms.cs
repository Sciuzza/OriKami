﻿using UnityEngine;
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
        public float ReAppearingTime;
        private SoundManager soundRef;

        void Awake()
        {
            soundRef = GameObject.FindGameObjectWithTag("GameController").GetComponent<SoundManager>();
        }

     

        IEnumerator MyCoroutine()
        {

            print("On platform");
            yield return new WaitForSeconds(disappearingTime);
            PlatformMesh.enabled = false;
            platformCollider.isTrigger = true;

        }

        IEnumerator DestroyingTimed()
        {
            print("On platform");
            yield return new WaitForSeconds(disappearingTime);
            this.platformCollider.gameObject.SetActive(false);
        }

        IEnumerator MyCoroutineExit()
        {
            soundRef.PlaySound(1, 10);
            yield return new WaitForSeconds(this.ReAppearingTime);
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
                StartCoroutine(this.DestroyingTimed());
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