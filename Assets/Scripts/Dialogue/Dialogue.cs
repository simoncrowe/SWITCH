using UnityEngine;
using System.Collections.Generic;
using LitJson;
using System;

//Instead of defining Awake(), override LateAwake()

public class Dialogue : MonoBehaviour
{

    public FirstPersonController userController;
    public bool canSuspendConversation = true;
    public string suspendConversationOptionText;
    protected Dictionary<string, Node> nodeIDs;
    public string firstKey;
    public string resumptionNodeKey;
    public string hungerNodeKey;
    public string deathNodeKey;
    protected Node currentNode;
    Node latestNode;
    public bool conversing;
    public TextAsset dialogueData;
    public string CurrentNodeID { get { return currentNode.ID;} set { currentNode = nodeIDs[value]; } }

    protected class Node
    {
        public string NPC { get; set; }
        public string ID { get; set; }
        public List<Choice> Choices { get; set; }

        public Node(string n, string i, List<Choice> c)
        {
            NPC = n;
            ID = i;
            Choices = c;
        }
    }

    protected struct Choice
    {
        private string target;
        private string player;

        public string Target
        {
            get { return target; }
            set { target = value; }
        }
        public string Player
        {
            get { return player; }
            set { player = value; }
        }
        public Choice(string playerText, string targetNode)
        {
            player = playerText;
            target = targetNode;
        }
    }

    public void SetDead()
    {
        currentNode = nodeIDs[deathNodeKey];
    }

    public void StartConversing()
    {
        conversing = true;
        InteractionObject.intaractionMenuActive = true;

        if (userController != null)
        {
            userController.LockMovement = true;
        }
    }

    void LoadData()
    {
        JsonReader reader = new JsonReader(dialogueData.text);
        int depth = 0;
        string propertyNameTemp = "";
        bool hasSavedNode = false;
        Node nodeTemp;
        string idTemp = "";
        string npcTextTemp = "";
        List<Choice> choicesTemp = new List<Choice>();
        string playerTextTemp = null;
        string TargetTemp = "";

        while (reader.Read())
        {
            if ((reader.Token == JsonToken.ObjectStart)
                || (reader.Token == JsonToken.ArrayStart))
            {
                depth++;
            }
            else if ((reader.Token == JsonToken.ObjectEnd)
                || (reader.Token == JsonToken.ArrayEnd))
            {
                depth--;
            }
            if (reader.Token == JsonToken.PropertyName)
            {
                propertyNameTemp = (string)reader.Value;
            }
            else if (depth == 4)
            {
                if (propertyNameTemp == "Player")
                {
                    playerTextTemp = (string)reader.Value;
                }
                else if (propertyNameTemp == "Target")
                {
                    TargetTemp = (string)reader.Value;
                }
            }
            else if (depth == 2)
            {
                if (propertyNameTemp == "NPC")
                {
                    npcTextTemp = (string)reader.Value;
                    hasSavedNode = false;
                }
                else if (propertyNameTemp == "ID")
                {
                    idTemp = (string)reader.Value;
                }
            }
            else if (depth == 3)
            {
                if (playerTextTemp != null)
                {
                    choicesTemp.Add(new Choice(playerTextTemp, TargetTemp));
                    playerTextTemp = null;
                }
            }
            else if (depth == 1)
            {
                if (!hasSavedNode)
                {
                    nodeTemp = new Node(npcTextTemp, idTemp, choicesTemp);
                    nodeIDs[idTemp] = nodeTemp;
                    choicesTemp = new List<Choice>();
                    hasSavedNode = true;
                }
            }
        }
    }

    protected void Start()
    {
        nodeIDs = new Dictionary<string, Node>();
        LoadData();
        currentNode = nodeIDs[firstKey];
        GUICalc();
        GUIChoicesCalc();
    }

    public void Update()
    {
        if (conversing)
        {
            for (int i = 1; i <= currentNode.Choices.Count; i++)
            {
                if (Input.GetKeyDown(i.ToString()))
                {
                    currentNode = nodeIDs[currentNode.Choices[i - 1].Target];
                    TryTriggerEvent();
                    GUIChoicesCalc();
                }
            }
        }
    }

    private void TryTriggerEvent()
    {
        EventManager.TriggerEvent("dialogue_" + currentNode.ID);
    }

