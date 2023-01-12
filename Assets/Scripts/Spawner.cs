using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemy_normal;
    public GameObject enemy_heavy;
    public GameObject enemy_range;

    public int random;
    public int enemy_count = 25;

    public Vector2 centre;
    public Vector2 size;

    public float interval = 4;

    public int spawn_number;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        Gizmos.DrawCube(centre, size);
    }

    public void Spawn(int number)
    {
        Vector2 position = centre + new Vector2(Random.Range(-size.x / 2, size.x / 2),
            Random.Range(-size.y / 2, size.y / 2));

        if(number <= 16)
        {
            enemy_normal.GetComponent<Enemy>().spawn_location = spawn_number;

            Instantiate(enemy_normal, position, Quaternion.identity);            
        }

        else if (number == 17 || number == 18)
        {
            enemy_heavy.GetComponent<Enemy>().spawn_location = spawn_number;

            Instantiate(enemy_heavy, position, Quaternion.identity);
        }

        else
        {
            enemy_range.GetComponent<Enemy_Ranged>().spawn_location = spawn_number;

            Instantiate(enemy_range, position, Quaternion.identity);
        }
    }

    void Start()
    {
        Spawn(Random.Range(1, 21));
        enemy_count -= 1;
    }

    void Update()
    { 
        if (enemy_count > 0)
        {
            if (interval > 0)
            {
                interval -= Time.deltaTime;
            }

            else
            {
                Spawn(Random.Range(1, 21));

                interval = 4;
                enemy_count -= 1;
            }
        }        
    }
}
