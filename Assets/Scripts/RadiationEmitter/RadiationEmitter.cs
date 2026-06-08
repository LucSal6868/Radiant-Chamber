using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ParticleSystem))]
public class RadiationEmitter : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    [Header("Path")]
    [SerializeField] private int length = 100;
    [SerializeField] private float scatter_frequency = 10f; // 1 in x change of scattering at each step
    [SerializeField] private float scatter_angle  = 25f; // angle of scattering in degrees
    [SerializeField] private float scatter_smoothness = 0.1f; 


    [Header("Trail")]
    [SerializeField] private int speed = 100;
    [SerializeField] private float drag = 0.1f;
    [SerializeField] private float particle_interval = 1f;
  


    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }
    
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            emit_radiation_particle();
        }
    }
    //----------------------------------------------------------------

    void emit_radiation_particle()
    {
        var trailPositions = CalculateTrailPositions();
        StartCoroutine(EmitTrailParticles(trailPositions));
    }

    List<Vector3> CalculateTrailPositions()
    {
        List<Vector3> positions = new List<Vector3>();

        Vector3 direction = Random.onUnitSphere;
        Vector3 targetDirection = direction;
        Vector3 currentPos = transform.position;
        float stepSize = particle_interval;
        float smoothSpeed = scatter_smoothness;

        int steps = Mathf.RoundToInt(length / stepSize);

        for (int i = 0; i < steps; i++)
        {
            if (scatter_frequency!= 0 && Random.value < 1f / scatter_frequency)
            {
                targetDirection = Quaternion.Euler(
                    Random.Range(-scatter_angle, scatter_angle),
                    Random.Range(-scatter_angle, scatter_angle),
                    Random.Range(-scatter_angle, scatter_angle)
                ) * direction;
            }

            direction = Vector3.Slerp(direction, targetDirection, smoothSpeed);
            currentPos += direction.normalized * stepSize;
            positions.Add(currentPos);
        }

        return positions;
    }

    IEnumerator EmitTrailParticles(List<Vector3> positions)
    {
        var emitParams = new ParticleSystem.EmitParams();
        int batchSize = speed;

        for (int i = 0; i < positions.Count; i += batchSize)
        {
            for (int j = i; j < Mathf.Min(i + batchSize, positions.Count); j++)
            {
                emitParams.position = positions[j];
                emitParams.applyShapeToPosition = false;
                _particleSystem.Emit(emitParams, 1);
            }
            batchSize = Mathf.Max(1, Mathf.RoundToInt(batchSize * drag));
            yield return null;
        }
    }
    //----------------------------------------------------------------  
}