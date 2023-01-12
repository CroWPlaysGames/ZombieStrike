using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Ranged : MonoBehaviour
{
    public int health = 100;
    public int current_health;

    public Health_Zombie healthbar;
    public Slider slider;
    public GameObject scoring;

    public float reload_time = 3f;
    private bool is_reloading = false;

    public float interval = 0f;
    public float attack_rate = 0.01f;
    public GameObject spit;
    public float spit_speed = 10f;
    private int mag_size = 1;
    public Transform gunsource;

    public GameObject player;

    public int spawn_location;

    public GameObject spawner1;
    public GameObject spawner2;
    public GameObject spawner3;
    public GameObject spawner4;

    public AudioSource hit;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        scoring = GameObject.FindGameObjectWithTag("HUD");

        current_health = health;
        healthbar.start_health(health);

        spawner1 = GameObject.Find("Spawner 1");
        spawner2 = GameObject.Find("Spawner 2");
        spawner3 = GameObject.Find("Spawner 3");
        spawner4 = GameObject.Find("Spawner 4");
    }

    public void TakeDamage(int damage)
    {
        current_health -= damage;

        healthbar.set_health(current_health);

        hit.Play();
    }

    public void Update()
    {
        if (slider.value == 0 || current_health <= 0)
        {
            scoring.GetComponent<HUD>().AddScore(150);

            if (spawn_location == 1)
            {
                spawner1.GetComponent<Spawner>().enemy_count += 1;
            }

            if (spawn_location == 2)
            {
                spawner2.GetComponent<Spawner>().enemy_count += 1;
            }

            if (spawn_location == 3)
            {
                spawner3.GetComponent<Spawner>().enemy_count += 1;
            }

            if (spawn_location == 4)
            {
                spawner4.GetComponent<Spawner>().enemy_count += 1;
            }

            Destroy(gameObject);
        }

        if (mag_size == 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (is_reloading == false && mag_size > 0)
        {
            if (Vector2.Distance(transform.position, player.transform.position) < 5f)
            {
                Shoot();
                mag_size--;
                is_reloading = true;
            }
        }
    }

    void Shoot()
    {

        GameObject shoot = Instantiate(spit, gunsource.position, gunsource.rotation);

        Rigidbody2D projectile = shoot.GetComponent<Rigidbody2D>();

        projectile.AddForce(gunsource.up * spit_speed, ForceMode2D.Impulse);
    }

    IEnumerator Reload()
    {
        is_reloading = true;

        yield return new WaitForSeconds(reload_time);

        mag_size = 1;

        is_reloading = false;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.GetComponent<PlayerController>().TakeDamage(10);

            Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), gameObject.GetComponent<BoxCollider2D>());

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
