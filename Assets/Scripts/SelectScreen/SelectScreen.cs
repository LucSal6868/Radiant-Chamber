using UnityEngine;

public class SelectScreen : MonoBehaviour
{
    [SerializeField] private GameObject[] elementPrefabs;
    [SerializeField] private Transform roullete;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private BoxCollider BoundingVolume;
    [SerializeField] private float rotateDuration = 0.3f;

    private int index = 0;
    private Quaternion targetRotation;
    private float rotateTimer = 1f; // Start at 1 so it's considered "done"
    private bool isRotating = false;

    private void Start()
    {
        targetRotation = roullete.rotation;
    }

    private void Update()
    {
        if (isRotating)
        {
            rotateTimer += Time.deltaTime;
            float t = Mathf.Clamp01(rotateTimer / rotateDuration);
            t = t * t * (3f - 2f * t);
            roullete.rotation = Quaternion.Lerp(roullete.rotation, targetRotation, t);

            if (rotateTimer >= rotateDuration)
            {
                roullete.rotation = targetRotation;
                isRotating = false;
            }
        }
    }

    public void turn_right()
    {
        if (isRotating) return; 
        index = ((index - 1) % elementPrefabs.Length + elementPrefabs.Length) % elementPrefabs.Length;
        targetRotation *= Quaternion.Euler(0, -360f / elementPrefabs.Length, 0);
        rotateTimer = 0f;
        isRotating = true;
    }

    public void turn_left()
    {
        if (isRotating) return;
        index = (index + 1) % elementPrefabs.Length;
        targetRotation *= Quaternion.Euler(0, 360f / elementPrefabs.Length, 0);
        rotateTimer = 0f;
        isRotating = true;
    }

    public void select_element()
    {
        GameObject element = Instantiate(elementPrefabs[index], spawnPoint.position, Quaternion.identity);
        RadiationEmitter emitter = element.GetComponent<RadiationEmitter>();
        emitter.boundingVolume = BoundingVolume;
    }
}