using UnityEngine;
using System.Collections;

public class BiomeController : MonoBehaviour
{
    public static BiomeController instance;

    [Header("References")]
    public Camera targetCamera;

    [Header("Lerp Settings")]
    public Color day;
    public Color snow;
    public float lerpDuration = 2f;

    public ParticleSystem snowParticle;

    private Coroutine lerpRoutine;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartColorTransition(0);
        }
    }

    public void StartColorTransition(int targetBiome)
    {
        if (lerpRoutine != null)
            StopCoroutine(lerpRoutine);

        switch (targetBiome)
        {
            case 0:
                lerpRoutine = StartCoroutine(LerpColors(snow, lerpDuration));
                snowParticle.gameObject.SetActive(true);
                break;
            case 1:
                lerpRoutine = StartCoroutine(LerpColors(day, lerpDuration));
                snowParticle.gameObject.SetActive(false);
                break;
            default:
                lerpRoutine = StartCoroutine(LerpColors(snow, lerpDuration));
                snowParticle.gameObject.SetActive(true);
                break;
        }
    }

    private IEnumerator LerpColors(Color toColor, float duration)
    {
        Color startFogColor = RenderSettings.fogColor;
        Color startCamColor = targetCamera.backgroundColor;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            RenderSettings.fogColor = Color.Lerp(startFogColor, toColor, t);
            targetCamera.backgroundColor = Color.Lerp(startCamColor, toColor, t);

            yield return null;
        }

        RenderSettings.fogColor = toColor;
        targetCamera.backgroundColor = toColor;
        lerpRoutine = null;
    }

}
