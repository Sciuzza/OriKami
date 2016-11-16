using UnityEngine;
using System.Collections;

public class PassaggioLivelloWheel : MonoBehaviour
{

    public int contatore = 0;

    void Update()
    {
        if (contatore == 3)
        {

            Debug.Log("Passaggio Livello Avvenuto");
        }
    }
}
