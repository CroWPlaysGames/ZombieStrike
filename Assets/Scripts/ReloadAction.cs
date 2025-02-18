using UnityEngine;

[System.Serializable]
public class ReloadAction
{
    public string name;
    public float startTime;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume;
    [HideInInspector] public AudioSource source;
}