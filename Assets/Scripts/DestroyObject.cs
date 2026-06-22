using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with: " + other.gameObject.name);
        if (other.GetComponent<RadiationEmitter>() != null)
        {
            Destroy(other.gameObject);
        }
    }

    
}
