using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Zombie Prefabs")]
    [SerializeField] private GameObject[] zombies;
    [Header("Spawn Percentages")]
    [SerializeField][Range(0f, 1f)] private float normalZombieChance;
    [SerializeField][Range(0f, 1f)] private float rangedZombieChance;
    [SerializeField][Range(0f, 1f)] private float heavyZombieChance;
    [Header("Spawn Management")]
    [SerializeField] private int totalEnemyCount;
    [SerializeField] private float spawnInterval;
    [SerializeField] private int spawnNumber;
    [HideInInspector] public int currentEnemyCount = 0;
    [SerializeField] private Vector2 centre;
    [SerializeField] private Vector2 size;
    private float timer = 0;


    void Update()
    {
        if (currentEnemyCount != totalEnemyCount)
        {
            if (timer <= 0)
            {
                Spawn(Random.Range(0f,1f));
                timer += spawnInterval;
            }

            timer -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        Gizmos.DrawCube(centre, size);
    }

    private void Spawn(float number)
    {
        Vector2 position = centre + new Vector2(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2));
        currentEnemyCount++;

        if (number <= normalZombieChance)
        {
            zombies[0].GetComponent<Enemy>().spawnLocation = spawnNumber;
            Instantiate(zombies[0], position, Quaternion.identity);
        }

        else if (number <= normalZombieChance + rangedZombieChance)
        {
            zombies[1].GetComponent<EnemyRanged>().spawnLocation = spawnNumber;
            Instantiate(zombies[1], position, Quaternion.identity);
        }

        else if (number <= normalZombieChance + rangedZombieChance + heavyZombieChance)
        {
            zombies[2].GetComponent<Enemy>().spawnLocation = spawnNumber;
            Instantiate(zombies[2], position, Quaternion.identity);
        }
    }    
}
