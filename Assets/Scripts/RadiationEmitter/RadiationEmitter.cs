using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ParticleSystem))]
public class RadiationEmitter : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    [SerializeField] private RadiationData data;

    [SerializeField] private bool active = true;
    private float _timer;


    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }
    

    private float wait = 0f;
    void Update()
    {
        if (!active) return;

        _timer += Time.deltaTime;

        if (_timer >= wait)
        {   
            wait = Random.Range(data.interval.x, data.interval.y);
            _timer -= wait;
            emit_radiation_particle();
        }
    }

    void emit_radiation_particle()
    {
        var trailPositions = CalculateTrailPositions();
        StartCoroutine(EmitTrailParticles(trailPositions));
    }

    // GENERATE A LIST OF POSITIONS FOR THE PARTICLE TRAIL BASED ON THE RADIATION DATA SETTINGS
    List<Vector3> CalculateTrailPositions()
    {
        List<Vector3> positions = new List<Vector3>();

        Vector3 direction = Random.onUnitSphere;
        Vector3 targetDirection = direction;
        Vector3 currentPos = transform.position;

        var len = Random.Range(data.length_range.x, data.length_range.y);
        int steps = Mathf.RoundToInt(len / data.particle_interval);

        for (int i = 0; i < steps; i++)
        {   

            // SCATTER PARTICLE DIRECTION
            if (data.scatter_frequency != 0 && data.scatter_angle != 0 && Random.value < 1f / data.scatter_frequency)
            {
                targetDirection = Quaternion.Euler(
                    Random.Range(-data.scatter_angle, data.scatter_angle),
                    Random.Range(-data.scatter_angle, data.scatter_angle),
                    Random.Range(-data.scatter_angle, data.scatter_angle)
                ) * direction;
            }
            // CONTINUE STRAIGHT
            else
            {
                targetDirection = direction;
            }

            direction = Vector3.Slerp(direction, targetDirection, data.scatter_smoothness);
            currentPos += direction.normalized * data.particle_interval;
            positions.Add(currentPos);
        }

        Debug.Log(positions.Count);
        return positions;
    }

    // DRAW PARTICLE TRAIL OVER TIME BASED ON TRAIL GENERATED
    // WILL DRAW [speed] NUMBER OF PARTICLES PER FRAME, WITH EXPONENTIAL DECAY BASED ON [drag]
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
                Vector3 travelDir = (positions[j] - (j > 0 ? positions[j-1] : transform.position)).normalized;
                Vector3 noise = travelDir * Random.Range(-data.particle_noise, data.particle_noise);

                emitParams.position = positions[j] + noise;
                emitParams.applyShapeToPosition = false;
                _particleSystem.Emit(emitParams, 1);
            }

            i = end;
            batchSize = Mathf.Max(1f, batchSize * (1f - data.drag));
            yield return null;
        }
    }
}