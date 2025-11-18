using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway Settings")]
    public float swayAmount = 1.5f;        // How much the weapon moves
    public float maxSwayAmount = 3f;      // Maximum sway distance
    public float swaySmoothness = 8f;     // How smooth the sway is

    [Header("Sway Rotation Settings")]
    public float rotationSwayAmount = 3f; // Rotation sway amount
    public float maxRotationSway = 5f;   // Maximum rotation sway
    public float rotationSmoothness = 12f; // Smoothness of rotation sway

    private Vector3 initialPosition;      // The weapon's starting position
    private Quaternion initialRotation;  // The weapon's starting rotation

    InputManager inputManager;
    void Start()
    {
        // Store the initial position and rotation of the weapon
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;

        inputManager = FindObjectOfType<InputManager>();
    }

    void Update()
    {
        // Get mouse movement input
        //float mouseX = Input.GetAxis("Mouse X");
        //float mouseY = Input.GetAxis("Mouse Y");

        float mouseX = inputManager.mouseX;
        float mouseY = inputManager.mouseY;

        // Calculate sway position based on mouse movement
        Vector3 targetPosition = new Vector3(
            Mathf.Clamp(-mouseX * swayAmount, -maxSwayAmount, maxSwayAmount),
            Mathf.Clamp(-mouseY * swayAmount, -maxSwayAmount, maxSwayAmount),
            0f
        );

        // Smoothly interpolate the weapon's position
        transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition + targetPosition, Time.deltaTime * swaySmoothness);

        // Calculate rotation sway based on mouse movement
        Vector3 targetRotation = new Vector3(
            Mathf.Clamp(-mouseY * rotationSwayAmount, -maxRotationSway, maxRotationSway),
            Mathf.Clamp(mouseX * rotationSwayAmount, -maxRotationSway, maxRotationSway),
            0f
        );

        // Smoothly interpolate the weapon's rotation
        transform.localRotation = Quaternion.Lerp(transform.localRotation, initialRotation * Quaternion.Euler(targetRotation), Time.deltaTime * rotationSmoothness);
    }
}
