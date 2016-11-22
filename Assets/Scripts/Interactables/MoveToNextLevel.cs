using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MoveToNextLevel : MonoBehaviour {

    public void NextLevel()
    {
        //For completed levels !
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            NextLevel();
        }
    }
}
