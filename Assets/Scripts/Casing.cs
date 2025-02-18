using System.Collections;
using UnityEngine;

public class Casing : MonoBehaviour
{
    [SerializeField] private AudioClip landing;
    [SerializeField][Range(0f,1f)] private float landingVolume;


    void Start()
    {
        StartCoroutine(Stop());
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
        yield return new WaitForSeconds(Random.Range(0.075f, 0.125f));
        FindAnyObjectByType<AudioManager>().Play(landing, landingVolume);
        GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
