using UnityEngine;
using System.Collections;

public class ButtonStairsWater : MonoBehaviour {

    public GameObject stairsWaterLinker;


    void Start()
    {
        stairsWaterLinker.SetActive(false);

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlCore>().activateWaterStairs.AddListener(ActivatingWaterStairs);

    }

    private void ActivatingWaterStairs()
    {
        stairsWaterLinker.SetActive(true);

    }
}
