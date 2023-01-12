using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Shotgun : MonoBehaviour
{
    public float bullet_speed = 30f;
    public int ammo = 6;
    private int mag_size;

    private float spread = 0f;

    public float firerate = 1f;
    private float interval = 0f;

    public float reload_time = 4f;
    private bool is_reloading = false;

    public Text AmmoClip;
    public Transform gunsource;
    public GameObject bullet_prefab;

    public AudioSource bang;
    public AudioSource reloading;

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

        if (Input.GetButton("Fire1") && Time.time >= interval && mag_size > 0)
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
        bang.Play();

        mag_size--;
        AmmoClip.text = (mag_size).ToString();

        while (spread != 6)
        {
            GameObject bullet = Instantiate(bullet_prefab, gunsource.position, gunsource.rotation);

            Rigidbody2D projectile = bullet.GetComponent<Rigidbody2D>();

            projectile.AddForce(gunsource.up * bullet_speed, ForceMode2D.Impulse);

            spread = spread + 1f;
        }

        spread = 0f;
    }

    IEnumerator Reload()
    {
        reloading.Play();        

        is_reloading = true;

        yield return new WaitForSeconds(0.6f);

        mag_size++;

        if (mag_size != 6)
        {
            reloading.Play();

            yield return new WaitForSeconds(0.6f);

            mag_size++;
        }

        if (mag_size != 6)
        {
            reloading.Play();

            yield return new WaitForSeconds(0.6f);

            mag_size++;
        }

        if (mag_size != 6)
        {
            reloading.Play();

            yield return new WaitForSeconds(0.6f);

            mag_size++;
        }

        if (mag_size != 6)
        {
            reloading.Play();

            yield return new WaitForSeconds(0.6f);

            mag_size++;
        }

        if (mag_size != 6)
        {
            reloading.Play();

            yield return new WaitForSeconds(0.6f);

            mag_size++;
        }

        mag_size = ammo;


        is_reloading = false;

        AmmoClip.text = mag_size.ToString();
    }
}