using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public Sprite weaponVisual;
    public Sprite weaponIcon;
    public Vector3 weaponPosition;
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject magazinePrefab;
    public int maxAmmoCapacity;
    public float maxMagCapacity;
    public int bulletSpeed;
    public float fireRate;
    private decimal fireInterval = 0;
    public float reloadTime;
    [HideInInspector] public bool reloading = false;
    [HideInInspector] public float magSize;
    [HideInInspector] public float ammoSize;
    public float pellets;
    public bool usesMagazine;
    [Header("Sound Management")]
    [SerializeField] private AudioClip shoot;
    [SerializeField][Range (0f, 1f)] private float shootVolume;
    public ReloadAction[] reloadAction;


    public void Shoot(Transform bulletSource)
    {
        if (Time.time >= (float)fireInterval && magSize > 0)
        {
            if (!reloading)
            {
                fireInterval = (decimal)(Time.time + 1f / fireRate);

                FindAnyObjectByType<AudioManager>().Play(shoot, shootVolume);

                magSize--;

                for (int i = 0; i < pellets; i++)
                {
                    GameObject bullet = Instantiate(bulletPrefab, bulletSource.position, bulletSource.rotation);
                    Rigidbody2D projectile = bullet.GetComponent<Rigidbody2D>();
                    projectile.AddForce(bulletSource.up * bulletSpeed, ForceMode2D.Impulse);
                }

                if (casingPrefab != null)
                {
                    GameObject bulletCasing = Instantiate(casingPrefab, GameObject.Find("Eject").GetComponent<Transform>().position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
                    Rigidbody2D casing = bulletCasing.GetComponent<Rigidbody2D>();
                    casing.AddForce(GameObject.Find("Eject").GetComponent<Transform>().up * 7, ForceMode2D.Impulse);
                }
            }

            else if (!usesMagazine)
            {
                Destroy(GameObject.Find("Reload Handler"));
                reloading = false;

                fireInterval = (decimal)(Time.time + 1f / fireRate);

                FindAnyObjectByType<AudioManager>().Play(shoot, shootVolume);

                magSize--;

                for (int i = 0; i < pellets; i++)
                {
                    GameObject bullet = Instantiate(bulletPrefab, bulletSource.position, bulletSource.rotation);
                    Rigidbody2D projectile = bullet.GetComponent<Rigidbody2D>();
                    projectile.AddForce(bulletSource.up * bulletSpeed, ForceMode2D.Impulse);
                }

                FindAnyObjectByType<HUD>().CloseReload();
            }        
        }
    }

    public void Reload()
    {
        if (ammoSize != 0 &&  magSize != maxMagCapacity && !reloading)
        {
            CoroutineHandler.Instance.StartCoroutine(ReloadWeapon());

            CoroutineHandler.Instance.StartCoroutine(EjectMagazine());
        }
    }

    public IEnumerator ReloadWeapon()
    {
        reloading = true;

        if (usesMagazine)
        {
            FindAnyObjectByType<HUD>().StartReload(reloadTime);
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
                FindAnyObjectByType<AudioManager>().Play(reloadAction);
                FindAnyObjectByType<HUD>().StartReload(reloadTime / maxMagCapacity);
                yield return new WaitForSeconds(reloadTime / maxMagCapacity);

                magSize++;
                ammoSize--;
            }
            
        }
        
        reloading = false;
    }

    private IEnumerator EjectMagazine()
    {
        FindAnyObjectByType<AudioManager>().Play(reloadAction);
        yield return new WaitForSeconds(0.25f);

        if (magazinePrefab != null)
        {

            GameObject emptyMagazine = Instantiate(magazinePrefab, GameObject.Find("Eject").GetComponent<Transform>().position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            Rigidbody2D magazine = emptyMagazine.GetComponent<Rigidbody2D>();
            magazine.AddForce(GameObject.Find("Eject").GetComponent<Transform>().up * -5, ForceMode2D.Impulse);
        }
    }
}