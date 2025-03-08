using UnityEngine;
using UnityEngine.InputSystem;

public class Input : MonoBehaviour
{
    public InputAction move;
    public InputAction sprint;
    public InputAction shoot;
    public InputAction reload;
    public InputAction flashlight;
    public InputAction throwGrenade;
    public InputAction useEquipment;
    public InputAction switchWeapons;
    public InputAction interact;
    public InputAction chat;
    public InputAction mainMenu;
    public InputAction shove;

    public bool isMessaging;


    public void ConfigureKeybinds()
    {
        move = InputSystem.actions.FindAction("Move");
        sprint = InputSystem.actions.FindAction("Sprint");
        shoot = InputSystem.actions.FindAction("Shoot");
        reload = InputSystem.actions.FindAction("Reload");
        flashlight = InputSystem.actions.FindAction("Flashlight");
        throwGrenade = InputSystem.actions.FindAction("Throw Grenade");
        useEquipment = InputSystem.actions.FindAction("Use Equipment");
        switchWeapons = InputSystem.actions.FindAction("Switch Weapon");
        interact = InputSystem.actions.FindAction("Interact");
        chat = InputSystem.actions.FindAction("Chat");
        mainMenu = InputSystem.actions.FindAction("Main Menu");
        shove = InputSystem.actions.FindAction("Shove");
    }
}
