using UnityEngine;
using System.Collections;

public class WinPuzzleScript : MonoBehaviour
{

    public bool trigger = false;

    PassaggioLivelloWheel passaggioLivello;

    void Start()
    {

        passaggioLivello = FindObjectOfType<PassaggioLivelloWheel>();

    }
        
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "WinPuzzle" && trigger == false)
        {
            trigger = true;

            passaggioLivello.contatore += 1;

            Debug.Log("Contatore incrementato di 1");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "WinPuzzle" && trigger == true)
        {
            trigger = false;

            passaggioLivello.contatore -= 1;


            Debug.Log("Contatore decrementato di 1");
        }
    }
}
