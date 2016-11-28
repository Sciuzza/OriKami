using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class MoveToPreviousLevel : MonoBehaviour {


    public bool isRoute2;

    public void PreviousLevel()
    {
        //For completed levels !
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
          PreviousLevel();
        }

        if (other.gameObject.tag == "Player" &&isRoute2)
        {



        }
    }
}
