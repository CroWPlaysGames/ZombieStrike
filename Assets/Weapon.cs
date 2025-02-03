using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Sprite weaponVisual;
    public Sprite weaponIcon;
    public Vector3 weaponPosition;

    public AudioClip shoot;
    public AudioClip reload;

    public GameObject bulletPrefab;

    public int maxAmmoCapacity;
    public int maxMagCapacity;
    public int damage;
    public int bulletSpeed;
    public int firerate;
    public int reloadTime;

    [HideInInspector]
    public int magSize;
}