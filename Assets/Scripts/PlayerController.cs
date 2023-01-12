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

    public GameObject Assault_Rifle;
    public GameObject Shotgun;
    public GameObject Sniper_Rifle;
    public GameObject Grenade_Launcher;

    void Start()
    {
        camera.transform.position = new Vector3(body.position.x, body.position.y, -10f);

        current_health = health;
        healthbar.start_health(health);
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mouseposition = camera.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKeyDown("1"))
        {
            Assault_Rifle.SetActive(true);
            Shotgun.SetActive(false);
            Sniper_Rifle.SetActive(false);
            Grenade_Launcher.SetActive(false);
        }

        if (Input.GetKeyDown("2"))
        {
            Assault_Rifle.SetActive(false);
            Shotgun.SetActive(true);
            Sniper_Rifle.SetActive(false);
            Grenade_Launcher.SetActive(false);
        }

        if (Input.GetKeyDown("3"))
        {
            Assault_Rifle.SetActive(false);
            Shotgun.SetActive(false);
            Sniper_Rifle.SetActive(true);
            Grenade_Launcher.SetActive(false);
        }

        if (Input.GetKeyDown("4"))
        {
            Assault_Rifle.SetActive(false);
            Shotgun.SetActive(false);
            Sniper_Rifle.SetActive(false);
            Grenade_Launcher.SetActive(true);
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
}
