using UnityEngine;
using System.Collections;


public class Puzzles : MonoBehaviour
{

    // bool per tutti i puzzle   
    public bool moveObject;
    public bool generateObject;
    public bool disableObject;
    public bool doorPuzzle;
    public bool isPuzzle4;

    public bool openDoor;
    public bool closeDoor;

    public bool isUp;
    public bool isDown;
    public bool moveUp;
    public bool moveDown;
    public bool moveRight;
    public bool moveLeft;
    public bool rotate;
    public bool keyHit = false;

    public GameObject generatedObject;
    public GameObject disabledObject;
    public GameObject rotateObject;

    public GameObject leftDoor;
    public GameObject rightDoor;

    public GameObject goUp;
    public GameObject goDown;
    public GameObject goRight;
    public GameObject goLeft;

    private Vector3 startPosLeftDoor;
    private Vector3 endPosLeftDoor;

    private Vector3 startPosRightDoor;
    private Vector3 endPosRightDoor;

    private Vector3 startPosUpObject;
    private Vector3 endPosUpObject;

    private Vector3 startDownObject;
    private Vector3 endDownObject;

    private Vector3 startLeftObject;
    private Vector3 endLeftObject;

    private Vector3 startRightObject;
    private Vector3 endRightObject;


    public float distance; // di quanto si muove la porta  x designer
    private float lerpTime = 5;
    private float currentLerpTime = 0;
    public float degrees;

    void Start()
    {
        if (openDoor)
        {
            startPosLeftDoor = leftDoor.transform.position;
            endPosLeftDoor = leftDoor.transform.position + leftDoor.transform.forward * distance;

            startPosRightDoor = rightDoor.transform.position;
            endPosRightDoor = rightDoor.transform.position - rightDoor.transform.forward * distance;
        }

        else if (closeDoor)
        {
            startPosLeftDoor = leftDoor.transform.position;
            endPosLeftDoor = leftDoor.transform.position + (leftDoor.transform.forward) * (-1) * distance;

            startPosRightDoor = rightDoor.transform.position;
            endPosRightDoor = rightDoor.transform.position + (rightDoor.transform.forward) * distance;
        }

        //else if (isPuzzle4)
        //{
        //    if (isUp)
        //    {
        //        startPosUpObject = goUp.transform.position;
        //        endPosUpObject = goUp.transform.position + Vector3.down * distance;

        //        startDownObject = goDown.transform.position;
        //        endDownObject = goDown.transform.position + Vector3.down * distance;

        //        startDownObject2 = goDown2.transform.position;
        //        endDownObject2 = goDown2.transform.position + Vector3.down * distance;
        //    }

        //    else if (isDown)
        //    {
        //        startPosUpObject = goUp.transform.position;
        //        endPosUpObject = goUp.transform.position + Vector3.up * distance;

        //        startDownObject = goDown.transform.position;
        //        endDownObject = goDown.transform.position + Vector3.up * distance;

        //        startDownObject2 = goDown2.transform.position;
        //        endDownObject2 = goDown2.transform.position + Vector3.up * distance;
        //    }

        //}

        else if (moveObject && moveUp)
        {
            startPosUpObject = goUp.transform.position;
            endPosUpObject = goUp.transform.position + goUp.transform.up * distance;
        }
        else if (moveObject && moveDown)
        {
            startDownObject = goDown.transform.position;
            endDownObject = goDown.transform.position + goDown.transform.up * (-1) * distance;
        }

        else if (moveObject && moveLeft)
        {
            startLeftObject = goLeft.transform.position;
            endLeftObject = goLeft.transform.position + goLeft.transform.right * (-1) * distance;
        }
        else if (moveObject && moveRight)
        {
            startRightObject = goRight.transform.position;
            endRightObject = goRight.transform.position + goRight.transform.right * distance;

        }

    }

