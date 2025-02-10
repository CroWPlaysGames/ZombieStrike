using UnityEngine;

public class Bullet : MonoBehaviour
{
    void Start()
    {     
        Destroy(gameObject, 2f);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject entity = collision.gameObject;

        GameObject bullet = GameObject.FindGameObjectWithTag("Bullet");

        if (entity.tag == "Enemy")
        {
            entity.GetComponent<Enemy>().TakeDamage(30);

            Destroy(gameObject);
        }

        else if (entity.tag == "Player")
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), GameObject.Find("Player").GetComponent<BoxCollider2D>());
        }

        else if (entity.tag == "Enemy Ranged")
        {
            entity.GetComponent<Enemy_Ranged>().TakeDamage(30);

            Destroy(gameObject);
        }

        else if (collision.gameObject.tag == "Bullet")
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), bullet.GetComponent<BoxCollider2D>());
        }

        else
        {
            Destroy(gameObject);
        }
    }
}