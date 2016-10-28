using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

public class Quest
{

    [XmlArray("Nodes")]
    [XmlArrayItem("Node")]
    public List<QuestNode> nodes;

    [XmlAttribute("QuestTitle")]
    string questTitle;

    [System.NonSerialized()]
    public string fileName;

    [System.NonSerialized()]
    private QuestNode currentNode;

    [System.NonSerialized]
    private int position;




    public QuestNode ProgressQuest()
    {
        if (nodes != null && position < nodes.Count)
        {
            currentNode = nodes[position];
            position++;
        }
        else
            return null;
        return currentNode;

    }

    public static Quest LoadQuest(string name)
    {
        var serializer = new XmlSerializer(typeof(Quest));
        var stream = new FileStream(name, FileMode.Open);
        var quest = serializer.Deserialize(stream) as Quest;
        stream.Close();

        return quest as Quest;
    }
}