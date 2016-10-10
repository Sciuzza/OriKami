using UnityEngine;
using System.Collections;

public class ButtonRamp : MonoBehaviour
{

    public GameObject rampLinker;


    void Start()
    {
        rampLinker.SetActive(false);

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlCore>().activateRamp.AddListener(ActivatingRamp);

    }

    private void ActivatingRamp()
    {
        rampLinker.SetActive(true);

    }
}
    
