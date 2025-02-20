using System.Collections;
using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    [Header("General Stats")]
    [SerializeField] private float health;
    private float currentHealth;
    private float attackInterval = 0f;
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackSpeed;
    [Header("Ranged Attack Management")]
    [SerializeField] private int spitDamage;
    [SerializeField] private float spitRange;
    [SerializeField] private float spitSpeed;
    [SerializeField] private float reloadTime;
    [SerializeField] private GameObject projectile;
    [HideInInspector] public int spawnLocation;
    private Transform attackSource;
    private bool reloading = false;
    [Header("Visual Management")]
    [SerializeField] private Sprite[] zombieVariants;
    [Header("Audio Management")]
    [SerializeField] private AudioClip hit;
    [SerializeField][Range(0f, 1f)] private float hitVolume;
    [SerializeField] private AudioClip spit;
    [SerializeField][Range(0f, 1f)] private float spitVolume;


    void Start()
    {
        currentHealth = health;
        attackSource = transform.Find("Source");
        GetComponentInChildren<SpriteRenderer>().sprite = zombieVariants[Random.Range(0, zombieVariants.Length)];
    }

    public void Update()
    {
        if (currentHealth <= 0)
        {
            GameObject.Find($"Spawner {spawnLocation}").GetComponent<Spawner>().currentEnemyCount--;
            Destroy(gameObject);
        }

        if (!reloading)
        {
            if (Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) < spitRange)
            {
                FindAnyObjectByType<AudioManager>().Play(spit, spitVolume, "effects");
                GameObject shoot = Instantiate(projectile, attackSource.position, attackSource.rotation);
                shoot.GetComponent<Spit>().SetDamage(spitDamage);
                shoot.GetComponent<Rigidbody2D>().AddForce(attackSource.up * spitSpeed, ForceMode2D.Impulse);
                StartCoroutine(Reload());
            }
        }

        if (Time.time >= attackInterval)
        {
            Physics2D.IgnoreCollision(GameObject.Find("Player").GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>(), false);
        }
    }

    IEnumerator Reload()
    {
        reloading = true;
        yield return new WaitForSeconds(reloadTime);
        reloading = false;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject entity = collision.gameObject;

        switch (entity.tag)
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
        if (currentHealth > 0)
        {
            FindAnyObjectByType<AudioManager>().Play(hit, hitVolume, "effects");
        }
    }
}
