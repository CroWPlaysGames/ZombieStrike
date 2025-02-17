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
    [HideInInspector] public bool exhausted = false;
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
    private UI ui;
    [HideInInspector] public float resistance = 0;
    [HideInInspector] public bool stimming = false;


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
        currentStamina = sprintDuration;
        currentSpeed = walkSpeed;
        hud = FindAnyObjectByType<HUD>();
        ui = FindAnyObjectByType<UI>();
        hud.SetMaxValues(health, sprintDuration);
        hud.UpdateHUD(equippedWeapon, spareWeapon, equipmentAmount, grenadeAmount);
    }

    void Update()
    {   
        if (!ui.paused)
        {
            // Shoot Equipped Weapon
            if (Input.GetButton("Fire1"))
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
                UseEquipment();
            }

            // Throw a Grenade
            if (Input.GetKeyDown(KeyCode.G))
            {
                ThrowGrenade();
            }

            // Reload Equipped Weapon
            if (Input.GetKeyDown(KeyCode.R))
            {
                equippedWeapon.Reload();
            }

            // Switch Equipped Weapon with Spare Weapon
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                SwitchWeapons(equippedWeapon, spareWeapon);
            }

            // Start Sprinting
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (!exhausted)
                {
                    currentSpeed = sprintSpeed;
                    currentStamina -= Time.deltaTime;
                }

                if (currentStamina <= 0)
                {
                    StartCoroutine(Recover());
                }
            }

            // Stop Sprinting
            if (Input.GetKeyUp(KeyCode.LeftShift) || currentStamina < 0)
            {
                currentSpeed = walkSpeed;
            }

            if (!Input.GetKey(KeyCode.LeftShift) && currentStamina < sprintDuration && !exhausted)
            {
                currentStamina += Time.deltaTime * 0.5f;
            }

            if (stimming)
            {
                exhausted = false;
                currentStamina = sprintDuration;
            }
        }
    }

    void FixedUpdate()
    {
        // Manage Movement and Direction of Player
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        print(movement); // Movement is causing issues with sprinting - not registering keys being let go
        mousePosition = FindAnyObjectByType<Camera>().ScreenToWorldPoint(Input.mousePosition);        
        GetComponent<Rigidbody2D>().MovePosition(GetComponent<Rigidbody2D>().position + currentSpeed * Time.fixedDeltaTime * movement);
        direction = mousePosition - GetComponent<Rigidbody2D>().position;
        GetComponent<Rigidbody2D>().rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        // Update HUD Info
        if (!ui.paused)
        {
            hud.UpdateHUD(equippedWeapon, spareWeapon, equipmentAmount, grenadeAmount);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage * (1 - resistance / 100);
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
        if (equipmentAmount > 0)
        {
            CancelReload();
            equipmentAmount--;
            switch (equipment.name)
            {
                case "Medkit":
                    equipment.GetComponent<Medkit>().UseMedkit();
                    break;
                case "Stim":
                    equipment.GetComponent<Stim>().UseStim();                    
                    break;
                default:
                    break;
            }
        }
    }

    private void ThrowGrenade()
    {
        if (grenadeAmount > 0)
        {
            CancelReload();
            Transform source = GameObject.Find("Gun Source").GetComponent<Transform>();
            GameObject liveGrenade = Instantiate(grenade, source.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            liveGrenade.GetComponent<Rigidbody2D>().AddForce(source.up * 5, ForceMode2D.Impulse);
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
        exhausted = true;
        currentSpeed = walkSpeed;
        yield return new WaitForSeconds(2);
        exhausted = false;
    }
}
