using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarBehavior : MonoBehaviour
{
    public float acceleration = 1500f;
    public float steering = 300f;
    public float brakeForce = 3000f;
    private float currentBrakeForce = 0f;
    private float currentSteerAngle = 0f;
    private float currentSpeed;
    private bool isBraking;

    private Rigidbody rb;

    private GameControls inputActions;


    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelBL;
    public WheelCollider wheelBR;

    public Transform wheelFLTrans;
    public Transform wheelFRTrans;
    public Transform wheelBLTrans;
    public Transform wheelBRTrans;

    void Awake()
    {
        inputActions = new GameControls();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        inputActions.Car.Enable();
    }

    private void OnDisable()
    {
        inputActions.Car.Disable();
    }

    void FixedUpdate()
    {
        currentSpeed = rb.velocity.magnitude;

        isBraking = inputActions.Car.Braking.ReadValue<float>() > 0.5f; // more than 50% pressed on gamepad (i think)
        currentBrakeForce = isBraking ? brakeForce : 0f;

        float accelerationInput = inputActions.Car.Acceleration.ReadValue<float>();

        // Removed logic for checking for braking, reapply if needed
        rb.AddForce(transform.forward * accelerationInput * acceleration * Time.fixedDeltaTime);

        ApplyBraking();

        float steeringInput = inputActions.Car.Steering.ReadValue<float>();
        currentSteerAngle = steering * steeringInput;
        transform.Rotate(0, currentSteerAngle * Time.fixedDeltaTime, 0);
    }
    void Update()
    {
        wheelFLTrans.Rotate(0, wheelFL.rpm / 60 * 360 * Time.deltaTime, 0);
        wheelFRTrans.Rotate(0, wheelFR.rpm / 60 * 360 * Time.deltaTime, 0);
        wheelBLTrans.Rotate(0, wheelBL.rpm / 60 * 360 * Time.deltaTime, 0);
        wheelBRTrans.Rotate(0, wheelBR.rpm / 60 * 360 * Time.deltaTime, 0);

        // Log all outputs from input actions
        Debug.Log(inputActions.Car.Acceleration.ReadValue<float>());
        Debug.Log(inputActions.Car.Braking.ReadValue<float>());
        Debug.Log(inputActions.Car.Steering.ReadValue<float>());
    }
    private void ApplyBraking()
    {
        if (isBraking)
        {
            rb.AddForce(-transform.forward * currentBrakeForce * Time.fixedDeltaTime);
        }
    }
}