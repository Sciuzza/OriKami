using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MoveToNextLevel : MonoBehaviour
{

    public event_string SceneChangeRequest;

    public string SceneName;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            this.SceneChangeRequest.Invoke(this.SceneName);
        }
    }
}
