using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")]
    private Rigidbody body;
    public int actualSpeed;
    public float moveSpeed;
    public float walkSpeed;

    private float desiredSpeed;
    private float prevDesiredSpeed;

    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;

    public float groundDrag;
    public Transform orientation;
    public MovementState state;

    private Vector3 moveDir;

    [Header("Jumping")]
    public float jumpPower;
    public int maxJumpCount;
    public float airMultiplier;
    float lastJumpTime;
    int jumpCount;
    bool canJump;

    [Header("Crouching")]
    public float crouchSpeed;
    private float crouchYScale = 0.5f;
    private float startYScale;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeDetect;
    private bool leavingSlope;

    [Header("Ground Check")]
    public LayerMask groundMask;
    private float playerHeight = 2;
    public bool isGrounded;


    [Header("Inputs")]
    public InputAction move;
    public InputAction jump;
    public InputAction crouch;

    private float vertInput;
    private float horzInput;

    [Header("Testing")]
    public float test;
    public Camera cam;
    public float fovChangeTime = 0.5f;
    public float camFollowTime = 0.5f;
    public bool slope;
    public float slopeValue;

    private void OnEnable()
    {
        move.Enable();
        jump.Enable();
        crouch.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        crouch.Disable();
    }

    public enum MovementState
    {
        walking,
        crouching,
        airborne
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;
        startYScale = transform.localScale.y;

        if (InputSystem.actions)
        {
            move = InputSystem.actions.FindAction("Player/Move");
            jump = InputSystem.actions.FindAction("Player/Jump");
            crouch = InputSystem.actions.FindAction("Player/Crouch");
            OnEnable();
        }
        //For portals to disable this script, through ControlScriptReference


    }

    private void Update()
    {
        actualSpeed = (int)body.linearVelocity.magnitude;
        // Ground Check
        //isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);
        isGrounded = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y + 0.35f, transform.position.z), 0.4f, groundMask);

        // If the player is grounded for longer that 0.25 seconds it resets the jump count
        if (isGrounded && (Time.time - lastJumpTime > 0.25))
        {
            jumpCount = maxJumpCount;
            canJump = true;
            leavingSlope = false;
        }

        InputHandle();
        SpeedControl();

        // Drag Handler
        if (isGrounded)
        {
            body.linearDamping = groundDrag;
        }
        else
        {
            body.linearDamping = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
        slope = OnSlope();
    }

    // Handles all the movement inputs and changes the movement states
    private void InputHandle()
    {
        horzInput = move.ReadValue<Vector2>().x;
        vertInput = move.ReadValue<Vector2>().y;

        // Crouching
        if (isGrounded && crouch.IsPressed())
        {
            // Changes state and speed
            state = MovementState.crouching;
            desiredSpeed = crouchSpeed;
        }

        // Walking
        else if (isGrounded)
        {
            state = MovementState.walking;
            desiredSpeed = walkSpeed;
        }

        // Airborne
        else
        {
            state = MovementState.airborne;
        }

        // Jumping
        if (jump.WasPressedThisFrame() && canJump && jumpCount > 0)
        {
            Jump();

            if (jumpCount == 0)
            {
                canJump = false;
            }
        }
        // Allows the player to be able to hold the jump key and auto jump when they hit the ground again
        else if (jump.IsPressed() && isGrounded && Time.time - lastJumpTime > 0.5)
        {
            Jump();
        }

        if (crouch.WasPressedThisFrame())
        {
            // Shrinks the player and pushes them to the floor
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            body.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        if (crouch.WasReleasedThisFrame())
        {
            // Enlarges the player back to normal size
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }

        // Checks for drastic change in desiredSpeed
        if (Mathf.Abs(desiredSpeed - prevDesiredSpeed) > 4f && moveSpeed != 0)
        {
            StopCoroutine(SmoothlyLerpMoveSpeed());
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredSpeed;
        }

        prevDesiredSpeed = desiredSpeed;
    }

    // Changes the move speed to the desired speed gradually over time instead of instantly changing it
    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0;
        float diff = Mathf.Abs(desiredSpeed - moveSpeed);
        float start = moveSpeed;

        while (time < diff)
        {
            moveSpeed = Mathf.Lerp(start, desiredSpeed, time / diff);

            time += Time.deltaTime * speedIncreaseMultiplier;

            yield return null;
        }

        moveSpeed = desiredSpeed;
    }

    // Moves the player when they are on slope/ground/air
    private void MovePlayer()
    {
        // Move direction
        moveDir = orientation.forward * vertInput + orientation.right * horzInput;

        // on ground
        if (isGrounded)
        {
            body.AddForce(10f * moveSpeed * moveDir.normalized, ForceMode.Force);
        }
        // when airborne
        else
        {
            body.AddForce(10f * airMultiplier * moveSpeed * moveDir.normalized, ForceMode.Force);
            body.AddForce(Vector3.down * 1, ForceMode.Force);
        }

        body.useGravity = !OnSlope();

    }

    // Limits the player's speed unless they exceed a speed threshhold or are dashing
    private void SpeedControl()
    {
        // Limits the speed on slope
        if (OnSlope() && !leavingSlope)
        {
            if (body.linearVelocity.magnitude > moveSpeed)
            {
                body.linearVelocity = body.linearVelocity.normalized * moveSpeed;
            }
        }
        else if (moveSpeed > moveSpeed + 3)
        {
            // No speed limiting
        }
        // Limits the speed on ground and airborne
        else
        {
            Vector3 flatVelocity = new Vector3(body.linearVelocity.x, 0f, body.linearVelocity.z);

            if (flatVelocity.magnitude > moveSpeed)
            {
                Vector3 limitedVelocty = flatVelocity.normalized * moveSpeed;
                body.linearVelocity = new Vector3(limitedVelocty.x, body.linearVelocity.y, limitedVelocty.z);
            }
        }

    }

    // Jumps
    private void Jump()
    {
        leavingSlope = true;

        // Resets the y velocity (keeps all jumps the same)
        body.linearVelocity = new Vector3(body.linearVelocity.x, 0f, body.linearVelocity.z);

        body.AddForce(transform.up * jumpPower, ForceMode.Impulse);
        lastJumpTime = Time.time;
        jumpCount--;
    }


    // Checks if the player is standing on a slope
    public bool OnSlope()
    {
        float height = playerHeight;
        if (Physics.Raycast(transform.position, Vector3.down, out slopeDetect, height * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeDetect.normal);
            slopeValue = angle;
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    // Gets the direction the player must move to walk parallel up the slope
    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeDetect.normal).normalized;
    }
}
