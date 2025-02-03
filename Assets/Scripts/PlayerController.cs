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
    public SpriteRenderer weaponPosition;
    public Transform bulletSource;


    void Start()
    {
        equippedWeapon = weaponSlot2;
        SwitchWeapons();

        camera.transform.position = new Vector3(body.position.x, body.position.y, -10f);

        current_health = health;
        healthbar.start_health(health);

        weaponSlot1.magSize = weaponSlot1.maxMagCapacity;
        weaponSlot1.ammoSize = weaponSlot1.maxAmmoCapacity;

        weaponSlot2.magSize = weaponSlot2.maxMagCapacity;
        weaponSlot2.ammoSize = weaponSlot2.maxMagCapacity;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mouseposition = camera.ScreenToWorldPoint(Input.mousePosition);
        
        if (Input.GetButton("Fire1") && Time.time >= equippedWeapon.fireInterval && equippedWeapon.magSize > 0
            && !hud.pause_menu.activeSelf)  // && game_over.activeSelf == false
        {
            equippedWeapon.Shoot(bulletSource);
        }

        if (Input.GetKeyDown("r") && equippedWeapon.magSize != equippedWeapon.maxMagCapacity && !equippedWeapon.reloading)
        {
            equippedWeapon.Reload();
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            SwitchWeapons();
        }

        UpdateHUD();
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
        
        if (equippedWeapon.Equals(weaponSlot1))
        {
            weaponPosition.sprite = weaponSlot2.weaponVisual;
            weaponPosition.transform.localPosition = weaponSlot2.weaponPosition;
            equippedWeapon = weaponSlot2;
        }

        else
        {
            weaponPosition.sprite = weaponSlot1.weaponVisual;
            weaponPosition.transform.localPosition = weaponSlot1.weaponPosition;
            equippedWeapon = weaponSlot1;
        }
    }

    private void UpdateHUD()
    {
        hud.weaponName.text = equippedWeapon.name;
        hud.primaryWeaponIcon.sprite = equippedWeapon.weaponIcon;
        hud.secondaryWeaponIcon.sprite = equippedWeapon.weaponIcon;
        hud.magCapacity.text = equippedWeapon.magSize.ToString();
        hud.maxAmmo.text = equippedWeapon.ammoSize.ToString();
    }
}
