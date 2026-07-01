using UnityEngine;

public class ShrinkPlayer : MonoBehaviour
{
    public float normalSize = 5f;
    public float shrinkSize = 1f;
    public float shrinkSpeed = 2f;

    private bool isShrunk = false;
    private bool isTransitioning = false;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (isTransitioning)
        {
            float targetSize = isShrunk ? shrinkSize : normalSize;
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(targetSize, targetSize, targetSize), shrinkSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.localScale, new Vector3(targetSize, targetSize, targetSize)) < 0.01f)
            {
                transform.localScale = new Vector3(targetSize, targetSize, targetSize);
                isTransitioning = false;
            }
        }
    }

    public void ToggleSize()
    {
        isShrunk = !isShrunk;
        isTransitioning = true;
    }

    public bool IsMovementAllowed()
    {
        return !isTransitioning;
    }
}