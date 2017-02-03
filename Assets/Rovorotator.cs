using UnityEngine;
using System.Collections;

public class Rovorotator : MonoBehaviour
{

    void Update()

    {

        transform.Rotate(new Vector3(0, 0, 90) * Time.deltaTime);

    }
    
}
