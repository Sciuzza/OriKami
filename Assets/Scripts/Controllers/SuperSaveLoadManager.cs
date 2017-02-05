using UnityEngine;
using System.Collections;

public class SuperSaveLoadManager : MonoBehaviour {


    private void Awake()
    {
        MenuManager mmTempLink = this.gameObject.GetComponent<MenuManager>();

        mmTempLink.newDataRequest.AddListener(this.ErasingOnDiskData);
        mmTempLink.loadDataRequest.AddListener(this.LoadingOnDiskData);
    }


    private void ErasingOnDiskData()
    {

    }

    private void LoadingOnDiskData()
    {
        
    }
}
