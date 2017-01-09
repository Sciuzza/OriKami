using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {

    SaveSystem save;

    void Awake()
    {
        save = new SaveSystem();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Save Trigger");
            save.SaveState();
        }
    }






}
