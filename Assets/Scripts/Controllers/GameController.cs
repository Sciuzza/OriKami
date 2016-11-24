using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GameController : MonoBehaviour {

    [HideInInspector]
    public GameObject player;

    [System.Serializable]
    public class gbEvent : UnityEvent<GameObject>
    {
    }

    

    [HideInInspector]
    public gbEvent initializer, designRunningTweaks;

    public UnityEvent currentInputChange;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        
    }

    void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            designRunningTweaks.Invoke(player);
            currentInputChange.Invoke();
        }
    }

    public bool FindingPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Missing Player in Scene");
            return false;
        }
        else
            return true;
    }

    public void InitializingScene()
    {
        if (FindingPlayer())
        {
            initializer.Invoke(player);

        }
    }
    
}
