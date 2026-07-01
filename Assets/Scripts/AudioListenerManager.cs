using UnityEngine;

[DefaultExecutionOrder(-100)]
public class AudioListenerManager : MonoBehaviour
{
    void Awake()
    {
        var listeners = FindObjectsOfType<AudioListener>(true);
        if (listeners.Length <= 1) return;

        AudioListener keep = null;
        foreach (var l in listeners)
        {
            var cam = l.GetComponent<Camera>();
            if (cam != null && cam.CompareTag("MainCamera"))
            {
                keep = l;
                break;
            }
        }
        if (keep == null) keep = listeners[0];

        foreach (var l in listeners)
        {
            if (l != keep)
                l.enabled = false;
        }
    }
}
