using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Radioactive Particle")]
public class RadioactiveParticleData : ScriptableObject
{
    [SerializeField] public Vector3 direction = Vector3.forward;
    [SerializeField] public float speed = 1f;
    [SerializeField] public float drag = 0.9f;
    [SerializeField] public float lifetime = 5f;
    [SerializeField] public Vector2 scatter_range = new Vector2(0.1f, 0.2f);
    [SerializeField] public float droplet_density = 0.75f;

    public void Init(Vector3 direction, float speed, float drag, float lifetime, Vector2 scatter_range, float droplet_density)
    {
        this.direction = direction;
        this.speed = speed;
        this.drag = drag;
        this.lifetime = lifetime;
        this.scatter_range = scatter_range;
        this.droplet_density = droplet_density;
    }
}
