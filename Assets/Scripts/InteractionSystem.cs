using UnityEngine;
using System.Collections;


public enum forms { standard, frog, dragon, armadillo };



public class InteractionSystem : PlayerCore
{
    /*
    public float pushStrength = 6.0f;
    private Rigidbody rbody;

    void Start()
    {
        rbody = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.tag == "Player")
        {

            rbody.AddForce(transform.forward * pushStrength);
        }
    }
    */

    public forms currentForm = forms.standard;

    string currentActForm = "Standard Form";

    GameObject frog, standard, dragon, armadillo;

    MovementSystem msLink;

    void Awake()
    {
        msLink = GameObject.FindGameObjectWithTag("Player").GetComponent<MovementSystem>();

        frog = GameObject.FindGameObjectWithTag("Frog Form");
        frog.SetActive(false);

        standard = GameObject.FindGameObjectWithTag("Standard Form");
        standard.SetActive(true);

        dragon = GameObject.FindGameObjectWithTag("Dragon Form");
        dragon.SetActive(false);

        armadillo = GameObject.FindGameObjectWithTag("Armadillo Form");
        armadillo.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown("1") && currentForm != forms.standard)
        {
            standard.SetActive(true);
            GameObject.FindGameObjectWithTag(currentActForm).SetActive(false);
            currentActForm = "Standard Form";
            currentForm = forms.standard;
            msLink.SettingMeshBounds(currentActForm);
        }
        else if (Input.GetKeyDown("2") && currentForm != forms.frog)
        {
            frog.SetActive(true);
            GameObject.FindGameObjectWithTag(currentActForm).SetActive(false);
            currentActForm = "Frog Form";
            currentForm = forms.frog;
            msLink.SettingMeshBounds(currentActForm);
        }
        else if (Input.GetKeyDown("3") && currentForm != forms.dragon && !msLink.isOnGround)
        {
            dragon.SetActive(true);
            GameObject.FindGameObjectWithTag(currentActForm).SetActive(false);
            currentActForm = "Dragon Form";
            currentForm = forms.dragon;
            msLink.SettingMeshBounds(currentActForm);
        }
        else if (Input.GetKeyDown("4") && currentForm != forms.armadillo)
        {
            armadillo.SetActive(true);
            GameObject.FindGameObjectWithTag(currentActForm).SetActive(false);
            currentActForm = "Armadillo Form";
            currentForm = forms.armadillo;
            msLink.SettingMeshBounds(currentActForm);
        }


    }

    public void SwitchToStandard()
    {
        standard.SetActive(true);
        GameObject.FindGameObjectWithTag(currentActForm).SetActive(false);
        currentActForm = "Standard Form";
        currentForm = forms.standard;
        msLink.SettingMeshBounds(currentActForm);
    }
}
