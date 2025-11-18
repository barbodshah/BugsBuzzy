using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Mouse Settings")]
    public float mouseSensitivity = 100f;

    [Header("Camera Rotation Constraints")]
    public float pitchMin = -90f; // Minimum angle for looking down
    public float pitchMax = 90f;  // Maximum angle for looking up
    //public float yawMin;
    //public float yawMax;

    private float pitch = 0f; // Rotation around the X-axis (up and down)
    private float yaw = 0f;   // Rotation around the Y-axis (left and right)


    void Awake()
    {
        pitch = 0;
        yaw = 90;
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    void Update()
    {
        // Get mouse movement input
        //float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        //float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        float mouseX = InputManager.Instance.mouseX * mouseSensitivity * Time.deltaTime;
        float mouseY = InputManager.Instance.mouseY * mouseSensitivity * Time.deltaTime;

        // Update yaw and pitch
        yaw += mouseX;
        pitch -= mouseY;

        // Clamp pitch to prevent over-rotation
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
        //yaw = Mathf.Clamp(yaw, yawMin, yawMax); 

        // Apply the rotation
        transform.eulerAngles = new Vector3(pitch, yaw, 0f);
    }
}
