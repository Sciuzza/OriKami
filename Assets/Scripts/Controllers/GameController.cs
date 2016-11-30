using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    #region Private Variables
    private GameObject player; 
    #endregion

    #region Event Variables
    [System.Serializable]
    public class gbEvent : UnityEvent<GameObject>
    {
    }

    public gbEvent gpInitializer, gameSettingsChanged;

    public UnityEvent ngpInitializer;
    #endregion

    #region Do not Destroy Behaviour
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    #region In game Design Tweaks
    void Update()
    {
        if (Input.GetKeyDown("k"))
            gameSettingsChanged.Invoke(player);
    }
    #endregion

    #region Initialization Methods
    public void InitializingScene()
    {
        if (FindingPlayer())
        {
            Debug.Log("Initializer Invoked Once");
            gpInitializer.Invoke(player);

        }
        else
        {
            Debug.Log("Not on Gameplay Scene");
            ngpInitializer.Invoke();
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
    #endregion
}