    IEnumerator DoorOpeningCO()
    {
        keyHit = true;

        while ((endPosLeftDoor - this.transform.position).magnitude > 0.5f && (endPosRightDoor - this.transform.position).magnitude > 0.5f)
        {
            if (Input.GetKey(KeyCode.E))
            {
                yield break;
            }
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime >= lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            float leftPerc = currentLerpTime / lerpTime;
            float rightPerc = currentLerpTime / lerpTime;
            leftDoor.transform.position = Vector3.Lerp(startPosLeftDoor, endPosLeftDoor, leftPerc);
            rightDoor.transform.position = Vector3.Lerp(startPosRightDoor, endPosRightDoor, rightPerc);
            yield return null;
        }

    }

    IEnumerator DoorClosingCO()
    {
        keyHit = true;
        while ((endPosLeftDoor - this.transform.position).magnitude > 0.5f && (endPosRightDoor - this.transform.position).magnitude > 0.5f)
        {

            currentLerpTime += Time.deltaTime;
            if (currentLerpTime >= lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            float leftPerc = currentLerpTime / lerpTime;
            float rightPerc = currentLerpTime / lerpTime;
            leftDoor.transform.position = Vector3.Lerp(startPosLeftDoor, endPosLeftDoor, leftPerc);
            rightDoor.transform.position = Vector3.Lerp(startPosRightDoor, endPosRightDoor, rightPerc);
            yield return null;
        }

    }

    IEnumerator ObjectMovingCO()
    {
        keyHit = true;
       
            while ((endPosUpObject - this.transform.position).magnitude > 0.5f ||
                   (endDownObject - this.transform.position).magnitude > 0.5f ||
                   (endLeftObject - this.transform.position).magnitude > 0.5f ||
                   (endRightObject - this.transform.position).magnitude > 0.5f)
            {
                currentLerpTime += Time.deltaTime;
                if (currentLerpTime >= lerpTime)
                {
                    currentLerpTime = lerpTime;
                }

                float leftPerc = currentLerpTime / lerpTime;
                float rightPerc = currentLerpTime / lerpTime;

                if (moveUp)
                {
                    goUp.transform.position = Vector3.Lerp(startPosUpObject, endPosUpObject, leftPerc);
                }
                else if (moveDown)
                {
                    goDown.transform.position = Vector3.Lerp(startDownObject, endDownObject, rightPerc);
                }

                else if (moveRight)
                {

                    goRight.transform.position = Vector3.Lerp(startRightObject, endRightObject, leftPerc);
                }
                else if (moveLeft)
                {
                    goLeft.transform.position = Vector3.Lerp(startLeftObject, endLeftObject, rightPerc);
                }

                yield return null;
            }
                
       
    }
    IEnumerator RotateMe(Vector3 byAngles, float inTime)
    {
        var fromAngle = rotateObject.transform.rotation;
        var toAngle = Quaternion.Euler(rotateObject.transform.eulerAngles + byAngles);
        for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
        {
            rotateObject.transform.rotation = Quaternion.Lerp(fromAngle, toAngle, t);
            yield return null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && generateObject)
        {
            Instantiate(generatedObject);
            generateObject = false;
        }

        if (other.gameObject.tag == "Player" && disableObject)
        {
            disabledObject.gameObject.SetActive(false);
            disableObject = false;
        }

        if (other.gameObject.tag == "Player" && doorPuzzle && openDoor)
        {
            Debug.Log("THE DOOR IS OPENING LOLLO COGLIONE");
            StartCoroutine(DoorOpeningCO());


        }
        if (other.gameObject.tag == "Player" && doorPuzzle && closeDoor)
        {
            Debug.Log("THE DOOR IS CLOSING LOLLO COGLIONE");
            StartCoroutine(DoorClosingCO());
        }

        if (other.gameObject.tag == "Player" && moveObject)
        {
            StartCoroutine(ObjectMovingCO());
        }

        if (other.gameObject.tag == "Player" && rotate)
        {
            Debug.Log("Is rotating");
            StartCoroutine(RotateMe(Vector3.up * degrees, 5));
        }

    }


}
