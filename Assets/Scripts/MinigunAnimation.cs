using UnityEngine;

public class MinigunAnimation : MonoBehaviour
{
    // Rotation speed variables
    public float acceleration = 50f; // Units per second²
    public float maxSpeed = 300f;    // Maximum speed in degrees per second
    public float minimumSpinDuration = 1.0f; // Minimum duration to rotate after a tap
    public float deccelerationMultiplier = 2f;
    [HideInInspector]
    public float currentSpeed = 0f; // Current rotation speed
    private float spinTimer = 0f;    // Timer to track the minimum spin duration

    public Minigun weapon;

    // Update is called once per frame
    void Update()
    {
        // Check if the left mouse button is pressed or held
        if (InputManager.Instance.click && weapon.weaponTemp < 1)
        {
            // Reset the spin timer to the minimum spin duration
            spinTimer = minimumSpinDuration;
        }
        else if (InputManager.Instance.hold && weapon.weaponTemp < 1)
        {
            // Continue holding the button extends rotation naturally
            spinTimer = Mathf.Max(spinTimer, Time.deltaTime);
        }

        // Decrease spin timer when not holding the button
        if (!InputManager.Instance.hold || weapon.weaponTemp >= 1)
        {
            spinTimer = Mathf.Max(0f, spinTimer - Time.deltaTime);
        }

        // Determine rotation speed
        if (spinTimer > 0)
        {
            // Accelerate rotation speed up to the maximum speed
            currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);
        }
        else
        {
            // Decelerate rotation speed to zero
            currentSpeed = Mathf.Max(currentSpeed - acceleration * Time.deltaTime * deccelerationMultiplier, 0f);
        }

        // Apply rotation around Z-axis
        transform.Rotate(Vector3.forward, currentSpeed * Time.deltaTime);
    }
}
