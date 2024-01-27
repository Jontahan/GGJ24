using UnityEngine;

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


    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelBL;
    public WheelCollider wheelBR;
 
    public Transform wheelFLTrans;
    public Transform wheelFRTrans;
    public Transform wheelBLTrans;
    public Transform wheelBRTrans;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        currentSpeed = rb.velocity.magnitude;

        isBraking = Input.GetKey(KeyCode.Space);
        currentBrakeForce = isBraking ? brakeForce : 0f;

        if (Input.GetKey(KeyCode.W) && !isBraking)
        {
            rb.AddForce(transform.forward * acceleration * Time.fixedDeltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-transform.forward * acceleration * 0.5f * Time.fixedDeltaTime);
        }

        ApplyBraking();

        float turn = Input.GetAxis("Horizontal");
        currentSteerAngle = steering * turn;
        transform.Rotate(0, currentSteerAngle * Time.fixedDeltaTime, 0);
    }
    void Update() 
    {
        wheelFLTrans.Rotate (0, wheelFL.rpm / 60 * 360 * Time.deltaTime, 0);
        wheelFRTrans.Rotate (0, wheelFR.rpm / 60 * 360 * Time.deltaTime, 0);
        wheelBLTrans.Rotate (0, wheelBL.rpm / 60 * 360 * Time.deltaTime, 0);
        wheelBRTrans.Rotate (0, wheelBR.rpm / 60 * 360 * Time.deltaTime, 0);
    }
    private void ApplyBraking()
    {
        if (isBraking)
        {
            rb.AddForce(-transform.forward * currentBrakeForce * Time.fixedDeltaTime);
        }
    }
}