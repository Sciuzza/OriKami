using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Collectibles : MonoBehaviour
{


    int goldenCollectible = 0;
    public Text countText;


    void OnTriggerEnter(Collider other)

    {
        if (other.gameObject.tag == "Collectibles")
        {
            other.gameObject.SetActive(false);
            goldenCollectible += 1;
            countText.text = goldenCollectible.ToString();

        }
    }


}