    public GUISkin customSkin;
    int maxLineLength;
    int lineDimY;
    float bottomBoxDimY;
    float topBoxDimY;
    int bottomBoxLocY;
    Vector2 topScrollPos;
    int topScrollDimY;
    int topScrollInDimY;
    Vector2 bottomScrollPos;
    int bottomScrollLocY;
    int scrollDimX;
    int bottomScrollDimY;
    int scrollInDimX;
    int bottomScrollInDimY;
    int ChoicesBottom;
    Rect[] ChoiceButtons;
    Rect[] ChoiceLables;

    void GUICalc()
    {
        lineDimY = 19;
        topBoxDimY = (float)Screen.height * .15f;
        bottomBoxDimY = (float)Screen.height * .35f;
        bottomBoxLocY = Screen.height - (int)bottomBoxDimY;
        topScrollDimY = (int)topBoxDimY - 10;
        bottomScrollLocY = (Screen.height - (int)bottomBoxDimY) + 5;
        bottomScrollDimY = (int)bottomBoxDimY - 10;
        scrollDimX = Screen.width - 10;
        scrollInDimX = Screen.width - 30;
        maxLineLength = (scrollInDimX - 30) / 10;
    }
    public void GUIChoicesCalc()
    {
        ChoiceButtons = new Rect[currentNode.Choices.Count];
        ChoiceLables = new Rect[currentNode.Choices.Count];
        bottomScrollInDimY = 0;
        int curButtDimY = 0;
        int curButtLocY = 0;
        for (int i = 0; i < currentNode.Choices.Count; i++)
        {
            curButtLocY += curButtDimY + 5;
            curButtDimY = 0;
            curButtDimY = currentNode.Choices[i].Player.Length / maxLineLength;
            curButtDimY += (currentNode.Choices[i].Player.Length % maxLineLength != 0) ? 1 : 0;
            curButtDimY *= lineDimY;
            curButtDimY += 10;
            ChoiceButtons[i] = new Rect(5, curButtLocY, scrollInDimX, curButtDimY);
            ChoiceLables[i] = new Rect(10, curButtLocY + 5, scrollInDimX - 10, curButtDimY - 10);
            bottomScrollInDimY += currentNode.Choices[i].Player.Length / maxLineLength;
            bottomScrollInDimY += (currentNode.Choices[i].Player.Length % maxLineLength != 0) ? 1 : 0;
        }
        bottomScrollInDimY *= lineDimY;
        bottomScrollInDimY += currentNode.Choices.Count * 15;
        ChoicesBottom = bottomScrollInDimY;
        bottomScrollInDimY += 34;
        topScrollInDimY = currentNode.NPC.Length / maxLineLength;
        topScrollInDimY += (currentNode.NPC.Length % maxLineLength != 0) ? 1 : 0;
        topScrollInDimY *= lineDimY;
        topScrollInDimY += 10;
    }
    void OnGUI()
    {
        if (conversing)
        {
            GUI.skin = customSkin;
            GUI.Box(new Rect(0, 0, Screen.width, topBoxDimY), "");
            GUI.Box(new Rect(0, bottomBoxLocY, Screen.width, bottomBoxDimY), "");
            topScrollPos = GUI.BeginScrollView(new Rect(5, 5, scrollDimX, topScrollDimY), topScrollPos,
            (new Rect(5, 5, scrollInDimX, topScrollInDimY)));
            GUI.Label(new Rect(5, 10, scrollInDimX, topScrollInDimY), currentNode.NPC);
            GUI.EndScrollView();
            bottomScrollPos = GUI.BeginScrollView(new Rect(5, bottomScrollLocY, scrollDimX, bottomScrollDimY),
            bottomScrollPos, new Rect(5, 5, scrollInDimX, bottomScrollInDimY));

            for (int i = 0; i < currentNode.Choices.Count; i++)
            {
                if (GUI.Button(ChoiceButtons[i], ""))
                {
                    currentNode = nodeIDs[currentNode.Choices[i].Target];
                    TryTriggerEvent();
                    GUIChoicesCalc();
                }
            }
            for (int i = 0; i < currentNode.Choices.Count; i++)
            {
                GUI.Label(ChoiceLables[i], (i + 1) + ". " + currentNode.Choices[i].Player);
            }

            if (canSuspendConversation)
            {
                if (GUI.Button(new Rect(5, ChoicesBottom + 5, scrollInDimX, lineDimY + 10), ""))
                {
                    conversing = false;
                    if (userController != null)
                    {
                        userController.LockMovement = false;
                    }
                    InteractionObject.intaractionMenuActive = false;
                }
                GUI.Label(new Rect(10, ChoicesBottom + 10, scrollInDimX - 10, lineDimY + 5), suspendConversationOptionText);
            }
            GUI.EndScrollView();
        }
    }

}