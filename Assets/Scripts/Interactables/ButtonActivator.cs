using UnityEngine;
using System.Collections;




    public class ButtonActivator : MonoBehaviour
    {

        public GameObject platformBridge;
        public GameObject platformBridge2;  //LOLLO
        public GameObject platformBridge3;  //LOLLO
        public GameObject platformBridge4;  //LOLLO
        public GameObject platformBridge5;  //LOLLO
        public GameObject platformBridge6;  //LOLLO
        public GameObject platformBridge7;  //LOLLO
        public GameObject platformBridge8;  //LOLLO

    void Start()
        {
            platformBridge.SetActive(false);
            platformBridge2.SetActive(false);   //LOLLO
            platformBridge3.SetActive(false);   //LOLLO
            platformBridge4.SetActive(false);   //LOLLO
            platformBridge5.SetActive(false);   //LOLLO
            platformBridge6.SetActive(false);   //LOLLO
            platformBridge7.SetActive(false);   //LOLLO
            platformBridge8.SetActive(false);   //LOLLO
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlCore>().activateSomething.AddListener(ActivatingPlatform);

        }

        private void ActivatingPlatform()
        {
            Debug.Log("Cristiano è un coglione");
            platformBridge.SetActive(true);
            platformBridge2.SetActive(true);    //LOLLO
            platformBridge3.SetActive(true);    //LOLLO
            platformBridge4.SetActive(true);    //LOLLO
            platformBridge5.SetActive(true);    //LOLLO
            platformBridge6.SetActive(true);    //LOLLO
            platformBridge7.SetActive(true);    //LOLLO
            platformBridge8.SetActive(true);    //LOLLO
        }


    }
