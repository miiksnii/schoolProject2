using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Flight Settings")]
    public float baseFlySpeed = 10f;
    public float accelerationRate = 10f;    // how fast we speed up when holding Shift
    public float decelerationRate = 10f;    // how fast we slow down when holding Space
    public float minSpeed = 2f;
    public float maxSpeed = 40f;

    [Header("Rotation Settings")]
    public float yawAmount = 120f;
    public float pitchMax = 45f;
    public float rollMax = 30f;
    public float rotationSmoothness = 5f;

    private float yaw;
    private float currentSpeed;

    void Start()
    {
        currentSpeed = baseFlySpeed;
    }

    void Update()
    {
        HandleSpeedControl();
        HandleMovement();
    }

    void HandleSpeedControl()
    {
        // Increase or decrease speed
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed += accelerationRate * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            currentSpeed -= decelerationRate * Time.deltaTime;
        }

        // Clamp to safe range
        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);
    }

    void HandleMovement()
    {
        // Get inputs
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Smooth yaw
        yaw += horizontalInput * yawAmount * Time.deltaTime;

        // Target pitch/roll
        float targetPitch = Mathf.Lerp(0f, pitchMax, Mathf.Abs(verticalInput)) * Mathf.Sign(verticalInput);
        float targetRoll = Mathf.Lerp(0f, rollMax, Mathf.Abs(horizontalInput)) * -Mathf.Sign(horizontalInput);

        // Smooth rotation
        Quaternion targetRotation = Quaternion.Euler(targetPitch, yaw, targetRoll);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, rotationSmoothness * Time.deltaTime);

        // Move forward
        transform.position += transform.forward * currentSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("danger"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
