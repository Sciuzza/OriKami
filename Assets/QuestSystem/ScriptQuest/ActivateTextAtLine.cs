﻿using UnityEngine;
using System.Collections;

public class ActivateTextAtLine : MonoBehaviour
{

    public TextAsset theText;

    public int startLine;
    public int endLine;

    public TextBoxManager theTextBox;
    //public BubbleDialogue theBubbleDialogue;
    //public BubblePlayerDialogue playerBubbleDialogue;

    public bool requireButtonPress;
    public bool isBubble = false;
    public bool isPlayerBubble = false;
    public bool destroyWhenActivated;
    private bool waitForPress;

    public Vector3 playerPos;
    public Vector3 npcPos, delta;

   
 


    void Start()
    {

        delta = new Vector3(playerPos.x - npcPos.x, 0.0f, playerPos.z - npcPos.z);

        if (isBubble)
        {
          //  theBubbleDialogue = FindObjectOfType<BubbleDialogue>();

        }
        else if (!isBubble)
        {
            theTextBox = FindObjectOfType<TextBoxManager>();
        }
        if (isPlayerBubble)
        {
          //  playerBubbleDialogue = FindObjectOfType<BubblePlayerDialogue>();
        }

    }

    void Update()
    {
        if (waitForPress && Input.GetKeyDown(KeyCode.J)&&!isBubble)
        {
            theTextBox.ReloadScript(theText);
            theTextBox.currentLine = startLine;
            theTextBox.endAtLine = endLine;
            theTextBox.EnableTextBox();

            if (destroyWhenActivated)
            {
                Destroy(gameObject);
            }
        }
        //if(waitForPress && Input.GetKeyDown(KeyCode.J) && isBubble)
        //{
        //    theBubbleDialogue.ReloadScript(theText);
        //    theBubbleDialogue.currentLine = startLine;
        //    theBubbleDialogue.endAtLine = endLine;
        //    theBubbleDialogue.EnableTextBox();

        //    if (destroyWhenActivated)
        //    {
        //        Destroy(gameObject);
        //    }
        //}
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            
            theTextBox.EnableTextBox();
            if (requireButtonPress)
            {
                waitForPress = true;
                return;
            }

            theTextBox.ReloadScript(theText);
            theTextBox.currentLine = startLine;
            theTextBox.endAtLine = endLine;
          

            if (destroyWhenActivated)
            {
                Destroy(gameObject);
            }
        }
        //else if (other.tag == "Player" && isPlayerBubble && !isBubble)
        //{
        //    if (requireButtonPress)
        //    {
        //        waitForPress = true;
        //        return;
        //    }

        //    playerBubbleDialogue.ReloadScript(theText);
        //    playerBubbleDialogue.currentLine = startLine;
        //    playerBubbleDialogue.endAtLine = endLine;
        //    playerBubbleDialogue.EnableTextBox();

        //    if (destroyWhenActivated)
        //    {
        //        Destroy(gameObject);
        //    }
        //}
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            waitForPress = false;
        }
    }

}
