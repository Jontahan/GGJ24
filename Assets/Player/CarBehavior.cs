using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarBehavior : MonoBehaviour
{
    // Car config
    public float[] gearForces = {0f, 0f, 0f};
    public float[] gearingThresholds = {0f, 0f, 0f};
    public float autoGearTime = 0f;
    public float steering = 300f;
    public float brakeForce = 3000f;
    
    // Internal engine variables
    private float currentBrakeForce = 0f;
    private float currentSteerAngle = 0f;
    private float targetSteerAngle = 0f;
    public float currentSpeed;
    private bool isBraking;
    private Rigidbody rb;
    public int currentGear = 0;
    private float currentAcceleration = 0f;
    private float gearTimer = 0f;

    private GameControls inputActions;

    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelBL;
    public WheelCollider wheelBR;
    public Transform wheelFLTrans;
    public Transform wheelFRTrans;
    public Transform wheelBLTrans;
    public Transform wheelBRTrans;
    public Transform steeringWheel;
    
    void Awake()
    {
        inputActions = new GameControls();
    }
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentAcceleration = gearForces[currentGear];
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
        UpdateGearing();
        currentSpeed = rb.velocity.magnitude;
        // Debug.Log(currentSpeed + " --- " + currentGear);

        isBraking = inputActions.Car.Braking.ReadValue<float>() > 0.5f; // more than 50% pressed on gamepad (i think)
        currentBrakeForce = isBraking ? brakeForce : 0f;

        float accelerationInput = inputActions.Car.Acceleration.ReadValue<float>();

        rb.AddForce(transform.forward * accelerationInput * currentAcceleration * Time.fixedDeltaTime);


        ApplyBraking();

        float steeringInput = inputActions.Car.Steering.ReadValue<float>();
        targetSteerAngle = steering * steeringInput;
        
        currentSteerAngle = Mathf.Lerp(currentSteerAngle, targetSteerAngle, Time.deltaTime * 2);
        currentSteerAngle *= Mathf.Clamp(currentSpeed / 7f, 0, 1);
        
        steeringWheel.Rotate(0, targetSteerAngle * Time.deltaTime, 0);

        var localVel = transform.InverseTransformDirection(rb.velocity);

        if (localVel.z < 0)
        {
            currentSteerAngle *= -1f;
        }

        transform.Rotate(0, currentSteerAngle * Time.fixedDeltaTime, 0);
    }

    void Update()
    {
        wheelFLTrans.Rotate(0, wheelFL.rpm / 60 * 360 * Time.deltaTime, 0);
        wheelFRTrans.Rotate(0, wheelFR.rpm / 60 * 360 * Time.deltaTime, 0);
        wheelBLTrans.Rotate(0, wheelBL.rpm / 60 * 360 * Time.deltaTime, 0);
        wheelBRTrans.Rotate(0, wheelBR.rpm / 60 * 360 * Time.deltaTime, 0);
    }

    private void UpdateGearing()
    {
        currentSpeed = rb.velocity.magnitude;
        
        if (currentGear < gearForces.Length - 1 && currentSpeed > gearingThresholds[currentGear]) 
        {
            gearTimer += Time.deltaTime;
            if (gearTimer > autoGearTime) 
            {
                currentGear += 1;
                gearTimer = 0;
            } 
        }
        else
        {
            gearTimer = 0;
        }
        if(currentGear > 0 && currentSpeed < gearingThresholds[currentGear] / 2) 
        {
            currentGear -= 1;
        }
        
        currentAcceleration = gearForces[currentGear];
    }

    private void ApplyBraking()
    {
        if (isBraking)
        {
            rb.AddForce(-transform.forward * currentBrakeForce * Time.fixedDeltaTime);
        }
    }
}