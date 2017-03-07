using UnityEngine;
using System.Collections;

public class FixCutscene : MonoBehaviour
{


    public GameObject startTrigger;
    public GameObject gbRef;


    private void Start()
    {
        this.StartCoroutine(this.Moving());
    }

    private IEnumerator Moving()
    {
        var timer = 0.0f;
        var timeTaker = 0.05f;

        var oriPos = this.startTrigger.transform.position;
        var tarPos = this.gbRef.transform.position;

        while (timer <= 1)
        {
            timer += Time.deltaTime / timeTaker;

            this.startTrigger.transform.position = Vector3.Lerp(oriPos, tarPos, timer);

            yield return null;
        }
    }

}
