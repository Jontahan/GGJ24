using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public float maxYawAngle = 90.0f;
    public float maxPitchAngle = 90.0f;
    float mouseSensitivity = 1000.0f;

    private float pitch = 0.0f;
    private float yaw = 0.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() 
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -maxPitchAngle, maxPitchAngle);

        yaw += mouseX;
        yaw = Mathf.Clamp(yaw, -maxYawAngle, maxYawAngle);

        Camera.main.transform.localRotation = Quaternion.Euler(pitch, yaw, 0);
    }
}