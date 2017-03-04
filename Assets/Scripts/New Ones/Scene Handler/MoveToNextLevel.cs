using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MoveToNextLevel : MonoBehaviour
{

    public event_string SceneChangeRequest;
    public event_vector3 RegisterPlayerPosRequest;


    public string SceneName;
    public Vector3 Position;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            Debug.Log("Sono dentro");
            this.RegisterPlayerPosRequest.Invoke(this.Position);
            this.SceneChangeRequest.Invoke(this.SceneName);
            

        }
    }
}
