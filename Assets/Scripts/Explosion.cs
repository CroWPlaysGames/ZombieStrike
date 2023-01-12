using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AudioSource boom;

    public void Start()
    {
        boom.Play();

        StartCoroutine(Delay());
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        GameObject entity = collision.gameObject;

        if (entity.tag == "Player")
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<CircleCollider2D>(), entity.GetComponent<BoxCollider2D>());

            entity.GetComponent<PlayerController>().TakeDamage(40);
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }
}
