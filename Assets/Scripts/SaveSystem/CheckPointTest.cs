using UnityEngine;
using System.Collections;

public class CheckPointTest : MonoBehaviour
{
    SaveSystem save;
    public GameObject Player;
    public bool saveGame, loadGame;

    public bool Triggered;

    void Awake()
    {
       save = GameObject.FindGameObjectWithTag("GameController").GetComponent<SaveSystem>();

    }

    void OnTriggerEnter(Collider other)
    {
        /*
        if (other.gameObject.tag == "Player" && saveGame)
        {
            Debug.Log("Saving....");
            save.SaveState();
        }

        if (other.gameObject.tag == "Player" && loadGame)
        {
            Debug.Log("Loading....");
            save.LoadState();
        }
        */

        if (other.gameObject.CompareTag("Player") && !this.Triggered)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<SuperDataManager>().RequestingSave();
            this.Triggered = true;
        }
    }


}
