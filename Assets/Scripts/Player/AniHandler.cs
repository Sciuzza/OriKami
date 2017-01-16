using UnityEngine;
using System.Collections;

public class AniHandler : MonoBehaviour {

    #region Public Variables
   
    #endregion

    #region Events
    
    #endregion

    #region Private Variables

    private Vector3 finalMoveDirTemp;
    #endregion

    #region Taking References and linking Events
    void Awake()
    {
        FSMChecker fsmCheckerTempLink = this.gameObject.GetComponent<FSMChecker>();

    

        MoveHandler moveHandTempLink = this.gameObject.GetComponent<MoveHandler>();

   
    }
    #endregion
}
