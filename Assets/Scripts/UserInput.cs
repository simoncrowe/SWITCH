using UnityEngine;

public class UserInput : MonoBehaviour
{
    public KeyCode menuKey;
    public InGameGUI inGameGUI;
    public FirstPersonController playerControler;
    void Start()
    {
        menuKey = KeyCode.M;
    }

    void Update()
    {
        if ((inGameGUI.ShowMenu) || (InteractionObject.intaractionMenuActive))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (Input.GetKeyDown(menuKey))
        {
            Debug.Log("Should be toggling menu!");
            inGameGUI.ShowMenu = !inGameGUI.ShowMenu;
            playerControler.LockMovement = true;
        }
    }
}
