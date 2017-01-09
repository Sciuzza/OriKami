using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {

    public Transform spawnPoint;


    // public SaveSystem save;

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        Debug.Log("Save Trigger");
    //        save.SaveState();
    //    }
    //}

  

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")

        {
            Debug.Log("Dentro");
            spawnPoint.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        }



    }



}
