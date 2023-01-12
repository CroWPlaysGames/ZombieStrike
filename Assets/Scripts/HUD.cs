using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    // Link GameObjects to equip weapon and change colour based on which one is equipped
    public GameObject Assault_Rifle_Panel;
    public GameObject Assault_Rifle_Image;

    public GameObject Shotgun_Panel;
    public GameObject Shotgun_Image;

    public GameObject Sniper_Rifle_Panel;
    public GameObject Sniper_Rifle_Image;

    public GameObject Grenade_Launcher_Panel;
    public GameObject Grenade_Launcher_Image;

    public int score;
    public int high_score;

    public Text current_score;
    public Text highest_score;

    public GameObject pause_menu;
    private bool paused = false;

    public Health_Player health_restore;
    public PlayerController health;

    private void OnEnable()
    {
        score = 0;

        current_score.text = score.ToString();
        highest_score.text = high_score.ToString();
    }

    public void AddScore(int reward)
    {
        score += reward;
    }

    public void Resume()
    {
        pause_menu.SetActive(false);

        Time.timeScale = 1f;

        paused = false;
    }

    public void Pause()
    {
        pause_menu.SetActive(true);

        Time.timeScale = 0f;

        paused = true;
    }

    public void Restart_Game()
    {
        health_restore.slider.value = health.health;

        Time.timeScale = 1f;

        if(high_score > PlayerPrefs.GetInt("high_score"))
        {
            PlayerPrefs.SetInt("high_score", high_score);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Main_Menu()
    {
        Time.timeScale = 1f;

        if (high_score > PlayerPrefs.GetInt("high_score"))
        {
            PlayerPrefs.SetInt("high_score", high_score);
        }

        SceneManager.LoadScene("Main Menu");
    }

    void Update()
    {
        highest_score.text = PlayerPrefs.GetInt("high_score").ToString();

        current_score.text = score.ToString();
        if (score > high_score)
        {
            high_score = score;

            highest_score.text = high_score.ToString();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused == true)
            {
                Resume();
            }

            else
            {
                Pause();
            }
        }

        if (Input.GetKeyDown("1"))
        {
            // By default, the Assault Rifle is eqipped, so it is set to white by default           

            Assault_Rifle_Panel.GetComponent<Image>().color = Color.white;
            Assault_Rifle_Image.GetComponent<Image>().color = Color.white;

            Shotgun_Panel.GetComponent<Image>().color = Color.gray;
            Shotgun_Image.GetComponent<Image>().color = Color.gray;

            Sniper_Rifle_Panel.GetComponent<Image>().color = Color.gray;
            Sniper_Rifle_Image.GetComponent<Image>().color = Color.gray;

            Grenade_Launcher_Panel.GetComponent<Image>().color = Color.gray;
            Grenade_Launcher_Image.GetComponent<Image>().color = Color.gray;
        }

        if (Input.GetKeyDown("2"))
        {
            Assault_Rifle_Panel.GetComponent<Image>().color = Color.gray;
            Assault_Rifle_Image.GetComponent<Image>().color = Color.gray;

            Shotgun_Panel.GetComponent<Image>().color = Color.white;
            Shotgun_Image.GetComponent<Image>().color = Color.white;

            Sniper_Rifle_Panel.GetComponent<Image>().color = Color.gray;
            Sniper_Rifle_Image.GetComponent<Image>().color = Color.gray;

            Grenade_Launcher_Panel.GetComponent<Image>().color = Color.gray;
            Grenade_Launcher_Image.GetComponent<Image>().color = Color.gray;
        }

        if (Input.GetKeyDown("3"))
        {
            Assault_Rifle_Panel.GetComponent<Image>().color = Color.gray;
            Assault_Rifle_Image.GetComponent<Image>().color = Color.gray;

            Shotgun_Panel.GetComponent<Image>().color = Color.gray;
            Shotgun_Image.GetComponent<Image>().color = Color.gray;

            Sniper_Rifle_Panel.GetComponent<Image>().color = Color.white;
            Sniper_Rifle_Image.GetComponent<Image>().color = Color.white;

            Grenade_Launcher_Panel.GetComponent<Image>().color = Color.gray;
            Grenade_Launcher_Image.GetComponent<Image>().color = Color.gray;
        }

        if (Input.GetKeyDown("4"))
        {
            Assault_Rifle_Panel.GetComponent<Image>().color = Color.gray;
            Assault_Rifle_Image.GetComponent<Image>().color = Color.gray;

            Shotgun_Panel.GetComponent<Image>().color = Color.gray;
            Shotgun_Image.GetComponent<Image>().color = Color.gray;

            Sniper_Rifle_Panel.GetComponent<Image>().color = Color.gray;
            Sniper_Rifle_Image.GetComponent<Image>().color = Color.gray;

            Grenade_Launcher_Panel.GetComponent<Image>().color = Color.white;
            Grenade_Launcher_Image.GetComponent<Image>().color = Color.white;
        }
    }
}