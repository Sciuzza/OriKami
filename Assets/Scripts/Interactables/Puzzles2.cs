using UnityEngine;
using System.Collections;

public class Puzzles2 : MonoBehaviour
{
    // bool per tutti i puzzle 


    public bool isPuzzle1;
    public bool isPuzzle2;
    public bool isPuzzle3;
    public bool isPuzzle4;

    public bool doorOpen;
    public bool doorClosed;

    public bool isUp;
    public bool isDown;

    private bool keyHit = false;

    public GameObject[] platforms;

    public GameObject rotatingObject;
    public GameObject generatedObject;
    public GameObject leftDoor;
    public GameObject rightDoor;

    public GameObject goUp;
    public GameObject goDown;
    public GameObject goDown2;


    private Vector3 startPosLeftDoor;
    private Vector3 endPosLeftDoor;

    private Vector3 startPosRightDoor;
    private Vector3 endPosRightDoor;

    public Vector3 startPosUpObject;
    public Vector3 endPosUpObject;

    private Vector3 startDownObject;
    private Vector3 endDownObject;

    private Vector3 startDownObject2;
    private Vector3 endDownObject2;


    public float distance; // di quanto si muove la porta  x designer
    private float lerpTime = 5;
    private float currentLerpTime = 0;
    void Update()
    {

    }

    void Start()
    {
        if (doorOpen)
        {
            startPosLeftDoor = leftDoor.transform.position;
            endPosLeftDoor = leftDoor.transform.position + Vector3.forward * distance;

            startPosRightDoor = rightDoor.transform.position;
            endPosRightDoor = rightDoor.transform.position + Vector3.back * distance;
        }

        else if (doorClosed)
        {
            // startPosUpObject = leftDoor.transform.position;
            endPosLeftDoor = leftDoor.transform.position + Vector3.back * distance;

            startPosRightDoor = rightDoor.transform.position;
            endPosRightDoor = rightDoor.transform.position + Vector3.forward * distance;
        }

        else if (isPuzzle4)
        {

            //for (int i = 0; i < platforms.Length; i++)

            startPosUpObject = GetComponent<Transform>().position;
            endPosUpObject = GetComponent<Transform>().position + Vector3.down * distance;




            //startDownObject = goDown.transform.position;
            //endDownObject = goDown.transform.position + Vector3.down * distance;

            //startDownObject2 = goDown2.transform.position;
            //endDownObject2 = goDown2.transform.position + Vector3.down * distance;



            //startPosUpObject = goUp.transform.position;
            //endPosUpObject = goUp.transform.position + Vector3.up * distance;

            //startDownObject = goDown.transform.position;
            //endDownObject = goDown.transform.position + Vector3.up * distance;

            //startDownObject2 = goDown2.transform.position;
            //endDownObject2 = goDown2.transform.position + Vector3.up * distance;


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
            leftDoor.transform.position = Vector3.Lerp(startPosLeftDoor, endPosLeftDoor, leftPerc);
            rightDoor.transform.position = Vector3.Lerp(startPosRightDoor, endPosRightDoor, rightPerc);
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
            leftDoor.transform.position = Vector3.Lerp(startPosLeftDoor, endPosLeftDoor, leftPerc);
            rightDoor.transform.position = Vector3.Lerp(startPosRightDoor, endPosRightDoor, rightPerc);
        }
        yield return null;
    }

