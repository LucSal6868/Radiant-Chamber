using UnityEngine;

[CreateAssetMenu(fileName = "NewRadiationData", menuName = "Radiation/Radiation Data")]
public class RadiationData : ScriptableObject
{
    // RANDOM LENGTH OF PARTICLE
    [Header("Particle")]
    public Vector2 length_range = new Vector2(50, 100);
    // 1 IN X CHANCE OF PARTICLE CHANGING DIRECTION
    public float scatter_frequency = 10f; 
    // ANGLE OF DIRECTION CHANGE
    public float scatter_angle  = 25f; 
    // SMOOTHNESS OF PARTCILE DIRECTION CHANGE (0-1, 0 = INSTANT, 1 = VERY SMOOTH)
    public float scatter_smoothness = 0.1f; 


    [Header("Trail")]
    // SPEED OF TRAIL DRAWING
    public int speed = 100;
    // EXPONETIAL SPEED DECAY
    public float drag = 0.01f;
    // DISTANCE BETWEEN EACH PARTICLE IN TRAIL
    public float particle_interval = 1f;
    // RANDOMNESS OF PARTICLE POSITION IN TRAIL
    public float particle_noise = 0.01f;

    [Header("Other")]
    public Vector2 interval = new Vector2(0.25f, 0.5f);

    [Header("Visuals")]
    public Color particleColor = Color.white;
    public float particleSize = 0.05f;
}
