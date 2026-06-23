using System.Collections;
using UnityEngine;

public class OpenRoof : MonoBehaviour
{
    private bool roof_open = false;
    private bool moving = false;

    [SerializeField] private GameObject roof1;
    [SerializeField] private GameObject roof2;
    [SerializeField] private MuonEmitter muon_emitter;
    [SerializeField] private GameObject open_sign;
    [SerializeField] private GameObject closed_sign;
    [SerializeField] private float slide_distance = 3f;
    [SerializeField] private float slide_speed = 2f;

    public void toggle_roof()
    {
        if (moving) return;
        StartCoroutine(SlideRoof());
    }

    IEnumerator SlideRoof()
    {
        moving = true;

        Vector3 roof1_start = roof1.transform.position;
        Vector3 roof2_start = roof2.transform.position;

        float direction = roof_open ? -1f : 1f;

        Vector3 roof1_target = roof1_start + Vector3.forward * slide_distance * direction;
        Vector3 roof2_target = roof2_start + Vector3.forward * slide_distance * -direction;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * slide_speed;
            roof1.transform.position = Vector3.Lerp(roof1_start, roof1_target, t);
            roof2.transform.position = Vector3.Lerp(roof2_start, roof2_target, t);
            yield return null;
        }

        roof1.transform.position = roof1_target;
        roof2.transform.position = roof2_target;

        roof_open = !roof_open;

        muon_emitter.set_emmiting(roof_open);
        open_sign.SetActive(roof_open);
        closed_sign.SetActive(!roof_open);

        moving = false;
    }
}