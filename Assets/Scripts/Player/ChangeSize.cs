using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;

public class ShrinkPlayer : MonoBehaviour
{
    public float normalSize = 5f;
    public float shrinkSize = 1f;
    public float shrinkSpeed = 2f;

    [SerializeField] private ContinuousMoveProvider moveProvider;

    private bool isShrunk = false;
    private bool isTransitioning = false;
    private CharacterController controller;
    private float baseMoveSpeed;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (moveProvider == null)
            moveProvider = FindObjectOfType<ContinuousMoveProvider>();
        if (moveProvider != null)
            baseMoveSpeed = moveProvider.moveSpeed;
    }

    void Update()
    {
        if (isTransitioning)
        {
            float targetSize = isShrunk ? shrinkSize : normalSize;
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(targetSize, targetSize, targetSize), shrinkSpeed * Time.deltaTime);

            UpdateMoveSpeed();

            if (Vector3.Distance(transform.localScale, new Vector3(targetSize, targetSize, targetSize)) < 0.01f)
            {
                transform.localScale = new Vector3(targetSize, targetSize, targetSize);
                isTransitioning = false;
                UpdateMoveSpeed();
            }
        }
    }

    void UpdateMoveSpeed()
    {
        if (moveProvider == null) return;
        float t = Mathf.InverseLerp(shrinkSize, normalSize, transform.localScale.x);
        moveProvider.moveSpeed = Mathf.Lerp(baseMoveSpeed * 0.75f, baseMoveSpeed, t);
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