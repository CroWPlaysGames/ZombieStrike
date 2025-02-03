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

    public GameObject weaponSlot1;
    public GameObject weaponSlot2;
    private GameObject equippedWeapon;
    public SpriteRenderer weaponPosition;


    void Start()
    {
        equippedWeapon = weaponSlot2;
        SwitchWeapons();

        camera.transform.position = new Vector3(body.position.x, body.position.y, -10f);

        current_health = health;
        healthbar.start_health(health);
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mouseposition = camera.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            SwitchWeapons();
        }
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
        if (equippedWeapon.Equals(weaponSlot1))
        {
            weaponPosition.sprite = weaponSlot2.GetComponent<Weapon>().weaponVisual;
            //weaponPosition.transform.position = weaponSlot2.GetComponent<Weapon>().weaponPosition;

            hud.weaponName.text = weaponSlot2.name;
            hud.primaryWeaponIcon.sprite = weaponSlot2.GetComponent<Weapon>().weaponIcon;
            hud.secondaryWeaponIcon.sprite = weaponSlot1.GetComponent<Weapon>().weaponIcon;
            hud.magCapacity.text = weaponSlot2.GetComponent<Weapon>().maxMagCapacity.ToString();
            hud.maxAmmo.text = weaponSlot2.GetComponent<Weapon>().maxAmmoCapacity.ToString();

            equippedWeapon = weaponSlot2;
        }

        else
        {
            weaponPosition.sprite = weaponSlot1.GetComponent<Weapon>().weaponVisual;
            //weaponPosition.transform.position = weaponSlot1.GetComponent<Weapon>().weaponPosition;

            hud.weaponName.text = weaponSlot1.name;
            hud.primaryWeaponIcon.sprite = weaponSlot1.GetComponent<Weapon>().weaponIcon;
            hud.secondaryWeaponIcon.sprite = weaponSlot2.GetComponent<Weapon>().weaponIcon;
            hud.magCapacity.text = weaponSlot1.GetComponent<Weapon>().maxMagCapacity.ToString();
            hud.maxAmmo.text = weaponSlot1.GetComponent<Weapon>().maxAmmoCapacity.ToString();

            equippedWeapon = weaponSlot1;
        }
    }
}
