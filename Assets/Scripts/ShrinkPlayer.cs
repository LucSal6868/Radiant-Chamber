using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class ShrinkPlayer : MonoBehaviour
{
    public float normalSize = 5f;
    public float shrinkSize = 1f;
    public float shrinkSpeed = 2f;
    public Transform chamberSpawnPoint;
    public Transform outsideSpawnPoint;
    public InputActionReference triggerAction;

    private bool isShrunk = false;
    private bool isTransitioning = false;
    private bool canInteractIn = false;
    private bool canInteractOut = false;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (triggerAction != null) triggerAction.action.Enable();
    }

    void Update()
    {
        bool triggerPressed = triggerAction != null && triggerAction.action.WasPressedThisFrame();
        bool pressed = Input.GetKeyDown(KeyCode.F) || triggerPressed;

        Debug.Log("triggerPressed: " + triggerPressed + " | F: " + Input.GetKeyDown(KeyCode.F));

        if (pressed && canInteractIn && !isShrunk)
        {
            isShrunk = true;
            isTransitioning = true;
        }

        if (pressed && canInteractOut && isShrunk)
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