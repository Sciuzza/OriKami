using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BubbleDialogue : MonoBehaviour
{

    public GameObject textBox;
    public ActivatrTextAtLineBubble linker;

    public Text theText;
    public TextAsset textFile;
    public string[] textLines;
    public bool isCoroutine;

    public int currentLine;
    public int endAtLine;

    public bool isActive;
    // public bool stopPlayerMovement;       LO DEVE IMPLEMENTARE CRISTIANO  

    void Start()
    {

     //  linker = FindObjectOfType<ActivatrTextAtLineBubble>();

        if (textFile != null)
        {
            textLines = (textFile.text.Split('\n'));
        }

        if (endAtLine == 0)
        {
            endAtLine = textLines.Length - 1;
        }

        if (isActive)
        {
            EnableTextBox();
        }
        else
        {
            DisableTextBox();
        }
    }

    void Update()
    {
        if (!isActive)
        {
            return;
        }

        if (isCoroutine)
        {
            isCoroutine = false;
            StartCoroutine(TextCO());

        }


    }
    public IEnumerator TextCO()
    {
        while (currentLine<=endAtLine)
        {
            Debug.Log(currentLine);
            theText.text = textLines[currentLine];
            currentLine += 1;
            yield return new WaitForSeconds(2f);
        }
          
              

        if (currentLine > endAtLine)
        {
            Debug.Log("Disabilito");
            DisableTextBox();
            //currentLine = 0;
            //isCoroutine = true;

        }

        //else {

        //    StartCoroutine(TextCO());
        //}

    }


    public void EnableTextBox()
    {
        textBox.SetActive(true);
        isActive = true;
    }

    public void DisableTextBox()
    {
        textBox.SetActive(false);
        isActive = false;

    }

    public void ReloadScript(TextAsset theText)
    {
        if (theText != null)
        {
            textLines = new string[1];
            textLines = (theText.text.Split('\n'));
        }
    }
}
