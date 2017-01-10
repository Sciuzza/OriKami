using UnityEngine;
using System.Collections;

public class CheckPointTest : MonoBehaviour {

    public SaveSystem save;
    public bool saveGame, loadGame; 


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
    }


}
