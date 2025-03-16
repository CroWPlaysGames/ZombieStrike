using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public float damage;


    void Start()
    {     
        Destroy(gameObject, 2f);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject entity = collision.gameObject;

        switch(entity.tag)
        {
            case "Player":
                Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), GameObject.Find("Player").GetComponentInChildren<BoxCollider2D>());
                break;
            case "Enemy":
                entity.GetComponentInParent<Enemy>().TakeDamage(damage);
                Destroy(gameObject);
                break;
            case "Enemy Ranged":
                entity.GetComponentInParent<EnemyRanged>().TakeDamage(damage);
                Destroy(gameObject);
                break;
            case "Bullet":
                Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }

    public void SetDamage(float value)
    {
        damage = value;
    }
}