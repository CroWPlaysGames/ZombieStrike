using UnityEngine;

public class Medkit : MonoBehaviour
{
    [SerializeField] private float healthRestore;
    [SerializeField] private AudioClip heal;
    [SerializeField] private float healVolume;


    public void UseMedkit()
    {
        FindAnyObjectByType<PlayerController>().currentHealth += healthRestore;
        //FindAnyObjectByType<AudioManager>().Play(heal, healVolume);
    }
}