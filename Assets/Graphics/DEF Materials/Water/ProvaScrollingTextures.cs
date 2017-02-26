using UnityEngine;
using System.Collections;

public class ProvaScrollingTextures : MonoBehaviour

{
    public float ScrollTextureX;
    public float ScrollTextureY;
    public string TextName;

    Material objMat;
    float offsetX;
    float offsetY;
    Vector2 offset;
    Renderer rendererlol;

    void Start()
    {
        rendererlol = GetComponent<Renderer>();
        objMat = rendererlol.material;
    }

    void Update()
    {
        
        offsetX = Time.time * ScrollTextureX;
        offsetY = Time.time * ScrollTextureY;
        offset = new Vector2(offsetX, offsetY);
        objMat.SetTextureOffset(TextName, offset);
        if (offsetX > 1)
            offsetX = 0;
        if (offsetY > 1)
            offsetY = 0;
        
    }
 
}









                                                                            