    IEnumerator TowerMovingUPCO(GameObject platform)
    {
        endPosUpObject = GetComponent<Transform>().position + Vector3.up * distance;
        while (true)
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime >= lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            float leftPerc = currentLerpTime / lerpTime;
            float rightPerc = currentLerpTime / lerpTime;


            //platform.transform.position = Vector3.Lerp(platform.GetComponent<Puzzles2>().endPosUpObject, platform.GetComponent<Puzzles2>().startPosUpObject, leftPerc);

            platform.transform.position = Vector3.Lerp(platform.GetComponent<Puzzles2>().endPosUpObject, platform.GetComponent<Puzzles2>().startPosUpObject, leftPerc);

            yield return null;
            //goDown.transform.position = Vector3.Lerp(startDownObject, endDownObject, rightPerc);
            //goDown2.transform.position = Vector3.Lerp(startDownObject2, endDownObject2, rightPerc);
        }
    }
    IEnumerator TowerMovingCO(GameObject platform)
    {
        keyHit = true;
        //endPosUpObject = GetComponent<Transform>().position + Vector3.down * distance;

        if (keyHit == true)
        {
            //if (platform.GetComponent<Puzzles2>().isUp)

            endPosUpObject = GetComponent<Transform>().position + Vector3.down * distance;
            while (platform.transform.position.y >= endPosUpObject.y)
            {
                currentLerpTime += Time.deltaTime;
                if (currentLerpTime >= lerpTime)
                {
                    currentLerpTime = lerpTime;
                }

                float leftPerc = currentLerpTime / lerpTime;
                float rightPerc = currentLerpTime / lerpTime;


                //platform.transform.position = Vector3.Lerp(platform.GetComponent<Puzzles2>().endPosUpObject, platform.GetComponent<Puzzles2>().startPosUpObject, leftPerc);

                platform.transform.position = Vector3.Lerp(platform.GetComponent<Puzzles2>().startPosUpObject, platform.GetComponent<Puzzles2>().endPosUpObject, leftPerc);
                Debug.Log(5);
                yield return null;
                //goDown.transform.position = Vector3.Lerp(startDownObject, endDownObject, rightPerc);
                //goDown2.transform.position = Vector3.Lerp(startDownObject2, endDownObject2, rightPerc);
            }



            endPosUpObject = GetComponent<Transform>().position + Vector3.up * distance;
            while (platform.transform.position == endPosUpObject)
            {
                currentLerpTime += Time.deltaTime;
                if (currentLerpTime >= lerpTime)
                {
                    currentLerpTime = lerpTime;
                }

                float leftPerc = currentLerpTime / lerpTime;
                float rightPerc = currentLerpTime / lerpTime;


                //platform.transform.position = Vector3.Lerp(platform.GetComponent<Puzzles2>().endPosUpObject, platform.GetComponent<Puzzles2>().startPosUpObject, leftPerc);

                platform.transform.position = Vector3.Lerp(platform.GetComponent<Puzzles2>().startPosUpObject, platform.GetComponent<Puzzles2>().endPosUpObject, leftPerc);
                Debug.Log(6);
                yield return null;
                //goDown.transform.position = Vector3.Lerp(startDownObject, endDownObject, rightPerc);
                //goDown2.transform.position = Vector3.Lerp(startDownObject2, endDownObject2, rightPerc);
            }

        }
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && isPuzzle2)
        {
            Instantiate(generatedObject);
            isPuzzle2 = false;
        }

        if (other.gameObject.tag == "Player" && isPuzzle3 && doorClosed)
        {
            StartCoroutine(DoorClosingCO());
        }

        if (other.gameObject.tag == "Player" && isPuzzle4)
        {
            foreach (var platform in platforms)
            {
                if (platform.GetComponent<Puzzles2>().isUp && platform != this.gameObject)
                {
                    
                    platform.GetComponent<Puzzles2>().isUp = false;
                    platform.GetComponent<Puzzles2>().isDown = true;
                    StartCoroutine(TowerMovingCO(platform));
                }
                else if (platform.GetComponent<Puzzles2>().isDown && platform != this.gameObject)
                {

                    
                    platform.GetComponent<Puzzles2>().isUp = true;
                    platform.GetComponent<Puzzles2>().isDown = false;
                    StartCoroutine(TowerMovingCO(platform));
                }

            }

        }
    }


    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && isPuzzle3 && doorOpen)
        {
            StartCoroutine(DoorOpeningCO());
            // leftFissure.transform.Translate(transform.right * 10 * Time.deltaTime);

        }



    }
}
