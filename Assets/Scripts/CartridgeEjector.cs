using UnityEngine;

public class CartridgeEjector : MonoBehaviour
{
    [Header("Setup")]
    public GameObject cartridgePrefab;       // The cartridge casing prefab
    public Transform ejectPoint;             // Position & direction to eject from
    public float ejectForce = 2f;
    public float lifetime = 5f;              // Time before cartridge auto-destroys

    public void Eject()
    {
        if (!cartridgePrefab || !ejectPoint) return;

        // Instantiate the cartridge
        GameObject cartridge = Instantiate(cartridgePrefab, ejectPoint.position, ejectPoint.rotation);

        // Apply physics
        Rigidbody rb = cartridge.GetComponent<Rigidbody>();
        if (rb)
        {
            // Apply randomized force & torque for natural behavior
            Vector3 forceDir = ejectPoint.right + ejectPoint.up * 0.3f + Random.insideUnitSphere * 0.1f;
            rb.AddForce(forceDir.normalized * ejectForce, ForceMode.Impulse);
        }

        // Auto-destroy after a delay
        Destroy(cartridge, lifetime);
    }
}
