using UnityEngine;
using UnityEngine.Rendering.Universal;

[System.Serializable] public class LightSource
{
    public string name;
    public Light2D light1;
    public Light2D light2;
    public float duration;
    public bool toggle;
}