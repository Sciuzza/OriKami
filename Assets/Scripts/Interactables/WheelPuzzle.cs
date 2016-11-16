using UnityEngine;
using System.Collections;

public class WheelPuzzle : MonoBehaviour
{

    public GameObject wheelA;
    public bool isRotating = false, isOnButton = false;
    public Quaternion quaObjective, quaCurrent, quaOriginal;



    // Use this for initialization
    void Start()
    {
        quaOriginal = wheelA.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

        

    }

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == ("Player") && isRotating == false && isOnButton == false)
        {
            isRotating = true;
            isOnButton = true;
            Debug.Log("Prova puzzle");
            quaCurrent = wheelA.transform.rotation;
            quaObjective = Quaternion.AngleAxis(120 +Quaternion.Angle(quaCurrent,quaOriginal), wheelA.transform.up);
            StartCoroutine(WheelRotation());

        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isOnButton = false;
        }
    }

    IEnumerator WheelRotation()
    {
        while (Quaternion.Angle(wheelA.transform.rotation,quaObjective) > 2f)
        {
            wheelA.transform.rotation = Quaternion.Slerp(wheelA.transform.rotation, quaObjective, Time.deltaTime);
           // wheelA.transform.Rotate(Vector3.up, 10 * Time.deltaTime, Space.World);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        wheelA.transform.rotation = quaObjective;
        isRotating = false;
    }


}
