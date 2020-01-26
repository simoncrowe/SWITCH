using UnityEngine;
using System.Collections.Generic;

public class InGameGUI : MonoBehaviour
{
    public GUISkin skin;
    protected List<UIMessage> messages;

    protected bool showMenu = false;
    public bool ShowMenu { get { return showMenu; } set { showMenu = value; } }

    void Awake()
    {
        messages = new List<UIMessage>(5);
    }

    void Update()
    {
        if (showMenu)
        {
            Time.timeScale = 0;
            AudioListener.pause = true;
        }
        else
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
            // Deletes expired messages and decrements remaining duration duration
            foreach (UIMessage m in messages)
            {
                if ((m.DisplayMode == UIMessageDisplayMode.Timed) && (m.ShouldDisplay))
                {
                    m.SetRemainingDuration(m.RemainingDuration - Time.deltaTime);
                    if (m.RemainingDuration < 0) m.SetShouldDiplay(false);
                }
            }
        }
    }
    void OnGUI()
    {
        GUI.skin = skin;
        // If game is unpaused
        if (!showMenu)
        {
            // display UI messages
            int messageDisplayIndex = 1;
            foreach (UIMessage m in messages)
            {
                if (m.ShouldDisplay)
                {
                    GUI.Label(new Rect(5, Screen.height - ((20 * (messageDisplayIndex)) + 5),
                                       Screen.width - 10, 20), m.Text);
                    messageDisplayIndex++;
                }
            }
        }
        else
        {
            Debug.Log("Should be showing menu!");
            GUI.Window(0, new Rect((Screen.width / 2) - 400, (Screen.height / 2) - 250, 800, 500), MainMenu, "Menu");
        }
    }
    protected void MainMenu(int id)
    {
        Debug.Log("CallingMainMenu Method.");
        if (GUI.Button(new Rect(360, 470, 80, 20), "Resume"))
        {
           showMenu = false;
        }
    }
    // Adds a unique UIMessage to the messages list; sorts list by message priority
    public void AddUIMessage(UIMessage newMessage)
    {
        bool duplicateMessage = false;
        foreach (UIMessage m in messages)
        {
            if (ReferenceEquals(newMessage, m)) duplicateMessage = true;
        }
        if (!duplicateMessage)
        {
            messages.Add(newMessage);
            messages.Sort();
        }
    }

}

