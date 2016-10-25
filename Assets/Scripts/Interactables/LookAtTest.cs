using UnityEngine;
using System.Collections;

public class LookAtTest : MonoBehaviour
{

    public Transform[] targetPosition = new Transform[3];
    public Quaternion target;
    public int positionIndex;
    public bool isRotating = false, isOnButton = false;
    

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

   public void OnTriggerController() {
        isOnButton = false;
    }
        
    

    IEnumerator movingWheel(Quaternion target)
    {
        while (Quaternion.Angle(this.transform.rotation, target) > 0.1f)
        {
           this.transform.rotation = Quaternion.Slerp(this.transform.rotation, target, Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        //blueWheel.transform.rotation = target;
        isRotating = false;
    }

}