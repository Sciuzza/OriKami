using UnityEngine;
using System.Collections;

public class ActivatrTextAtLineBubble : MonoBehaviour {
    public TextAsset theText;

    public int startLine;
    public int endLine;
        
    public BubbleDialogue theBubbleDialogue;
   

    public bool requireButtonPress;
    private bool waitForPress;
    public bool isBubble = false;
    public bool destroyWhenActivated;


    void Start()
    {
        if (isBubble)
        {
              theBubbleDialogue = FindObjectOfType<BubbleDialogue>();

        }
     
    }

    void Update()
    {
        //if (waitForPress && Input.GetKeyDown(KeyCode.J) && !isBubble)
        //{
        //    theTextBox.ReloadScript(theText);
        //    theTextBox.currentLine = startLine;
        //    theTextBox.endAtLine = endLine;
        //    theTextBox.EnableTextBox();

        //    if (destroyWhenActivated)
        //    {
        //        Destroy(gameObject);
        //    }
        //}
        if(waitForPress && Input.GetKeyDown(KeyCode.J) && isBubble)
       {
            theBubbleDialogue.ReloadScript(theText);
            theBubbleDialogue.currentLine = startLine;
            theBubbleDialogue.endAtLine = endLine;
            theBubbleDialogue.EnableTextBox();

            if (destroyWhenActivated)
            {
                Destroy(gameObject);
          }
        }
    }

    void OnTriggerEnter(Collider other)
    {
       if (other.tag == "Player")
        {
            Debug.Log("ciOA!");
            if (requireButtonPress)
           {
                waitForPress = true;
               return;
            }

            theBubbleDialogue.ReloadScript(theText);
            theBubbleDialogue.currentLine = startLine;
            theBubbleDialogue.endAtLine = endLine;
            theBubbleDialogue.EnableTextBox();
   
            if (destroyWhenActivated)
           {
              Destroy(gameObject);
           }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            waitForPress = false;
        }
    }
}
