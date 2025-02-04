using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    /*
    public enum WeaponType
    {
        fullAuto,
        burst,
        semiAuto,
        shotgun,
        projectile
    }

    public WeaponType weaponType = new();
    */
    public Sprite weaponVisual;
    public Sprite weaponIcon;
    public Vector3 weaponPosition;

    public GameObject bulletPrefab;

    public int maxAmmoCapacity;
    public float maxMagCapacity;
    public int bulletSpeed;
    public float fireRate;
    private decimal fireInterval = 0;
    public float reloadTime;
    [HideInInspector]
    public bool reloading = false;
    [HideInInspector]
    public float magSize;
    [HideInInspector]
    public float ammoSize;
    public float pellets;
    public bool usesMagazine;


    public void Shoot(Transform bulletSource)
    {
        if (Time.time >= (float)fireInterval && magSize > 0)
        {
            if (!reloading)
            {
                fireInterval = (decimal)(Time.time + 1f / fireRate);

                FindAnyObjectByType<AudioManager>().Play($"{name} Shoot");

                magSize--;

                for (int i = 0; i < pellets; i++)
                {
                    GameObject bullet = Instantiate(bulletPrefab, bulletSource.position, bulletSource.rotation);
                    Rigidbody2D projectile = bullet.GetComponent<Rigidbody2D>();
                    projectile.AddForce(bulletSource.up * bulletSpeed, ForceMode2D.Impulse);
                }
            }

            else if (!usesMagazine)
            {
                Destroy(GameObject.Find("Reload Handler"));
                reloading = false;

                fireInterval = (decimal)(Time.time + 1f / fireRate);

                FindAnyObjectByType<AudioManager>().Play($"{name} Shoot");

                magSize--;

                for (int i = 0; i < pellets; i++)
                {
                    GameObject bullet = Instantiate(bulletPrefab, bulletSource.position, bulletSource.rotation);
                    Rigidbody2D projectile = bullet.GetComponent<Rigidbody2D>();
                    projectile.AddForce(bulletSource.up * bulletSpeed, ForceMode2D.Impulse);
                }
            }        
        }        
    }

    public void Reload()
    {
        if (ammoSize != 0 &&  magSize != maxMagCapacity && !reloading)
        {
            CoroutineHandler.Instance.StartCoroutine(ReloadWeapon());
        }
    }

    public IEnumerator ReloadWeapon()
    {
        reloading = true;

        if (usesMagazine)
        {
            FindAnyObjectByType<AudioManager>().Play($"{name} Reload");
            yield return new WaitForSeconds(reloadTime);

            if (ammoSize <= maxMagCapacity)
            {
                magSize = ammoSize;
                ammoSize = 0;
            }

            else
            {
                ammoSize += magSize - maxMagCapacity;
                magSize = maxMagCapacity;
            }
        }

        else
        {
            while ((maxMagCapacity - magSize) > 0 && ammoSize > 0)
            {
                FindAnyObjectByType<AudioManager>().Play($"{name} Reload");
                yield return new WaitForSeconds(reloadTime / maxMagCapacity);

                magSize++;
                ammoSize--;
            }
            
        }
        
        reloading = false;
    }
}