using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Route2Access : MonoBehaviour {
    private void LoadingScenebyName()
    {
        SceneManager.LoadScene("Armadillos' Village");
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            LoadingScenebyName();
        }
    }

}
