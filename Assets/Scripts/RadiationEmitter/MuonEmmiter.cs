using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(BoxCollider))]
public class MuonEmitter : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private BoxCollider _boundingVolume;

    [SerializeField] private RadiationData data;
    [SerializeField] private bool active = true;

    private float _timer;
    private float _wait = 0f;

    public void set_emmiting(bool value)
    {
        active = value;
    }
    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _boundingVolume = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (!active) return;

        _timer += Time.deltaTime;

        if (_timer >= _wait)
        {
            _wait = Random.Range(data.interval.x, data.interval.y);
            _timer -= _wait;
            EmitRadiationParticle();
        }
    }

    Vector3 GetRandomTopPoint()
    {
        Bounds bounds = _boundingVolume.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        float y = bounds.max.y;

        return new Vector3(x, y, z);
    }

    bool IsInBounds(Vector3 pos)
    {
        return _boundingVolume.bounds.Contains(pos);
    }

    void EmitRadiationParticle()
    {
        var trailPositions = CalculateTrailPositions();
        StartCoroutine(EmitTrailParticles(trailPositions));
    }

    List<Vector3> CalculateTrailPositions()
    {
        List<Vector3> positions = new List<Vector3>();
        Vector3 direction = new Vector3(
            Random.Range(-data.scatter_angle, data.scatter_angle),
            -1f,
            Random.Range(-data.scatter_angle, data.scatter_angle)
        ).normalized;

        Vector3 currentPos = GetRandomTopPoint();

        var len = Random.Range(data.length_range.x, data.length_range.y);
        int steps = Mathf.RoundToInt(len / data.particle_interval);

        for (int i = 0; i < steps; i++)
        {
            currentPos += direction * data.particle_interval;
            if (!IsInBounds(currentPos)) break;
            positions.Add(currentPos);
        }

        return positions;
    }

    IEnumerator EmitTrailParticles(List<Vector3> positions)
    {
        var emitParams = new ParticleSystem.EmitParams();
        float batchSize = data.speed;
        int i = 0;

        while (i < positions.Count)
        {
            int count = Mathf.Max(1, Mathf.RoundToInt(batchSize));
            int end = Mathf.Min(i + count, positions.Count);

            for (int j = i; j < end; j++)
            {
                Vector3 travelDir = (positions[j] - (j > 0 ? positions[j - 1] : transform.position)).normalized;
                Vector3 noise = travelDir * Random.Range(-data.particle_noise, data.particle_noise);
                Vector3 finalPos = positions[j] + noise;

                if (!IsInBounds(finalPos)) continue;

                emitParams.position = finalPos;
                emitParams.applyShapeToPosition = false;
                _particleSystem.Emit(emitParams, 1);
            }

            i = end;
            batchSize = Mathf.Max(1f, batchSize * (1f - data.drag));
            yield return null;
        }
    }
}