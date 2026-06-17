using UnityEngine;

public class VRMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public Camera PlayerCamera;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 forward = PlayerCamera.transform.forward;
        forward.y = 0;
        forward = forward.normalized;

        Vector3 right = PlayerCamera.transform.right;
        right.y = 0;
        right = right.normalized;

        Vector3 direction = (forward * v) + (right * h);

        controller.Move(direction * moveSpeed * Time.deltaTime);
    }
}