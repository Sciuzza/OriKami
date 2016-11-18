using UnityEngine;
using System.Collections;


public class Puzzles : MonoBehaviour
{

    // bool per tutti i puzzle 
    public bool isPuzzle1;
    public bool isPuzzle2;
    public bool isPuzzle3;
    public bool isPuzzle4;
    public bool doorOpen;
    public bool doorClosed;
    private bool keyHit = false;

    public GameObject rotatingObject;
    public GameObject generatedObject;
    public GameObject leftFissure;
    public GameObject rightFissure;
    public GameObject platformGameobject;

    private Vector3 startPosLeftDoor;
    private Vector3 endPosLeftDoor;

    private Vector3 startPosRightDoor;
    private Vector3 endPosRightDoor;

    public float distance; // di quanto si muove la porta  x designer
    private float lerpTime = 5;
    private float currentLerpTime = 0;


    void Start()
    {
        if (doorOpen)
        {
            startPosLeftDoor = leftFissure.transform.position;
            endPosLeftDoor = leftFissure.transform.position + Vector3.forward * distance;

            startPosRightDoor = rightFissure.transform.position;
            endPosRightDoor = rightFissure.transform.position + Vector3.back * distance;
        }

        else if (doorClosed)
        {
            startPosLeftDoor = leftFissure.transform.position;
            endPosLeftDoor = leftFissure.transform.position + Vector3.back * distance;

            startPosRightDoor = rightFissure.transform.position;
            endPosRightDoor = rightFissure.transform.position + Vector3.forward * distance;
        }

    }

    IEnumerator DoorOpeningCO()
    {
        keyHit = true;
        if (keyHit == true)
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime >= lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            float leftPerc = currentLerpTime / lerpTime;
            float rightPerc = currentLerpTime / lerpTime;
            leftFissure.transform.position = Vector3.Lerp(startPosLeftDoor, endPosLeftDoor, leftPerc);
            rightFissure.transform.position = Vector3.Lerp(startPosRightDoor, endPosRightDoor, rightPerc);
        }

        yield return null;
    }

    IEnumerator DoorClosingCO()
    {
        keyHit = true;
        if (keyHit == true)
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime >= lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            float leftPerc = currentLerpTime / lerpTime;
            float rightPerc = currentLerpTime / lerpTime;
            leftFissure.transform.position = Vector3.Lerp(startPosLeftDoor, endPosLeftDoor, leftPerc);
            rightFissure.transform.position = Vector3.Lerp(startPosRightDoor, endPosRightDoor, rightPerc);
        }
        yield return null;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && isPuzzle2)
        {
            Instantiate(generatedObject);
            isPuzzle2 = false;
        }

    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && isPuzzle3 && doorOpen)
        {
            StartCoroutine(DoorOpeningCO());
            // leftFissure.transform.Translate(transform.right * 10 * Time.deltaTime);

        }

        if (other.gameObject.tag == "Player" && isPuzzle3 && doorClosed)
        {
            StartCoroutine(DoorClosingCO());
        }


    }

}
