using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Mouse Settings")]
    public float mouseSensitivity = 100f;

    [Header("Camera Rotation Constraints")]
    public float pitchMin = -90f;
    public float pitchMax = 90f;

    private float pitch = 0f;
    private float yaw = 0f;

    public bool canMove = false;

    void Awake()
    {
        pitch = 0;
        yaw = 90;
    }

    void Update()
    {
        float mouseX = InputManager.Instance.mouseX * mouseSensitivity * Time.deltaTime;
        float mouseY = InputManager.Instance.mouseY * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;

        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        if (canMove)
        {
            transform.localEulerAngles = new Vector3(pitch, 0f, 0f);
            transform.parent.eulerAngles = new Vector3(0f, yaw, 0f);
        }
        else
        {
            transform.eulerAngles = new Vector3(pitch, yaw, 0f);
        }
    }
}
