using UnityEngine;
using UnityEngine.UI;

public class Health_Zombie : MonoBehaviour
{
    public Slider slider;

    public Gradient gradient;
    public Image fill;

    public void start_health(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public void set_health(int health)
    {
        slider.value = health;
    }

    public void Update()
    {
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
