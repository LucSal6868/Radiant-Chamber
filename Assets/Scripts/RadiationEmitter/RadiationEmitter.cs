using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ParticleSystem))]
public class RadiationEmitter : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    [Header("Path")]
    [SerializeField] private float speed = 100f;
    [SerializeField] private float drag = 5f;

    [Header("Trail")]
    [SerializeField] private    
    float particle_interval = 1f;
  


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

        float distanceCovered = 0f;
        float nextEmitDistance = 0f;
        float t = 0f;
        float dt = 0.001f;
        Vector3 direction = Random.onUnitSphere;;

        float totalDistance = speed / drag;

        while (distanceCovered < totalDistance * 0.99f)
        {
            t += dt;

            distanceCovered = (speed / drag) * (1f - Mathf.Exp(-drag * t));

            if (distanceCovered >= nextEmitDistance)
            {
                Vector3 worldPos = transform.position + direction.normalized * distanceCovered;
                positions.Add(worldPos);
                nextEmitDistance += particle_interval;
            }
        }

        return positions;
    }

    IEnumerator EmitTrailParticles(List<Vector3> positions)
    {
        var emitParams = new ParticleSystem.EmitParams();
        int batchSize = 100; 

        for (int i = 0; i < positions.Count; i += batchSize)
        {
            for (int j = i; j < Mathf.Min(i + batchSize, positions.Count); j++)
            {
                emitParams.position = positions[j];
                emitParams.applyShapeToPosition = false;
                _particleSystem.Emit(emitParams, 1);
            }
            yield return null;
        }
    }
    //----------------------------------------------------------------  
}