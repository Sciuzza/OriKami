using UnityEngine;
using System.Collections;

public class DestroyableObjects : MonoBehaviour {

    

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Armadillo Form")
        {
            Debug.Log("FANCULO");
            Destroy(this.gameObject);
        }
    }
}
