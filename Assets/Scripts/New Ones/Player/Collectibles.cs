using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Collectibles : MonoBehaviour
{


    public int GoldenCollectible = 0;
    public int DWall = 0;
    public int Collectible3 = 0;
    public int Collectible4 = 0;

    public Text countText;


    private void Awake()
    {
        FSMChecker fsmTempLink = this.gameObject.GetComponent<FSMChecker>();

        fsmTempLink.IncrementCollectibleRequest.AddListener(this.CollectibleHandler);
    }

    void OnTriggerEnter(Collider other)

    {
        if (other.gameObject.tag == "Collectibles")
        {
            other.gameObject.SetActive(false);
            this.GoldenCollectible += 1;
            countText.text = this.GoldenCollectible.ToString();

        }
    }

    private void CollectibleHandler(int index)
    {
        switch (index)
        {
            case 0:
                this.GoldenCollectible++;
                break;
            case 1:
                this.DWall++;
                break;
            case 2:
                this.Collectible3++;
                break;
            case 3:
                this.Collectible4++;
                break;
        }
    }
}





