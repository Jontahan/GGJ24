using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public float maxYawAngle = 90.0f;
    public float maxPitchAngle = 90.0f;
    public float mouseSensitivity = 1000.0f;

    private float pitch = 0.0f;
    private float yaw = 0.0f;

    private GameControls inputActions;

    void Awake()
    {
        inputActions = new GameControls();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private Vector2 smoothInput;

    private Vector2 GetSmoothedInput(Vector2 rawInput)
    {
        // Apply exponential moving average smoothing
        // 0.7, 3.7, 7.0 
        smoothInput = Vector2.Lerp(smoothInput, rawInput, 7f * Time.deltaTime);
        return smoothInput;
    }

    void Update()
    {
        Vector2 rawInputHead = inputActions.Player.Head.ReadValue<Vector2>();
        Vector2 rawInputHead2 = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector2 smoothInputHead = GetSmoothedInput(rawInputHead);
        float mouseX = smoothInputHead[0] * mouseSensitivity * Time.deltaTime;
        float mouseY = smoothInputHead[1] * mouseSensitivity * Time.deltaTime;
        // float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        // float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;


        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -maxPitchAngle, maxPitchAngle);

        yaw += mouseX;
        yaw = Mathf.Clamp(yaw, -maxYawAngle, maxYawAngle);

        Camera.main.transform.localRotation = Quaternion.Euler(pitch, yaw, 0);
    }
}