using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Groups")]
    [SerializeField] private AudioMixerGroup music;
    [SerializeField] private AudioMixerGroup effects;
    [SerializeField] private AudioMixerGroup ambient;
    private float reloadTimer = 0;


    public void Play(AudioClip sound, float volume, string audiomixer)
    {
        StartCoroutine(PlaySound(sound, volume, audiomixer));
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

    private IEnumerator PlaySound(AudioClip sound, float volume, string audiomixer)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = sound;
        source.volume = volume;

        switch(audiomixer)
        {
            case "music":
                source.outputAudioMixerGroup = music;
                break;
            case "effects":
                source.outputAudioMixerGroup = effects;
                break;
            case "ambient":
                source.outputAudioMixerGroup = ambient;
                break;
        }

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
        action.source.outputAudioMixerGroup = effects;
        action.source.Play();
        yield return new WaitForSeconds(action.clip.length);
        Destroy(action.source);
    }
}
