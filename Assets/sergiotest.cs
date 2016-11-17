using UnityEngine;
using System.Collections;

public class sergiotest : MonoBehaviour {


    private Vector3 screenPoint;
    private Vector3 offset;
    public GameObject anchorPoint;

    void OnMouseDown()
    {
        anchorPoint.transform.position = Input.mousePosition;
        this.gameObject.transform.SetParent(anchorPoint.transform);
        this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
    }

    void OnMouseDrag()
    {
        anchorPoint.transform.position = Input.mousePosition;

    }
}
