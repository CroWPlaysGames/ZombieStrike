using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject HUD;
    [HideInInspector] public bool paused;


    void Start()
    {
        paused = false;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        HUD.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                ResumeGame();
            }

            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        HUD.SetActive(true);
        Time.timeScale = 1f;
        paused = false;
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
        HUD.SetActive(false);
        Time.timeScale = 0f;
        paused = true;
    }

    public void OptionsMenu()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        gameOverMenu.SetActive(true);
        paused = true;
        Time.timeScale = 0;
    }
}
