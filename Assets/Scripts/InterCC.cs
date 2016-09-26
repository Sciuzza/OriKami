using UnityEngine;
using System.Collections;

public enum forms { standard, frog, dragon, armadillo };

public class InterCC : MonoBehaviour {


    private MoveCC moveLink;
    private CharacterController ccLink;

    // to be placed on static script
    private string currentActForm = "Standard Form";
    private GameObject frog, standard, dragon, armadillo;
    public forms currentForm = forms.standard;


    // Use this for initialization
    void Awake () {

        moveLink = GetComponent<MoveCC>();
        ccLink = GetComponent<CharacterController>();

        frog = GameObject.FindGameObjectWithTag("Frog Form");
        frog.SetActive(false);

        standard = GameObject.FindGameObjectWithTag("Standard Form");
        standard.SetActive(true);

        dragon = GameObject.FindGameObjectWithTag("Dragon Form");
        dragon.SetActive(false);

        armadillo = GameObject.FindGameObjectWithTag("Armadillo Form");
        armadillo.SetActive(false);

        

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("1") && currentForm != forms.standard)
        {
            standard.SetActive(true);
            GameObject.FindGameObjectWithTag(currentActForm).SetActive(false);
            currentActForm = "Standard Form";
            currentForm = forms.standard;
            
        }
        else if (Input.GetKeyDown("2") && currentForm != forms.frog)
        {
            frog.SetActive(true);
            GameObject.FindGameObjectWithTag(currentActForm).SetActive(false);
            currentActForm = "Frog Form";
            currentForm = forms.frog;
           
        }
        else if (Input.GetKeyDown("3") && currentForm != forms.dragon && !ccLink.isGrounded)
        {
            dragon.SetActive(true);
            GameObject.FindGameObjectWithTag(currentActForm).SetActive(false);
            currentActForm = "Dragon Form";
            currentForm = forms.dragon;
           
        }
        else if (Input.GetKeyDown("4") && currentForm != forms.armadillo)
        {
            armadillo.SetActive(true);
            GameObject.FindGameObjectWithTag(currentActForm).SetActive(false);
            currentActForm = "Armadillo Form";
            currentForm = forms.armadillo;
            
        }
    }
}
