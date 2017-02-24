using UnityEngine;
using System.Collections;

public class SpecialTreePlat : MonoBehaviour
{

    public GameObject RotTargetGb;
    public float InTime, OutTime;

    private Quaternion oriRot, TargetRot;
    private float timePassed;

    private bool goingIn, goingOut;
    private float fractionIn, fractionOut;

    private void Awake()
    {
        this.oriRot = this.gameObject.transform.rotation;
        this.TargetRot = this.RotTargetGb.transform.rotation;
        this.fractionIn = this.InTime;
        this.fractionOut = this.OutTime;
        this.timePassed = 0;
    }


    private void OnTriggerEnter(Collider hit)
    {
        if (hit.CompareTag("Player"))
        {
            this.StopAllCoroutines();
            this.goingOut = false;
            this.goingIn = true;
            this.StartCoroutine(this.RotateFromAToB(this.gameObject.transform.rotation, this.TargetRot, this.InTime));
        }
    }

    private void OnTriggerExit(Collider hit)
    {
        if (hit.CompareTag("Player"))
        {
            this.StopAllCoroutines();
            this.goingOut = true;
            this.goingIn = false;
            this.StartCoroutine(this.RotateFromAToB(this.gameObject.transform.rotation, this.oriRot, this.OutTime));
        }
    }

    private IEnumerator RotateFromAToB(Quaternion fromRot, Quaternion toRot, float timeTaken)
    {
        if (this.goingIn && !this.goingOut && this.timePassed != 0)
        {
            timeTaken = this.InTime - ((((1 - this.timePassed) * this.fractionOut) / this.OutTime) * this.InTime);
            this.fractionIn = timeTaken;
           // Debug.Log("Fraction In " + this.fractionIn);
        }
        else if (!this.goingIn && this.goingOut && this.timePassed != 0)
        {
            timeTaken = this.OutTime - ((((1 - this.timePassed) * this.fractionIn) / this.InTime) * this.OutTime);
            this.fractionOut = timeTaken;
           // Debug.Log("Fraction Out " + this.fractionOut);
        }

        Debug.Log(this.timePassed);
        this.timePassed = 0;

        while (this.timePassed < 1)
        {
            this.timePassed += Time.deltaTime / timeTaken;

            this.gameObject.transform.rotation = Quaternion.Slerp(fromRot, toRot, this.timePassed);

            yield return null;
        }

        this.timePassed = 0;
        this.fractionIn = this.InTime;
        this.fractionOut = this.OutTime;
    }
}
