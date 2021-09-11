using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Fiedls

    [SerializeField] CharacterController controller;

    // Properties for timed speed booser:
    float playerDefaultSpeed;
    float timeToResetPlayerSpeedToDefault;
    bool startSpeedResetCountDown = false;
    [SerializeField] float speed = 12f;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float gravity = -9.8f;

    [SerializeField] Transform groundCheckPosition;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask;
    
    Vector3 playerVelocity;
    bool isGrounded;

    const float defaultPlayerHeightforGroundCheck = -2f;
    
    #endregion

    void Awake()
    {
        playerDefaultSpeed = speed;
    }

    void OnEnable()
    {
        PlayerHealth.OnPlayerDied += DisablePlayerController;
        SpeedCollectable.OnSpeedCollectable += UpdateSpeed;
    }

    void UpdateSpeed(float NewSpeed, float Duration)
    {
        speed = NewSpeed;
        timeToResetPlayerSpeedToDefault = Time.time + Duration;
        startSpeedResetCountDown = true;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheckPosition.position, groundDistance, groundMask);
        
        ResetYVelocityWhenGrounded();

        Move();

        Jump();
        
        simulateGravity();

        CheckIfSpeedBoostEffectIsOver();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movementDirection = transform.right * x + transform.forward * z;

        controller.Move(movementDirection * speed * Time.deltaTime);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // formula for velocity needed to jump a certain height =>  v = Sqrt(Height X -2 X gravity)
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void simulateGravity()
    {
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void ResetYVelocityWhenGrounded()
    {
        if (isGrounded && playerVelocity.y < 0)
            playerVelocity.y = defaultPlayerHeightforGroundCheck; 
    }

    void DisablePlayerController()
    {
        controller.enabled = false;
    }

    void CheckIfSpeedBoostEffectIsOver()
    {
        if (startSpeedResetCountDown && Time.time >= timeToResetPlayerSpeedToDefault)
        {
            speed = playerDefaultSpeed;
            startSpeedResetCountDown = false;
        }
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerDied -= DisablePlayerController;
        SpeedCollectable.OnSpeedCollectable -= UpdateSpeed;
    }
}
