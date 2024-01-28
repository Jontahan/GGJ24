using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarSoundManager : MonoBehaviour
{

    public AudioClip carIgnition;
    public AudioClip carIdle;
    public AudioClip carDriving;
    public AudioClip carGearShift;
    public AudioClip carHonkStart;
    public AudioClip carHonkLoop;
    public AudioClip[] carDrifting;

    private AudioSource carGearShiftAudioSource;
    private AudioSource carEngineAudioSource;
    private AudioSource carDriftAudioSource;
    private AudioSource carHonkAudioSource;

    private GameControls inputActions;

    private Coroutine engineDriveCoroutine;
    private Coroutine engineStartupCoroutine;
    private Coroutine driftCoroutine;

    // Front left and right wheel used to calculate slip for drifting sounds
    private WheelCollider wheelFL;
    private WheelCollider wheelFR;
    private bool hasDrifted = false; // used to prevent drift sound from playing multiple times
    private float timeOfLastDriftSound = 0f;


    void Awake()
    {
        inputActions = new GameControls();
        inputActions.Car.Enable();
        wheelFL = transform.Find("Colliders/WheelFL").GetComponent<WheelCollider>();
        wheelFR = transform.Find("Colliders/WheelFR").GetComponent<WheelCollider>();
        var carAudioSources = GameObject.Find("PlayerCar").GetComponents<AudioSource>();
        carEngineAudioSource = carAudioSources[0];
        carGearShiftAudioSource = carAudioSources[1];
        carDriftAudioSource = carAudioSources[2];
        carHonkAudioSource = carAudioSources[3];
        engineStartupCoroutine = StartCoroutine(EngineStartup());
    }

    IEnumerator EngineStartup()
    {
        carGearShiftAudioSource.PlayOneShot(carIgnition);
        yield return new WaitForSeconds(carIgnition.length - 1f);
        carEngineAudioSource.clip = carIdle;
        carEngineAudioSource.loop = true;
        carEngineAudioSource.volume = 0.5f;
        carEngineAudioSource.Play();
    }

    IEnumerator EngineDrive()
    {
        // create for loop that goes through all gears
        // play gear shift sound
        // play driving sound
        // wait for driving sound to finish
        // if still accelerating, repeat
        // assuming 3 gears, update if needed later

        // Disable EngineStartup in case it isn't finished yet
        StopCoroutine(engineStartupCoroutine);

        carEngineAudioSource.Stop();
        carEngineAudioSource.volume = 0.8f;
        for (int i = 0; i < 3; i++)
        {
            carGearShiftAudioSource.PlayOneShot(carGearShift);
            yield return new WaitForSeconds(carGearShift.length - 0.5f);
            carEngineAudioSource.PlayOneShot(carDriving);
            yield return new WaitForSeconds(carDriving.length - carGearShift.length / 2);
        }

        // afterwards, just repeat car driving sound
        while (true)
        {
            carEngineAudioSource.PlayOneShot(carDriving);
            yield return new WaitForSeconds(carDriving.length);
        }

    }

    IEnumerator DriftCoroutine()
    {
        while (true)
        {
            // wait between 0.5 and 1.5 seconds
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.5f));
            // get random drift sound and play it
            var randomDriftSound = carDrifting[UnityEngine.Random.Range(0, carDrifting.Length)];
            carDriftAudioSource.PlayOneShot(randomDriftSound);
            // wait until drift sound is finished
            yield return new WaitForSeconds(randomDriftSound.length);

        }
    }

    void CalculateWheelSlip(WheelCollider wheelCollider)
    {
        WheelHit wheelHit;
        if (wheelCollider.GetGroundHit(out wheelHit))
        {
            // Calculate slip ratio for longitudinal slip or slip angle for lateral slip
            float sidewaysSlip = wheelHit.sidewaysSlip;

            // You can define thresholds for when you consider the car to be drifting
            // For example:
            if (Mathf.Abs(sidewaysSlip) > 0.3)
            {
                // The car is drifting
                if (Time.time - timeOfLastDriftSound > UnityEngine.Random.Range(0.2f, 0.5f))
                {
                    // get random drift sound and play it
                    var randomDriftSound = carDrifting[UnityEngine.Random.Range(0, carDrifting.Length)];
                    carDriftAudioSource.clip = randomDriftSound;
                    carDriftAudioSource.Play();
                    timeOfLastDriftSound = Time.time + randomDriftSound.length;
                }
            }
            else
            {
                // Stop the drift sound
                carDriftAudioSource.Stop();
            }
        }
    }

    // Wheelslip calculated for drifting sounds
    void FixedUpdate()
    {
        CalculateWheelSlip(wheelFL);
        CalculateWheelSlip(wheelFR);
    }

    // Update is called once per frame
    void Update()
    {

        // If user presses , do a honk
        if (Input.GetButtonDown("Fire2"))
        {
            carHonkAudioSource.PlayOneShot(carHonkStart);
        }
        else if (Input.GetButton("Fire2"))
        {
            if (!carHonkAudioSource.isPlaying)
            {
                carHonkAudioSource.clip = carHonkLoop;
                carHonkAudioSource.loop = true;
                carHonkAudioSource.Play();
            }
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            carHonkAudioSource.Stop();
        }


        // When we first press the acceleration button, start the engine
        if (inputActions.Car.Acceleration.triggered)
        {
            if (engineDriveCoroutine != null)
            {
                StopCoroutine(engineDriveCoroutine);
                engineDriveCoroutine = null;
            }
            engineDriveCoroutine = StartCoroutine(EngineDrive());
        }

        // if input is no longer active, stop the engine
        if (inputActions.Car.Acceleration.ReadValue<float>() == 0f && engineDriveCoroutine != null)
        {
            Debug.Log("Stopping engine");
            StopCoroutine(engineDriveCoroutine);
            engineDriveCoroutine = null;
            carGearShiftAudioSource.Stop();
            carEngineAudioSource.Stop();
            carEngineAudioSource.volume = 0.5f;
            carEngineAudioSource.clip = carIdle;
            carEngineAudioSource.loop = true;
            carEngineAudioSource.Play();
        }
    }
}
