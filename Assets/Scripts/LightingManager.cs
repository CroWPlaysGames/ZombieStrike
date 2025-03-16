using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightingManager : MonoBehaviour
{
    [SerializeField] private LightSource[] lights;


    void Start()
    {
        foreach (LightSource lightSet in lights)
        {
            if (lightSet.toggle)
            {
                StartCoroutine(FlickerLights(lightSet.light1, lightSet.light2, lightSet.duration));
            }

            else
            {
                StartCoroutine(SwitchLights(lightSet.light1, lightSet.light2, lightSet.duration));
            }
        }
    }

    private IEnumerator SwitchLights(Light2D light1, Light2D light2, float duration)
    {
        light1.enabled = false;
        light2.enabled = true;

        yield return new WaitForSeconds(duration);

        light1.enabled = true;
        light2.enabled = false;

        yield return new WaitForSeconds(duration);

        StartCoroutine(SwitchLights(light1, light2, duration));
    }

    private IEnumerator FlickerLights(Light2D light1, Light2D light2, float duration)
    {
        light1.enabled = true;
        light2.enabled = true;

        yield return new WaitForSeconds(duration);

        light1.enabled = false;
        light2.enabled = false;

        yield return new WaitForSeconds(duration);

        StartCoroutine(FlickerLights(light1, light2, duration));
    }
}
