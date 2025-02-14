using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    private float reloadTimer = 0;


    public void Play(AudioClip sound, float volume)
    {
        StartCoroutine(PlaySound(sound, volume));
    }

    public void Play(ReloadAction[] reloadActions)
    {
        foreach (ReloadAction action in reloadActions)
        {
            reloadTimer += action.startTime;
            StartCoroutine(PlayReloadSounds(action));
        }

        reloadTimer = 0;
    }

    private IEnumerator PlaySound(AudioClip sound, float volume)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = sound;
        source.volume = volume;
        source.Play();
        yield return new WaitForSeconds(sound.length);
        Destroy(source);
    }

    private IEnumerator PlayReloadSounds(ReloadAction action)
    {
        yield return new WaitForSeconds(reloadTimer);
        action.source = gameObject.AddComponent<AudioSource>();
        action.source.clip = action.clip;
        action.source.volume = action.volume;
        action.source.Play();
        yield return new WaitForSeconds(action.clip.length);
        Destroy(action.source);
    }
}
