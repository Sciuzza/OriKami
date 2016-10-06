using UnityEngine;
using System.Collections;

namespace Orikami
{



    public class ButtonActivator : MonoBehaviour
    {

        public GameObject platformBridge;

        void Start()
        {
            platformBridge.SetActive(false);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlCore>().activateSomething.AddListener(ActivatingPlatform);

        }

        private void ActivatingPlatform()
        {
            Debug.Log("Cristiano è un coglione");
            platformBridge.SetActive(true);
        }


    }
}