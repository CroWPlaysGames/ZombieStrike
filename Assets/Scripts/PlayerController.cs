using System;
using System.Collections;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    [Header("General Stats")]
    public float health;
    [HideInInspector] public float currentHealth;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float sprintDuration;
    [HideInInspector] public float currentStamina;
    private float currentSpeed;
    private bool exhausted = false;
    [Header("Equipment Management")]
    [SerializeField] private Weapon equippedWeapon;
    [SerializeField] private Weapon spareWeapon;
    private SpriteRenderer weaponPosition;
    [SerializeField] private GameObject equipment;
    [SerializeField][Range(0,3)] private int equipmentAmount;
    [SerializeField] private GameObject grenade;
    [SerializeField][Range(0, 3)] private int grenadeAmount;
    [Header("Audio Management")]
    [SerializeField] private AudioClip hit;
    [SerializeField][Range(0f, 1f)] private float hitVolume;
    [SerializeField] private AudioClip useEquipment;
    [SerializeField][Range(0f, 1f)] private float useEquipmentVolume;
    [SerializeField] private AudioClip throwGrenade;
    [SerializeField][Range(0f, 1f)] private float throwGrenadeVolume;
    private Vector2 movement;
    private Vector2 direction;
    private Vector2 mousePosition;
    private HUD hud;


    void Start()
    {
        // Setup Weapon Visuals
        weaponPosition = GameObject.Find("Equipped Weapon").GetComponent<SpriteRenderer>();
        weaponPosition.sprite = equippedWeapon.weaponVisual;
        weaponPosition.transform.localPosition = equippedWeapon.weaponPosition;

        // Setup Equipped Items
        equippedWeapon.magSize = equippedWeapon.maxMagCapacity;
        equippedWeapon.ammoSize = equippedWeapon.maxAmmoCapacity;
        spareWeapon.magSize = spareWeapon.maxMagCapacity;
        spareWeapon.ammoSize = spareWeapon.maxAmmoCapacity;

        // Setup HUD
        currentHealth = health;
        currentSpeed = walkSpeed;
        hud = FindAnyObjectByType<HUD>();
        hud.SetMaxHealth(health);
        hud.SetMaxStamina(sprintDuration);
        currentStamina = sprintDuration;
        hud.UpdateHUD(equippedWeapon, spareWeapon, equipmentAmount, grenadeAmount);
    }

    void Update()
    {
        // Fetch Input Variables
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mousePosition = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
        
        // Shoot Equipped Weapon
        if (Input.GetButton("Fire1") && !hud.pauseMenu.activeSelf && !hud.gameOverMenu.activeSelf)
        {
            equippedWeapon.Shoot();
        }

        // Toggle Flashlight
        if (Input.GetKeyDown(KeyCode.F))
        {
            Light2D flashlight = GameObject.Find("Flashlight").GetComponent<Light2D>();

            switch (flashlight.enabled)
            {
                case true:
                    flashlight.enabled = false;
                    break;
                case false:
                    flashlight.enabled = true;
                    break;
            }
        }

        // Use Equipment Item
        if (Input.GetKeyDown(KeyCode.X))
        {
            CancelReload();
            UseEquipment();
        }

        // Throw a Grenade
        if (Input.GetKeyDown(KeyCode.G))
        {
            CancelReload();
            ThrowGrenade();
        }

        // Reload Equipped Weapon
        if (Input.GetKeyDown(KeyCode.R))
        {
            equippedWeapon.Reload();
        }

        // Switch Equipped Weapon with Spare Weapon
        if (Input.GetAxis("Mouse ScrollWheel") != 0 || Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapons(equippedWeapon, spareWeapon);
        }

        // Start Sprinting
        if (Input.GetKeyDown(KeyCode.LeftShift) && !exhausted)
        {
            currentSpeed = sprintSpeed;
            currentStamina -= Time.deltaTime*100;

            if (currentStamina <= 0)
            {
                StartCoroutine(Recover());
            }

            print(currentStamina);
        }

        // Stop Sprinting
        if (Input.GetKeyUp(KeyCode.LeftShift) || currentStamina.Equals(0))
        {
            currentSpeed = walkSpeed;
        }

        hud.UpdateHUD(equippedWeapon, spareWeapon, equipmentAmount, grenadeAmount);

        if (Input.GetKeyUp(KeyCode.LeftShift) && currentStamina < sprintDuration && !exhausted)
        {
            //currentStamina += Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().MovePosition(GetComponent<Rigidbody2D>().position + currentSpeed * Time.fixedDeltaTime * movement);
        direction = mousePosition - GetComponentInChildren<Rigidbody2D>().position;
        GetComponent<Rigidbody2D>().rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }

    private void SwitchWeapons(Weapon weapon1, Weapon weapon2)
    {
        CancelReload();
        weaponPosition.sprite = weapon2.weaponVisual;
        weaponPosition.transform.localPosition = weapon2.weaponPosition;
        equippedWeapon = weapon2;
        spareWeapon = weapon1;
    }

    private void UseEquipment()
    {
        if (equipmentAmount > 0 && currentHealth < 100)
        {
            currentHealth += 40;
            equipmentAmount--;
        }
    }

    private void ThrowGrenade()
    {
        if (grenadeAmount > 0)
        {
            GameObject liveGrenade = Instantiate(grenade, GameObject.Find("Gun Source").GetComponent<Transform>().position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            liveGrenade.GetComponent<Rigidbody2D>().AddForce(GameObject.Find("Gun Source").GetComponent<Transform>().up * 5, ForceMode2D.Impulse);
            grenadeAmount--;
        }
    }

    private void CancelReload()
    {
        equippedWeapon.reloading = false;
        Destroy(GameObject.Find("Reload Handler"));
        hud.CloseReload();
    }

    private IEnumerator Recover()
    {
        print("Exhausted");
        exhausted = true;
        yield return new WaitForSeconds(3);
        exhausted = false;
    }
}
