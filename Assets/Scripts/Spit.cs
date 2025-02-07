using UnityEngine;

public class Spit : MonoBehaviour
{
    public AudioSource sound;
    [SerializeField] private float duration;

    void Start()
    {
        sound.Play();

        Destroy(gameObject, duration);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject entity = collision.gameObject;

        if (collision.gameObject.tag == "Player")
        {
            entity.GetComponent<PlayerController>().TakeDamage(20);

            Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), entity.GetComponent<BoxCollider2D>());

            Destroy(gameObject);
        }
    }
}
