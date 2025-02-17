using System.Collections;
using UnityEngine;

public class Magazine : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Drop());
        Destroy(gameObject, 5f);
    }

    private IEnumerator Drop()
    {
        yield return new WaitForSeconds(Random.Range(0.075f, 0.125f));
        GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
        GetComponent<BoxCollider2D>().enabled = false;        
    }
}