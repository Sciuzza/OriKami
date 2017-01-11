using UnityEngine;
using System.Collections;

public class CheckPointTest : MonoBehaviour {

    public SaveSystem save;
    public GameObject Player;
    public Transform SpawnPoint;
    public bool saveGame, loadGame, spawn;


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && saveGame)
        {
            Debug.Log("SONO DENTRO");
            save.SaveState();
        }

        if (other.gameObject.tag == "Player" &&  loadGame)
        {
            Debug.Log("Sono Morto");
            save.LoadState();
        }

        if (other.gameObject.tag == "Player" && spawn)
        {
            Debug.Log("Sono Respawno");
            Player.transform.position = SpawnPoint.position;
        }
    }


}
