using UnityEngine;

public class ShrinkPlayer : MonoBehaviour
{
    public float normalSize = 5f;
    public float shrinkSize = 1f;
    public float shrinkSpeed = 2f;
    public Transform chamberSpawnPoint;   // TeleportOutsideChamber sphere position (inside chamber)
    public Transform outsideSpawnPoint;   // TeleportInChamber sphere position (outside chamber)

    private bool isShrunk = false;
    private bool isTransitioning = false;
    private bool canInteractIn = false;
    private bool canInteractOut = false;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Trigger shrink (going IN)
        if (Input.GetKeyDown(KeyCode.F) && canInteractIn && !isShrunk)
        {
            isShrunk = true;
            isTransitioning = true;
        }

        // Trigger grow (going OUT)
        if (Input.GetKeyDown(KeyCode.F) && canInteractOut && isShrunk)
        {
            isShrunk = false;
            isTransitioning = true;
        }

        if (isTransitioning)
        {
            float targetSize = isShrunk ? shrinkSize : normalSize;
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(targetSize, targetSize, targetSize), shrinkSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.localScale, new Vector3(targetSize, targetSize, targetSize)) < 0.01f)
            {
                controller.enabled = false;
                transform.position = isShrunk ? chamberSpawnPoint.position : outsideSpawnPoint.position;
                controller.enabled = true;
                isTransitioning = false;
            }
        }
    }

    public bool IsMovementAllowed()
    {
        return !isTransitioning;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "TeleportInChamber") canInteractIn = true;
        if (other.name == "TeleportOutsideChamber") canInteractOut = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "TeleportInChamber") canInteractIn = false;
        if (other.name == "TeleportOutsideChamber") canInteractOut = false;
    }
}