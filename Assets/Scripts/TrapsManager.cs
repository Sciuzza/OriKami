using UnityEngine;
using System.Collections;

public class TrapsManager : MonoBehaviour
{
    
    #region SingleTone
    [HideInInspector]
    public static TrapsManager instance = null;

    void Awake()
    {

        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
      
        #endregion
      
    }

    void Update()
    {

           

    }
    
}
