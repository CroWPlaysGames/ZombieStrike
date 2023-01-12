using UnityEngine;

public class Sniper_Bullet : MonoBehaviour
{
    void Start()
    {       
        Destroy(gameObject, 2f);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject entity = collision.gameObject;
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (entity.tag == "Enemy")
        {
            entity.GetComponent<Enemy>().TakeDamage(60);
        }

        else if (entity.tag == "Player")
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), player.GetComponent<BoxCollider2D>());
        }

        else
        {
            Destroy(gameObject);
        }
    }
}