using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    public Text weaponName;
    public Image primaryWeaponIcon;
    public Image secondaryWeaponIcon;
    public Text magCapacity;
    public Text maxAmmo;

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
    }

    public void UpdateHUD(Weapon equippedWeapon, Weapon spareWeapon)
    {
        weaponName.text = equippedWeapon.name;
        primaryWeaponIcon.sprite = equippedWeapon.weaponIcon;
        secondaryWeaponIcon.sprite = spareWeapon.weaponIcon;
        magCapacity.text = equippedWeapon.magSize.ToString();
        maxAmmo.text = equippedWeapon.ammoSize.ToString();
    }
}