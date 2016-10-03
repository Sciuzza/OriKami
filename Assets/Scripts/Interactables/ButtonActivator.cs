using UnityEngine;
using System.Collections;

public class ButtonActivator : MonoBehaviour {

    void Awake()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlCore>().activateSomething.AddListener(ActivatingPlatform);
    }

   private void ActivatingPlatform()
    {
        Debug.Log("Cristiano è un coglione");
        GameObject.Find("Cube (344)").SetActive(true);
    }

   
}
