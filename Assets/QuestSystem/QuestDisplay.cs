using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QuestDisplay : MonoBehaviour
{

    public Quest quest;

    public Image nodeImage;
    public Text nodeTitle;
    public Text nodeText;

    private QuestNode currentNode;

    public void Initialize(Quest quest)
    {
        this.quest = quest;
        SetupUIElements();
    }


    public void SetupUIElements()
    {
        currentNode = quest.ProgressQuest();

        if (currentNode == null)
        {
            gameObject.SetActive(false);
            return;
        }

        nodeImage.sprite = GameObject.Find("QuestBook").GetComponent<QuestManager>().GetIcon(currentNode.nodeImage);
        nodeTitle.text = currentNode.nodeTitle;
        nodeText.text = currentNode.nodeText;

        if (nodeImage.sprite == null)
            nodeImage.gameObject.SetActive(false);

    }
}