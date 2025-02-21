using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AmmoResupply : MonoBehaviour
{
    [SerializeField] private float cooldown;
    [SerializeField][Range(0, 100)] private int supplyPercentage;
    private bool onCooldown = false;
    [SerializeField] private SpriteRenderer interactIcon;
    [SerializeField] private Text input;
    [SerializeField] private Image cooldownSlider;
    [SerializeField] private Image cooldownSliderBackground;
    [SerializeField] private AudioClip resupply;
    [SerializeField][Range(0f, 1f)] private float resupplyVolume;


    void Update()
    {
        if(Input.GetButtonDown("Interact"))
        {
            Weapon equippedWeapon = FindAnyObjectByType<PlayerController>().equippedWeapon;
            Weapon spareWeapon = FindAnyObjectByType<PlayerController>().spareWeapon;
            print(equippedWeapon.ammoSize);

            if (!onCooldown && (equippedWeapon.ammoSize < equippedWeapon.maxAmmoCapacity || spareWeapon.ammoSize < spareWeapon.maxAmmoCapacity))
            {
                StartCoroutine(AmmoReload());
                onCooldown = true;
                equippedWeapon.Rearm(supplyPercentage);
                spareWeapon.Rearm(supplyPercentage);
                FindAnyObjectByType<AudioManager>().Play(resupply, resupplyVolume, "effects");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!onCooldown)
            {
                interactIcon.enabled = true;
                input.enabled = true;
            }

            else
            {
                interactIcon.enabled = true;
                cooldownSlider.enabled = true;
                cooldownSliderBackground.enabled = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactIcon.enabled = false;
            input.enabled = false;
            cooldownSlider.enabled = false;
            cooldownSliderBackground.enabled = false;
        }
    }

    private IEnumerator AmmoReload()
    {
        input.enabled = false;
        cooldownSlider.enabled = true;
        cooldownSliderBackground.enabled = true;

        float time = 0.0f;
        while (time < cooldown)
        {
            cooldownSlider.fillAmount = time / Mathf.Max(cooldown, 0.01f);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        onCooldown = false;
    }
}
