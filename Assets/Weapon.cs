using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public enum WeaponType
    {
        fullAuto,
        burst,
        semiAuto,
        shotgun,
        projectile
    }

    public WeaponType weaponType = new();

    public Sprite weaponVisual;
    public Sprite weaponIcon;
    public Vector3 weaponPosition;

    public AudioClip shootSoundClip;
    public AudioClip reloadShoundClip;

    public GameObject bulletPrefab;

    public int maxAmmoCapacity;
    public int maxMagCapacity;
    public int damage;
    public int bulletSpeed;
    public int fireRate;
    [HideInInspector]
    public float fireInterval = 0f;
    public int reloadTime;
    public bool reloading = false;

    [HideInInspector]
    public int magSize;
    [HideInInspector]
    public int ammoSize;

    public void Shoot(Transform bulletSource)
    {
        fireInterval = Time.time + 1f / fireRate;

        gameObject.GetComponent<AudioSource>().clip = shootSoundClip;
        gameObject.GetComponent<AudioSource>().Play();

        magSize--;

        GameObject bullet = Instantiate(bulletPrefab, bulletSource.position, bulletSource.rotation);
        Rigidbody2D projectile = bullet.GetComponent<Rigidbody2D>();
        projectile.AddForce(bulletSource.up * bulletSpeed, ForceMode2D.Impulse);
    }

    public void Reload()
    {
        if (ammoSize != 0)
        {
            StartCoroutine(ReloadTimer());
            return;
        }
    }

    IEnumerator ReloadTimer()
    {
        reloading = true;

        gameObject.GetComponent<AudioSource>().clip = reloadShoundClip;
        gameObject.GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(reloadTime);

        magSize = ammoSize - maxMagCapacity;
        reloading = false;
    }
}