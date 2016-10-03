using UnityEngine;
using System.Collections;

public enum forms { standard, frog, dragon, armadillo };

public class InterCC : MonoBehaviour {




    private PlCore coreLink;
    private CharacterController ccLink;

    void Awake()
    {
        coreLink = this.GetComponent<PlCore>();
        ccLink = this.GetComponent<CharacterController>();
    }

 
	
	// Update is called once per frame
	void Update () {

        KMInputsManager();
        JoyInputManager();


    }

 

    private void KMInputsManager()
    {
        if (Input.GetKeyDown("1") && coreLink.currentForm != forms.standard)
        {
            coreLink.standard.SetActive(true);
            GameObject.FindGameObjectWithTag(coreLink.currentActForm).SetActive(false);
            coreLink.currentActForm = "Standard Form";
            coreLink.currentForm = forms.standard;

        }
        else if (Input.GetKeyDown("2") && coreLink.currentForm != forms.frog)
        {
            coreLink.frog.SetActive(true);
            GameObject.FindGameObjectWithTag(coreLink.currentActForm).SetActive(false);
            coreLink.currentActForm = "Frog Form";
            coreLink.currentForm = forms.frog;

        }
        else if (Input.GetKeyDown("3") && coreLink.currentForm != forms.dragon && !ccLink.isGrounded)
        {
            coreLink.dragon.SetActive(true);
            GameObject.FindGameObjectWithTag(coreLink.currentActForm).SetActive(false);
            coreLink.currentActForm = "Dragon Form";
            coreLink.currentForm = forms.dragon;

        }
        else if (Input.GetKeyDown("4") && coreLink.currentForm != forms.armadillo)
        {
            coreLink.armadillo.SetActive(true);
            GameObject.FindGameObjectWithTag(coreLink.currentActForm).SetActive(false);
            coreLink.currentActForm = "Armadillo Form";
            coreLink.currentForm = forms.armadillo;

        }
    }


    private void JoyInputManager()
    {
        if (Input.GetKeyDown("1") && coreLink.currentForm != forms.standard)
        {
            coreLink.standard.SetActive(true);
            GameObject.FindGameObjectWithTag(coreLink.currentActForm).SetActive(false);
            coreLink.currentActForm = "Standard Form";
            coreLink.currentForm = forms.standard;

        }
        else if (Input.GetAxis("LRTButton") > 0 && coreLink.currentForm != forms.frog)
        {
            coreLink.frog.SetActive(true);
            GameObject.FindGameObjectWithTag(coreLink.currentActForm).SetActive(false);
            coreLink.currentActForm = "Frog Form";
            coreLink.currentForm = forms.frog;

        }
        else if (Input.GetAxis("LRTButton") < 0 && coreLink.currentForm != forms.dragon && !ccLink.isGrounded)
        {
            coreLink.dragon.SetActive(true);
            GameObject.FindGameObjectWithTag(coreLink.currentActForm).SetActive(false);
            coreLink.currentActForm = "Dragon Form";
            coreLink.currentForm = forms.dragon;

        }
        else if (Input.GetButtonDown("LBButton") && coreLink.currentForm != forms.armadillo)
        {
            coreLink.armadillo.SetActive(true);
            GameObject.FindGameObjectWithTag(coreLink.currentActForm).SetActive(false);
            coreLink.currentActForm = "Armadillo Form";
            coreLink.currentForm = forms.armadillo;

        }


        
    }

}
