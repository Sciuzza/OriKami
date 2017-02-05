using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Collectibles : MonoBehaviour
{


    public int GoldenCollectible = 0;
    public int Collectible2 = 0;
    public int Collectible3 = 0;
    public int Collectible4 = 0;

    public Text countText;


    void OnTriggerEnter(Collider other)

    {
        if (other.gameObject.tag == "Collectibles")
        {
            other.gameObject.SetActive(false);
            this.GoldenCollectible += 1;
            countText.text = this.GoldenCollectible.ToString();

        }
    }


}





