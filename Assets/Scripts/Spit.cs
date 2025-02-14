using UnityEngine;

public class Spit : MonoBehaviour
{
    [HideInInspector] public float damage;

    void Start()
    {
        Destroy(gameObject, 0.75f);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject entity = collision.gameObject;

        switch(entity.tag)
        {
            case "Player":
                entity.GetComponent<PlayerController>().TakeDamage(damage);
                break;
            default:
                break;
        }

        Destroy(gameObject);
    }

    public void SetDamage(float value)
    {
        damage = value;
    }
}
