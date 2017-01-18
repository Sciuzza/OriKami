using UnityEngine;
using System.Collections;

public class RotateAndResize : MonoBehaviour {

	public float speed, rotationSpeed;
	public Vector3 finalSize = new Vector3(1f, 1f, 1f);

	public Transform form;

	private int giri = 1;
	private bool isShrinking = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Shrink ();
		Rotate ();
	}

	public void Shrink()
	{
		
			if (isShrinking) {
				form.localScale = Vector3.MoveTowards (form.localScale, finalSize, speed * Time.deltaTime);
				if (form.localScale.x <= finalSize.x + 0.01f) 
				{
					isShrinking = false;
				}
			} 
			else 
			{
				form.localScale = Vector3.MoveTowards (form.localScale, new Vector3(1f,1f,1f), speed * Time.deltaTime);
				if (form.localScale.x >= 0.99f)
				{
					isShrinking = true;
				}
			}
	}

	public void Rotate()
	{
		form.localEulerAngles = Vector3.MoveTowards (form.localEulerAngles, new Vector3 (giri*360f, 0f, 0f), rotationSpeed);
		if (form.localEulerAngles.x >= giri * 359.99) 
		{
			giri++;
		}
	}
}
