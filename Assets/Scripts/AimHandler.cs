using UnityEngine;

public class AimHandler : MonoBehaviour
{
    [Header("Lerp Settings")]
    public Vector3 offset = new Vector3(0, 0, 1f); // Relative offset when aiming
    public float lerpSpeed = 5f;

    private Vector3 originalPosition;
    private Vector3 targetPosition;
    public bool isAiming = false;

    void Start()
    {
        originalPosition = transform.localPosition;
        targetPosition = originalPosition;
    }

    void Update()
    {
        // Determine target position
        targetPosition = isAiming ? originalPosition + offset : originalPosition;

        // Smoothly move to target
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * lerpSpeed);
    }
    public void ToggleAim()
    {
        isAiming = !isAiming;
    }
}
