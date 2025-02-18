using UnityEngine;

public class Stim : MonoBehaviour
{
    [SerializeField][Range(0f, 100f)] private float resistanceAmount;
    [SerializeField] private float resistanceDuration;
    [SerializeField] private AudioClip inject;
    [SerializeField][Range(0f, 1f)] private float injectVolume;


    public void UseStim()
    {
        FindAnyObjectByType<PlayerController>().stimming = true;
        FindAnyObjectByType<PlayerController>().resistance += resistanceAmount;
        Invoke(nameof(Refresh), resistanceDuration);
        //FindAnyObjectByType<AudioManager>().Play(inject, injectVolume);
    }

    private void Refresh()
    {
        FindAnyObjectByType<PlayerController>().stimming = false;
        FindAnyObjectByType<PlayerController>().resistance -= resistanceAmount;
    }
}