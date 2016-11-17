using UnityEngine;
using System.Collections;


public class Puzzles : MonoBehaviour {

    // bool per tutti i puzzle 
    public bool isPuzzle1;
    public bool isPuzzle2;
    public bool isPuzzle3;
    public bool isPuzzle4;
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

    private float distance = 1f; // di quanto si muove la porta 
    private float lerpTime = 5;
    private float currentLerpTime = 0;
     
    
    void Start()
    {
        startPosLeftDoor = leftFissure.transform.position;
        endPosLeftDoor = leftFissure.transform.position + Vector3.forward * distance;

        startPosRightDoor = rightFissure.transform.position;
        endPosRightDoor = rightFissure.transform.position + Vector3.back * distance;
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
        if (other.gameObject.tag == "Player" && isPuzzle3)
        {
            StartCoroutine(DoorOpeningCO());
            // leftFissure.transform.Translate(transform.right * 10 * Time.deltaTime);

        }
    }

     


   
}
