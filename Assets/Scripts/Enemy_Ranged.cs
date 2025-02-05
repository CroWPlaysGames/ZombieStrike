using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Ranged : MonoBehaviour
{
    public int health;
    private int current_health;
    public int score;
    public Health_Zombie healthbar;
    public Slider slider;
    public float reload_time;
    private bool reloading = false;
    private float interval = 0f;
    public float attack_rate;
    public GameObject spit;
    public float spit_speed;
    private int mag_size = 1;
    public Transform gunsource;
    public int meleeDamage;
    [HideInInspector]
    public int spawn_location;
    public AudioSource hit;


    void Start()
    {
        current_health = health;
        healthbar.start_health(health);
    }

    public void Update()
    {
        if (current_health <= 0)
        {
            FindAnyObjectByType<HUD>().AddScore(score);
            GameObject.Find($"Spawner {spawn_location}").GetComponent<Spawner>().enemy_count += 1;
            Destroy(gameObject);
        }

        if (mag_size == 0 && !reloading)
        {
            StartCoroutine(Reload());
            return;
        }

        if (!reloading)
        {
            if (Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) < 5f)
            {
                Shoot();
            }
        }

        if (Time.time >= interval)
        {
            Physics2D.IgnoreCollision(GameObject.Find("Player").GetComponent<BoxCollider2D>(), gameObject.GetComponent<BoxCollider2D>(), false);
        }
    }

    void Shoot()
    {
        GameObject shoot = Instantiate(spit, gunsource.position, gunsource.rotation);
        Rigidbody2D projectile = shoot.GetComponent<Rigidbody2D>();
        projectile.AddForce(gunsource.up * spit_speed, ForceMode2D.Impulse);
        mag_size--;
    }

    IEnumerator Reload()
    {
        reloading = true;
        yield return new WaitForSeconds(reload_time);
        mag_size++;
        reloading = false;
    }

    public void TakeDamage(int damage)
    {
        current_health -= damage;
        healthbar.set_health(current_health);
        hit.Play();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject.Find("Player").GetComponent<PlayerController>().TakeDamage(meleeDamage);

            Physics2D.IgnoreCollision(GameObject.Find("Player").GetComponent<BoxCollider2D>(), gameObject.GetComponent<BoxCollider2D>());

            interval = Time.time + 10f / attack_rate;
        }

        if (collision.gameObject.tag == "Explosion")
        {
            hit.Play();

            GameObject boom = GameObject.FindGameObjectWithTag("Explosion");

            Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), boom.GetComponent<CircleCollider2D>());

            TakeDamage(60);
        }
    }
}
