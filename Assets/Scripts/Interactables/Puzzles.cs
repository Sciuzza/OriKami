using UnityEngine;
using System.Collections;


public class Puzzles : MonoBehaviour {

    public  float degree;

    public bool isPuzzle1;
    public bool isPuzzle2;
    public bool isPuzzle3;
    public bool isPuzzle4;

    public GameObject rotatingObject; 



    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && isPuzzle4)
        {
            StartCoroutine(MyCo());
        }
    }


    IEnumerator MyCo()
    {
        while (true)
        {
            rotatingObject.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, degree), Time.deltaTime);
            if (rotatingObject.transform.eulerAngles.y >= degree)
            {
                rotatingObject.transform.eulerAngles =new Vector3(0f, degree);
                yield break;
            }
            else
            yield return null;
        }
    }
}
