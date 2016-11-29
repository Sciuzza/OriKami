using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Route2Access : MonoBehaviour {

    public bool toArmadillo;
    public bool toRoute2;

    private void LoadingScenebyName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && toArmadillo==true)
        {
            LoadingScenebyName("Armadillos' Village");

        }

        if (other.gameObject.tag == "Player" && toRoute2 == true)
        {
            LoadingScenebyName("Route 2");

        }
    }

}
