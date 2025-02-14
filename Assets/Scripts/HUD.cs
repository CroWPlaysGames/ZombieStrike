using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class HUD : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    private bool paused = false;
    private IEnumerator reloadAction;
    [SerializeField] private Image[] equipmentSlots;
    [SerializeField] private Image[] grenadeSlots;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                Resume();
            }

            else
            {
                Pause();
            }
        }
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
        SetMaxHealth(FindAnyObjectByType<PlayerController>().health);

        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Main_Menu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("Main Menu");
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

        GameObject.Find("Health").GetComponent<Slider>().value = FindAnyObjectByType<PlayerController>().currentHealth;
        GameObject.Find("Stamina").GetComponent<Slider>().value = FindAnyObjectByType<PlayerController>().currentStamina;
    }

    public void StartReload(float duration)
    {
        GameObject.Find("Reload").GetComponent<Image>().enabled = true;
        Color greyed = GameObject.Find("Primary Icon").GetComponent<Image>().color;
        greyed.a = 0.5f;
        GameObject.Find("Primary Icon").GetComponent<Image>().color = greyed;
        reloadAction = ReloadProgress(GameObject.Find("Reload").GetComponent<Image>(), duration);
        StartCoroutine(reloadAction);
    }

    public void CloseReload()
    {
        if (GameObject.Find("Reload").GetComponent<Image>().enabled)
        {
            Color greyed = GameObject.Find("Primary Icon").GetComponent<Image>().color;
            greyed.a = 1f;
            GameObject.Find("Primary Icon").GetComponent<Image>().color = greyed;

            GameObject.Find("Reload").GetComponent<Image>().fillAmount = 0;
            GameObject.Find("Reload").GetComponent<Image>().enabled = false;

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

    public void SetMaxHealth(float value)
    {
        GameObject.Find("Health").GetComponent<Slider>().maxValue = value;
    }

    public void SetMaxStamina(float value)
    {
        GameObject.Find("Stamina").GetComponent<Slider>().maxValue = value;
    }

    private void GameOver()
    {
        gameOverMenu.SetActive(true);
        Time.timeScale = 0;
    }
}