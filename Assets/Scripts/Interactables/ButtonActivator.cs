using UnityEngine;
using System.Collections;


public class ButtonActivator : MonoBehaviour
{

    public GameObject platformBridge;
   


    void Start()
    {
        platformBridge.SetActive(false);
       
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlCore>().activateBridge.AddListener(ActivatingPlatform);
        
    }

    private void ActivatingPlatform()
    {
        platformBridge.SetActive(true);

    }

  


}
