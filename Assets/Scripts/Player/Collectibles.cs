using UnityEngine;
using System.Collections;

public class Collectibles : MonoBehaviour {

	void OnTriggerEnter(Collider other)

	{
		if (other.gameObject.CompareTag("Collectibles"))
		{
			other.gameObject.SetActive(false);
		}
	}

}

