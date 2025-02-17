using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class Weapon : MonoBehaviour
{
    [Header("General Stats")]
    [SerializeField] private float damage;
    [SerializeField] private float fireRate;
    private decimal fireInterval = 0;
    public float maxMagCapacity;
    public int maxAmmoCapacity;
    [SerializeField] private int bulletSpeed;
    [SerializeField] private float reloadTime;
    [HideInInspector] public bool reloading = false;
    [HideInInspector] public float magSize;
    [HideInInspector] public float ammoSize;
    [SerializeField] private float pellets;
    [SerializeField] private bool usesMagazine;
    [Header("Visual Management")]
    public Sprite weaponVisual;
    public Sprite weaponIcon;
    public Vector2 weaponPosition;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject casingPrefab;
    [SerializeField] private GameObject magazinePrefab;
    [Header("Sound Management")]
    [SerializeField] private AudioClip shoot;
    [SerializeField][Range (0f, 1f)] private float shootVolume;
    [SerializeField] private ReloadAction[] reloadAction;


    public void Shoot()
    {
        if (Time.time >= (float)fireInterval && magSize > 0)
        {
            if (!reloading)
            {
                FireBullet();
            }

            // Allows Shotguns and Revolvers to continue shooting while reloading
            else if (!usesMagazine)
            {
                FindAnyObjectByType<HUD>().CloseReload();
                Destroy(GameObject.Find("Reload Handler"));
                reloading = false;
                FireBullet();
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
            Transform eject = GameObject.Find("Eject").GetComponent<Transform>();
            GameObject emptyMagazine = Instantiate(magazinePrefab, eject.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            emptyMagazine.GetComponent<Rigidbody2D>().AddForce(eject.up * 3, ForceMode2D.Impulse);
        }
    }

    private IEnumerator Flash()
    {
        Light2D flash = GameObject.Find("Flash").GetComponent<Light2D>();
        flash.enabled = true;
        yield return new WaitForSeconds(0.05f);
        flash.enabled = false;
    }

    private void FireBullet()
    {
        FindAnyObjectByType<AudioManager>().Play(shoot, shootVolume);
        Transform source = GameObject.Find("Gun Source").GetComponent<Transform>();
        fireInterval = (decimal)(Time.time + 1f / fireRate);
        magSize--;

        for (int i = 0; i < pellets; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, source.position, source.rotation);
            bullet.GetComponent<Bullet>().SetDamage(damage);
            bullet.GetComponent<Rigidbody2D>().AddForce(source.up * bulletSpeed, ForceMode2D.Impulse);
            CoroutineHandler.Instance.StartCoroutine(Flash());
        }

        if (casingPrefab != null)
        {
            Transform eject = GameObject.Find("Eject").GetComponent<Transform>();
            GameObject casing = Instantiate(casingPrefab, eject.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            casing.GetComponent<Rigidbody2D>().AddForce(eject.up * 7, ForceMode2D.Impulse);
        }
    }
}