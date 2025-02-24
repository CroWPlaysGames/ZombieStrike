using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour
{
    private IEnumerator reloadAction;
    [SerializeField] private Image[] equipmentSlots;
    [SerializeField] private Image[] grenadeSlots;
    

    public void UpdateHUD(GameObject equippedWeapon, GameObject spareWeapon, int equipment, int grenades)
    {
        GameObject.Find("Weapon Name").GetComponent<Text>().text = equippedWeapon.name;
        GameObject.Find("Primary Icon").GetComponent<Image>().sprite = equippedWeapon.GetComponent<Weapon>().weaponIcon;
        GameObject.Find("Secondary Icon").GetComponent<Image>().sprite = spareWeapon.GetComponent<Weapon>().weaponIcon;
        GameObject.Find("Mag Capacity").GetComponent<Text>().text = equippedWeapon.GetComponent<Weapon>().magSize.ToString();
        GameObject.Find("Max Ammo").GetComponent<Text>().text = equippedWeapon.GetComponent<Weapon>().ammoSize.ToString();

        UpdateAmount(equipmentSlots, equipment, GameObject.Find("Equipment Icon").GetComponent<Image>());
        UpdateAmount(grenadeSlots, grenades, GameObject.Find("Grenade Icon").GetComponent<Image>());

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

    private void UpdateAmount(Image[] slots, int amount, Image icon)
    {
        icon.enabled = true;

        foreach (Image slot in slots)
        {
            slot.enabled = false;
        }

        switch (amount)
        {
            case 0:
                icon.enabled = false;
                break;
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

    public void SetMaxValues(float health, float stamina)
    {
        GameObject.Find("Health").GetComponent<Slider>().maxValue = health;
        GameObject.Find("Stamina").GetComponent<Slider>().maxValue = stamina;
    }
}