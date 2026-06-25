using System.Collections;
using UnityEngine;

public class Inspect : MonoBehaviour
{
    [Header("Camera")]
    public Camera targetCamera;

    [Header("Clip Distances")]
    public float farDistance = 100f;
    public float inspectDistance = 0.3f;

    [Header("Settings")]
    public float lerpDuration = 1f;

    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(LerpFarClip(targetCamera.farClipPlane, inspectDistance));
    }

    public void FadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(LerpFarClip(targetCamera.farClipPlane, farDistance));
    }

    private IEnumerator LerpFarClip(float from, float to)
    {
        float elapsed = 0f;

        while (elapsed < lerpDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / lerpDuration;

            // Cubic ease-out
            t = 1f - Mathf.Pow(1f - t, 3f);

            targetCamera.farClipPlane = Mathf.Lerp(from, to, t);

            yield return null;
        }

        targetCamera.farClipPlane = to;
    }
}