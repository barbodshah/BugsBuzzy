using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    [Header("Mouse Settings")]
    public float mouseSensitivity = 100f;

    [Header("References")]
    public Transform playerBody;

    private float xRotation = 0f;

    void Start()
    {
    }

    void Update()
    {
        // Get mouse input
        float mouseX = FPSInputManager.Instance.mouseX * mouseSensitivity * Time.deltaTime;
        float mouseY = FPSInputManager.Instance.mouseY * mouseSensitivity * Time.deltaTime;

        // Rotate camera up/down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit camera to not flip

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate player body left/right
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
