using System.Collections;
using UnityEngine;

public class Stim : MonoBehaviour
{
    [SerializeField][Range(0f, 100f)] private float resistanceAmount;
    [SerializeField] private float resistanceDuration;
    [SerializeField] private AudioClip inject;
    [SerializeField][Range(0f, 1f)] private float injectVolume;


    public void UseStim()
    {
        CoroutineHandler.Instance.StartCoroutine(Resist());
        //FindAnyObjectByType<AudioManager>().Play(inject, injectVolume);
    }

    private IEnumerator Resist()
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();
        player.stimming = true;
        player.resistance += resistanceAmount;
        yield return new WaitForSeconds(resistanceDuration);
        player.stimming = false;
        player.resistance -= resistanceAmount;
        CoroutineHandler.Instance.StopCoroutine(Resist());
    }
}