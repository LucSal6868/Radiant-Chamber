using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ParticleSystem))]
public class RadiationEmitter : MonoBehaviour
{
    [SerializeField] private RadioactiveParticleData test_particle_data;
    private ParticleSystem ps;

    // ------------------------------------------------------------------------------

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        //create_particle(test_particle_data);
        //ps.Emit(new ParticleSystem.EmitParams {position = position}, 1);
    }

    private void Update()
{
    if (Keyboard.current.spaceKey.wasPressedThisFrame)
    {
        create_particle(test_particle_data);
    }

    if (particle != null)
    {
        traverse_particle();
    }
}

    // ------------------------------------------------------------------------------

    RadioactiveParticleData current_particle_data;
    class Particle
    {
        public Vector3 position;
        public Vector3 last_position;
        public Vector3 velocity;
        public float lifetime;
    }

    Particle particle;

    private void create_particle(RadioactiveParticleData data)
    {
        current_particle_data = data;
        particle = new Particle
        {
            position = transform.position,
            last_position = transform.position,
            velocity = data.direction.normalized * data.speed,
            lifetime = data.lifetime
        };

        
    }

    private ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
    private void traverse_particle()
    {
        particle.last_position = particle.position;

        particle.position += particle.velocity * Time.deltaTime;
        particle.velocity *= current_particle_data.drag;
        Debug.Log(particle.velocity.magnitude);

        Vector3 delta = particle.position - particle.last_position;
        float distance = delta.magnitude;

        if (distance < 0.001f)
        {
            particle = null;
            return;
        }

        Vector3 direction = delta / distance;
        
        float spacing = 1f;
        int steps = Mathf.FloorToInt(distance / spacing);

        if (steps <= 0)
            return;

        Vector3 start = particle.last_position;

        for (int i = 1; i <= steps; i++)
        {
            Vector3 spawnPos = start + direction * (i * spacing);

            emitParams.position = spawnPos;
            //emitParams.velocity = Vector3.zero;
            ps.Emit(emitParams, 1);
        }
    }
}   
