using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class HUD : MonoBehaviour
{
    [HideInInspector]
    public int score;
    private int high_score;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    private bool paused = false;
    private Text currentScoreValue;
    private Text highScoreValue;
    private Image reload;
    private IEnumerator reloadAction;
    [SerializeField] private Image[] equipmentSlots;
    [SerializeField] private Image[] grenadeSlots;


    private void OnEnable()
    {
        score = 0;
        currentScoreValue = GameObject.Find("Current Score Value").GetComponent<Text>();
        highScoreValue = GameObject.Find("High Score Value").GetComponent<Text>();
        reload = GameObject.Find("Reload").GetComponent<Image>();

        currentScoreValue.text = score.ToString();
        highScoreValue.text = high_score.ToString();
    }

    public void AddScore(int reward)
    {
        score += reward;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);

        Time.timeScale = 1f;

        paused = false;
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);

        Time.timeScale = 0f;

        paused = true;
    }

    public void Restart_Game()
    {
        FindAnyObjectByType<Health_Player>().slider.value = FindAnyObjectByType<PlayerController>().health;

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
        highScoreValue.text = PlayerPrefs.GetInt("high_score").ToString();

        currentScoreValue.text = score.ToString();
        if (score > high_score)
        {
            high_score = score;

            highScoreValue.text = high_score.ToString();
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

    public void UpdateHUD(Weapon equippedWeapon, Weapon spareWeapon, int equipment, int grenades)
    {
        GameObject.Find("Weapon Name").GetComponent<Text>().text = equippedWeapon.name;
        GameObject.Find("Primary Icon").GetComponent<Image>().sprite = equippedWeapon.weaponIcon;
        GameObject.Find("Secondary Icon").GetComponent<Image>().sprite = spareWeapon.weaponIcon;
        GameObject.Find("Mag Capacity").GetComponent<Text>().text = equippedWeapon.magSize.ToString();
        GameObject.Find("Max Ammo").GetComponent<Text>().text = equippedWeapon.ammoSize.ToString();

        UpdateAmount(equipmentSlots, equipment);
        UpdateAmount(grenadeSlots, grenades);
    }

    public void StartReload(float duration)
    {
        reload.enabled = true;
        Color greyed = GameObject.Find("Primary Icon").GetComponent<Image>().color;
        greyed.a = 0.5f;
        GameObject.Find("Primary Icon").GetComponent<Image>().color = greyed;
        reloadAction = ReloadProgress(reload, duration);
        StartCoroutine(reloadAction);
    }

    public void CloseReload()
    {
        if (reload.enabled)
        {
            Color greyed = GameObject.Find("Primary Icon").GetComponent<Image>().color;
            greyed.a = 1f;
            GameObject.Find("Primary Icon").GetComponent<Image>().color = greyed;

            reload.fillAmount = 0;
            reload.enabled = false;

            StopCoroutine(reloadAction);
        }        
    }

    private IEnumerator ReloadProgress(Image reload, float duration)
    {
        float time = 0.0f;
        while (time < duration)
        {
            reload.fillAmount = time / Mathf.Max(duration, 0.01f);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        CloseReload();
    }

    private void UpdateAmount(Image[] slots, int amount)
    {
        foreach (Image slot in slots)
        {
            slot.enabled = false;
        }

        switch (amount)
        {
            case 1:
                slots[2].enabled = true;
                break;
            case 2:
                slots[1].enabled = true;
                slots[3].enabled = true;
                break;
            case 3:
                slots[0].enabled = true;
                slots[2].enabled = true;
                slots[4].enabled = true;
                break;
            default:
                break;
        }
    }
}