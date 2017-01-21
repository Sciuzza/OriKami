using UnityEngine;
using System.Collections;

public class CheckPointTest : MonoBehaviour
{
    SaveSystem save;
    public GameObject Player;
    public bool saveGame, loadGame;

    void Awake()
    {
       save = GameObject.FindGameObjectWithTag("GameController").GetComponent<SaveSystem>();

    }

    void OnTriggerEnter(Collider other)
    {
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
    }


}
