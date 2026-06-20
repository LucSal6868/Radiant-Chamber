using System.Collections;
using UnityEngine;

public class ChangeSize : MonoBehaviour
{
    private bool isSmall = false;
    private bool isChangingSize = false;

    public void ToggleSize()
    {
        if (!isChangingSize)
        {
            float targetScale = isSmall ? 1f : 0.1f;
            StartCoroutine(ChangeSizeCoroutine(targetScale));
        }
    }

    private IEnumerator ChangeSizeCoroutine(float scale)
    {
        isChangingSize = true;

        Vector3 startScale = transform.localScale;
        Vector3 targetScale = Vector3.one * scale;

        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / duration;

            transform.localScale = Vector3.Lerp(
                startScale,
                targetScale,
                t
            );

            yield return null;
        }

        transform.localScale = targetScale;

        isSmall = scale < 1f;
        isChangingSize = false;
    }
}