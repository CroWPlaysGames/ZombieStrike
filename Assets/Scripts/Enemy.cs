using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int health;
    private int current_health;
    public int score;
    public Health_Zombie healthbar;
    public Slider slider;
    private float interval = 0f;
    public float attack_rate;
    public int meleeDamage;
    [HideInInspector]
    public int spawn_location;
    public AudioSource hit;

    
    void Start()
    {        
        current_health = health;
        healthbar.start_health(health);
    }

    public void TakeDamage(int damage)
    {
        hit.Play();

        current_health -= damage;

        healthbar.set_health(current_health);
    }

    public void Update()
    {
        if (current_health <= 0)
        {
            FindAnyObjectByType<HUD>().AddScore(score);
            GameObject.Find($"Spawner {spawn_location}").GetComponent<Spawner>().enemy_count += 1;
            Destroy(gameObject);
        }

        if (Time.time >= interval)
        {
            Physics2D.IgnoreCollision(GameObject.Find("Player").GetComponent<BoxCollider2D>(), gameObject.GetComponent<BoxCollider2D>(), false);
        }
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
