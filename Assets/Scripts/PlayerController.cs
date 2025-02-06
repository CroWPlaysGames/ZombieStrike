using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int health;
    public float moveSpeed;
    private int current_health;
    public new Camera camera;
    private Vector2 movement;
    private Vector2 mouseposition;
    private Vector2 direction;
    private HUD hud;
    public Weapon weaponSlot1;
    public Weapon weaponSlot2;
    private Weapon equippedWeapon;
    private Weapon spareWeapon;
    public SpriteRenderer weaponPosition;
    public Transform bulletSource;


    void Start()
    {
        equippedWeapon = weaponSlot1;
        spareWeapon = weaponSlot2;
        weaponPosition.sprite = equippedWeapon.weaponVisual;
        weaponPosition.transform.localPosition = equippedWeapon.weaponPosition;
        hud = FindAnyObjectByType<HUD>();
        hud.UpdateHUD(equippedWeapon, spareWeapon);

        camera.transform.position = new Vector3(GetComponent<Rigidbody2D>().position.x, GetComponent<Rigidbody2D>().position.y, -10f);

        current_health = health;
        FindAnyObjectByType<Health_Player>().start_health(health);

        weaponSlot1.magSize = weaponSlot1.maxMagCapacity;
        weaponSlot1.ammoSize = weaponSlot1.maxAmmoCapacity;

        weaponSlot2.magSize = weaponSlot2.maxMagCapacity;
        weaponSlot2.ammoSize = weaponSlot2.maxAmmoCapacity;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mouseposition = camera.ScreenToWorldPoint(Input.mousePosition);
        
        if (Input.GetButton("Fire1") && !hud.pauseMenu.activeSelf && !hud.gameOverMenu.activeSelf)
        {
            equippedWeapon.Shoot(bulletSource);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            equippedWeapon.Reload();
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0 || Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapons();
        }

        hud.UpdateHUD(equippedWeapon, spareWeapon);
    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().MovePosition(GetComponent<Rigidbody2D>().position + movement * moveSpeed * Time.fixedDeltaTime);

        direction = mouseposition - GetComponent<Rigidbody2D>().position;

        GetComponent<Rigidbody2D>().rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        camera.transform.position = new Vector3(GetComponent<Rigidbody2D>().position.x, GetComponent<Rigidbody2D>().position.y, -10f);
    }

    public void TakeDamage(int damage)
    {
        current_health -= damage;
        FindAnyObjectByType<Health_Player>().set_health(current_health);
    }

    private void SwitchWeapons()
    {
        equippedWeapon.reloading = false;
        Destroy(GameObject.Find("Reload Handler"));
        hud.CloseReload();

        if (equippedWeapon.Equals(weaponSlot1))
        {
            weaponPosition.sprite = weaponSlot2.weaponVisual;
            weaponPosition.transform.localPosition = weaponSlot2.weaponPosition;
            equippedWeapon = weaponSlot2;
            spareWeapon = weaponSlot1;
        }

        else
        {
            weaponPosition.sprite = weaponSlot1.weaponVisual;
            weaponPosition.transform.localPosition = weaponSlot1.weaponPosition;
            equippedWeapon = weaponSlot1;
            spareWeapon = weaponSlot2;
        }
    }
}
