using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float move_speed = 3f;

    public int health = 100;
    public int current_health;
    public Health_Player healthbar;

    public new Camera camera;
    public Rigidbody2D body;

    Vector2 movement;
    Vector2 mouseposition;
    Vector2 direction;

    public HUD hud;

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
        FindAnyObjectByType<HUD>().UpdateHUD(equippedWeapon, spareWeapon);

        camera.transform.position = new Vector3(body.position.x, body.position.y, -10f);

        current_health = health;
        healthbar.start_health(health);

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
        
        if (Input.GetButton("Fire1") && !hud.pause_menu.activeSelf)  // && game_over.activeSelf == false
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
        body.MovePosition(body.position + movement * move_speed * Time.fixedDeltaTime);

        direction = mouseposition - body.position;

        body.rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        camera.transform.position = new Vector3(body.position.x, body.position.y, -10f);
    }

    public void TakeDamage(int damage)
    {
        current_health -= damage;

        healthbar.set_health(current_health);
    }

    private void SwitchWeapons()
    {
        equippedWeapon.reloading = false;
        Destroy(GameObject.Find("Reload Handler"));
        FindAnyObjectByType<HUD>().CloseReload();

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
