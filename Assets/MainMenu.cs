using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject singleplayerMenu;
    [SerializeField] private GameObject multiplayerMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject helpMenu;
    [Header("Audio Management")]
    [SerializeField] private AudioClip buttonPress;
    [SerializeField][Range(0f, 1f)] private float buttonPressVolume;
    

    public void Singleplayer()
    {
        ButtonPress();
        mainMenu.SetActive(false);
        singleplayerMenu.SetActive(true);
    }

    public void Multiplayer()
    {
        ButtonPress();
        mainMenu.SetActive(false);
        multiplayerMenu.SetActive(true);
    }

    public void OptionsMenu()
    {
        ButtonPress();
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void HelpMenu()
    {
        ButtonPress();
        mainMenu.SetActive(false);
        helpMenu.SetActive(true);
    }

    public void Character()
    {
        ButtonPress();
    }

    public void Achievements()
    {
        ButtonPress();
    }

    public void Return()
    {
        ButtonPress();
        singleplayerMenu.SetActive(false);
        multiplayerMenu.SetActive(false);
        optionsMenu.SetActive(false);
        helpMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void QuitGame()
    {
        ButtonPress();
        Application.Quit();
    }

    public void ButtonPress()
    {
        FindAnyObjectByType<AudioManager>().Play(buttonPress, buttonPressVolume, "effects");
    }
}