using System.Collections;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public GameObject explosion;

    public void Start()
    {      
        StartCoroutine(Delay());
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject entity = collision.gameObject;
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (entity.tag == "Enemy")
        {
            Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);

            Destroy(gameObject);
        }

        else if (entity.tag == "Player")
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), player.GetComponent<BoxCollider2D>());
        }

        else
        {
            Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);

            Destroy(gameObject);
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f);

        Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);

        Destroy(gameObject);
    }
}
