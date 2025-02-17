using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("General Stats")]
    [SerializeField] private float health;
    [HideInInspector] public int spawnLocation;
    private float currentHealth;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackSpeed;
    private float attackInterval = 0f;
    [Header("Visual Management")]
    [SerializeField] private Sprite[] zombieVariants;
    [Header("Audio Management")]
    [SerializeField] private AudioClip hit;
    [SerializeField][Range(0f,1f)] private float hitVolume;
    private bool isHurt = false;


    void Start()
    {
        currentHealth = health;
        GetComponentInChildren<SpriteRenderer>().sprite = zombieVariants[Random.Range(0, zombieVariants.Length)];
    }    

    public void Update()
    {
        if (currentHealth <= 0)
        {
            GameObject.Find($"Spawner {spawnLocation}").GetComponent<Spawner>().currentEnemyCount--;
            Destroy(gameObject);
        }

        if (Time.time >= attackInterval)
        {
            Physics2D.IgnoreCollision(GameObject.Find("Player").GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>(), false);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject entity = collision.gameObject;

        switch(entity.tag)
        {
            case "Player":
                FindAnyObjectByType<PlayerController>().TakeDamage(attackDamage);
                Physics2D.IgnoreCollision(GameObject.Find("Player").GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
                attackInterval = Time.time + 10f / attackSpeed;
                break;
            case "Explosion":
                CircleCollider2D explosion = GameObject.FindGameObjectWithTag("Explosion").GetComponent<CircleCollider2D>();
                Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), explosion);
                TakeDamage(120);
                break;
            default:
                break;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth > 0 && !isHurt)
        {
            StartCoroutine(HurtCooldown());
            FindAnyObjectByType<AudioManager>().Play(hit, hitVolume);
        }
    }

    private IEnumerator HurtCooldown()
    {
        isHurt = true;
        yield return new WaitForSeconds(0.1f);
        isHurt = false;
    }
}
