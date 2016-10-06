using UnityEngine;
using System.Collections;

public class DescendingPlatform : MonoBehaviour {

    float weight = 350f;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.rigidbody)
        {
            Debug.Log("Speriamo funzioni");
            hit.rigidbody.AddForce(-hit.normal * weight);
        }

    }
} 
