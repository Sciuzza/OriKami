using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MoveToNextLevel : MonoBehaviour
{

    public event_vector3_string RegisterPlayerPosRequest;


    public string SceneName;
    public Transform posTransf;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            Debug.Log("Sono dentro");
            this.RegisterPlayerPosRequest.Invoke(this.posTransf.position, this.SceneName);
        }
    }
}
