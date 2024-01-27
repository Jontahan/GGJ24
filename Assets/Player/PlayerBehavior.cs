using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerBehavior : MonoBehaviour
{
    public float maxYawAngle = 90.0f;
    public float maxPitchAngle = 90.0f;
    public float mouseSensitivity = 1000.0f;

    private float pitch = 0.0f;
    private float yaw = 0.0f;

    private int drunkLevel = 0;
    private float[] drunkLevelSmoothing = { 14.0f, 7.0f, 3.7f, 0.7f };
    public TextMeshProUGUI drunkLevelText;

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
        drunkLevelText = GameObject.Find("DrunkLevelText").GetComponent<TextMeshProUGUI>();
    }

    private Vector2 smoothInput;

    private Vector2 GetSmoothedInput(Vector2 rawInput)
    {
        // Apply exponential moving average smoothing
        // 0.7, 3.7, 7.0, 14.0 is normal?
        smoothInput = Vector2.Lerp(smoothInput, rawInput, drunkLevelSmoothing[drunkLevel] * Time.deltaTime);
        return smoothInput;
    }

    void Update()
    {
        Vector2 rawInputHead = inputActions.Player.Head.ReadValue<Vector2>();
        Vector2 smoothInputHead = GetSmoothedInput(rawInputHead);
        float mouseX = smoothInputHead[0] * mouseSensitivity * Time.deltaTime;
        float mouseY = smoothInputHead[1] * mouseSensitivity * Time.deltaTime;


        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -maxPitchAngle, maxPitchAngle);

        yaw += mouseX;
        yaw = Mathf.Clamp(yaw, -maxYawAngle, maxYawAngle);

        Camera.main.transform.localRotation = Quaternion.Euler(pitch, yaw, 0);


        // DEBUG ONLY: Increase and decrease drunk level
        if (Input.GetMouseButtonDown(0))
        {
            if (drunkLevel < 3)
                drunkLevel++;
            drunkLevelText.text = "Level: " + drunkLevel.ToString();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (drunkLevel > 0)
                drunkLevel--;
            drunkLevelText.text = "Level: " + drunkLevel.ToString();
        }
    }
}