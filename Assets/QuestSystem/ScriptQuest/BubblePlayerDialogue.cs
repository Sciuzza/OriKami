using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BubblePlayerDialogue : MonoBehaviour {

    public GameObject textBox;

    public Text theText;
    public Canvas playerCanvas;

    public TextAsset textFile;
    public string[] textLines;

    public int currentLine;
    public int endAtLine;

    public MoveCC player;

    public bool isActive;
    public bool stopPlayerMovement;

    void Start()
    {
        player = FindObjectOfType<MoveCC>();

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

        theText.text = textLines[currentLine];

        if (Input.GetKeyDown(KeyCode.Return))
        {
            currentLine += 1;
        }

        if (currentLine > endAtLine)
        {
            DisableTextBox();
        }
    }

    public void EnableTextBox()
    {
        
        playerCanvas.enabled = true;
        textBox.SetActive(true);
        isActive = true;
    }

    public void DisableTextBox()
    {
        
        playerCanvas.enabled = false;
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
