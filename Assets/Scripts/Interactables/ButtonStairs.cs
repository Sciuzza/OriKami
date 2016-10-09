using UnityEngine;
using System.Collections;

public class ButtonStairs : MonoBehaviour
{


    public GameObject stairsLinker;
    
    
    void Start()
    {
        stairsLinker.SetActive(false);

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlCore>().activateStairs.AddListener(ActivatingStairs);

    }

    private void ActivatingStairs()
    {
        stairsLinker.SetActive(true);

    }
}
