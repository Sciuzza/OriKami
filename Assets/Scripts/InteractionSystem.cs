using UnityEngine;
using System.Collections;

public class InteractionSystem : PlayerCore
{
    public float pushStrength = 6.0f;
    private Rigidbody rbody;

    void Start()
    {
        rbody = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.tag == "Player")
        {

            rbody.AddForce(transform.forward * pushStrength);
        }
    }
}
