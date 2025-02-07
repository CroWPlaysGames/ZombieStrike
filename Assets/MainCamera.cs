using UnityEngine;

public class MainCamera : MonoBehaviour
{  
    private Transform player;
    [SerializeField] private float cameraHeight;
    [SerializeField] private float lerpSpeed;
    private Vector3 offset;
    private Vector3 targetPosition;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        transform.position = new Vector3(player.position.x, player.position.y, cameraHeight);
        offset = transform.position - player.position;
    }

    void FixedUpdate()
    {
        targetPosition = player.position + offset;
        targetPosition.z = cameraHeight;
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);
    }
}
