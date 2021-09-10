using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    [SerializeField] float speed = 12f;
    float defaultSpeed;
    float timeToResetSpeedToDefault;
    bool StartSpeedResetCountDown = false;

    public float gravity = -9.8f;
    public float jumpHeight = 3f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;
    bool isGrounded;

    void Awake()
    {
        defaultSpeed = speed;
    }

    void OnEnable()
    {
        PlayerHealth.OnPlayerDied += DisablePlayerController;
        SpeedCollectable.OnSpeedCollectable += UpdateSpeed;
    }

    void UpdateSpeed(float Speed, float Duration)
    {
        speed = Speed;
        timeToResetSpeedToDefault = Time.time + Duration;
        StartSpeedResetCountDown = true;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }    

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward *z;

        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if(StartSpeedResetCountDown && Time.time >= timeToResetSpeedToDefault)
        {
            speed = defaultSpeed;
            StartSpeedResetCountDown = false;
        }
    }

    void DisablePlayerController()
    {
        controller.enabled = false;
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerDied -= DisablePlayerController;
        SpeedCollectable.OnSpeedCollectable -= UpdateSpeed;
    }
}
