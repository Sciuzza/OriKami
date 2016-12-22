using UnityEngine;
using System.Collections;

public class FloatingPlatform : MonoBehaviour
{

    Rigidbody rb;
    public float bounciness = 0f;
    // Use this for initialization
    void Start()
    {
        rb.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    

    void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Tocco fisica");
            rb.AddForce(Vector2.up * bounciness, ForceMode.Impulse);
            
            
        }          
        
    }

}
