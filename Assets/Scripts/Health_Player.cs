using UnityEngine;
using UnityEngine.UI;

public class Health_Player : MonoBehaviour
{
    public Slider slider;

    public GameObject pause_menu;
    public GameObject game_over;
    public GameObject HUD_pause;

    public Text score;
    public GameObject beaten;

    public void start_health(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void set_health(int health)
    {
        slider.value = health;
    }

    public void Update()
    {
        if(slider.value <= 0)
        {
            game_over.SetActive(true);

            if (HUD_pause.GetComponent<HUD>().score > PlayerPrefs.GetInt("high_score"))
            {
                beaten.SetActive(true);
            }

            score.text = HUD_pause.GetComponent<HUD>().score.ToString();

            Time.timeScale = 0f;
        }
    }
}
