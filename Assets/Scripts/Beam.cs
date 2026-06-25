using UnityEngine;

public class Beam : MonoBehaviour
{

    public float floatStrength = 15f;
    public float damping = 4f;
    public bool counteractGravity = true;

    public bool showGizmo = true;

    private Rigidbody currentTarget;

    [Header("Rotation")]
    public Vector3 spinAxis = Vector3.up;
    public float spinStrength = 5f;
    public float spinDamping = 1f;


    public Inspect inspectScript; // Reference to the Inspect script


    void OnTriggerEnter(Collider other)
    {
        inspectScript.FadeIn();
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;

        // Release
        ReleaseTarget();

        // Take the new one
        currentTarget = rb;
        currentTarget.gameObject.layer = LayerMask.NameToLayer("INSPECT");
        rb.useGravity = false;
    }

    void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null || rb != currentTarget) return;
        
        inspectScript.FadeOut();
        currentTarget.gameObject.layer = 0;
        currentTarget.useGravity = true;
        ReleaseTarget();
    }


    void FixedUpdate()
    {
        if (currentTarget == null) return;

        Vector3 toCenter = transform.position - currentTarget.position;
        currentTarget.AddForce(toCenter * floatStrength);
        currentTarget.AddForce(-currentTarget.linearVelocity * damping);

        if (counteractGravity)
            currentTarget.AddForce(-Physics.gravity * currentTarget.mass);

        // Spin
        Vector3 currentSpin = currentTarget.angularVelocity;
        Vector3 desiredSpin = spinAxis.normalized * spinStrength;
        currentTarget.AddTorque((desiredSpin - currentSpin) * spinDamping);
    }


    void ReleaseTarget()
    {
        if (currentTarget == null) return;
        currentTarget.useGravity = true;
        Vector3 randomPos = Random.insideUnitSphere * 5f;
        Vector3 launchVector = Vector3.Scale(randomPos, new Vector3(1f, 0f, 1f));
        currentTarget.AddForce(launchVector, ForceMode.VelocityChange);
        currentTarget = null;
    }

    void OnDisable() => ReleaseTarget();
    void OnDestroy() => ReleaseTarget();


    void OnDrawGizmos()
    {
        if (!showGizmo) return;
        Gizmos.color = new Color(0.2f, 0.6f, 1f, 0.25f);
        Gizmos.DrawSphere(transform.position, 1f);
        Gizmos.color = new Color(0.2f, 0.6f, 1f, 0.8f);
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}