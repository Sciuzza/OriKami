using UnityEngine;
using System.Collections;

public class WheelActivator : MonoBehaviour
{

    public bool isRotating = false, isOnButton = false;
    public LookAtTest[] wheelLinker = new LookAtTest[2];
    public 

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && isRotating == false && isOnButton == false)
        {

            wheelLinker[0].IndexController();
            wheelLinker[1].IndexController();

        }
        
    }


    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isOnButton = false;
            wheelLinker[0].OnTriggerController();
            wheelLinker[1].OnTriggerController();

        }
    }

}
