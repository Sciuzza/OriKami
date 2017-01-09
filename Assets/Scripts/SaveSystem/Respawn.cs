using UnityEngine;
using System.Collections;

public class Respawn : MonoBehaviour {

    public Transform SpawnPoint;
    public GameObject Player;


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")

        {
            Debug.Log("Dentro");
            Player.transform.position = SpawnPoint.position;

        }

    }


}
