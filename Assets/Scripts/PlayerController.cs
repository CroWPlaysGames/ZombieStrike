using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;

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
    public GameObject equippedWeapon;
    public GameObject spareWeapon;
    private SpriteRenderer weaponPosition;
    [SerializeField] private GameObject equipment;
    [SerializeField] private GameObject grenade;
    [SerializeField][Range(0, 3)] private int equipmentAmount;
    [SerializeField][Range(0, 3)] private int grenadeAmount;
    [Header("Audio Management")]
    [SerializeField] private AudioClip hitSound;
    [SerializeField][Range(0f, 1f)] private float hitVolume;
    [SerializeField] private AudioClip useEquipmentSound;
    [SerializeField][Range(0f, 1f)] private float useEquipmentVolume;
    [SerializeField] private AudioClip throwGrenadeSound;
    [SerializeField][Range(0f, 1f)] private float throwGrenadeVolume;
    private Vector2 movement;
    private Vector2 direction;
    private Vector2 mousePosition;
    private HUD hud;
    private UI ui;
    [SerializeField] private Input input;
    [HideInInspector] public float resistance = 0;
    [HideInInspector] public bool stimming = false;
    [Header("Animations")]
    private Animator animator;
    [HideInInspector] public bool isShooting;


    void Start()
    {
        // Setup Controls
        input = FindAnyObjectByType<Input>();
        input.ConfigureKeybinds();
        animator = GetComponent<Animator>();

        // Setup Weapon Visuals
        weaponPosition = GameObject.Find("Equipped Weapon").GetComponent<SpriteRenderer>();
        weaponPosition.sprite = equippedWeapon.GetComponent<Weapon>().weaponVisual;
        weaponPosition.transform.localPosition = equippedWeapon.GetComponent<Weapon>().weaponPosition;

        // Setup Equipped Items
        equippedWeapon.GetComponent<Weapon>().magSize = equippedWeapon.GetComponent<Weapon>().maxMagCapacity;
        equippedWeapon.GetComponent<Weapon>().ammoSize = equippedWeapon.GetComponent<Weapon>().maxAmmoCapacity;
        spareWeapon.GetComponent<Weapon>().magSize = spareWeapon.GetComponent<Weapon>().maxMagCapacity;
        spareWeapon.GetComponent<Weapon>().ammoSize = spareWeapon.GetComponent<Weapon>().maxAmmoCapacity;

        // Setup HUD
        currentHealth = health;
        currentStamina = sprintDuration;
        currentSpeed = walkSpeed;
        ui = FindAnyObjectByType<UI>();
        hud = FindAnyObjectByType<HUD>();
        hud.SetMaxValues(health, sprintDuration);
        hud.UpdateHUD(equippedWeapon, spareWeapon, equipmentAmount, grenadeAmount);
    }

    void Update()
    {   
        if (!ui.paused && !input.isMessaging)
        {
            if (input.shoot.IsInProgress())
            {
                equippedWeapon.GetComponent<Weapon>().Shoot();
            }

            if (!input.shoot.IsInProgress())
            {
                animator.SetBool("isShooting", false);
            }

            if (input.reload.WasPressedThisFrame())
            {
                equippedWeapon.GetComponent<Weapon>().Reload();
            }

            if (input.useEquipment.WasPressedThisFrame())
            {
                UseEquipment();
            }

            if (input.throwGrenade.WasPressedThisFrame())
            {
                ThrowGrenade();
            }

            if (input.shove.WasPressedThisFrame())
            {
                Shove();
            }

            if (input.flashlight.WasPressedThisFrame())
            {
                Light2D flashlight = GameObject.Find("Flashlight").GetComponent<Light2D>();
                if (flashlight.enabled)
                {
                    flashlight.enabled = false;
                }

                else
                {
                    flashlight.enabled = true;
                }
            }

            // Switch Equipped Weapon with Spare Weapon
            if (input.switchWeapons.WasPressedThisFrame())
            {
                SwitchWeapons(equippedWeapon, spareWeapon);
            }

            if (input.sprint.IsInProgress())
            {
                if (input.sprint.enabled)
                {
                    currentSpeed = sprintSpeed;
                    currentStamina -= Time.deltaTime;
                }

                if (currentStamina <= 0)
                {
                    input.sprint.Disable();
                    currentSpeed = walkSpeed;
                    Invoke(nameof(Recover), 2);
                }
            }

            if (!input.sprint.IsInProgress() && input.sprint.enabled && currentStamina < sprintDuration)
            {
                currentSpeed = walkSpeed;
                currentStamina += Time.deltaTime * 0.5f;
            }

            if (stimming)
            {
                input.sprint.Enable();
                currentStamina = sprintDuration;
            }
        }
    }

    void FixedUpdate()
    {
        // Manage Movement and Direction of Player
        movement = input.move.ReadValue<Vector2>();
        mousePosition = FindAnyObjectByType<Camera>().ScreenToWorldPoint(Mouse.current.position.ReadValue());
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

    private void SwitchWeapons(GameObject weapon1, GameObject weapon2)
    {
        CancelReload();
        weaponPosition.sprite = weapon2.GetComponent<Weapon>().weaponVisual;
        weaponPosition.transform.localPosition = weapon2.GetComponent<Weapon>().weaponPosition;
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
            GameObject liveGrenade = Instantiate(grenade, source.position, Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360)));
            liveGrenade.GetComponent<Rigidbody2D>().AddForce(source.up * 5, ForceMode2D.Impulse);
            grenadeAmount--;
        }
    }

    private void CancelReload()
    {
        equippedWeapon.GetComponent<Weapon>().reloading = false;
        Destroy(GameObject.Find("Reload Handler"));
        hud.CloseReload();
    }

    private void Recover()
    {
        input.sprint.Enable();
    }

    public void StartShooting()
    {
        animator.SetBool("isShooting", true);
    }

    public void StopShooting()
    {
        animator.SetBool("isShooting", false);
    }

    private void Shove()
    {
        animator.SetTrigger("shoveLarge");
    }
}
