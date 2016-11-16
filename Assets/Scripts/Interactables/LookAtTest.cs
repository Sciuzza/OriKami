using UnityEngine;
using System.Collections;

public class LookAtTest : MonoBehaviour
{

    public Transform[] targetPosition = new Transform[3];
    public Quaternion target;
    public int positionIndex;
    public bool isRotating = false, isOnButton = false;
    public float currentVelocity;


    public void IndexController()
    {

        isRotating = true;
        isOnButton = true;

        if (positionIndex <= 1)
            positionIndex++;


        else
            positionIndex = 0;

        target = Quaternion.LookRotation(targetPosition[positionIndex].forward, targetPosition[positionIndex].up);

        StartCoroutine(movingWheel(target));


    }

    public void OnTriggerController()
    {
        isOnButton = false;
    }

    
    IEnumerator movingWheel(Quaternion target)
    {
        while (Quaternion.Angle(this.transform.rotation, target) != 0)
        {
            this.transform.rotation = Quaternion.Slerp(target, this.transform.rotation, Mathf.SmoothDamp(0, 1, ref currentVelocity, Time.deltaTime / 6));
            yield return null;
        }



        isRotating = false;
    }

}