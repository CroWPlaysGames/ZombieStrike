using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int health;
    private int currentHealth;
    [SerializeField] private int moveSpeed;
    private Vector2 movement;
    private Vector2 direction;
    private Vector2 mousePosition;
    private HUD hud;
    [SerializeField] private Weapon equippedWeapon;
    [SerializeField] private Weapon spareWeapon;
    private SpriteRenderer weaponPosition;


    void Start()
    {
        // Setup Health
        currentHealth = health;
        FindAnyObjectByType<Health_Player>().start_health(health);

        // Setup Weapon Visuals
        weaponPosition = GameObject.Find("Equipped Weapon").GetComponent<SpriteRenderer>();
        weaponPosition.sprite = equippedWeapon.weaponVisual;
        weaponPosition.transform.localPosition = equippedWeapon.weaponPosition;

        // Setup Weapons
        equippedWeapon.magSize = equippedWeapon.maxMagCapacity;
        equippedWeapon.ammoSize = equippedWeapon.maxAmmoCapacity;
        spareWeapon.magSize = spareWeapon.maxMagCapacity;
        spareWeapon.ammoSize = spareWeapon.maxAmmoCapacity;

        // Setup HUD Weapons
        hud = FindAnyObjectByType<HUD>();
        hud.UpdateHUD(equippedWeapon, spareWeapon);
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
            equippedWeapon.Shoot(GameObject.Find("Gun Source").GetComponent<Transform>());
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

        hud.UpdateHUD(equippedWeapon, spareWeapon);
    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().MovePosition(GetComponent<Rigidbody2D>().position + moveSpeed * Time.fixedDeltaTime * movement);
        direction = mousePosition - GetComponent<Rigidbody2D>().position;
        GetComponent<Rigidbody2D>().rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        FindAnyObjectByType<Health_Player>().set_health(currentHealth);
    }

    private void SwitchWeapons(Weapon weapon1, Weapon weapon2)
    {
        // Cancel Reload of Equipped Weapon
        equippedWeapon.reloading = false;
        Destroy(GameObject.Find("Reload Handler"));
        hud.CloseReload();

        // Update Weapon Visuals
        weaponPosition.sprite = weapon2.weaponVisual;
        weaponPosition.transform.localPosition = weapon2.weaponPosition;
        equippedWeapon = weapon2;
        spareWeapon = weapon1;
    }
}
