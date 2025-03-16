using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject explosion;
    [SerializeField] private float fuse;

    void Start()
    {
        StartCoroutine(Stop());
        StartCoroutine(Delay());
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject entity = collision.gameObject;

        if (!entity.tag.Equals("Player"))
        {
            GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
        }
    }

    IEnumerator Stop()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
        GetComponent<BoxCollider2D>().enabled = false;
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(fuse);
        Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(gameObject);
    }
}
