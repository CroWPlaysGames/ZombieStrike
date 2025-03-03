using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject helpMenu;
    [Header("Audio Management")]
    [SerializeField] private AudioClip buttonPress;
    [SerializeField][Range(0f, 1f)] private float buttonPressVolume;

    public void Singleplayer()
    {
        FindAnyObjectByType<AudioManager>().Play(buttonPress, buttonPressVolume, "effects");
        SceneManager.LoadScene("Endless Mode");
    }

    public void Multiplayer()
    {
        FindAnyObjectByType<AudioManager>().Play(buttonPress, buttonPressVolume, "effects");
        SceneManager.LoadScene("Game");
    }

    public void OptionsMenu()
    {
        FindAnyObjectByType<AudioManager>().Play(buttonPress, buttonPressVolume, "effects");
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void HelpMenu()
    {
        FindAnyObjectByType<AudioManager>().Play(buttonPress, buttonPressVolume, "effects");
        mainMenu.SetActive(false);
        helpMenu.SetActive(true);
    }

    public void Character()
    {
        FindAnyObjectByType<AudioManager>().Play(buttonPress, buttonPressVolume, "effects");
    }

    public void Achievements()
    {
        FindAnyObjectByType<AudioManager>().Play(buttonPress, buttonPressVolume, "effects");
    }

    public void Return()
    {
        FindAnyObjectByType<AudioManager>().Play(buttonPress, buttonPressVolume, "effects");
        optionsMenu.SetActive(false);
        helpMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void QuitGame()
    {
        FindAnyObjectByType<AudioManager>().Play(buttonPress, buttonPressVolume, "effects");
        Application.Quit();
    }
}
