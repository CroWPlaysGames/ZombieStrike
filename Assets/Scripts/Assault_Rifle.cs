using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Assault_Rifle : MonoBehaviour
{
    public int damage = 25;
    public float bullet_speed = 30f;
    public int ammo = 30;
    private int mag_size;

    public float firerate = 8f;
    private float interval = 0f;

    public float reload_time = 4f;
    private bool is_reloading = false;

    public Text AmmoClip;
    public Transform gunsource;
    public GameObject bullet_prefab;

    public GameObject pause_menu;
    public GameObject game_over;
    
    void Start()
    {
        AmmoClip.text = ammo.ToString();

        mag_size = ammo;
    }

    void OnEnable()
    {
        is_reloading = false;
    }

    void Update()
    {
        if (is_reloading)
            return;

        if (mag_size <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= interval && mag_size > 0 
            && pause_menu.activeSelf == false && game_over.activeSelf == false)
        {
            interval = Time.time + 1f / firerate;

            Shoot();
        }

        if (Input.GetKeyDown("r") && mag_size != ammo)
        {
            StartCoroutine(Reload());
            return;
        }
    }

    void Shoot()
    {
        Object.FindAnyObjectByType<AudioManager>().Play("AR15 Shoot");
        //bang.Play();

        mag_size--;
        AmmoClip.text = (mag_size).ToString();

        GameObject bullet = Instantiate(bullet_prefab, gunsource.position, gunsource.rotation);

        Rigidbody2D projectile = bullet.GetComponent<Rigidbody2D>();

        projectile.AddForce(gunsource.up * bullet_speed, ForceMode2D.Impulse);
    }

    IEnumerator Reload()
    {
        is_reloading = true;
        Object.FindAnyObjectByType<AudioManager>().Play("AR15 Reload");

        yield return new WaitForSeconds(reload_time);

        mag_size = ammo;
        AmmoClip.text = mag_size.ToString();
        is_reloading = false;
    }    
}
