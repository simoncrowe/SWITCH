using UnityEngine;
using System.Collections.Generic;

public class InGameGUI : MonoBehaviour
{
    public bool hasCursor = true;
    public FirstPersonController userController;
    public GUISkin skin;
    public Texture normalCursor;
    public Texture converseCursor;
    public Texture useCursor;
    private List<UIMessage> messages;
    private CursorType curCursor = CursorType.Normal;
    public CursorType CurrentCursor
    {
        get { return curCursor; }
        set { curCursor = value; }
    }
    private bool showMenu = false;
    public bool ShowMenu { get { return showMenu; } set { showMenu = value; } }

    public enum CursorType { Normal, Converse, Use, None }

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
        if (!userController.LockMovement)
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
            if (hasCursor)
            {
                // Draw appropriate cursor
                switch (curCursor)
                {
                    case CursorType.Normal:
                        GUI.DrawTexture(new Rect((Screen.width / 2) - (normalCursor.width / 2),
                                                 (Screen.height / 2) - (normalCursor.height / 2),
                                                 normalCursor.width, normalCursor.height), normalCursor);
                        break;
                    case CursorType.Use:
                        GUI.DrawTexture(new Rect((Screen.width / 2) - (useCursor.width / 2),
                                                 (Screen.height / 2) - (useCursor.height / 2),
                                                 useCursor.width, useCursor.height), useCursor);
                        break;
                    case CursorType.Converse:
                        GUI.DrawTexture(new Rect((Screen.width / 2) - (converseCursor.width / 2),
                                                 (Screen.height / 2) - (converseCursor.height / 2),
                                converseCursor.width, converseCursor.height), converseCursor);
                        break;
                    case CursorType.None:
                        break;
                }
            }
        }
        else if (showMenu)
        {
            Debug.Log("Should be showing menu!");
            userController.LockMovement = true;
            GUI.Window(0, new Rect((Screen.width / 2) - 400, (Screen.height / 2) - 250, 800, 500), MainMenu, "Menu");
        }
    }
    void MainMenu(int id)
    {
        Debug.Log("CallingMainMenu Method.");
        if (GUI.Button(new Rect(360, 470, 80, 20), "Resume"))
        {
            userController.LockMovement = false; showMenu = false;
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

