using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    [SerializeField] private SpriteRenderer interactIcon;
    [SerializeField] private Text inputText;
    [SerializeField] private BoxCollider2D doorCollider;
    private Input input;
    private bool playerProximity = false;
    private bool isOpened;
    private Animator animator;


    void Start()
    {
        input = FindAnyObjectByType<Input>();
        animator = GetComponent<Animator>();
    }
 
    void Update()
    {
        if (input.interact.WasPressedThisFrame() && playerProximity)
        {
            if (isOpened)
            {
                animator.ResetTrigger("open");
                animator.SetTrigger("close");
            }

            else
            {
                animator.ResetTrigger("close");
                animator.SetTrigger("open");
            }

            isOpened = !isOpened;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
        {
            doorCollider.enabled = true;
        }

        else
        {
            doorCollider.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactIcon.enabled = true;
            inputText.enabled = true;
            playerProximity = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactIcon.enabled = false;
            inputText.enabled = false;
            playerProximity = false;
        }
    }
}
