using UnityEngine;
using System.Collections;


public class Puzzles : MonoBehaviour
{
    public bool isPlatform;

    //Gestione Movimento oggetti  
    public bool moveObject;
    public bool moveUp;
    public bool moveDown;
    public bool moveRight;
    public bool moveLeft;

    //Generare e disabilitare oggetti 
    public bool generateObject;
    public bool disableObject;

    // x Attivare gestione Porte 
    public bool doorPuzzle;

    //Puzzle pilastri da fare 
    public bool isPuzzle4;


    public bool isUp;
    public bool isDown;

    //Gestire apertura e chiusura porte 
    public bool openDoor;
    public bool closeDoor;

    // x rotazione continua  
    public bool keepRotating;
    public bool targetKeepRotating;

    // x raotione in gradi
    public bool rotate;
    public bool keyHit = false;

    // di quanti gradi si muove l'oggetto 
    public float degrees;

    // di quanto si muove l'oggetto  x designer
    public float distance;
    public float movingSpeed = 0f;

    // x regolare la velocità di rotazione 
    public float rotationSpeed;

    public GameObject generatedObject;
    public GameObject disabledObject;
    public GameObject rotateObject;

    public GameObject leftDoor;
    public GameObject rightDoor;

    public GameObject goUp;
    public GameObject goDown;
    public GameObject goRight;
    public GameObject goLeft;
    public GameObject keep_Rotating;

    private Vector3 startPosLeftDoor;
    private Vector3 endPosLeftDoor;

    private Vector3 startPosRightDoor;
    private Vector3 endPosRightDoor;

    private Vector3 startPosUpObject;
    private Vector3 endPosUpObject = new Vector3(10000, 10000, 10000);

    private Vector3 startDownObject;
    private Vector3 endDownObject = new Vector3(10000, 10000, 10000);

    private Vector3 startLeftObject;
    private Vector3 endLeftObject = new Vector3(10000, 10000, 10000);

    private Vector3 startRightObject;
    private Vector3 endRightObject = new Vector3(10000, 10000, 10000);

    private float lerpTime = 5;
    private float currentLerpTime = 0;

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

    void Update()
    {
        if (keepRotating)
        {
            this.gameObject.transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime));
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
        this.currentLerpTime = 0;

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
        this.currentLerpTime = 0;

        while (this.currentLerpTime < this.lerpTime)
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
                goUp.transform.position = Vector3.Lerp(startPosUpObject, endPosUpObject, leftPerc * movingSpeed);
            }
            else if (moveDown)
            {
                goDown.transform.position = Vector3.Lerp(startDownObject, endDownObject, rightPerc * movingSpeed);
            }

            else if (moveRight)
            {

                goRight.transform.position = Vector3.Lerp(startRightObject, endRightObject, leftPerc * movingSpeed);
            }
            else if (moveLeft)
            {
                goLeft.transform.position = Vector3.Lerp(startLeftObject, endLeftObject, rightPerc * movingSpeed);
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
        if (other.gameObject.tag == "Player" && generateObject && !this.keyHit)
        {
            if (generatedObject != null)
            {
                Instantiate(generatedObject);
                generateObject = false;
            }

        }
        if (other.gameObject.tag == "ActivatorTrigger" && generateObject && !this.keyHit) //Attenzione l'oggetto DEVE Avere RigidBody !!!!!
        {
            Debug.Log("TOCCATO");
            Instantiate(generatedObject);
            generateObject = false;

        }

        if (other.gameObject.tag == "Player" && disableObject && !this.keyHit)
        {
            if (disabledObject != null)
            {
                disabledObject.gameObject.SetActive(false);
                disableObject = false;
            }

        }
        if (other.gameObject.tag == "ActivatorTrigger" && disableObject && !this.keyHit)
        {
            if (disabledObject != null)
            {
                disabledObject.gameObject.SetActive(false);
                disableObject = false;
            }
        }

        if (other.gameObject.tag == "Player" && doorPuzzle && openDoor && !this.keyHit)
        {

            StartCoroutine(DoorOpeningCO());
        }
        if (other.gameObject.tag == "Player" && doorPuzzle && closeDoor && !this.keyHit)
        {
            StartCoroutine(DoorClosingCO());
        }

        if (other.gameObject.tag == "Player" && moveObject && !this.keyHit)
        {
            StartCoroutine(ObjectMovingCO());
        }

        if (other.gameObject.tag == "Player" && rotate && !keyHit)
        {
            StartCoroutine(RotateMe(Vector3.up * degrees, 5));
        }

    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && keepRotating)
        {
            //in caso di morte
           
        }
        if (other.gameObject.tag == "Player" && keepRotating && isPlatform)
        {
            other.transform.SetParent(this.gameObject.transform);
        }

        if (other.gameObject.tag=="Player" && targetKeepRotating)
        {
            keep_Rotating.transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime));
        }
        //if (other.gameObject.tag == "Player")
        //{
        //    other.transform.SetParent(this.gameObject.transform);
        //}
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.parent = null;
            
        }
              
    }
}
