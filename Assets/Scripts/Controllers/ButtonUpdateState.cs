using UnityEngine;
using System.Collections;

public class ButtonUpdateState : MonoBehaviour {

    #region Edit Mode Methods

    public void OnValidate()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<SuperDataManager>().UpdatingButState(this.gameObject.GetComponent<Puzzles>());
    }
    #endregion
}
