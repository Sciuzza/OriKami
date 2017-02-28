using UnityEngine;
using System.Collections;

public class FallingSound : MonoBehaviour {

    private bool deployed;
    public float deploymentHeight;
    private SoundManager soundRef;


    void Awake()
    {
        soundRef = GameObject.FindGameObjectWithTag("Player").GetComponent<SoundManager>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        RaycastHit hit;
        Ray landingRay = new Ray(this.transform.position, Vector3.down);

        Debug.DrawRay(transform.position, Vector3.down * deploymentHeight);
        if (!deployed)
        {
            if (Physics.Raycast(landingRay,out hit, deploymentHeight))
            {
               
                
                soundRef.PlaySound(0, 1);
            }
        }



	}
}
