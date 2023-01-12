using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    public GameObject main_menu;
    public GameObject how_to_play;

    public AudioSource button;

    public void Play_Game()
    {
        button.Play();

        SceneManager.LoadScene("Game");

        Time.timeScale = 1f;
    }

    public void How_To_Play()
    {
        button.Play();

        how_to_play.SetActive(true);

        main_menu.SetActive(false);
    }

    public void Return_To_Menu()
    {
        button.Play();

        how_to_play.SetActive(false);

        main_menu.SetActive(true);
    }

    public void Quit_Game()
    {
        button.Play();

        Application.Quit();
    }
}
